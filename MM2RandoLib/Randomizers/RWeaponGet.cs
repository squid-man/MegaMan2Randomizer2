using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;

namespace MM2Randomizer.Randomizers
{
    public class RWeaponGet : IRandomizer
    {
        private Dictionary<EBossIndex, ERMWeaponValueBit> mNewWeaponOrder;

        private readonly StringBuilder debug = new();
        public override String ToString()
        {
            return debug.ToString();
        }

        public RWeaponGet()
        {
            this.debug = new();
            this.mNewWeaponOrder = new()
            {
                { EBossIndex.Heat, ERMWeaponValueBit.HeatMan },
                { EBossIndex.Air, ERMWeaponValueBit.AirMan },
                { EBossIndex.Wood, ERMWeaponValueBit.WoodMan },
                { EBossIndex.Bubble, ERMWeaponValueBit.BubbleMan },
                { EBossIndex.Quick, ERMWeaponValueBit.QuickMan },
                { EBossIndex.Flash, ERMWeaponValueBit.FlashMan },
                { EBossIndex.Metal, ERMWeaponValueBit.MetalMan },
                { EBossIndex.Crash, ERMWeaponValueBit.CrashMan },
            };
        }

        /// <summary>
        /// Shuffle which Robot Master awards which weapon.
        /// </summary>
        public void Randomize(Patch in_Patch, RandomizationContext in_Context)
        {
            // StageBeat    Address    Value
            // -----------------------------
            // Heat Man     0x03C289   1
            // Air Man      0x03C28A   2
            // Wood Man     0x03C28B   4
            // Bubble Man   0x03C28C   8
            // Quick Man    0x03C28D   16
            // Flash Man    0x03C28E   32
            // Metal Man    0x03C28F   64
            // Crash Man    0x03C290   128
            this.mNewWeaponOrder = in_Context.Seed.Shuffle(this.mNewWeaponOrder);

            // Create table for which weapon is awarded by which robot master
            // This also affects which portrait is blacked out on the stage select
            // This also affects which teleporter deactivates after defeating a Wily 5 refight boss
            foreach (EBossIndex i in EBossIndex.RobotMasters)
            {
                in_Patch.Add((Int32)(ERMStageWeaponAddress.HeatMan + i.Offset), (Byte)this.mNewWeaponOrder[i], $"{(EDmgVsBoss.Offset)i} Weapon Get");
            }

            // Create a copy of the default weapon order table to be used by teleporter function
            // This is needed to fix teleporters breaking from the new weapon order.
            // Unused space at end of bank
            in_Patch.Add(0x03f310, (Byte)ERMWeaponValueBit.HeatMan, "Custom Array of Default Weapon Order");
            in_Patch.Add(0x03f311, (Byte)ERMWeaponValueBit.AirMan, "Custom Array of Default Weapon Order");
            in_Patch.Add(0x03f312, (Byte)ERMWeaponValueBit.WoodMan, "Custom Array of Default Weapon Order");
            in_Patch.Add(0x03f313, (Byte)ERMWeaponValueBit.BubbleMan, "Custom Array of Default Weapon Order");
            in_Patch.Add(0x03f314, (Byte)ERMWeaponValueBit.QuickMan, "Custom Array of Default Weapon Order");
            in_Patch.Add(0x03f315, (Byte)ERMWeaponValueBit.FlashMan, "Custom Array of Default Weapon Order");
            in_Patch.Add(0x03f316, (Byte)ERMWeaponValueBit.MetalMan, "Custom Array of Default Weapon Order");
            in_Patch.Add(0x03f317, (Byte)ERMWeaponValueBit.CrashMan, "Custom Array of Default Weapon Order");

            // Change function to call $f300 instead of $c279 when looking up defeated refight boss to
            // get our default weapon table, fixing the teleporter softlock
            in_Patch.Add(0x03843b, 0x00, "Teleporter Fix Custom Function Call Byte 1");
            in_Patch.Add(0x03843c, 0xf3, "Teleporter Fix Custom Function Call Byte 2");

            // Create table for which stage is selectable on the stage select screen (independent of it being blacked out)
            foreach (EBossIndex i in EBossIndex.RobotMasters)
            {
                in_Patch.Add((Int32)(ERMStageSelect.FirstStageInMemory + i.Offset), (Byte)this.mNewWeaponOrder[i], "Selectable Stage Fix for Random Weapon Get");
            }

            // Dump the boss rewards to the log
            debug.AppendLine("WeaponGet Table:");
            debug.AppendLine("-------------------------------------");
            // This table is just a convenient way to get the weapon names
            Dictionary<EWeaponIndex, EDmgVsBoss> weaponTable = EDmgVsBoss.GetTables(includeBuster: false, includeTimeStopper: true);
            foreach (EBossIndex i in EBossIndex.RobotMasters)
            {
                String weaponName = weaponTable[GetWeaponIndex(this.mNewWeaponOrder[i])].WeaponName;
                debug.AppendLine($"{i.Name} stage\t -> {weaponName}");
            }
        }

        public void FixPortraits<T>(ref Dictionary<EBossIndex, T> portraitBG_x, ref Dictionary<EBossIndex, T> portraitBG_y)
        {
            // Since the acquired-weapons table's elements are powers of two, get a new array of their 0-7 index
            Dictionary<EBossIndex, EBossIndex> newWeaponIndex = GetShuffleIndexPermutation();

            // Permute portrait x/y values via the shuffled acquired-weapons array 
            Dictionary<EBossIndex, T> cpy = new();

            List<EBossIndex> bosses = EBossIndex.RobotMasters.ToList();

            foreach (EBossIndex i in bosses)
            {
                cpy[newWeaponIndex[i]] = portraitBG_y[i];
            }

            portraitBG_y = new Dictionary<EBossIndex, T>(cpy);
            cpy.Clear();

            foreach (EBossIndex i in bosses)
            {
                cpy[newWeaponIndex[i]] = portraitBG_x[i];
            }

            portraitBG_x = new Dictionary<EBossIndex, T>(cpy);
        }

        /// <summary>
        /// Get an array of the shuffled acquired-weapons' 0-7 index, since the original table's elements are bitwise/powers of 2.
        /// Uses the field <see cref="NewWeaponOrder"/>. Must be called after <see cref="Randomize(Patch, Random)"/>.
        /// </summary>
        /// <returns>An array of the new locations of the 8 awarded weapons. The index represents the original robot master index,
        /// in the order H A W B Q F M C. The value represents the index of the new location.
        /// </returns>
        public Dictionary<EBossIndex, EBossIndex> GetShuffleIndexPermutation()
        {
            Dictionary<EBossIndex, EBossIndex> newWeaponIndex = new();

            foreach (EBossIndex origIndex in EBossIndex.RobotMasters)
            {
                EBossIndex newIndex = GetBossIndex(this.mNewWeaponOrder[origIndex]);
                newWeaponIndex[origIndex] = newIndex;
            }

            return newWeaponIndex;
        }

        public static EWeaponIndex GetWeaponIndex(ERMWeaponValueBit in_WeaponValueBit)
        {
            return in_WeaponValueBit switch
            {
                ERMWeaponValueBit.HeatMan => EWeaponIndex.Heat,
                ERMWeaponValueBit.AirMan => EWeaponIndex.Air,
                ERMWeaponValueBit.WoodMan => EWeaponIndex.Wood,
                ERMWeaponValueBit.BubbleMan => EWeaponIndex.Bubble,
                ERMWeaponValueBit.QuickMan => EWeaponIndex.Quick,
                ERMWeaponValueBit.FlashMan => EWeaponIndex.Flash,
                ERMWeaponValueBit.MetalMan => EWeaponIndex.Metal,
                ERMWeaponValueBit.CrashMan => EWeaponIndex.Crash,
                _ => throw new IndexOutOfRangeException(),
            };
        }

        public static EBossIndex GetBossIndex(ERMWeaponValueBit in_WeaponValueBit)
        {
            return in_WeaponValueBit switch
            {
                ERMWeaponValueBit.HeatMan => EBossIndex.Heat,
                ERMWeaponValueBit.AirMan => EBossIndex.Air,
                ERMWeaponValueBit.WoodMan => EBossIndex.Wood,
                ERMWeaponValueBit.BubbleMan => EBossIndex.Bubble,
                ERMWeaponValueBit.QuickMan => EBossIndex.Quick,
                ERMWeaponValueBit.FlashMan => EBossIndex.Flash,
                ERMWeaponValueBit.MetalMan => EBossIndex.Metal,
                ERMWeaponValueBit.CrashMan => EBossIndex.Crash,
                _ => throw new IndexOutOfRangeException(),
            };
        }

    }
}
