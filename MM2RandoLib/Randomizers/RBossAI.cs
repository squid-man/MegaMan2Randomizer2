using System;
using System.Collections.Generic;
using System.Linq;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;

namespace MM2Randomizer.Randomizers
{
    public class RBossAI : IRandomizer
    {
        public RBossAI() { }

        public void Randomize(Patch in_Patch, RandomizationContext in_Context)
        {
            this.ChangeHeat(in_Patch, in_Context.Seed);
            this.ChangeAir(in_Patch, in_Context.Seed);
            this.ChangeWood(in_Patch, in_Context.Seed);
            this.ChangeBubble(in_Patch, in_Context.Seed);
            this.ChangeQuick(in_Patch, in_Context.Seed);
            this.ChangeFlash(in_Patch, in_Context.Seed);
            this.ChangeMetal(in_Patch, in_Context.Seed);
            this.ChangeCrash(in_Patch, in_Context.Seed);
        }

        /// <summary>
        /// Heatman AI 0x02C16E - 0x02C1FE
        /// </summary>
        protected void ChangeHeat(Patch in_Patch, ISeed in_Seed)
        {
            //
            // Projectile Y distances
            //

            //0x02C207 default 07, good from 03 - 08
            in_Patch.Add(0x02C207, in_Seed.NextUInt8(3, 9), "Heatman Projectile 1 Y-Distance");

            //0x02C208 default 05, good from 04 - 07
            in_Patch.Add(0x02C208, in_Seed.NextUInt8(4, 8), "Heatman Projectile 2 Y-Distance");

            //0x02C209 default 03, good from 03 - 05
            in_Patch.Add(0x02C209, in_Seed.NextUInt8(3, 6), "Heatman Projectile 3 Y-Distance");

            //
            // Projectile x distances, 0x3A 0x2E 0x1C
            //

            // The lower value, the faster speed. Different for each fireball.

            //0x02C20A - 1st value should be 71 to hit megaman, or, from 48 to 128
            in_Patch.Add(0x02C20A, in_Seed.NextUInt8(48, 129), "Heatman Projectile 1 X-Distance");

            //0x02C20B - 2nd value should be 46 to hit megaman, 0r, from 34 to 64
            in_Patch.Add(0x02C20B, in_Seed.NextUInt8(34, 65), "Heatman Projectile 2 X-Distance");

            //0x02C20C - 3rd value should be 23 to hit megaman, or, from 16 to 48
            in_Patch.Add(0x02C20C, in_Seed.NextUInt8(16, 49), "Heatman Projectile 3 X-Distance");


            //
            // 30/60/90 frame delay
            // Choose delay interval from 10-40 frames
            //

            Byte delay = in_Seed.NextUInt8(10, 41);
            //0x02C29D - Delay 1 0x1F
            in_Patch.Add(0x02C29D, delay, "Heatman Invuln Delay 1");
            //0x02C29E - Delay 2 0x3E
            in_Patch.Add(0x02C29E, (Byte)(delay * 2), "Heatman Invuln Delay 2");
            //0x02C29F - Delay 3 0x5D
            in_Patch.Add(0x02C29F, (Byte)(delay * 3), "Heatman Invuln Delay 3");

            //
            //0x02C253 - Charge velocity(0x04, 0x08 or more usually puts him on side of screen)
            //

            in_Patch.Add(0x02C253, in_Seed.NextUInt8(2, 6), "Heatman Charge Velocity");
        }


        /// <summary>
        /// Airman AI 0x02C2F3 - 0x02C50A
        /// </summary>
        protected void ChangeAir(Patch in_Patch, ISeed in_Seed)
        {
            // Create random Air Shooter patterns

            //0x02C393 - Tornado 0 Pattern 0 y-vel fraction
            //0x02C395 - Tornado 1 Pattern 0 y-vel frac
            //...
            //0x02C39A - Tornado 0 Pattern 1 y-vel frac
            //... ...
            //0x02C3B1 - Tornado 0 Pattern 0 y-vel integer
            //0x02C3CF - Tornado 0 Pattern 0 x-vel fraction
            //0x02C3ED - Tornado 0 Pattern 0 x-vel integer
            //0x02C40B - Tornado 0 Pattern 0 delay before stop
            const Int32 TORNADO_TABLE_LENGTH = 30;

            // Write y-vel fractions: 00-FF
            for (Int32 i = 0; i < TORNADO_TABLE_LENGTH; i++)
            {
                in_Patch.Add(0x02C393 + i, in_Seed.NextUInt8(256), String.Format("Airman Tornado {0} Y-Vel Frac", i));
            }

            // Write y-vel integers: FF-03, rare 04
            Byte[] VELOCITY_Y_TABLE = { 0xFF, 0x00, 0x01, 0x02, 0x03 };

            for (Int32 i = 0; i < TORNADO_TABLE_LENGTH; i++)
            {
                Byte velocityY = 0;

                if (in_Seed.NextDouble() > 0.9)
                {
                    velocityY = 0x04;
                }
                else
                {
                    velocityY = in_Seed.NextElement(VELOCITY_Y_TABLE);
                }

                in_Patch.Add(0x02C3B1 + i, velocityY, String.Format("Airman Tornado {0} Y-Vel Int", i));
            }

            // Write x-vel fractions: 00-FF
            for (Int32 i = 0; i < TORNADO_TABLE_LENGTH; i++)
            {
                in_Patch.Add(0x02C3CF + i, in_Seed.NextUInt8(256), String.Format("Airman Tornado {0} X-Vel Frac", i));
            }

            // Write x-vel integers: 00-04, rare 04, common 03
            Byte[] VELOCITY_X_TABLE = { 0x00, 0x01, 0x02 };

            for (Int32 i = 0; i < TORNADO_TABLE_LENGTH; i++)
            {
                Byte velocityX = 0;

                Double chance = in_Seed.NextDouble();

                if (chance > 0.9)
                {
                    velocityX = 0x04;
                }
                else if (chance > 0.6)
                {
                    velocityX = 0x03;
                }
                else
                {
                    velocityX = in_Seed.NextElement(VELOCITY_X_TABLE);
                }

                in_Patch.Add(0x02C3ED + i, velocityX, String.Format("Airman Tornado {0} X-Vel Int", i));
            }

            // Write delays: 05-2A
            for (Int32 i = 0; i < TORNADO_TABLE_LENGTH; i++)
            {
                in_Patch.Add(0x02C40B + i, in_Seed.NextUInt8(5, 42), String.Format("Airman Tornado {0} Delay Time", i));
            }

            // 0x02C30C - Num patterns before jumping 0x03 (do 1-4)
            in_Patch.Add(0x02C30C, in_Seed.NextUInt8(1, 5), "Airman Patterns Before Jump");

            //0x02C4DD - First Jump y-vel frac, 0xE6
            //0x02C4DE - Second Jump y-vel frac, 0x76
            //0x02C4E0 - First Jump y-vel Int32, 0x04
            //0x02C4E1 - Second Jump y-vel Int32 0x07
            //0x02C4E3 - First Jump x-vel frac, 0x39
            //0x02C4E4 - Second Jump x-vel frac 0x9a
            //0x02C4E6 - First Jump x-vel Int32, 0x01
            //0x02C4E7 - Second Jump x-vel Int32 0x01
            // Pick x-vel integers for both jumps first. Must add up to 2 or 3.
            Byte rSum = in_Seed.NextUInt8(2, 4);
            Byte jump1x = in_Seed.NextUInt8(rSum + 1);
            Byte jump2x = (Byte)(rSum - jump1x);
            in_Patch.Add(0x02C4E6, jump1x, "Airman X-Velocity Integer Jump 1");
            in_Patch.Add(0x02C4E7, jump2x, "Airman X-Velocity Integer Jump 2");

            // If a jump's x-Int32 is 0, its corresponding y-Int32 must be 5-6
            // If a jump's x-Int32 is 1, its corresponding y-Int32 must be 4-5
            // If a jump's x-Int32 is 2, its corresponding y-Int32 must be 3-5
            // If a jump's x-Int32 is 3, its corresponding y-Int32 must be 2-4
            Byte jump1y = this.AirmanGetJumpYVelocity(jump1x, in_Seed);
            Byte jump2y = this.AirmanGetJumpYVelocity(jump2x, in_Seed);
            in_Patch.Add(0x02C4E0, jump1y, "Airman Y-Velocity Integer Jump 1");
            in_Patch.Add(0x02C4E1, jump2y, "Airman Y-Velocity Integer Jump 2");

            // Random x and y-vel fractions for both jumps
            //stream.Position = 0x02C4DD; // 1st jump y-vel frac
            // If jump is 7 and fraction is > 0xF0, Airman gets stuck!
            in_Patch.Add(0x02C4DD, in_Seed.NextUInt8(241), "Airman Y-Velocity Fraction Jump 1");
            in_Patch.Add(0x02C4DE, in_Seed.NextUInt8(241), "Airman Y-Velocity Fraction Jump 2");
            in_Patch.Add(0x02C4E3, in_Seed.NextUInt8(256), "Airman X-Velocity Fraction Jump 1");
            in_Patch.Add(0x02C4E4, in_Seed.NextUInt8(256), "Airman X-Velocity Fraction Jump 2");
        }

        private Byte AirmanGetJumpYVelocity(Int32 xVelInt, ISeed in_Seed)
        {
            Int32 jumpYMax = 0;
            Int32 jumpYMin = 0;

            switch (xVelInt)
            {
                case 0:
                {
                    jumpYMax = 6;
                    jumpYMin = 5;
                    break;
                }

                case 1:
                {
                    jumpYMax = 5;
                    jumpYMax = 4;
                    break;
                }

                case 2:
                {
                    jumpYMax = 5;
                    jumpYMin = 3;
                    break;
                }

                case 3:
                {
                    jumpYMax = 4;
                    jumpYMin = 2;
                    break;
                }

                default:
                {
                    break;
                }
            }

            return in_Seed.NextUInt8(jumpYMin, jumpYMax + 1);
        }


        protected void ChangeWood(Patch in_Patch, ISeed in_Seed)
        {
            // Woodman AI

            // Some unused addresses for later:
            //0x02C567 - Falling leaf y-pos start, 0x20
            //0x03DA34 - Leaf shield y-velocity while it's attached to woodman, lol.

            //0x02C537 - Delay between leaves 18. Do 6 to 32.
            in_Patch.Add(0x02C537, in_Seed.NextUInt8(6, 33), "Woodman Leaf Spacing Delay");

            //0x02C5DD - Jump height, 4. Do 3 to 7.
            in_Patch.Add(0x02C5DD, in_Seed.NextUInt8(3, 8), "Woodman Jump Y-Velocity");

            //0x02C5E2 - Jump distance, 1. Do 1 to 4.
            in_Patch.Add(0x02C5E2, in_Seed.NextUInt8(1, 5), "Woodman Jump X-Velocity");

            //0x02C5A9 - Shield launch speed, 4. Do 1 to 8.
            in_Patch.Add(0x02C5A9, in_Seed.NextUInt8(1, 9), "Woodman Shield Launch X-Velocity");

            //0x02C553 - Number of falling leaves, 0x03. Do 0x02 20% of the time.
            if (in_Seed.NextDouble() > 0.8)
            {
                in_Patch.Add(0x02C553, 0x02, "Woodman Falling Leaf Quantity");
            }

            //0x02C576 - Falling leaf x-vel, 0x02. Do 0x01 or 0x02, but with a low chance for 0x00 and lower chance for 0x03
            Byte[] VELOCITY_X_TABLE =
            {
                0x00,0x00,0x00,
                0x03,
                0x01,0x01,0x01,0x01,0x01,0x01,
                0x02,0x02,0x02,0x02,
            };

            in_Patch.Add(0x02C576, in_Seed.NextElement(VELOCITY_X_TABLE), "Woodman Falling Leaf X-Velocity");

            //0x03D8F6 - 0x02, change to 0x06 for an interesting leaf shield pattern 20% of the time
            if (in_Seed.NextDouble() > 0.8)
            {
                in_Patch.Add(0x03D8F6, 0x06, "Woodman Leaf Shield Pattern");
            }

            //0x03B855 - Leaf fall speed(sort of ?) 0x20.
            // Decrease value to increase speed. At 0x40, it doesn't fall.
            // 20% of the time, change to a high number to instantly despawn leaves for a fast pattern.
            // Do from 0x00 to 0x24.  Make less than 0x1A a lower chance.
            Byte velocityY;

            if (in_Seed.NextDouble() > 0.8)
            {
                velocityY = 0xA0; // Leaves go upwards
            }
            else
            {
                Byte[] VELOCITY_Y_TABLE =
                {
                    0x08, 0x18, 0x1C, // Fall faster
                    0x1D, 0x1E, 0x20, 0x21, 0x22, 0x23, 0x24 // Fall slower
                };

                velocityY = in_Seed.NextElement(VELOCITY_Y_TABLE);
            }

            in_Patch.Add(0x03B855, velocityY, "Woodman Falling Leaf Y-Velocity");
        }


        protected void ChangeBubble(Patch in_Patch, ISeed in_Seed)
        {
            // Bubbleman's AI

            //0x02C707 - Y-pos to reach before falling, 0x50.
            Byte[] BUBBLEMAN_HEIGHT_APEX_TABLE = { 80, 64, 80, 64, 96, 128 };
            in_Patch.Add(0x02C707, in_Seed.NextElement(BUBBLEMAN_HEIGHT_APEX_TABLE), "Bubbleman Y Max Height");

            //0x02C70B - Falling speed integer, 0xFF.
            Byte[] BUBBLEMAN_FALL_SPEED_TABLE = { 254, 255, 255, 255, 255 };
            in_Patch.Add(0x02C70B, in_Seed.NextElement(BUBBLEMAN_FALL_SPEED_TABLE), "Bubbleman Y-Velocity Falling");

            //0x02C710 - Landing x-tracking speed, integer, 0x00.
            Byte[] BUBBLEMAN_HORIZONTAL_TRACKING_SPEED_TABLE = { 0, 0, 0, 0, 0, 1 };
            in_Patch.Add(0x02C710, in_Seed.NextElement(BUBBLEMAN_HORIZONTAL_TRACKING_SPEED_TABLE), "Bubbleman X-Velocity Falling");

            //0x02C6D3 - Rising speed integer, 0x01.
            Byte[] BUBBLEMAN_RISING_SPEED_TABLE = { 1, 1, 2, 2, 3 };
            in_Patch.Add(0x02C6D3, in_Seed.NextElement(BUBBLEMAN_RISING_SPEED_TABLE), "Bubbleman Y-Velocity Rising");

            //0x02C745 - Delay between water gun shots, 0x12
            Byte[] BUBBLEMAN_ARM_CANNON_SHOT_DELAY_TABLE = { 4, 8, 12, 16, 18, 20, 24, 28 };
            in_Patch.Add(0x02C745, in_Seed.NextElement(BUBBLEMAN_ARM_CANNON_SHOT_DELAY_TABLE), "Bubbleman Water Gun Cooldown");

            // WARNING: THIS ADDRESS IS SHARED WITH THE VELOCITY OF THE DEATH BEAMS IN QUICKMAN STAGE!!
            ////0x03DA19 - X-Vel water gun, Int 0x04
            //bytes = new Byte[] { 0x02, 0x03, 0x04, 0x05, };
            //rInt = in_Seed.Next(bytes.Length);
            //in_Patch.Add(0x03DA19, bytes[rInt], "Bubbleman X-Vel Water Gun, Int");

            ////0x03DA1A - X-Vel water gun, Frac 0x40
            //bytes = new Byte[] { 0x40, 0x80, 0xC0, 0x00, };
            //rInt = in_Seed.Next(bytes.Length);
            //in_Patch.Add(0x03DA1A, bytes[rInt], "Bubbleman X-Vel Water Gun, Frac");

            //0x03DA25 - X-Vel bubble shot, Int 0x01
            Byte[] BUBBLEMAN_BUBBLE_LEAD_SHOT_X_VELOCITY_TABLE = { 0, 0, 1, 1, 2 };
            in_Patch.Add(0x03DA25, in_Seed.NextElement(BUBBLEMAN_BUBBLE_LEAD_SHOT_X_VELOCITY_TABLE), "Bubbleman X-Vel Bubble, Int");

            //0x03DA26 - X-Vel bubble shot, Frac 0x00
            Byte[] BUBBLEMAN_BUBBLE_LEAD_SHOT_X_VELOCITY_FRACTION_TABLE = { 128, 192, 255 };
            in_Patch.Add(0x03DA26, in_Seed.NextElement(BUBBLEMAN_BUBBLE_LEAD_SHOT_X_VELOCITY_FRACTION_TABLE), "Bubbleman X-Vel Bubble, Frac");

            //0x03DA4D - Y-Vel bubble shot initial, Int (0x03)
            Byte[] BUBBLEMAN_BUBBLE_LEAD_SHOT_Y_VELOCITY_TABLE = { 2, 3, 4, 5 };
            in_Patch.Add(0x03DA4D, in_Seed.NextElement(BUBBLEMAN_BUBBLE_LEAD_SHOT_Y_VELOCITY_TABLE), "Bubbleman Y-Vel Bubble Initial, Int");

            //0x03DA4E - Y-Vel bubble shot initial, Frac (0x76)
            Byte[] BUBBLEMAN_BUBBLE_LEAD_SHOT_Y_VELOCITY_FRACTION_TABLE = { 0, 64, 128, 192 };
            in_Patch.Add(0x03DA4E, in_Seed.NextElement(BUBBLEMAN_BUBBLE_LEAD_SHOT_Y_VELOCITY_FRACTION_TABLE), "Bubbleman Y-Vel Bubble Initial, Frac");

            //0x03B747 - Y-Vel bubble shot bounce, Int (0x03)
            Byte[] BUBBLEMAN_BUBBLE_LEAD_BOUNCE_Y_VELOCITY_TABLE = { 2, 3, 4, 5 };
            in_Patch.Add(0x03B747, in_Seed.NextElement(BUBBLEMAN_BUBBLE_LEAD_BOUNCE_Y_VELOCITY_TABLE), "Bubbleman Y-Vel Bubble Bounce, Int");

            //0x03B74C - Y-Vel bubble shot bounce, Frac (0x76)
            Byte[] BUBBLEMAN_BUBBLE_LEAD_BOUNCE_Y_VELOCITY_FRACTION_TABLE = { 0, 64, 128, 192 };
            in_Patch.Add(0x03B74C, in_Seed.NextElement(BUBBLEMAN_BUBBLE_LEAD_BOUNCE_Y_VELOCITY_FRACTION_TABLE), "Bubbleman Y-Vel Bubble Bounce, Frac");
        }

        protected void ChangeQuick(Patch in_Patch, ISeed in_Seed)
        {
            // Other addresses with potential:
            //0x02C872 - Projectile type, 0x59

            // Quickman's AI
            //0x02C86E - Number of Boomerangs, 3, do from 1 - 10
            in_Patch.Add(0x02C86E, in_Seed.NextUInt8(1, 11), "Quickman Number of Boomerangs");

            //0x02C882 - Boomerang: delay before arc 37. 0 for no arc, or above like 53. do from 5 to 53.
            in_Patch.Add(0x02C882, in_Seed.NextUInt8(5, 54), "Quickman Boomerang Delay 1");

            //0x02C887 - Boomerang speed when appearing, 4, do from 1 to 7
            in_Patch.Add(0x02C887, in_Seed.NextUInt8(1, 8), "Quickman Boomerang Velocity Integer 1");

            //0x03B726 - Boomerang speed secondary, 4, does this affect anything else?
            in_Patch.Add(0x03B726, in_Seed.NextUInt8(1, 8), "Quickman Boomerang Velocity Integer 2");

            // For all jumps, choose randomly from 2 to 10
            //0x02C8A3 - Middle jump, 7
            in_Patch.Add(0x02C8A3, in_Seed.NextUInt8(2, 11), "Quickman Jump Height 1 Integer");

            //0x02C8A4 - High jump, 8
            in_Patch.Add(0x02C8A4, in_Seed.NextUInt8(2, 11), "Quickman Jump Height 2 Integer");

            //0x02C8A5 - Low jump, 4
            in_Patch.Add(0x02C8A5, in_Seed.NextUInt8(2, 11), "Quickman Jump Height 3 Integer");

            //0x02C8E4 - Running time, 62, do from 24 to 80
            in_Patch.Add(0x02C8E4, in_Seed.NextUInt8(24, 81), "Quickman Running Time");

            //0x02C8DF - Running speed, 2, do from 1 to 5
            in_Patch.Add(0x02C8DF, in_Seed.NextUInt8(1, 6), "Quickman Running Velocity Integer");
        }

        protected void ChangeFlash(Patch in_Patch, ISeed in_Seed)
        {
            // Unused addresses
            //0x02CA71 - Projectile type 0x35
            //0x02CA52 - "Length of time stopper / projectile frequency ?"

            // Flashman's AI

            //0x02C982 - Walk velocity integer 1, do from 0 to 3
            in_Patch.Add(0x02C982, in_Seed.NextUInt8(4), "Flashman Walk Velocity Integer");

            //0x02C97D - Walk velocity fraction 6, do 0 to 255
            in_Patch.Add(0x02C97D, in_Seed.NextUInt8(256), "Flashman Walk Velocity Fraction");

            //0x02C98B - Delay before time stopper 187 frames. Do from 30 frames to 240 frames
            in_Patch.Add(0x02C98B, in_Seed.NextUInt8(30, 241), "Flashman Delay Before Time Stopper");

            //0x02CAC6 - Jump distance integer 0, do 0 to 3
            // TODO do fraction also
            in_Patch.Add(0x02CAC6, in_Seed.NextUInt8(4), "Flashman Jump X-Velocity Integer");

            //0x02CACE - Jump height 4, do 3 to 8
            in_Patch.Add(0x02CACE, in_Seed.NextUInt8(3, 9), "Flashman Jump Y-Velocity Integer");

            //0x02CA81 - Projectile speed 8, do 2 to 10
            in_Patch.Add(0x02CA81, in_Seed.NextUInt8(2, 11), "Flashman Projectile Velocity Integer");

            //0x02CA09 - Number of projectiles to shoot 6, do 3 to 16
            in_Patch.Add(0x02CA09, in_Seed.NextUInt8(3, 17), "Flashman Number of Projectiles");
        }

        protected void ChangeMetal(Patch in_Patch, ISeed in_Seed)
        {
            // Unused addresses
            //0x02CC2D - Projectile type
            //0x02CC29 - Metal Blade sound effect 0x20

            // Metalman AI

            //0x02CC3F - Speed of Metal blade 4, do 2 to 9
            in_Patch.Add(0x02CC3F, in_Seed.NextUInt8(2, 10), "Metalman Projectile Velocity Integer");

            //0x02CC1D - Odd change to attack behavior, 0x06, only if different than 6. Give 25% chance.
            if (in_Seed.NextDouble() > 0.75)
            {
                in_Patch.Add(0x02CC1D, 0x05, "Metalman Alternate Attack Behavior");
            }

            //0x02CBB5 - Jump Height 1 0x06, do from 03 - 07 ? higher than 7 bonks ceiling
            //0x02CBB6 - Jump Height 2 0x05
            //0x02CBB7 - Jump Height 3 0x04

            // Shuffle the list of jump heights to get three different heights
            List<Byte> jumpHeight = in_Seed.Shuffle(new Byte[] { 3, 4, 5, 6, 7 }).ToList();

            for (Int32 i = 0; i < 3; i ++)
            {
                in_Patch.Add(0x02CBB5 + i, jumpHeight[i], String.Format("Metalman Jump {0} Y-Velocity Integer", i + 1));
            }
        }

        protected void ChangeCrash(Patch in_Patch, ISeed in_Seed)
        {
            // Unused addresses
            //0x02CDAF - Jump velocity fraction? 0x44 Double check

            // Crashman AI

            // Crash Man's routine for attack
            //0x02CCf2 - Walk x-vel fraction 0x47
            in_Patch.Add(0x02CCf2, in_Seed.NextUInt8(), "Crashman Walk X-Velocity Fraction");

            //0x02CCF7 - Walk x-vel integer 0x01, do 0 to 2
            in_Patch.Add(0x02CCF7, in_Seed.NextUInt8(2), "Crashman Walk X-Velocity Integer");

            //0x02CD07 - Jump behavior 0x27. 0x17 = always jumping, any other value = doesn't react with a jump.
            // Give 25% chance for each unique behavior, and 50% for default.
            // UPDATE: One of these two behaviors breaks and Crashman goes crazy. I think it's 0x17. disable.
            Byte jumpType = 0x27;

            if (in_Seed.NextDouble() > 0.75)
            //{
            //    jumpType = 0x17;
            //}
            //else if (rDbl > 0.5)
            {
                jumpType = 0x26;
            }

            in_Patch.Add(0x02CD07, jumpType, "Crashman Special Jump Behavior");

            //0x02CD2A - Jump y-vel intger, 6, do from 2 to 10
            in_Patch.Add(0x02CD2A, in_Seed.NextUInt8(2, 11), "Crashman Jump Y-Velocity Integer");

            //0x02CDD3 - Shot behavior, 0x5E, change to have him always shoot when jumping, 20% chance
            if (in_Seed.NextDouble() > 0.80)
            {
                in_Patch.Add(0x02CDD3, 0x50, "Crashman Disable Single Shot");
            }

            //0x02CDEE - Crash Bomber velocity, 0x06, do from 2 to 8
            in_Patch.Add(0x02CDEE, in_Seed.NextUInt8(2, 9), "Crashman Crash Bomber X-Velocity");
        }
    }
}
