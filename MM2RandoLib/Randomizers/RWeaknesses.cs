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
        // Clashman  1 6 A 0 1 0 1 0 0
        public static Dictionary<EBossIndex, Dictionary<EWeaponIndex, Int32>> BotWeaknesses = new Dictionary<EBossIndex, Dictionary<EWeaponIndex, Int32>>()
        {
            {
                EBossIndex.Heat, new Dictionary<EWeaponIndex, Int32>() {
                    { EWeaponIndex.Buster, 2 },
                    { EWeaponIndex.Heat, 0 },
                    { EWeaponIndex.Air, 2 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 6 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Quick, 2 },
                    { EWeaponIndex.Metal, 1 },
                    { EWeaponIndex.Clash, 0 },
                }
            },
            {
                EBossIndex.Air, new Dictionary<EWeaponIndex, Int32>() {
                    { EWeaponIndex.Buster, 2 },
                    { EWeaponIndex.Heat, 6 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 8 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Quick, 2 },
                    { EWeaponIndex.Metal, 0 },
                    { EWeaponIndex.Clash, 0 },
                }
            },
            {
                EBossIndex.Wood, new Dictionary<EWeaponIndex, Int32>() {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 10 },
                    { EWeaponIndex.Air, 4 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Quick, 0 },
                    { EWeaponIndex.Metal, 2 },
                    { EWeaponIndex.Clash, 2 },
                }
            },
            {
                EBossIndex.Bubble, new Dictionary<EWeaponIndex, Int32>() {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 0 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Quick, 2 },
                    { EWeaponIndex.Metal, 4 },
                    { EWeaponIndex.Clash, 2 },
                }
            },
            {
                EBossIndex.Quick, new Dictionary<EWeaponIndex, Int32>() {
                    { EWeaponIndex.Buster, 2 },
                    { EWeaponIndex.Heat, 10 },
                    { EWeaponIndex.Air, 2 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Flash, 1 },
                    { EWeaponIndex.Quick, 0 },
                    { EWeaponIndex.Metal, 0 },
                    { EWeaponIndex.Clash, 4 },
                }
            },
            {
                EBossIndex.Flash, new Dictionary<EWeaponIndex, Int32>() {
                    { EWeaponIndex.Buster, 2 },
                    { EWeaponIndex.Heat, 6 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 2 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Quick, 0 },
                    { EWeaponIndex.Metal, 4 },
                    { EWeaponIndex.Clash, 3 },
                }
            },
            {
                EBossIndex.Metal, new Dictionary<EWeaponIndex, Int32>() {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 4 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Quick, 4 },
                    { EWeaponIndex.Metal, 10 },
                    { EWeaponIndex.Clash, 0 },
                }
            },
            {
                EBossIndex.Clash, new Dictionary<EWeaponIndex, Int32>() {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 6 },
                    { EWeaponIndex.Air, 10 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 1 },
                    { EWeaponIndex.Flash, 0 },
                    { EWeaponIndex.Quick, 1 },
                    { EWeaponIndex.Metal, 0 },
                    { EWeaponIndex.Clash, 0 },
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
        public static Dictionary<EBossIndex, Dictionary<EWeaponIndex, Int32>> WilyWeaknesses = new Dictionary<EBossIndex, Dictionary<EWeaponIndex, Int32>>()
        {
             {
                EBossIndex.Dragon, new Dictionary<EWeaponIndex, Int32>() {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 8 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Quick, 1 },
                    { EWeaponIndex.Metal, 0 },
                    { EWeaponIndex.Clash, 1 },
                }
            },
            {
                EBossIndex.Pico, new Dictionary<EWeaponIndex, Int32>() {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 3 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 10 },
                    { EWeaponIndex.Quick, 7 },
                    { EWeaponIndex.Metal, 7 },
                    { EWeaponIndex.Clash, 0 },
                }
            },
            {
                EBossIndex.Guts, new Dictionary<EWeaponIndex, Int32>() {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 8 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 1 },
                    { EWeaponIndex.Quick, 2 },
                    { EWeaponIndex.Metal, 0 },
                    { EWeaponIndex.Clash, 1 },
                }
            },
            {
                EBossIndex.Boobeam, new Dictionary<EWeaponIndex, Int32>() {
                    { EWeaponIndex.Buster, 0 },
                    { EWeaponIndex.Heat, 0 },
                    { EWeaponIndex.Air, 0 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Quick, 0 },
                    { EWeaponIndex.Metal, 0 },
                    { EWeaponIndex.Clash, 11 },
                }
            },
            {
                EBossIndex.Machine, new Dictionary<EWeaponIndex, Int32>() {
                    { EWeaponIndex.Buster, 1 },
                    { EWeaponIndex.Heat, 14 },
                    { EWeaponIndex.Air, 1 },
                    { EWeaponIndex.Wood, 0 },
                    { EWeaponIndex.Bubble, 0 },
                    { EWeaponIndex.Quick, 1 },
                    { EWeaponIndex.Metal, 1 },
                    { EWeaponIndex.Clash, 4 },
                }
            },
            {
                EBossIndex.Alien, new Dictionary<EWeaponIndex, Int32>() {
                    { EWeaponIndex.Buster, 255 },
                    { EWeaponIndex.Heat, 255 },
                    { EWeaponIndex.Air, 255 },
                    { EWeaponIndex.Wood, 255 },
                    { EWeaponIndex.Bubble, 1 },
                    { EWeaponIndex.Quick, 255 },
                    { EWeaponIndex.Metal, 255 },
                    { EWeaponIndex.Clash, 255 },
                }
            },
        };

        private Dictionary<EBossIndex, Dictionary<EWeaponIndex, Char>> WilyWeaknessInfo = new Dictionary<EBossIndex, Dictionary<EWeaponIndex, Char>>()
        {
            {
                EBossIndex.Dragon, new Dictionary<EWeaponIndex, Char>()
            },
            {
                EBossIndex.Pico, new Dictionary<EWeaponIndex, Char>()
            },
            {
                EBossIndex.Guts, new Dictionary<EWeaponIndex, Char>()
            },
            {
                EBossIndex.Boobeam, new Dictionary<EWeaponIndex, Char>()
            },
            {
                EBossIndex.Machine, new Dictionary<EWeaponIndex, Char>()
            },
            {
                EBossIndex.Alien, new Dictionary<EWeaponIndex, Char>()
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
            Dictionary<EWeaponIndex, EDmgVsBoss> bossPrimaryWeaknessAddresses = EDmgVsBoss.GetTables(false, true);
            Dictionary<EBossIndex, EDmgVsBoss> bossWeaknessShuffled = bossPrimaryWeaknessAddresses
                .ToDictionary(x => EDmgVsBoss.GetBossIndexFromWeaponIndex(x.Key), x => x.Value);
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
                EWeaponIndex.Clash
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
                    in_Patch.Add(bossPrimaryWeaknessAddresses[j] + (Int32)i, damage, String.Format("{0} Damage to {1}", bossPrimaryWeaknessAddresses[j].WeaponName, (EDmgVsBoss.Offset)i));
                    BotWeaknesses[i][j] = damage;
                }

                // Write the primary weakness for this boss
                Byte dmgPrimary = RWeaknesses.GetRoboDamagePrimary(in_Seed, bossWeaknessShuffled[i]);
                in_Patch.Add(bossWeaknessShuffled[i] + (Int32)i, dmgPrimary, $"{bossWeaknessShuffled[i].WeaponName} Damage to {(EDmgVsBoss.Offset)i} (Primary)");

                // Write the secondary weakness for this boss (next element in list)
                // Secondary weakness will either do 2 damage or 4 if it is Atomic Fire
                // Time Stopper cannot be a secondary weakness. Instead it will heal that boss.
                // As a result, one Robot Master will not have a secondary weakness
                EBossIndex i2 = (EBossIndex)(((Int32)i + 1) % bossWeaknessShuffled.Count);
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
                    in_Patch.Add(0x02C08F, (Byte)i, $"Time-Stopper Heals {(EDmgVsBoss.Offset)i} (Special Code)");
                }
                in_Patch.Add(weakWeap2 + (Int32)i, dmgSecondary, $"{weakWeap2.WeaponName} Damage to {(EDmgVsBoss.Offset)i} (Secondary)");

                // Add buster damage
                {
                    Byte busterDmg = (Byte)(busterList.Contains(i) ? 2 : 1);
                    in_Patch.Add(EDmgVsBoss.U_DamageP + (Int32)i, busterDmg, $"Buster Damage to {(EDmgVsBoss.Offset)i}");
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
                    in_Patch.Add(wpn.Address + (Int32)i, 0x07, $"{wpn.WeaponName} Damage to {(EDmgVsBoss.Offset)i} (Great)");
                    BotWeaknesses[i][wpn.Index] = 0x07;
                }
                else if (bossWithUltimateWeakness == i)
                {
                    // Ultimate weakness. Can't be Buster or Flash. Deal 10 damage.
                    EDmgVsBoss wpn = EDmgVsBoss.GetTables(false, false)[weaponUltimateWeakness];
                    in_Patch.Add(wpn.Address + (Int32)i, 0x0A, $"{wpn.WeaponName} Damage to {(EDmgVsBoss.Offset)i} (Ultimate)");
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
                foreach(EWeaponIndex j in Enum.GetValues(typeof(EWeaponIndex)))
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
                damage += (Byte)(RWeaponBehavior.GetAmmoUsage(EWeaponIndex.Clash) + 1);
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
                EDmgVsEnemy enemyWeak1;
                EDmgVsEnemy enemyWeak2;
                EDmgVsEnemy enemyWeak3;

                // List of special weapon damage tables for bosses (no flash or buster)
                Dictionary<EWeaponIndex, EDmgVsBoss> dmgPtrBosses = EDmgVsBoss.GetTables(false, false);
                EDmgVsBoss bossWeak1;
                EDmgVsBoss bossWeak2;
                EDmgVsBoss bossWeak3;
                EDmgVsBoss bossWeak4;

                #region Dragon

                // Dragon
                // 25% chance to have a buster vulnerability
                Double rBuster = in_Seed.NextDouble();
                Byte busterDmg = 0x00;

                if (rBuster > 0.75)
                {
                    busterDmg = 0x01;
                }

                in_Patch.Add(EDmgVsBoss.U_DamageP + (Int32)EDmgVsBoss.Offset.Dragon.Value, busterDmg, "Buster Damage to Dragon");
                WilyWeaknesses[EBossIndex.Dragon][EWeaponIndex.Buster] = busterDmg;

                // Choose 2 special weapon weaknesses
                Dictionary<EWeaponIndex, EDmgVsBoss> dragon = new Dictionary<EWeaponIndex, EDmgVsBoss>(dmgPtrBosses);

                //1st weakness
                Int32 rInt = in_Seed.NextInt32(dragon.Count);
                bossWeak1 = dragon.Values.ElementAt(rInt);
                dragon.Remove(dragon.Keys.ElementAt(rInt));

                //2nd weakness
                rInt = in_Seed.NextInt32(dragon.Count);
                bossWeak2 = dragon.Values.ElementAt(rInt);

                // For each weapon, apply the weaknesses and immunities
                foreach( EWeaponIndex i in dmgPtrBosses.Keys )
                {
                    EDmgVsBoss weapon = dmgPtrBosses[i];

                    // Dragon weak
                    if (weapon == bossWeak1 || weapon == bossWeak2)
                    {
                        // Deal 1 damage with weapons that cost 1 or less ammo
                        Byte damage = 0x01;

                        // Deal damage = ammoUsage - 1, minimum 2 damage
                        if (RWeaponBehavior.GetAmmoUsage(weapon.Index) > 1)
                        {
                            Int32 tryDamage = (Int32)RWeaponBehavior.GetAmmoUsage(weapon.Index) - 0x01;
                            damage = (tryDamage < 2) ? (Byte)0x02 : (Byte)tryDamage;
                        }
                        in_Patch.Add(weapon + (Int32)EDmgVsBoss.Offset.Dragon.Value, damage, String.Format("{0} Damage to Dragon", weapon.WeaponName));
                        WilyWeaknesses[EBossIndex.Dragon][weapon.Index] = damage;
                    }
                    // Dragon immune
                    else
                    {
                        in_Patch.Add(weapon + (Int32)EDmgVsBoss.Offset.Dragon.Value, 0x00, String.Format("{0} Damage to Dragon", weapon.WeaponName));
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
                Dictionary<EWeaponIndex, EDmgVsEnemy> pico = new Dictionary<EWeaponIndex, EDmgVsEnemy>(dmgPtrEnemies);

                //1st weakness
                rInt = in_Seed.NextInt32(pico.Count);
                enemyWeak1 = pico.Values.ElementAt(rInt);
                pico.Remove(pico.Keys.ElementAt(rInt));

                //2nd weakness
                rInt = in_Seed.NextInt32(pico.Count);
                enemyWeak2 = pico.Values.ElementAt(rInt);
                pico.Remove(pico.Keys.ElementAt(rInt));

                //3rd weakness
                rInt = in_Seed.NextInt32(pico.Count);
                enemyWeak3 = pico.Values.ElementAt(rInt);

                foreach(EWeaponIndex i in dmgPtrEnemies.Keys)
                {
                    EDmgVsEnemy weapon = dmgPtrEnemies[i];
                    Byte damage = 0x00;
                    Char level = ' ';

                    if (weapon == enemyWeak1)
                    {
                        damage = (Byte)(RWeaponBehavior.GetAmmoUsage(weapon.Index) * 10);

                        if (damage < 2)
                        {
                            damage = 3;
                        }

                        level = '^';
                    }
                    else if (weapon == enemyWeak2)
                    {
                        damage = (Byte)(RWeaponBehavior.GetAmmoUsage(weapon.Index) * 6);

                        if (damage < 2)
                        {
                            damage = 2;
                        }

                        level = '*';
                    }
                    else if (weapon == enemyWeak3)
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

                in_Patch.Add(EDmgVsBoss.U_DamageP + ((Int32)EDmgVsBoss.Offset.Guts.Value), busterDmg, String.Format("Buster Damage to Guts Tank"));
                WilyWeaknesses[EBossIndex.Guts][EWeaponIndex.Buster] = busterDmg;

                // Choose 2 special weapon weaknesses
                Dictionary<EWeaponIndex, EDmgVsBoss> guts = new Dictionary<EWeaponIndex, EDmgVsBoss>(dmgPtrBosses);

                //1st weakness
                rInt = in_Seed.NextInt32(guts.Count);
                bossWeak1 = guts.Values.ElementAt(rInt);
                guts.Remove(guts.Keys.ElementAt(rInt));

                //2nd weakness
                rInt = in_Seed.NextInt32(guts.Count);
                bossWeak2 = guts.Values.ElementAt(rInt);

                foreach(EWeaponIndex i in dmgPtrBosses.Keys)
                {
                    EDmgVsBoss weapon = dmgPtrBosses[i];

                    // Guts weak
                    if (weapon == bossWeak1 || weapon == bossWeak2)
                    {
                        // Deal 1 damage with weapons that cost 1 or less ammo
                        Byte damage = 0x01;

                        // Deal damage = ammoUsage - 1, minimum 2 damage
                        if (RWeaponBehavior.GetAmmoUsage(weapon.Index) > 1)
                        {
                            Int32 tryDamage = (Int32)RWeaponBehavior.GetAmmoUsage(weapon.Index) - 0x01;
                            damage = (tryDamage < 2) ? (Byte)0x02 : (Byte)tryDamage;
                        }
                        in_Patch.Add(weapon + (Int32)EDmgVsBoss.Offset.Guts.Value, damage, String.Format("{0} Damage to Guts Tank", weapon.WeaponName));
                        WilyWeaknesses[EBossIndex.Guts][weapon.Index] = damage;
                    }
                    // Guts immune
                    else
                    {
                        in_Patch.Add(weapon + (Int32)EDmgVsBoss.Offset.Guts.Value, 0x00, String.Format("{0} Damage to Guts Tank", weapon.WeaponName));
                        WilyWeaknesses[EBossIndex.Guts][weapon.Index] = 0x00;
                    }
                }

                #endregion

                #region Buebeam Trap

                // Buebeam
                // 5 Orbs + 3 Required Barriers (5 total barriers, 4 in speedrun route)
                // Choose a weakness for both Barriers and Orbs, scale damage to have plenty of energy
                // If the same weakness for both, scale damage further
                // If any weakness is Atomic Fire, ensure that there's enough ammo
                
                // Randomize Crash Barrier weakness
                Dictionary<EWeaponIndex, EDmgVsEnemy> dmgBarrierList = EDmgVsEnemy.GetTables(true);

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
                foreach(EWeaponIndex i in dmgBarrierList.Keys)
                {
                    // Deal damage with weakness, and 0 for everything else
                    Byte damage = (Byte)dmgW4;
                    EDmgVsEnemy wpn = dmgBarrierList[i];
                    if (wpn != wpnBarrier)
                    {
                        damage = 0;
                    }
                    in_Patch.Add(wpn.Address + EDmgVsEnemy.Offset.ClashBarrier_W4, damage, String.Format("{0} Damage to Clash Barrier 1", wpn.WeaponName));
                    in_Patch.Add(wpn.Address + EDmgVsEnemy.Offset.ClashBarrier_Other, damage, String.Format("{0} Damage to Clash Barrier 2", wpn.WeaponName));
                }

                // Remove Barrier weakness from list first (therefore, different Buebeam weakness)
                dmgBarrierList.Remove(rBarrierWeakness);

                // Get Buebeam weakness
                rInt = in_Seed.NextInt32(dmgBarrierList.Count);
                EDmgVsEnemy wpnBuebeam = dmgBarrierList.Values.ElementAt(rInt);

                // Add Barrier weakness back to list for counting later
                dmgBarrierList.Add(rBarrierWeakness, wpnBarrier);

                // Scale damage to be slightly more capable than killing 5 buebeams at full ammo
                dmgW4 = 0x01;
                if (wpnBuebeam != EDmgVsEnemy.DamageP)
                {
                    Int32 totalShots = (Int32)(28 / RWeaponBehavior.GetAmmoUsage(wpnBuebeam.Index));
                    Int32 numHitsPerBuebeam = (Int32)(totalShots / 5);

                    if (numHitsPerBuebeam > 1)
                    {
                        numHitsPerBuebeam--;
                    }

                    if (numHitsPerBuebeam > 8)
                    {
                        numHitsPerBuebeam = 8;
                    }

                    dmgW4 = (Int32)Math.Ceiling(20d / numHitsPerBuebeam);
                }

                // Add Buebeam damage values to patch, as well as array for use by Text and other modules later
                foreach(EWeaponIndex i in dmgBarrierList.Keys)
                {
                    Byte damage = (Byte)dmgW4;
                    EDmgVsEnemy wpn = dmgBarrierList[i];
                    if (wpn != wpnBuebeam)
                    {
                        damage = 0;
                    }
                    in_Patch.Add(wpn.Address + EDmgVsEnemy.Offset.Buebeam, damage, String.Format("{0} Damage to Buebeam Trap", wpnBuebeam.WeaponName));

                    // Add to damage table (skipping heat if necessary)
                    // TODO: is this logic with the comparison still right?
                    if (RWeaponBehavior.GetAmmoUsage(EWeaponIndex.Heat) > 5 && i >= EWeaponIndex.Heat)
                    {
                        WilyWeaknesses[EBossIndex.Boobeam][i + 1] = damage;
                    }
                    else
                    {
                        WilyWeaknesses[EBossIndex.Boobeam][i] = damage;
                    }
                }

                #endregion

                #region Wily Machine

                // Machine
                // Will have 4 weaknesses and potentially a Buster weakness
                // Phase 1 will disable 2 of the weaknesses, taking no damage
                // Phase 2 will re-enable them, but disable 1 other weakness
                // Mega Man 2 behaves in a similar fashion, disabling Q and A in phase 1, but only disabling H in phase 2

                // 75% chance to have a buster vulnerability
                rBuster = in_Seed.NextDouble();
                busterDmg = 0x00;

                if (rBuster > 0.25)
                {
                    busterDmg = 0x01;
                }

                in_Patch.Add(EDmgVsBoss.U_DamageP + (Int32)EDmgVsBoss.Offset.Machine.Value, busterDmg, String.Format("Buster Damage to Wily Machine"));
                WilyWeaknesses[EBossIndex.Machine][EWeaponIndex.Buster] = busterDmg;

                // Choose 4 special weapon weaknesses
                Dictionary<EWeaponIndex, EDmgVsBoss> machine = new Dictionary<EWeaponIndex, EDmgVsBoss>(dmgPtrBosses);

                //1st weakness
                rInt = in_Seed.NextInt32(machine.Count);
                bossWeak1 = machine.Values.ElementAt(rInt);
                machine.Remove(machine.Keys.ElementAt(rInt));

                //2nd weakness
                rInt = in_Seed.NextInt32(machine.Count);
                bossWeak2 = machine.Values.ElementAt(rInt);
                machine.Remove(machine.Keys.ElementAt(rInt));

                //3rd weakness
                rInt = in_Seed.NextInt32(machine.Count);
                bossWeak3 = machine.Values.ElementAt(rInt);
                machine.Remove(machine.Keys.ElementAt(rInt));

                //4th weakness
                rInt = in_Seed.NextInt32(machine.Count);
                bossWeak4 = machine.Values.ElementAt(rInt);

                // TODO: this needs a way to skip flash
                foreach(EWeaponIndex i in dmgPtrBosses.Keys)
                {
                    EDmgVsBoss weapon = dmgPtrBosses[i];

                    // Machine weak
                    if (weapon == bossWeak1 || weapon == bossWeak2 || weapon == bossWeak3 || weapon == bossWeak4)
                    {
                        // Deal 1 damage with weapons that cost 1 or less ammo
                        Byte damage = 0x01;

                        // Deal damage = ammoUsage
                        if (RWeaponBehavior.GetAmmoUsage(weapon.Index) > 1)
                        {
                            damage = (Byte)RWeaponBehavior.GetAmmoUsage(weapon.Index);
                        }
                        in_Patch.Add(weapon + (Int32)EDmgVsBoss.Offset.Machine.Value, damage, String.Format("{0} Damage to Wily Machine", weapon.WeaponName));
                        WilyWeaknesses[EBossIndex.Machine][weapon.Index] = damage;
                    }
                    // Machine immune
                    else
                    {
                        in_Patch.Add(weapon + (Int32)EDmgVsBoss.Offset.Machine.Value, 0x00, String.Format("{0} Damage to Wily Machine", weapon.WeaponName));
                        WilyWeaknesses[EBossIndex.Machine][weapon.Index] = 0x00;
                    }

                    // Disable weakness 1 and 2 on Wily Machine Phase 1
                    if (weapon == bossWeak1)
                    {
                        in_Patch.Add(0x02DA2E, (Byte)i, String.Format("Wily Machine Phase 1 Resistance 1 ({0})", weapon.WeaponName));
                    }
                    if (weapon == bossWeak2)
                    {
                        in_Patch.Add(0x02DA32, (Byte)i, String.Format("Wily Machine Phase 1 Resistance 2 ({0})", weapon.WeaponName));
                    }
                    // Disable weakness 3 on Wily Machine Phase 2
                    if (weapon == bossWeak3)
                    {
                        in_Patch.Add(0x02DA3A, (Byte)i, String.Format("Wily Machine Phase 2 Resistance ({0})", weapon.WeaponName));
                    }
                }

                #endregion

                #region Alien

                // Alien
                // Buster Heat Air Wood Bubble Quick Clash Metal
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
                        in_Patch.Add(weapon + (Int32)EDmgVsBoss.Offset.Alien.Value, alienDamage, String.Format("{0} Damage to Alien", weapon.WeaponName));
                        WilyWeaknesses[EBossIndex.Alien][i] = alienDamage;
                    }
                    else
                    {
                        in_Patch.Add(weapon + (Int32)EDmgVsBoss.Offset.Alien.Value, 0xFF, String.Format("{0} Damage to Alien", weapon.WeaponName));
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

                        if (j == EWeaponIndex.Flash)
                        {
                            debug.Append("X\t"); // skip flash
                        }
                    }
                    String bossName = "";
                    switch (i)
                    {
                        case EBossIndex.Dragon:
                            bossName = "dragon";
                            break;
                        case EBossIndex.Pico:
                            bossName = "picopico-kun";
                            break;
                        case EBossIndex.Guts:
                            bossName = "guts";
                            break;
                        case EBossIndex.Boobeam:
                            bossName = "boobeam";
                            break;
                        case EBossIndex.Machine:
                            bossName = "machine";
                            break;
                        case EBossIndex.Alien:
                            bossName = "alien";
                            break;
                        default: break;
                    }
                    debug.AppendLine("< " + bossName);
                }
                debug.Append(Environment.NewLine);

        } // End method RandomizeWilyUJ


    } 
}
