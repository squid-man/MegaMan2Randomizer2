using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using MM2Randomizer.Enums;
using MM2Randomizer.Extensions;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;
using MM2Randomizer.Randomizers;
using MM2Randomizer.Randomizers.Stages;
using MM2Randomizer.Resources;
using MM2Randomizer.Settings;
using MM2Randomizer.Settings.Options;

namespace MM2Randomizer.Utilities
{
    public static class MiscHacks
    {
        public static Dictionary<ResourceNode, ResourceNode?> ApplyOneIpsPerDir(
            RandomizationContext context,
            string basePath,
            bool canBeNull = true,
            ISeed? seed = null,
            Patch? patch = null,
            string? outFileName = null,
            ResourceTree? resTree = null)
        {
            if (seed is null)
                seed = context.Seed;
            if (patch is null)
                patch = context.Patch;
            if (outFileName is null)
                outFileName = RandomizationContext.TEMPORARY_FILE_NAME;
            if (resTree is null)
                resTree = context.ResourceTree;

            return ApplyOneIpsPerDir(
                resTree,
                seed,
                patch,
                basePath,
                canBeNull,
                outFileName);
        }

        public static  Dictionary<ResourceNode, ResourceNode?> ApplyOneIpsPerDir(
            ResourceTree resTree,
            ISeed seed,
            Patch patch,
            string basePath,
            bool canBeNull,
            string outFileName)
        {
            var relRoot = resTree.Find(basePath);
            var selDirNodes = relRoot.PickOneFilePerDirectory(
                seed, 
                canBeNull,
                null,
                n => n.Name.EndsWith(".ips", StringComparison.InvariantCultureIgnoreCase));

            foreach (var (dirNode, fileNode) in selDirNodes)
            {
                if (fileNode is null)
                    continue;

                var ips = resTree.LoadResource(fileNode);
                patch.ApplyIPSPatch(outFileName, ips);
            }

            return selDirNodes;
        }

        public static void DrawTitleScreenChanges(Patch p, String in_SeedBase26, RandomizationSettings settings)
        {
            // Adjust cursor positions
            p.Add(0x0362D4, 0x90, "Title screen Cursor top position"); // default 0x98
            p.Add(0x0362D5, 0xA0, "Title screen Cursor bottom position"); // default 0xA8

            //
            // Draw version header and value onto the title screen
            //

            Byte[] versionHeader = "VER. ".AsIntroString();
            p.Add(0x037402, versionHeader, "Title Screen Version Header");

            System.Reflection.Assembly assembly = typeof(RandomMM2).Assembly;
            Version version = assembly.GetName().Version ?? throw new NullReferenceException(@"Assembly version cannot be null");
            String stringVersion = version.ToString();

            for (Int32 i = 0; i < stringVersion.Length; i++)
            {
                Byte value = stringVersion[i].AsIntroCharacter();
                p.Add(0x037407 + i, value, "Title Screen Version Value");
            }


            //
            // Draw the hash header and value onto the title screen
            //

            Byte[] hashHeader = "HASH ".AsIntroString();
            p.Add(0x0373C2, hashHeader, "Title Screen Hash Header");

            String seedAlpha = in_SeedBase26;
            for (Int32 i = 0; i < seedAlpha.Length; i++)
            {
                Byte value = seedAlpha[i].AsIntroCharacter();
                p.Add(0x0373C7 + i, value, "Title Screen Hash Value");
            }

            //
            // Draw the flags string onto the game start screen
            //

            Byte[] behaviorFlagHeader = "FLAG ".AsIntroString();
            p.Add(0x0373A2, behaviorFlagHeader, "Title Screen flags");

            String behaviorFlags = settings.GetBehaviorFlagsString();
            for (Int32 i = 0; i < 14; i++)
            {
                Byte value = behaviorFlags[i].AsIntroCharacter();
                p.Add(0x0373A7 + i, value, $"Title Screen Flags: {behaviorFlags[i]}");
            }

            Byte[] behaviorFlagHeader2 = "     ".AsIntroString();
            p.Add(0x037382, behaviorFlagHeader2, "Title Screen flags 2");

            for (Int32 i = 0; i < 14; i++)
            {
                Byte value = behaviorFlags[14 + i].AsIntroCharacter();
                p.Add(0x037387 + i, value, $"Title Screen Flags: {behaviorFlags[i]}");
            }

            Byte[] cosmeticFlagHeader = "COSM ".AsIntroString();
            p.Add(0x037362, cosmeticFlagHeader, "Title Screen hash");

            String cosmeticFlags = settings.GetCosmeticFlagsString();
            for (Int32 i = 0; i < cosmeticFlags.Length; i++)
            {
                Byte value = cosmeticFlags[i].AsIntroCharacter();
                p.Add(0x037367 + i , value, $"Title Screen Flags: {cosmeticFlags[i]}");
            }


            // Draw tournament mode/spoiler free information
            if (settings.IsTournament)
            {
                p.Add(0x037564,
                    settings.SettingsPreset!.TournamentTitleScreenString!.AsIntroString(), 
                    "Title Screen Tournament Text");

                // Draw Hash symbols
                // Use $B8-$BF with custom gfx, previously unused tiles after converting from MM2U to RM2
                //p.Add(0x037367, (Byte)(0xB0), "Title Screen Flags");
                //p.Add(0x037368, (Byte)(0xB1), "Title Screen Flags");
                //p.Add(0x037369, (Byte)(0xB2), "Title Screen Flags");
                //p.Add(0x03736A, (Byte)(0xB3), "Title Screen Flags");
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public static void SetETankKeep(Patch p)
        {
            p.Add(0x07C1CC, 0xEA, "Disable ETank clear on Game Over 1");
            p.Add(0x07C1CD, 0xEA, "Disable ETank clear on Game Over 2");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="jVersion"></param>
        public static void SetWily5NoMusicChange(Patch p)
        {
            p.Add(0x0383DA, 0xEA, "Disable Music on Boss Defeat 1");
            p.Add(0x0383DB, 0xEA, "Disable Music on Boss Defeat 2");
            p.Add(0x0383DC, 0xEA, "Disable Music on Boss Defeat 3");
            p.Add(0x03848A, 0xEA, "Disable Music on Boss Defeat 4");
            p.Add(0x03848B, 0xEA, "Disable Music on Boss Defeat 5");
            p.Add(0x03848C, 0xEA, "Disable Music on Boss Defeat 6");
            p.Add(0x02E070, 0xEA, "Disable Music on Boss Defeat 7");
            p.Add(0x02E071, 0xEA, "Disable Music on Boss Defeat 8");
            p.Add(0x02E072, 0xEA, "Disable Music on Boss Defeat 9");
        }

        /// <summary>
        /// TODO
        /// </summary>
        public static void SetFastWeaponGetText(Patch p)
        {
            //Int32 address = (jVersion) ? 0x037C51 : 0x037D4A;
            Int32 address = 0x037D4A;
            p.Add(address, 0x04, "Weapon Get Text Write Delay");
        }

        /// <summary>
        /// </summary>
        public static void SetHitPointChargingSpeed(Patch p, ChargingSpeedOption chargingSpeed)
        {
            Int32 address = 0x03831B;
            p.Add(address, (Byte)chargingSpeed, "Hit Point Charging Speed");
        }

        /// <summary>
        /// </summary>
        public static void SetWeaponEnergyChargingSpeed(Patch p, ChargingSpeedOption chargingSpeed)
        {
            Int32 address = 0x03835A;
            p.Add(address, (Byte)chargingSpeed, "Weapon Energy Charging Speed");
        }

        /// <summary>
        /// </summary>
        public static void SetEnergyTankChargingSpeed(Patch p, ChargingSpeedOption chargingSpeed)
        {
            Int32 address = 0x0352B2;
            p.Add(address, (Byte)chargingSpeed, "Energy Tank Charging Speed");
        }

        /// <summary>
        /// </summary>
        public static void SetRobotMasterEnergyChargingSpeed(Patch p, ChargingSpeedOption chargingSpeed)
        {
            Int32 address = 0x02C142;
            p.Add(address, (Byte)chargingSpeed, "Robot Master Energy Charging Speed");
        }

        /// <summary>
        /// </summary>
        public static void SetCastleBossEnergyChargingSpeed(Patch p, ChargingSpeedOption chargingSpeed)
        {
            Int32 address = 0x02E12B;
            p.Add(address, (Byte)chargingSpeed, "Castle Boss Energy Charging Speed");
        }

        /// <summary>
        /// This will speed up the wily map cutscene in between Wily stages by about 2 seconds
        /// </summary>
        /// <param name="p"></param>
        public static void SetFastWilyMap(Patch p)
        {
            // This is the number of frames to wait after drawing the path on the map before fade out.
            // Default value 0x7D (125 frames), change to 0x10.
            p.Add(0x0359B8, 0x10, "Fast Wily Map");
        }

        /// <summary>
        /// This will change the delay defeating a boss and teleporting out of the field to be much shorter.
        /// The victory fanfare will not play, and you teleport out exactly 10 frames after landing the killing
        /// blow on a robot master, and faster for Wily bosses as well. This indirectly fixes the issue of
        /// potentially zipping out of Bubbleman or other robot masters' chambers, since you teleport immediately.
        /// </summary>
        /// <param name="p"></param>
        public static void SetFastBossDefeatTeleport(Patch p)
        {
            // 0x02E0AF: Time until teleport after fanfare starts. ($FD, change to $40)
            // 0x02E0A2: Time until boss-defeat fanfare starts. Note that if set too low without any additional
            //           changes, a softlock may occur after some Wily bosses. Change from $FD to $10, then
            //           modify other areas that set the intial value of $05A7 (address storing our comparison).
            //           It turns out taht Mechadragon, Picopico-kun, and Gutsdozer set the intial value to $70
            //           (at 0x02D16F). Buebeam has its own special routine with extra explosions, setting the 
            //           initial value to $80 (at 0x02D386). Wily Machine and Alien do not call these subroutines
            //           and no further modification is needed.
            // 0x02D170: Wily 1/2/3 boss defeat, time until fanfare starts. ($70, change to $10)
            //           Must be less or equal to value above (at 0x02E0A2)
            // 0x02D386: Buebeam defeat, time until fanfare starts. ($80 change to $10)
            //           Must be less or equal to value above (at 0x02E0A2)
            //
            // The original subroutine that uses 0x02E0A2 is as follows:
            //
            // BossDefeatWaitForTeleport:
            //  0B:A08B: 4E 21 04  LSR $0421    // Not sure what this is for, but it gets zeroed out after a couple loops
            //  0B:A08E: AD A7 05  LDA $05A7    // $05A7 frequently stores a frame-counter or a 'state' value
            //  0B:A091: C9 10     CMP #$FD     // Compare value at $05A7 with 0xFD
            //  0B:A093: B0 04     BCS PlayFanfare_ThenWait  // If value at $05A7 >= 0xFD, jump to PlayFanfare_ThenWait
            //  0B:A095: EE A7 05  INC $05A7    // Increase value at $05A7 by 1
            //  0B:A098: 60        RTS          // Return
            // PlayFanfare_ThenWait:
            //  0B:A099                         // Play fanfare once, then wait to teleport....
            //  ...
            //
            // When defeating Wily 1, 2, 3, or 4, the BossDefeatWaitForTeleport subroutine is entered for the first time
            // with $05A7 having a value of 0x70 or 0x80; if you change the comparison value at $2E0A2 from 0xFD to a
            // value smaller than the intial $05A7, an infinite loop occurs. 

            p.Add(0x02E0AF, 0x40, "Fast Boss Defeat Teleport: Teleport delay after fanfare");
            p.Add(0x02E0A2, 0x10, "Fast Boss Defeat Teleport: Global delay before fanfare");
            p.Add(0x02D170, 0x10, "Fast Boss Defeat Teleport: W1/2/3 boss delay before fanfare");
            p.Add(0x02D386, 0x10, "Fast Boss Defeat Teleport: W4 boss delay before fanfare");

            // Also, NOP out the code that plays the fanfare. It's too distorted sounding when immediately teleporting.
            // Or TODO in the future, change to a different sound?
            //  02E0B3: A9 15      LDA #$15       // Let A = the fanfare sound value (15)
            //  02E0B5: 20 51 C0   JSR PlaySound  // Jump to "PlaySound" function, which plays the value in A
            for (Int32 i = 0; i < 5; i++)
            {
                p.Add(0x02E0B3 + i, 0xEA, "Fast Boss Defeat Teleport: Fanfare sound NOP");
            }
        }

        internal static void DisableScreenFlashing(Patch p, Boolean enableFasterCutsceneText, Boolean enableRandomizationOfColorPalettes)
        {
            p.Add(0x3412E, 0x1F, "Disable Stage Select Flashing");
            p.Add(0x3596D, 0x0F, "Wily Map Flash Color");
            if (!enableFasterCutsceneText)
            {
                // This sequence is disabled by FastText, and the patch conflicts with it.
                p.Add(0x37C98, 0x0F, "Item Get Flash Color");
            }

            p.Add(0x2CA04, 0x0F, "Flash Man Fire Flash Color");
            p.Add(0x2CC7C, 0x0F, "Metal Man Periodic Flash Color");

            p.Add(0x37A1A, 0xEA, "NOP Ending Palette Flash");
            p.Add(0x377A5, 0x00, "Disable Ending Screen Flash");

            // Dragon
            p.Add(0x2D1B2, 0x63, "Dragon Hit Flash Palette Index");
            p.Add(0x2D187, 0x63, "Dragon Hit Restore Palette Index");
            if (!enableRandomizationOfColorPalettes)
            {
                p.Add(0x2D1B0, 0x37, "Dragon Hit Flash Color");
                p.Add(0x2D185, 0x27, "Dragon Hit Restore Color");
            }
            p.Add(0x2D3A0, 0x0F, "Dragon Defeat Flash Color");

            // Guts Tank
            p.Add(0x2D661, 0x5C, "Guts Tank Flash Palette Index");
            // p.Add(0x2D65F, 0x0F, "Guts Tank Flash Color");

            // Wily Machine
            p.Add(0x2DA96, 0x63, "Wily Machine Flash Palette Index");
            p.Add(0x2DA23, 0x63, "Wily Machine Restore Palette Index");
            if (!enableRandomizationOfColorPalettes)
            {
                p.Add(0x2DA94, 0x25, "Wily Machine Flash Color");
                p.Add(0x2DA21, 0x35, "Wily Machine Restore Color");
            }

            // Alien
            p.Add(0x2DC97, 0x0F, "Alien Hit Flash Color");
            p.Add(0x2DD6C, 0x0F, "Alien Defeat Flash Color");
            p.Add(0x2DF1B, 0x0F, "Alien Explision Flash Color");
        }

        public static void SetFastReadyText(Patch p)
        {
            p.Add(0x038147, 0x60, "READY Text Delay");
        }

        /// <summary>
        /// TODO
        /// </summary>
        public static void SetBurstChaser(Patch p)
        {
            p.Add(0x038921, 0x03, "Mega Man Walk X-Velocity Integer");
            p.Add(0x03892C, 0x00, "Mega Man Walk X-Velocity Fraction");
            p.Add(0x038922, 0x03, "Mega Man Air X-Velocity Integer");
            p.Add(0x03892D, 0x00, "Mega Man Air X-Velocity Fraction");
            p.Add(0x0386EF, 0x01, "Mega Man Ladder Climb Up Integer");
            p.Add(0x03872E, 0xFE, "Mega Man Ladder Climb Down Integer");

            //Int32 address = (jVersion) ? 0x07D4A4 : 0x07D4A7;
            Int32 address = 0x07D4A7;
            p.Add(address, 0x08, "Buster Projectile X-Velocity Integer");
        }

        /// <summary>
        /// Skip 3 of the 4 extra pages of text that occur when receiving an item, and only show
        /// the last page "GET EQUIPPED WITH ITEM X"
        /// </summary>
        /// <param name="p"></param>
        public static void SkipItemGetPages(Patch p)
        {
            // At 0x037C88, A62ABD81C24A09A08D2004EE20044CD0BC
            p.Add(0x037C88, 0xA6, "Fast Item Get Patch");
            p.Add(0x037C89, 0x2A, "Fast Item Get Patch");
            p.Add(0x037C8A, 0xBD, "Fast Item Get Patch");
            p.Add(0x037C8B, 0x81, "Fast Item Get Patch");
            p.Add(0x037C8C, 0xC2, "Fast Item Get Patch");
            p.Add(0x037C8D, 0x4A, "Fast Item Get Patch");
            p.Add(0x037C8E, 0x09, "Fast Item Get Patch");
            p.Add(0x037C8F, 0xA0, "Fast Item Get Patch");
            p.Add(0x037C90, 0x8D, "Fast Item Get Patch");
            p.Add(0x037C91, 0x20, "Fast Item Get Patch");
            p.Add(0x037C92, 0x04, "Fast Item Get Patch");
            p.Add(0x037C93, 0xEE, "Fast Item Get Patch");
            p.Add(0x037C94, 0x20, "Fast Item Get Patch");
            p.Add(0x037C95, 0x04, "Fast Item Get Patch");
            p.Add(0x037C96, 0x4C, "Fast Item Get Patch");
            p.Add(0x037C97, 0xD0, "Fast Item Get Patch");
            p.Add(0x037C98, 0xBC, "Fast Item Get Patch");
        }

        /// <summary>
        /// Enabling Random Weapons or Random Stages will cause the wrong Robot Master portrait to
        /// be blacked out when a stage is completed. The game uses your acquired weapons to determine
        /// which portrait to black-out. This function changes the lookup table for x and y positions
        /// of portraits to black-out based on what was randomized.
        /// </summary>
        public static void FixPortraits(Patch Patch, Boolean is8StagesRandom, RStages randomStages, Boolean isWeaponGetRandom, RWeaponGet randomWeaponGet)
        {
            // Arrays of default values for X and Y of the black square that marks out each portrait
            // Index of arrays are stage order, e.g. Heat, Air, etc.
            // Note: It's terrible to be repeating the boss list here, but at least this way
            // the connection between each boss and their offset is explicit, which would not be the
            // case if we used the EBossIndex.RobotMasters property instead.
            Dictionary<EBossIndex, Byte> portraitBG_y = new()
            {
                { EBossIndex.Heat, 0x21 },
                { EBossIndex.Air, 0x20 },
                { EBossIndex.Wood, 0x21 },
                { EBossIndex.Bubble, 0x20 },
                { EBossIndex.Quick, 0x20 },
                { EBossIndex.Flash, 0x22 },
                { EBossIndex.Metal, 0x22 },
                { EBossIndex.Crash, 0x22 },
            };
            Dictionary<EBossIndex, Byte> portraitBG_x = new()
            {
                { EBossIndex.Heat, 0x86 },
                { EBossIndex.Air, 0x8E },
                { EBossIndex.Wood, 0x96 },
                { EBossIndex.Bubble, 0x86 },
                { EBossIndex.Quick, 0x96 },
                { EBossIndex.Flash, 0x8E },
                { EBossIndex.Metal, 0x86 },
                { EBossIndex.Crash, 0x96 },
            };

            // Adjusting the sprites is not necessary because the hacked portrait graphics ("?" images)
            // only use the background, and the sprites have been blacked out. Left in for reference.
            //Byte[] portraitSprite_x = new Byte[] { 0x3C, 0x0C, 0x4C, 0x00, 0x20, 0x84, 0x74, 0xA4 };
            //Byte[] portraitSprite_y = new Byte[] { 0x10, 0x14, 0x28, 0x0C, 0x1C, 0x20, 0x10, 0x18 };

            // Apply changes to portrait arrays based on shuffled stages
            if (is8StagesRandom)
            {
                randomStages.FixPortraits(ref portraitBG_x, ref portraitBG_y);
            }

            // Apply changes to portrait arrays based on shuffled weapons. Only need a standard "if" with no "else",
            // because the arrays must be permuted twice if both randomization settings are enabled.
            if (isWeaponGetRandom)
            {
                randomWeaponGet.FixPortraits(ref portraitBG_x, ref portraitBG_y);
            }

            foreach (EBossIndex i in EBossIndex.RobotMasters)
            {
                Byte y = portraitBG_y[i];
                Byte x = portraitBG_x[i];
                Patch.Add(0x034541 + i.Offset, y, $"Stage Select Portrait {i.Offset + 1} Y-Pos Fix");
                Patch.Add(0x034549 + i.Offset, x, $"Stage Select Portrait {i.Offset + 1} X-Pos Fix");
                // Changing this sprite table misplaces their positions by default.
                //stream.Position = 0x03460D + i;
                //stream.WriteByte(portraitSprite_y[i]);
                //stream.Position = 0x034615 + i;
                //stream.WriteByte(portraitSprite_x[i]);
            }
        }

        // TODO;
        public static void FixWeaponLetters(Patch Patch, RWeaponGet randomWeaponGet, RStages randomStages, RText rText)
        {
            Dictionary<EWeaponIndex, EWeaponIndex> shuffledWeapons = randomWeaponGet
                .GetShuffleIndexPermutation()
                .ToDictionary(x => x.Key.ToWeaponIndex(), x => x.Value.ToWeaponIndex());
            rText.FixWeaponLetters(Patch, shuffledWeapons);
        }

        /// <summary>
        /// No longer needed since press is included in enemy damage rando table
        /// </summary>
        public static void EnablePressDamage(Patch Patch)
        {
            Patch.Add(EDmgVsEnemy.DamageP + EDmgVsEnemy.Offset.Press, 0x01, "Buster Damage Against Press");
        }

        public static void FixM445PaletteGlitch(Patch p)
        {
            for (Int32 i = 0; i < 3; i++)
            {
                p.Add(0x395BD + i, 0xEA, "M-445 Palette Glitch Fix");
            }
        }

        /// <summary>
        /// Manual tuning of specific enemy damage values on top of vanilla MM2.
        /// </summary>
        /// <param name="p"></param>
        public static void NerfDamageValues(Patch p)
        {
            p.Add(0x7ED6C + 0x61, 0x04, "Woodman's Leaf Shield Attack Nerf");
        }

        public static void DisableChangkeyMakerPaletteSwap(Patch p)
        {
            // Stop palette change when enemy appears
            // $3A4F6 > 0E:A4E6: 20 59 F1 JSR $F159
            // Change to 4C 55 A5 (JMP $A555, which returns immediately) 
            p.Add(0x3A4F6, 0x4C, "Disable Changkey Maker palette swap 1");
            p.Add(0x3A4F7, 0x55, "Disable Changkey Maker palette swap 1");
            p.Add(0x3A4F8, 0xA5, "Disable Changkey Maker palette swap 1");

            // Stop palette change on kill/despawn:
            // $3A562 > 0E:A552: 20 59 F1 JSR $F159
            // Change to EA EA EA (NOP)
            p.Add(0x3A562, 0xEA, "Disable Changkey Maker palette swap 2");
            p.Add(0x3A563, 0xEA, "Disable Changkey Maker palette swap 2");
            p.Add(0x3A564, 0xEA, "Disable Changkey Maker palette swap 2");
        }

        /// <summary>
        /// Prevents E-Tank use when MegaMan has full life.
        /// </summary>
        /// <param name="p">Patch to apply the data to.</param>
        public static void PreventETankUseAtFullLife(Patch p)
        {
            // Original E-Tank Menu Command begins at 0D:9281:
            // $9281: Menu Page and Menu Position Checking.        
            // $9292:A5 A7     LDA $00A7 ;$00A7 is ETankCount
            // $9294:F0 DE     BEQ $9274 ;Return if ETankCount == 0          
            // $9296:C6 A7     DEC $00A7 ;Decrement ETankCount
            // $9298:AD C0 06  LDA $06C0 ;$06C0 is Life
            // $929B:C9 1C     CMP #$1C
            // $929D:F0 D5     BEQ $9274 ;Return if Life == 28
            // while (Life != 28)
            // {
            //      $929F:A5 1C     LDA $001C ;$001C is a FrameCounter.
            //      $92A1:29 03     AND #$03
            //      $92A3:D0 08     BNE $92AD ;if FrameCounter % 4 != 0 JMP $92AD
            //      if(FrameCounter % 4 == 0)
            //      {
            //           $92A5: EE C0 06 INC Life  ;Raise Life by 1.
            //           $92A8: A9 28    LDA #$28
            //           $92AA: 20 51 C0 JSR $C051 ;Play Life Gain Sound
            //      }
            //
            //      ; Not sure what the next 2 commands are doing.
            //      ; Seem like part of the reglar game/draw loop since FrameCounter is updated.
            //      $92AD: 20 96 93 JSR $9396
            //      $92B0: 20 AB C0 JSR $C0AB ;Wait for next frame and update controller
            //      $92B3: 4C 98 92 JMP $9298 ;Loop while (Life != 28)
            // }

            Int32 prgOffset = 0x30010 - 0x4000;
            Int32 patchLocation = 0x9296 + prgOffset;
            Byte[] patchBytes = new Byte[]
            {
                0xAD, 0xC0, 0x06,   // LDA $6C0 ; Do not proceed if life is full
                0xC9, 0x1C,         // CMP #$1C
                0xF0, 0xD7,         // BEQ $9274

                0xC6, 0xA7,         // DEC $A7 ; Decrement e-tanks
            
                0xA5, 0x1C,         // LDA $1C ; 929F: Load frame counter
                0x29, 0x03,         // AND #$3 ; If multiple of 4 frames increase life - THIS WILL BE PATCHED OVER

                0x20, 0x77, 0xBF,   // JSR $BF77 ; Call code that wouldn't fit here
                0x20, 0x96, 0x93,   // JSR $9396 ; Do mostly ordinary stuff
                0x20, 0xAB, 0xC0,   // JSR $C0AB

                0xAD, 0xC0, 0x06,   // LDA $6C0 ; If life not full loop, else done
                0xC9, 0x1C,         // CMP #$1C
                0xF0, 0xC1,         // BEQ $9274
                0x4C, 0x9F, 0x92,   // JMP $929F
            };

            p.Add(patchLocation, patchBytes, "Prevent E-Tank Use at Full Life");

            Byte[] eTankSubroutineBytes = new Byte[]
            {
                0xD0, 0x03,         // BNE $BF7C ; Skip life increase if nonzero

                0xEE, 0xC0, 0x06,   // INC $6C0 ; Increase life

                0xA5, 0x1C,         // LDA $1C ; BF7C: If multiple of 4 frames...
                0x29, 0x03,         // AND #$3
                0xD0, 0x05,         // BNE $BF87

                0xA9, 0x28,         // LDA #$28 ; ...play life filling sound
                0x4C, 0x51, 0xC0,   // JMP $C051

                0x60,               // RTS ; BF87
            };

            // Start at 0D:BF77 (should be 0x37F87).
            Int32 etankSubLocation = 0xBF77 + prgOffset;
            p.Add(etankSubLocation, eTankSubroutineBytes, "Prevent E-Tank Use at Full Life");
        }

        public static void MakeStageSelectDefault(Patch patch)
        {
            patch.Add(0x37bea, 1, "Set stage select to default option");
        }

        /// <summary>
        /// Load the IPS for the specified Mega Man player sprite, or null if the specified sprite is the default. Throws FileNotFoundException if the sprite cannot be found (should never happen).
        /// </summary>
        private static byte[]? LoadMegaManSpriteIps(
            ResourceTree resTree,
            PlayerSpriteOption sprite)
        {
            string spriteName = sprite.ToString();
            var enumInfo = sprite.GetType().GetMember(spriteName).First();
            var pathAttr = enumInfo.GetCustomAttribute<PlayerSpritePathAttribute>();
            var dirAttr = enumInfo.GetCustomAttribute<PlayerSpriteParentDirectoryAttribute>();

            var relRoot = resTree.Find("SpritePatches.Characters");
            ResourceNode? tgtNode = null;
            if (pathAttr is not null)
            {
                //// Does this code even work? It's not used and hasn't been tested.
                if (pathAttr.Path == "")
                    return null;

                string searchStr = $".{pathAttr.Path}.ips";
                foreach (var node in relRoot.Descendants.Where(n => n.IsFile))
                {
                    if (string.Compare(
                        node.Path.Substring(relRoot.Path.Length),
                        searchStr,
                        StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        tgtNode = node;
                        break;
                    }
                }
            }
            else
            {
                var dirNode = relRoot.Find(dirAttr?.Name ?? spriteName);
                tgtNode = dirNode.Files.FirstOrDefault(n => string.Compare(
                    n.Name, 
                    $"{spriteName}.ips", 
                    StringComparison.InvariantCultureIgnoreCase) == 0);
                if (tgtNode is null)
                    tgtNode = dirNode.Files.FirstOrDefault(n => string.Compare(
                        n.Name,
                        $"PlayerCharacter_{spriteName}.ips",
                        StringComparison.InvariantCultureIgnoreCase) == 0);
            }

            if (tgtNode is null)
                throw new FileNotFoundException();

            return resTree.LoadResource(tgtNode);
        }

        /// <summary>
        /// Replace the player's sprite graphics with a different sprite.
        /// This method applies the graphics patch directly to the ROM at
        /// tempFileName. If 'MegaMan' is the sprite, no patch is applied.
        /// </summary>
        public static void SetNewMegaManSprite(
            ResourceTree resTree,
            Patch p, 
            String tempFileName, 
            PlayerSpriteOption sprite)
        {
#if DEBUG
            // Verify all sprites work
            foreach (var spriteValue in Enum.GetValues<PlayerSpriteOption>())
                LoadMegaManSpriteIps(resTree, spriteValue);
#endif

            var ips = LoadMegaManSpriteIps(resTree, sprite);
            if (ips is not null)
                p.ApplyIPSPatch(tempFileName, ips);
        }

        /// <summary>
        /// Loads the IPS patch corresponding to the specified enum value. Throws FileNotFoundException if the specified patch cannot be found (should never happen).
        /// </summary>
        /// <param name="basePath">The base path of which all options are descendants.</param>
        /// <param name="fallbackPrefix">If a resource of the form $"{basePath}.{value.ToString()}.ips" cannot be found, try $"{basePath}.{fallbackPrefix}{value.ToString()}.ips".</param>
        private static byte[] LoadEnumBasedIps<TEnum>(
            ResourceTree resTree, 
            string basePath,
            string? fallbackPrefix,
            TEnum value)
            where TEnum : struct, Enum
        {
            var relRoot = resTree.Find(basePath);
            string valueName = value.ToString();
            var node = relRoot.Files.FirstOrDefault(n => string.Compare(
                n.Name,
                $"{valueName}.ips",
                StringComparison.InvariantCultureIgnoreCase) == 0);
            if (node is null && fallbackPrefix is not null)
                node = relRoot.Files.FirstOrDefault(n => string.Compare(
                    n.Name,
                    $"{fallbackPrefix}{valueName}.ips",
                    StringComparison.InvariantCultureIgnoreCase) == 0);

            if (node is null)
                throw new FileNotFoundException();

            return resTree.LoadResource(node);
        }

        /// <summary>
        /// Applies the IPS patch corresponding to the specified enum value. Throws FileNotFoundException if the specified patch cannot be found (should never happen).
        /// </summary>
        /// <param name="p">The patcher.</param>
        /// <param name="tempFileName">The filename to apply the patch to.</param>
        /// <param name="basePath">The base path of which all options are descendants.</param>
        /// <param name="fallbackPrefix">If a resource of the form $"{basePath}.{value.ToString()}.ips" cannot be found, try $"{basePath}.{fallbackPrefix}{value.ToString()}.ips".</param>
        /// <param name="defaultValue">The value of TEnum which corresponds to no patch being applied.</param>
        private static void ApplyEnumBasedIps<TEnum>(
            ResourceTree resTree,
            Patch p,
            String tempFileName,
            string basePath,
            string? fallbackPrefix,
            TEnum? defaultValue,
            TEnum value)
            where TEnum : struct, Enum
        {
#if DEBUG
            foreach (var testValue in Enum.GetValues<TEnum>())
                if (defaultValue is null 
                    || !testValue.Equals(defaultValue))
                    LoadEnumBasedIps(
                        resTree, 
                        basePath,
                        fallbackPrefix,
                        testValue);
#endif

            if (defaultValue is not null && value.Equals(defaultValue))
                return;

            var ips = LoadEnumBasedIps(
                resTree, basePath, fallbackPrefix, value);
            p.ApplyIPSPatch(tempFileName, ips);
        }

        /// <summary>
        /// Replace the HUD elements in the game with different sprites.
        /// This method applies the graphics patch directly to the ROM at
        /// tempFileName. If 'Default' is the HUD element, no patch is applied.
        /// </summary>
        public static void SetNewHudElement(
            ResourceTree resTree, 
            Patch p, 
            String tempFileName, 
            HudElementOption hudElement)
        {
            ApplyEnumBasedIps(resTree,
                p,
                tempFileName,
                "SpritePatches.HudElements",
                "HudElements_",
                HudElementOption.Default,
                hudElement);
        }


        /// <summary>
        /// Replace the font in the game with different sprites.
        /// This method applies the graphics patch directly to the ROM at
        /// tempFileName. If 'Default' is the font , no patch is applied.
        /// </summary>
        public static void SetNewFont(
            ResourceTree resTree,
            Patch p, 
            String tempFileName, 
            FontOption font)
        {
            ApplyEnumBasedIps(resTree,
                p,
                tempFileName,
                "SpritePatches.Fonts",
                "Font_",
                FontOption.Default,
                font);
        }


        /// <summary>
        /// Reduces lag in various places (underwater, end of boss fight, and possibly other places) by disabling a subroutine
        /// that just delays until an NMI occurs.
        /// </summary>
        /// <param name="p"></param>
        public static void ReduceUnderwaterLag(Patch p)
        {
            p.Add((Int32)ESubroutineAddress.WasteAFrame, Opcode6502.RTS, "Turn the 'waste a frame' subroutine into a NOP");
        }

        /// <summary>
        /// This method will modify the game loop to spawn weapon energy
        /// pickups in Wily 5.
        /// 
        ///
        /// At 0x0E:816A (bank E, 0x3817A in the ROM), there is a CMP
        /// instruction to value 0x0C (CMP #0x0C). This is checking if the
        /// current stage is Wily 5 (0x0C).  If the current stage is Wily 5,
        /// it jumps to a special game loop at 0x0E:8223 (bank E, 0x38233
        /// in the ROM), which has a special setup routine for the teleport
        /// room, otherwise, it jumps to 0x0E:8171, which is the normal game loop.
        /// 
        /// The special Wily 5 game loop does not include the call instruction to 
        /// the subroutine that spawns items or enemies, but the loop itself is 
        /// otherwise identical to the base game loop.
        /// 
        /// This method changes the jump to 0x0E:8223 to call the subroutine 
        /// that sets up the teleporters. Because of the convenient ordering 
        /// of instructions, this function will return directly to 0x0E:8171
        /// - the start of the normal game loop - exactly as desired. As a 
        /// result, 0x0E:8223-8267 is now unused space that may be otherwise used.
        /// </summary>
        public static void AddWily5SubroutineWithItemSpawns(Patch p)
        {
            Byte[] newJsrFor0x816E = new Byte[]
            {
                Opcode6502.JSR, 0xDE, 0x81
            };

            const Int32 AddressOfInitialJump = 0x3817E;

            p.Add(AddressOfInitialJump, newJsrFor0x816E);
        }

        public static void AddLargeWeaponEnergyRefillPickupsToWily5TeleporterRoom(Patch p)
        {
            // The enemy and item spawn information will be made into a data
            // structure that can be written to the ROM in logical chunks, but,
            // in the interest of time, these values are being manually added
            // to the addresses required.

            // Each stage in the game gets a total of 256 respawnable
            // items/enemies and 64 spawn-once items/enemies.
            // That is (256 * 4) + (64 * 4) == 1,280 bytes per stage.
            //
            // NOTE: pairs of stages share the same data allocation.
            //
            // Stage data starts at address 0x3610 in the ROM.
            //
            // The sequence per stage is as follows:
            //  256 bytes for respawnable item (enemies) screen locations.
            //  256 bytes for respawnable item (enemies) x-coordinate positions.
            //  256 bytes for respawnable item (enemies) y-coordinate positions.
            //  256 bytes for respawnable item (enemies) IDs.
            //  64 bytes for spawn-once item screen locations.
            //  64 bytes for spawn-once item x-coordinate positions.
            //  64 bytes for spawn-once item y-coordinate positions.
            //  64 bytes for spawn-once item IDs.

            const Byte LargeWeaponEnergyRefillType = 0x78;

            // Weapon energy for the initial teleporter room
            const Int32 LargeWeaponEnergyRefill1_EnemyRoomNumberIndicatorAddress = 0x00013626;
            const Int32 LargeWeaponEnergyRefill2_EnemyRoomNumberIndicatorAddress = 0x00013627;

            const Byte LargeWeaponEnergyRefill1_RoomNumber = 0x18;
            const Byte LargeWeaponEnergyRefill2_RoomNumber = 0x18;

            const Int32 LargeWeaponEnergyRefill1_PositionXAddress = 0x00013726;
            const Int32 LargeWeaponEnergyRefill2_PositionXAddress = 0x00013727;

            const Byte LargeWeaponEnergyRefill1_PositionX = 0x58;
            const Byte LargeWeaponEnergyRefill2_PositionX = 0xA8;

            const Int32 LargeWeaponEnergyRefill1_PositionYAddress = 0x00013826;
            const Int32 LargeWeaponEnergyRefill2_PositionYAddress = 0x00013827;

            const Byte LargeWeaponEnergyRefill1_PositionY = 0x69;
            const Byte LargeWeaponEnergyRefill2_PositionY = 0x69;

            const Int32 LargeWeaponEnergyRefill1_TypeAddress = 0x00013926;
            const Int32 LargeWeaponEnergyRefill2_TypeAddress = 0x00013927;

            p.Add(LargeWeaponEnergyRefill1_EnemyRoomNumberIndicatorAddress, LargeWeaponEnergyRefill1_RoomNumber);
            p.Add(LargeWeaponEnergyRefill2_EnemyRoomNumberIndicatorAddress, LargeWeaponEnergyRefill2_RoomNumber);

            p.Add(LargeWeaponEnergyRefill1_PositionXAddress, LargeWeaponEnergyRefill1_PositionX);
            p.Add(LargeWeaponEnergyRefill2_PositionXAddress, LargeWeaponEnergyRefill2_PositionX);

            p.Add(LargeWeaponEnergyRefill1_PositionYAddress, LargeWeaponEnergyRefill1_PositionY);
            p.Add(LargeWeaponEnergyRefill2_PositionYAddress, LargeWeaponEnergyRefill2_PositionY);

            p.Add(LargeWeaponEnergyRefill1_TypeAddress, LargeWeaponEnergyRefillType);
            p.Add(LargeWeaponEnergyRefill2_TypeAddress, LargeWeaponEnergyRefillType);


            // Weapon energy for the teleporter room with Wily Machine teleporter
            const Int32 LargeWeaponEnergyRefill3_EnemyRoomNumberIndicatorAddress = 0x00013628;
            const Int32 LargeWeaponEnergyRefill4_EnemyRoomNumberIndicatorAddress = 0x00013629;

            const Byte LargeWeaponEnergyRefill3_RoomNumber = 0x28;
            const Byte LargeWeaponEnergyRefill4_RoomNumber = 0x28;

            const Int32 LargeWeaponEnergyRefill3_PositionXAddress = 0x00013728;
            const Int32 LargeWeaponEnergyRefill4_PositionXAddress = 0x00013729;

            const Byte LargeWeaponEnergyRefill3_PositionX = 0x58;
            const Byte LargeWeaponEnergyRefill4_PositionX = 0xA8;

            const Int32 LargeWeaponEnergyRefill3_PositionYAddress = 0x00013828;
            const Int32 LargeWeaponEnergyRefill4_PositionYAddress = 0x00013829;

            const Byte LargeWeaponEnergyRefill3_PositionY = 0x69;
            const Byte LargeWeaponEnergyRefill4_PositionY = 0x69;

            const Int32 LargeWeaponEnergyRefill3_TypeAddress = 0x00013928;
            const Int32 LargeWeaponEnergyRefill4_TypeAddress = 0x00013929;


            p.Add(LargeWeaponEnergyRefill3_EnemyRoomNumberIndicatorAddress, LargeWeaponEnergyRefill3_RoomNumber);
            p.Add(LargeWeaponEnergyRefill4_EnemyRoomNumberIndicatorAddress, LargeWeaponEnergyRefill4_RoomNumber);

            p.Add(LargeWeaponEnergyRefill3_PositionXAddress, LargeWeaponEnergyRefill3_PositionX);
            p.Add(LargeWeaponEnergyRefill4_PositionXAddress, LargeWeaponEnergyRefill4_PositionX);

            p.Add(LargeWeaponEnergyRefill3_PositionYAddress, LargeWeaponEnergyRefill3_PositionY);
            p.Add(LargeWeaponEnergyRefill4_PositionYAddress, LargeWeaponEnergyRefill4_PositionY);

            p.Add(LargeWeaponEnergyRefill3_TypeAddress, LargeWeaponEnergyRefillType);
            p.Add(LargeWeaponEnergyRefill4_TypeAddress, LargeWeaponEnergyRefillType);
        }

        public static void EnableLeftwardWallEjection(ResourceTree resTree, Patch p, String tempFileName)
        {
            p.ApplyIPSPatch(tempFileName, resTree.LoadResource("leftwardwallejectionpatch.ips"));
        }

        public static void DisablePauseLock(ResourceTree resTree, Patch p, String tempFileName)
        {
            p.ApplyIPSPatch(tempFileName, resTree.LoadResource("pausepatch.ips"));
        }

        public static void EnableMercilessMode(ResourceTree resTree, Patch p, String tempFileName)
        {
            p.ApplyIPSPatch(tempFileName, resTree.LoadResource("mercilesspatch.ips"));
        }

        public static void EnableBirdEggFix(ResourceTree resTree, Patch p, String tempFileName)
        {
            p.ApplyIPSPatch(tempFileName, resTree.LoadResource("mm2bird_egg_fix.ips"));
        }
    }
}
