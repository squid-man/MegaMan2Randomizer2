using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;

namespace MM2Randomizer.Randomizers
{
    public class RWeaknesses : IRandomizer
    {
        // Robot Master damage table. If RWeaknesses module is not enabled, these default values will be used.
        //           P H A W B F Q M C
        // Heatman   2 0 2 0 6 0 2 1 0
        // Airman    2 6 0 8 0 0 2 0 0
        // Woodman   1 A 4 0 0 0 0 2 2
        // Bubbleman 1 0 0 0 0 0 2 4 2
        // Quickman  2 A 2 0 0 1 0 0 4
        // Flashman  2 6 0 0 2 0 0 4 3
        // Metalman  1 4 0 0 0 0 4 A 0
        // Crashman  1 6 A 0 1 0 1 0 0
        public static readonly Dictionary<EBossIndex, Dictionary<EWeaponIndex, Int32>> BotWeaknesses = new()
        {
            {
                EBossIndex.Heat, new(){
                    { EWeaponIndex.Buster, 2 },
                    { EWeaponIndex.Heat, 0 },
                    { EWeaponIndex.Air, 2 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 6 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Quick, 2 },
                    { EWeaponIndex.Metal, 1 },
                    { EWeaponIndex.Crash, 0 },
                }
            },
            {
                EBossIndex.Air, new() {
                    { EWeaponIndex.Buster, 2 },
                    { EWeaponIndex.Heat, 6 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 8 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Quick, 2 },
                    { EWeaponIndex.Metal, 0 },
                    { EWeaponIndex.Crash, 0 },
                }
            },
            {
                EBossIndex.Wood, new() {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 10 },
                    { EWeaponIndex.Air, 4 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Quick, 0 },
                    { EWeaponIndex.Metal, 2 },
                    { EWeaponIndex.Crash, 2 },
                }
            },
            {
                EBossIndex.Bubble, new() {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 0 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Quick, 2 },
                    { EWeaponIndex.Metal, 4 },
                    { EWeaponIndex.Crash, 2 },
                }
            },
            {
                EBossIndex.Quick, new() {
                    { EWeaponIndex.Buster, 2 },
                    { EWeaponIndex.Heat, 10 },
                    { EWeaponIndex.Air, 2 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Flash, 1 },
                    { EWeaponIndex.Quick, 0 },
                    { EWeaponIndex.Metal, 0 },
                    { EWeaponIndex.Crash, 4 },
                }
            },
            {
                EBossIndex.Flash, new() {
                    { EWeaponIndex.Buster, 2 },
                    { EWeaponIndex.Heat, 6 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 2 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Quick, 0 },
                    { EWeaponIndex.Metal, 4 },
                    { EWeaponIndex.Crash, 3 },
                }
            },
            {
                EBossIndex.Metal, new() {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 4 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Quick, 4 },
                    { EWeaponIndex.Metal, 10 },
                    { EWeaponIndex.Crash, 0 },
                }
            },
            {
                EBossIndex.Crash, new() {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 6 },
                    { EWeaponIndex.Air, 10 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 1 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Quick, 1 },
                    { EWeaponIndex.Metal, 0 },
                    { EWeaponIndex.Crash, 0 },
                }
            },
        };

        // Wily boss damage table. If module not enabled, these defaults will be used. Flash ignored.
        //        P H A W B Q M C
        // Dragon 1 8 0 0 0 1 0 1
        // Pico   1 3 0 0 A 7 7 0
        // Guts   1 8 0 0 1 2 0 1
        // Bueb   0 0 0 0 0 0 0 B
        // WilyM  1 E 1 0 0 1 1 4
        // Alien  X X X X 1 X X X
        public static readonly Dictionary<EBossIndex, Dictionary<EWeaponIndex, Int32>> WilyWeaknesses = new()
        {
             {
                EBossIndex.Dragon, new () {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 8 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Quick, 1 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Metal, 0 },
                    { EWeaponIndex.Crash, 1 },
                }
            },
            {
                EBossIndex.Pico, new() {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 3 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 10 },
                    { EWeaponIndex.Quick, 7 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Metal, 7 },
                    { EWeaponIndex.Crash, 0 },
                }
            },
            {
                EBossIndex.Guts, new() {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 8 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 1 },
                    { EWeaponIndex.Quick, 2 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Metal, 0 },
                    { EWeaponIndex.Crash, 1 },
                }
            },
            {
                EBossIndex.Boobeam, new() {
                    { EWeaponIndex.Buster, 0 },
                    { EWeaponIndex.Heat, 0 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Quick, 0 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Metal, 0 },
                    { EWeaponIndex.Crash, 11 },
                }
            },
            {
                EBossIndex.Machine, new() {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 14 },
                    { EWeaponIndex.Air, 1 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Quick, 1 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Metal, 1 },
                    { EWeaponIndex.Crash, 4 },
                }
            },
            {
                EBossIndex.Alien, new() {
                    { EWeaponIndex.Buster, 255 },
                    { EWeaponIndex.Heat, 255 },
                    { EWeaponIndex.Air, 255 },
                    { EWeaponIndex.Wood, 255 },
                    { EWeaponIndex.Bubble, 1 },
                    { EWeaponIndex.Quick, 255 },
                    { EWeaponIndex.Flash, 255 },
                    { EWeaponIndex.Metal, 255 },
                    { EWeaponIndex.Crash, 255 },
                }
            },
        };

        private readonly Dictionary<EBossIndex, Dictionary<EWeaponIndex, Char>> WilyWeaknessInfo = new()
        {
            {
                EBossIndex.Dragon, new()
            },
            {
                EBossIndex.Pico, new()
            },
            {
                EBossIndex.Guts, new()
            },
            {
                EBossIndex.Boobeam, new()
            },
            {
                EBossIndex.Machine, new()
            },
            {
                EBossIndex.Alien, new()
            },
        };

        private StringBuilder debug = new StringBuilder();
        public override String ToString()
        {
            return debug.ToString();
        }

        public RWeaknesses() { }

        public void Randomize(Patch in_Patch, RandomizationContext in_Context)
        {
            debug = new StringBuilder();
            RandomizeU(in_Patch, in_Context.Seed);
            RandomizeWilyUJ(in_Patch, in_Context.Seed);
        }

        /// <summary>
        /// Identical to RandomWeaknesses() but using Mega Man 2 (U).nes offsets
        /// </summary>
        private void RandomizeU(Patch in_Patch, ISeed in_Seed)
        {
            Dictionary<EWeaponIndex, EDmgVsBoss> bossPrimaryWeaknessAddresses = EDmgVsBoss.GetTables(includeBuster: false, includeTimeStopper: true);
            Dictionary<EBossIndex, EDmgVsBoss> bossWeaknessShuffled = bossPrimaryWeaknessAddresses
                .ToDictionary(x => x.Key.ToBossIndex(), x => x.Value);
            bossWeaknessShuffled = in_Seed.Shuffle(bossWeaknessShuffled);

            // Preparation: Disable redundant Atomic Fire healing code
            // (Note that 0xFF in any weakness table is sufficient to heal a boss)
            in_Patch.Add(0x02E66D, 0xFF, "Atomic Fire Boss To Heal" ); // Normally "00" to indicate Heatman.

            List<EBossIndex> bossIndexes = BotWeaknesses.Keys.ToList();

            // Select 4 robots to be weak against Buster
            List<EBossIndex> busterList = in_Seed.Shuffle(new List<EBossIndex>(bossIndexes)).ToList().GetRange(0, 4);

            // Select 2 robots to be very weak to some weapon
            List<EBossIndex> veryWeakBots = in_Seed.Shuffle(new List<EBossIndex>(bossIndexes)).ToList().GetRange(0, 2);
            EBossIndex bossWithGreatWeakness = veryWeakBots[0];
            EBossIndex bossWithUltimateWeakness = veryWeakBots[1];

            // Select 2 weapons to deal great damage to the 2 bosses above (exclude buster, flash)
            EWeaponIndex[] weaponsExcludingBusterAndFlash = new EWeaponIndex[]
            {
                EWeaponIndex.Heat,
                EWeaponIndex.Air,
                EWeaponIndex.Wood,
                EWeaponIndex.Bubble,
                EWeaponIndex.Quick,
                EWeaponIndex.Metal,
                EWeaponIndex.Crash
            };
            List<EWeaponIndex> greatWeaknessWeapons = in_Seed.Shuffle(weaponsExcludingBusterAndFlash).ToList().GetRange(0, 2);
            EWeaponIndex weaponGreatWeakness = greatWeaknessWeapons[0];
            EWeaponIndex weaponUltimateWeakness = greatWeaknessWeapons[1];


            // Foreach boss
            foreach (EBossIndex i in bossIndexes)
            {
                // First, fill in special weapon tables with a 50% chance to block or do 1 damage
                foreach(EWeaponIndex j in bossPrimaryWeaknessAddresses.Keys)
                {
                    Double rTestImmune = in_Seed.NextDouble();
                    Byte damage = 0;
                    if (rTestImmune > 0.5)
                    {
                        if (bossPrimaryWeaknessAddresses[j] == EDmgVsBoss.U_DamageH)
                        {
                            // ...except for Atomic Fire, which will do some more damage
                            damage = (Byte)(RWeaponBehavior.GetAmmoUsage(EWeaponIndex.Heat) / 2);
                        }
                        else if (bossPrimaryWeaknessAddresses[j] == EDmgVsBoss.U_DamageF)
                        {
                            damage = 0x00;
                        }
                        else
                        {
                            damage = 0x01;
                        }
                    }
                    in_Patch.Add(bossPrimaryWeaknessAddresses[j] + i.Offset, damage, String.Format("{0} Damage to {1}", bossPrimaryWeaknessAddresses[j].WeaponName, (EDmgVsBoss.Offset)i));
                    BotWeaknesses[i][j] = damage;
                }

                // Write the primary weakness for this boss
                Byte dmgPrimary = RWeaknesses.GetRoboDamagePrimary(in_Seed, bossWeaknessShuffled[i]);
                in_Patch.Add(bossWeaknessShuffled[i] + i.Offset, dmgPrimary, $"{bossWeaknessShuffled[i].WeaponName} Damage to {(EDmgVsBoss.Offset)i} (Primary)");

                // Write the secondary weakness for this boss (next element in list)
                // Secondary weakness will either do 2 damage or 4 if it is Atomic Fire
                // Time Stopper cannot be a secondary weakness. Instead it will heal that boss.
                // As a result, one Robot Master will not have a secondary weakness
                EBossIndex i2 = i.NextBoss(bossWeaknessShuffled.Keys.ToList());
                EDmgVsBoss weakWeap2 = bossWeaknessShuffled[i2];
                Byte dmgSecondary = 0x02;
                if (weakWeap2 == EDmgVsBoss.U_DamageH)
                {
                    dmgSecondary = 0x04;
                }
                else if (weakWeap2 == EDmgVsBoss.U_DamageF)
                {
                    dmgSecondary = 0x00;

                    // Address in Time-Stopper code that normally heals Flashman, change to heal this boss instead
                    in_Patch.Add(0x02C08F, (Byte)i.Offset, $"Time-Stopper Heals {(EDmgVsBoss.Offset)i} (Special Code)");
                }
                in_Patch.Add(weakWeap2 + i.Offset, dmgSecondary, $"{weakWeap2.WeaponName} Damage to {(EDmgVsBoss.Offset)i} (Secondary)");

                // Add buster damage
                {
                    Byte busterDmg = (Byte)(busterList.Contains(i) ? 2 : 1);
                    in_Patch.Add(EDmgVsBoss.U_DamageP + i.Offset, busterDmg, $"Buster Damage to {(EDmgVsBoss.Offset)i}");
                    BotWeaknesses[i][EWeaponIndex.Buster] = busterDmg;
                }

                // Save info
                EWeaponIndex weapIndexPrimary = bossWeaknessShuffled[i].Index;
                BotWeaknesses[i][weapIndexPrimary] = dmgPrimary;
                EWeaponIndex weapIndexSecondary = weakWeap2.Index;
                BotWeaknesses[i][weapIndexSecondary] = dmgSecondary;

                // Independently, apply a great weakness and an ultimate weakness (potentially overriding a previous weakness)
                if (bossWithGreatWeakness == i)
                {
                    // Great weakness. Can't be Buster or Flash. Deal 7 damage.
                    EDmgVsBoss wpn = EDmgVsBoss.GetTables(false, false)[weaponGreatWeakness];
                    in_Patch.Add(wpn.Address + i.Offset, 0x07, $"{wpn.WeaponName} Damage to {(EDmgVsBoss.Offset)i} (Great)");
                    BotWeaknesses[i][wpn.Index] = 0x07;
                }
                else if (bossWithUltimateWeakness == i)
                {
                    // Ultimate weakness. Can't be Buster or Flash. Deal 10 damage.
                    EDmgVsBoss wpn = EDmgVsBoss.GetTables(false, false)[weaponUltimateWeakness];
                    in_Patch.Add(wpn.Address + i.Offset, 0x0A, $"{wpn.WeaponName} Damage to {(EDmgVsBoss.Offset)i} (Ultimate)");
                    BotWeaknesses[i][wpn.Index] = 0x0A;
                }
            }

            // TODO: Fix this debug output, it's incorrect. It corresponds to the stages, not bosses. Needs
            // to be permuted based on random bosses in boss room.
            debug.AppendLine("Robot Master Weaknesses:");
            debug.AppendLine("P\tH\tA\tW\tB\tQ\tF\tM\tC:");
            debug.AppendLine("--------------------------------------------");
            foreach(EBossIndex i in BotWeaknesses.Keys)
            {
                foreach(EWeaponIndex j in EWeaponIndex.All)
                {
                    debug.Append(String.Format("{0}\t", BotWeaknesses[i][j]));
                }
                debug.AppendLine("< " + ((EDmgVsBoss.Offset)i).ToString());
            }
            debug.Append(Environment.NewLine);

        }

        /// <summary>
        /// Do 3 or 4 damage for high-ammo weapons, and ammo-damage + 1 for the others
        /// Time Stopper will always do 1 damage.
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        private static Byte GetRoboDamagePrimary(ISeed in_Seed, EDmgVsBoss weapon)
        {
            // Flat 25% chance to do 2 extra damage
            Byte damage = 0;
            Double rExtraDmg = in_Seed.NextDouble();
            if (rExtraDmg > 0.75)
            {
                damage = 2;
            }

            if (weapon == EDmgVsBoss.U_DamageH)
            {
                damage += (Byte)(RWeaponBehavior.GetAmmoUsage(EWeaponIndex.Heat) + 1);
            }
            else if (weapon == EDmgVsBoss.U_DamageA)
            {
                damage += (Byte)(RWeaponBehavior.GetAmmoUsage(EWeaponIndex.Air) + 1);
            }
            else if (weapon == EDmgVsBoss.U_DamageW)
            {
                damage += (Byte)(RWeaponBehavior.GetAmmoUsage(EWeaponIndex.Wood) + 1);
            }
            else if (weapon == EDmgVsBoss.U_DamageF)
            {
                return 1;
            }
            else if (weapon == EDmgVsBoss.U_DamageC)
            {
                damage += (Byte)(RWeaponBehavior.GetAmmoUsage(EWeaponIndex.Crash) + 1);
            }

            // 50% chance to cap the minimum damage at 4, else cap minimum damage at 3
            rExtraDmg = in_Seed.NextDouble();
            if (rExtraDmg > 0.5)
            {
                if (damage < 4)
                {
                    damage = 4;
                }
            }
            else
            {
                if (damage < 3)
                {
                    damage = 3;
                }
            }

            return damage;
        }

        /// <summary>
        /// TODO
        /// </summary>
        private void RandomizeWilyUJ(Patch in_Patch, ISeed in_Seed)
        {
                // List of special weapon damage tables for enemies
                Dictionary<EWeaponIndex, EDmgVsEnemy> dmgPtrEnemies = EDmgVsEnemy.GetTables(false);
                KeyValuePair<EWeaponIndex, EDmgVsEnemy> enemyWeak1;
                KeyValuePair<EWeaponIndex, EDmgVsEnemy> enemyWeak2;
                KeyValuePair<EWeaponIndex, EDmgVsEnemy> enemyWeak3;

                // List of special weapon damage tables for bosses (no flash or buster)
                Dictionary<EWeaponIndex, EDmgVsBoss> dmgPtrBosses = EDmgVsBoss.GetTables(false, false);
                KeyValuePair<EWeaponIndex, EDmgVsBoss> bossWeak1;
                KeyValuePair<EWeaponIndex, EDmgVsBoss> bossWeak2;
                KeyValuePair<EWeaponIndex, EDmgVsBoss> bossWeak3;
                KeyValuePair<EWeaponIndex, EDmgVsBoss> bossWeak4;

                #region Dragon

                // Dragon
                // 25% chance to have a buster vulnerability
                Double rBuster = in_Seed.NextDouble();
                Byte busterDmg = 0x00;

                if (rBuster > 0.75)
                {
                    busterDmg = 0x01;
                }

                in_Patch.Add(EDmgVsBoss.U_DamageP + EDmgVsBoss.Offset.Dragon, busterDmg, "Buster Damage to Dragon");
                WilyWeaknesses[EBossIndex.Dragon][EWeaponIndex.Buster] = busterDmg;

                // Choose 2 special weapon weaknesses
                Dictionary<EWeaponIndex, EDmgVsBoss> dragon = new(dmgPtrBosses);

                //1st weakness
                bossWeak1 = in_Seed.NextElementAndRemove(dragon);

                //2nd weakness
                bossWeak2 = in_Seed.NextElementAndRemove(dragon);

                // For each weapon, apply the weaknesses and immunities
                foreach (EWeaponIndex i in dmgPtrBosses.Keys)
                {
                    EDmgVsBoss weapon = dmgPtrBosses[i];

                    // Dragon weak
                    if (weapon == bossWeak1.Value || weapon == bossWeak2.Value)
                    {
                        // Deal 1 damage with weapons that cost 1 or less ammo
                        Byte damage = 0x01;

                        // Deal damage = ammoUsage - 1, minimum 2 damage
                        if (RWeaponBehavior.GetAmmoUsage(weapon.Index) > 1)
                        {
                            Int32 tryDamage = (Int32)RWeaponBehavior.GetAmmoUsage(weapon.Index) - 0x01;
                            damage = (tryDamage < 2) ? (Byte)0x02 : (Byte)tryDamage;
                        }
                        in_Patch.Add(weapon + EDmgVsBoss.Offset.Dragon, damage, String.Format("{0} Damage to Dragon", weapon.WeaponName));
                        WilyWeaknesses[EBossIndex.Dragon][weapon.Index] = damage;
                    }
                    // Dragon immune
                    else
                    {
                        in_Patch.Add(weapon + EDmgVsBoss.Offset.Dragon, 0x00, String.Format("{0} Damage to Dragon", weapon.WeaponName));
                        WilyWeaknesses[EBossIndex.Dragon][weapon.Index] = 0x00;
                    }
                }

                #endregion

                #region Picopico-kun

                // Picopico-kun
                // 20 HP each
                // 25% chance for buster to deal 3-7 damage
                rBuster = in_Seed.NextDouble();
                busterDmg = 0x00;
                if (rBuster > 0.75)
                {
                    busterDmg = (Byte)(in_Seed.NextInt32(5) + 3);
                }
                in_Patch.Add(EDmgVsEnemy.DamageP + EDmgVsEnemy.Offset.PicopicoKun, busterDmg, String.Format("Buster Damage to Picopico-Kun"));
                WilyWeaknesses[EBossIndex.Pico][EWeaponIndex.Buster] = busterDmg;

                // Deal ammoUse x 10 for the main weakness
                // Deal ammoUse x 6 for another
                // Deal ammoUse x 3 for another
                Dictionary<EWeaponIndex, EDmgVsEnemy> pico = new(dmgPtrEnemies);

                //1st weakness
                enemyWeak1 = in_Seed.NextElementAndRemove(pico);

                //2nd weakness
                enemyWeak2 = in_Seed.NextElementAndRemove(pico);

                //3rd weakness
                enemyWeak3 = in_Seed.NextElementAndRemove(pico);

                foreach (EWeaponIndex i in dmgPtrEnemies.Keys)
                {
                    EDmgVsEnemy weapon = dmgPtrEnemies[i];
                    Byte damage = 0x00;
                    Char level = ' ';

                    if (weapon == enemyWeak1.Value)
                    {
                        damage = (Byte)(RWeaponBehavior.GetAmmoUsage(weapon.Index) * 10);

                        if (damage < 2)
                        {
                            damage = 3;
                        }

                        level = '^';
                    }
                    else if (weapon == enemyWeak2.Value)
                    {
                        damage = (Byte)(RWeaponBehavior.GetAmmoUsage(weapon.Index) * 6);

                        if (damage < 2)
                        {
                            damage = 2;
                        }

                        level = '*';
                    }
                    else if (weapon == enemyWeak3.Value)
                    {
                        damage = (Byte)(RWeaponBehavior.GetAmmoUsage(weapon.Index) * 3);

                        if (damage < 2)
                        {
                            damage = 2;
                        }
                    }

                    // If any weakness is Atomic Fire, deal 20 damage
                    //if (weapon == EDmgVsEnemy.DamageH && (enemyWeak1 == weapon || enemyWeak2 == weapon || enemyWeak3 == weapon))
                    //{
                    //    damage = 20;
                    //}

                    // Bump up already high damage values to 20
                    if (damage >= 14)
                    {
                        damage = 20;
                    }
                    in_Patch.Add(weapon + EDmgVsEnemy.Offset.PicopicoKun, damage, String.Format("{0} Damage to Picopico-Kun{1}", weapon.WeaponName, level));
                    WilyWeaknesses[EBossIndex.Pico][i] = damage;
                    WilyWeaknessInfo[EBossIndex.Pico][i] = level;
                }

                #endregion

                #region Guts

                // Guts
                // 25% chance to have a buster vulnerability
                rBuster = in_Seed.NextDouble();
                busterDmg = 0x00;

                if (rBuster > 0.75)
                {
                    busterDmg = 0x01;
                }

                in_Patch.Add(EDmgVsBoss.U_DamageP + EDmgVsBoss.Offset.Guts, busterDmg, String.Format("Buster Damage to Guts Tank"));
                WilyWeaknesses[EBossIndex.Guts][EWeaponIndex.Buster] = busterDmg;

                // Choose 2 special weapon weaknesses
                Dictionary<EWeaponIndex, EDmgVsBoss> guts = new(dmgPtrBosses);

                //1st weakness
                bossWeak1 = in_Seed.NextElementAndRemove(guts);

                //2nd weakness
                bossWeak2 = in_Seed.NextElementAndRemove(guts);

                foreach (EWeaponIndex i in dmgPtrBosses.Keys)
                {
                    EDmgVsBoss weapon = dmgPtrBosses[i];

                    // Guts weak
                    if (weapon == bossWeak1.Value || weapon == bossWeak2.Value)
                    {
                        // Deal 1 damage with weapons that cost 1 or less ammo
                        Byte damage = 0x01;

                        // Deal damage = ammoUsage - 1, minimum 2 damage
                        if (RWeaponBehavior.GetAmmoUsage(weapon.Index) > 1)
                        {
                            Int32 tryDamage = (Int32)RWeaponBehavior.GetAmmoUsage(weapon.Index) - 0x01;
                            damage = (tryDamage < 2) ? (Byte)0x02 : (Byte)tryDamage;
                        }
                        in_Patch.Add(weapon + EDmgVsBoss.Offset.Guts, damage, String.Format("{0} Damage to Guts Tank", weapon.WeaponName));
                        WilyWeaknesses[EBossIndex.Guts][weapon.Index] = damage;
                    }
                    // Guts immune
                    else
                    {
                        in_Patch.Add(weapon + EDmgVsBoss.Offset.Guts, 0x00, String.Format("{0} Damage to Guts Tank", weapon.WeaponName));
                        WilyWeaknesses[EBossIndex.Guts][weapon.Index] = 0x00;
                    }
                }

                #endregion

                #region Boobeam Trap

                // Boobeam
                // 5 Orbs + 3 Required Barriers (5 total barriers, 4 in speedrun route)
                // Choose a weakness for both Barriers and Orbs, scale damage to have plenty of energy
                // If the same weakness for both, scale damage further
                // If any weakness is Atomic Fire, ensure that there's enough ammo
                
                // Randomize Crash Barrier weakness
                Dictionary<EWeaponIndex, EDmgVsEnemy> dmgBarrierList = EDmgVsEnemy.GetTables(includeBuster: true);

                // Remove Heat as possibility if it costs too much ammo
                if (RWeaponBehavior.GetAmmoUsage(EWeaponIndex.Heat) > 5)
                {
                    dmgBarrierList.Remove(EWeaponIndex.Heat);
                    WilyWeaknesses[EBossIndex.Boobeam][EWeaponIndex.Heat] = 0;
                }

                // Get Barrier weakness
                EWeaponIndex rBarrierWeakness = in_Seed.NextElement(dmgBarrierList.Keys);
                EDmgVsEnemy wpnBarrier = dmgBarrierList[rBarrierWeakness];

                // Scale damage to be slightly more capable than killing 5 barriers at full ammo
                Int32 dmgW4 = 0x01;
                if (wpnBarrier != EDmgVsEnemy.DamageP)
                {
                    Int32 totalShots = (Int32)(28 / RWeaponBehavior.GetAmmoUsage(wpnBarrier.Index));
                    Int32 numHitsPerBarrier = (Int32)(totalShots / 5);

                    if (numHitsPerBarrier > 1)
                    {
                        numHitsPerBarrier--;
                    }

                    if (numHitsPerBarrier > 8)
                    {
                        numHitsPerBarrier = 8;
                    }

                    dmgW4 = (Int32)Math.Ceiling(20d / numHitsPerBarrier);
                }

                // We consult the full damage table here so that we can also update
                // any weapons that have been filtered out, such as heat or flash
                Dictionary<EWeaponIndex, EDmgVsEnemy> fullDmgList = EDmgVsEnemy.GetTables(true);
                foreach (EWeaponIndex i in fullDmgList.Keys)
                {
                    // Deal damage with weakness, and 0 for everything else
                    Byte damage = (Byte)dmgW4;
                    EDmgVsEnemy wpn = fullDmgList[i];
                    if (wpn != wpnBarrier)
                    {
                        damage = 0;
                    }
                    in_Patch.Add(wpn.Address + EDmgVsEnemy.Offset.CrashBarrier_W4, damage, String.Format("{0} Damage to Crash Barrier 1", wpn.WeaponName));
                    in_Patch.Add(wpn.Address + EDmgVsEnemy.Offset.CrashBarrier_Other, damage, String.Format("{0} Damage to Crash Barrier 2", wpn.WeaponName));

                }

                // Remove Barrier weakness from list first (therefore, different Boobeam weakness)
                // At this point we removed:
                //   * heat (if it's expensive)
                //   * flash
                //   * the barrier weakness
                dmgBarrierList.Remove(rBarrierWeakness);

                // Get Boobeam weakness
                KeyValuePair<EWeaponIndex, EDmgVsEnemy> wpnBoobeam = in_Seed.NextElement(dmgBarrierList);

                // Add Barrier weakness back to list for counting later
                // Now flash and possibly heat are the only weapons missing
                dmgBarrierList.Add(rBarrierWeakness, wpnBarrier);

                // Scale damage to be slightly more capable than killing 5 buebeams at full ammo
                dmgW4 = 0x01;
                if (wpnBoobeam.Value != EDmgVsEnemy.DamageP)
                {
                    Int32 totalShots = (Int32)(28 / RWeaponBehavior.GetAmmoUsage(wpnBoobeam.Value.Index));
                    Int32 numHitsPerBoobeam = (Int32)(totalShots / 5);

                    if (numHitsPerBoobeam > 1)
                    {
                        numHitsPerBoobeam--;
                    }

                    if (numHitsPerBoobeam > 8)
                    {
                        numHitsPerBoobeam = 8;
                    }

                    dmgW4 = (Int32)Math.Ceiling(20d / numHitsPerBoobeam);
                }

                // Add Boobeam damage values to patch, as well as array for use by Text and other modules later
                foreach (EWeaponIndex i in fullDmgList.Keys)
                {
                    Byte damage = (Byte)dmgW4;
                    EDmgVsEnemy wpn = fullDmgList[i];
                    if (wpn != wpnBoobeam.Value)
                    {
                        damage = 0;
                    }
                    in_Patch.Add(wpn.Address + EDmgVsEnemy.Offset.Boobeam, damage, String.Format("{0} Damage to Boobeam Trap", wpnBoobeam.Value.WeaponName));

                    // Add to damage table
                    // Note: We used to special case heat here when it's expensive. However, in that
                    // case we already excluded it from being the weakness when we chose a random weakness.
                    // The logic in this loop will set its value to 0 in that case so the special case
                    // is no longer necessary.
                    WilyWeaknesses[EBossIndex.Boobeam][i] = damage;
                }

                #endregion

                #region Wily Machine

                // Machine
                // Will have 4 weaknesses and potentially a Buster weakness
                // Phase 1 will disable 2 of the weaknesses, taking no damage
                // Phase 2 will re-enable them, but disable 1 other weakness
                // Mega Man 2 behaves in a similar fashion, disabling Q and A in phase 1, but only disabling H in phase 2

                // 12.5% chance to have a buster vulnerability
                //
                // 1 out of 8, since the Wily Machine is never
                // weak to Time Stopper
                rBuster = in_Seed.NextDouble();
                busterDmg = 0x00;

                if (rBuster > 0.875)
                {
                    busterDmg = 0x01;
                }

                in_Patch.Add(EDmgVsBoss.U_DamageP + EDmgVsBoss.Offset.Machine, busterDmg, String.Format("Buster Damage to Wily Machine"));
                WilyWeaknesses[EBossIndex.Machine][EWeaponIndex.Buster] = busterDmg;

                // Choose 4 special weapon weaknesses
                Dictionary<EWeaponIndex, EDmgVsBoss> machine = new(dmgPtrBosses);

                //1st weakness
                bossWeak1 = in_Seed.NextElementAndRemove(machine);

                //2nd weakness
                bossWeak2 = in_Seed.NextElementAndRemove(machine);

                //3rd weakness
                bossWeak3 = in_Seed.NextElementAndRemove(machine);

                //4th weakness
                bossWeak4 = in_Seed.NextElementAndRemove(machine);

                // TODO: this needs a way to skip flash
                foreach(EWeaponIndex i in dmgPtrBosses.Keys)
                {
                    EDmgVsBoss weapon = dmgPtrBosses[i];

                    // Machine weak
                    if (weapon == bossWeak1.Value || weapon == bossWeak2.Value || weapon == bossWeak3.Value || weapon == bossWeak4.Value)
                    {
                        // Deal 1 damage with weapons that cost 1 or less ammo
                        Byte damage = 0x01;

                        // Deal damage = ammoUsage
                        if (RWeaponBehavior.GetAmmoUsage(weapon.Index) > 1)
                        {
                            damage = (Byte)RWeaponBehavior.GetAmmoUsage(weapon.Index);
                        }
                        in_Patch.Add(weapon + EDmgVsBoss.Offset.Machine, damage, String.Format("{0} Damage to Wily Machine", weapon.WeaponName));
                        WilyWeaknesses[EBossIndex.Machine][weapon.Index] = damage;
                    }
                    // Machine immune
                    else
                    {
                        in_Patch.Add(weapon + EDmgVsBoss.Offset.Machine, 0x00, String.Format("{0} Damage to Wily Machine", weapon.WeaponName));
                        WilyWeaknesses[EBossIndex.Machine][weapon.Index] = 0x00;
                    }

                    // Disable weakness 1 and 2 on Wily Machine Phase 1
                    if (weapon == bossWeak1.Value)
                    {
                        in_Patch.Add(0x02DA2E, (Byte)i.Offset, String.Format("Wily Machine Phase 1 Resistance 1 ({0})", weapon.WeaponName));
                    }
                    if (weapon == bossWeak2.Value)
                    {
                        in_Patch.Add(0x02DA32, (Byte)i.Offset, String.Format("Wily Machine Phase 1 Resistance 2 ({0})", weapon.WeaponName));
                    }
                    // Disable weakness 3 on Wily Machine Phase 2
                    if (weapon == bossWeak3.Value)
                    {
                        in_Patch.Add(0x02DA3A, (Byte)i.Offset, String.Format("Wily Machine Phase 2 Resistance ({0})", weapon.WeaponName));
                    }
                }

                #endregion

                #region Alien

                // Alien
                // Buster Heat Air Wood Bubble Quick Crash Metal
                Byte alienDamage = 1;
                Dictionary<EWeaponIndex, EDmgVsBoss> alienWeapons = EDmgVsBoss.GetTables(true, false);
                EWeaponIndex rWeaponIndex = in_Seed.NextElement(alienWeapons.Keys);

                // Deal two damage for 1-ammo weapons (or buster)
                if (RWeaponBehavior.GetAmmoUsage(alienWeapons[rWeaponIndex].Index) == 1)
                {
                    alienDamage = 2;
                }
                // For 2+ ammo use weapons, deal 20% more than that in damage, rounded up
                else if (RWeaponBehavior.GetAmmoUsage(alienWeapons[rWeaponIndex].Index) > 1)
                {
                    alienDamage = (Byte)Math.Ceiling(RWeaponBehavior.GetAmmoUsage(alienWeapons[rWeaponIndex].Index) * 1.2);
                }

                // Apply weakness and erase others (flash will remain 0xFF)
                foreach(EWeaponIndex i in alienWeapons.Keys)
                {
                    EDmgVsBoss weapon = alienWeapons[i];

                    if (i == rWeaponIndex)
                    {
                        in_Patch.Add(weapon + EDmgVsBoss.Offset.Alien, alienDamage, String.Format("{0} Damage to Alien", weapon.WeaponName));
                        WilyWeaknesses[EBossIndex.Alien][i] = alienDamage;
                    }
                    else
                    {
                        in_Patch.Add(weapon + EDmgVsBoss.Offset.Alien, 0xFF, String.Format("{0} Damage to Alien", weapon.WeaponName));
                        WilyWeaknesses[EBossIndex.Alien][i] = 0xFF;
                    }
                }

                #endregion

                debug.AppendLine("Wily Boss Weaknesses:");
                debug.AppendLine("P\tH\tA\tW\tB\tQ\tF\tM\tC:");
                debug.AppendLine("--------------------------------------------");
                foreach (EBossIndex i in WilyWeaknesses.Keys)
                {
                    foreach (EWeaponIndex j in WilyWeaknesses[i].Keys)
                    {
                        debug.Append(String.Format("{0}\t", WilyWeaknesses[i][j]));
                    }
                    debug.AppendLine("< " + i.Name);
                }
                debug.Append(Environment.NewLine);

        } // End method RandomizeWilyUJ


    } 
}
