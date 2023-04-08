using System;
using System.Collections.Generic;
using System.Linq;

namespace MM2Randomizer.Enums
{
    public class EBossIndex
    {
        // These need to appear before other members in this file, otherwise we can't use them in the
        // constructor
        private static readonly List<EBossIndex> mAll = new();
        private static readonly List<EBossIndex> mRobotMasters = new();
        private static readonly List<EBossIndex> mWilyBosses = new();

        // Robot masters
        public static readonly EBossIndex Heat = new(0, "Heat");
        public static readonly EBossIndex Air = new(1, "Air");
        public static readonly EBossIndex Wood = new(2, "Wood");
        public static readonly EBossIndex Bubble = new(3, "Bubble");
        public static readonly EBossIndex Quick = new(4, "Quick");
        public static readonly EBossIndex Flash = new(5, "Flash");
        public static readonly EBossIndex Metal = new(6, "Metal");
        public static readonly EBossIndex Crash = new(7, "Crash");

        // Wily castle bosses
        private const Int32 DragonValue = 8;
        public static readonly EBossIndex Dragon = new(DragonValue, "Dragon");
        public static readonly EBossIndex Pico = new(9, "Picopico-kun");
        public static readonly EBossIndex Guts = new(10, "Guts");
        public static readonly EBossIndex Boobeam = new(11, "Boobeam");
        public static readonly EBossIndex Machine = new(12, "Machine");
        public static readonly EBossIndex Alien = new(13, "Alien");

        public Int32 Offset { get; private init; }
        public String Name { get; private init; }
        public static IReadOnlyList<EBossIndex> All { get { return mAll.ToList(); } }
        public static IReadOnlyList<EBossIndex> RobotMasters { get { return mRobotMasters.ToList(); } }
        public static IReadOnlyList<EBossIndex> WilyBosses { get { return mWilyBosses.ToList(); } }

        /// <summary>
        /// Gets the index of the next boss in the list, in a cyclic way. That is,
        /// it wraps around if it's already at the last one.
        ///
        /// Throws an exception if "this" is not in the list.
        /// </summary>
        /// <returns></returns>
        public EBossIndex NextBoss(IList<EBossIndex> in_Bosses)
        {
            Int32 index = in_Bosses.IndexOf(this);
            if (-1 == index) {
                throw new IndexOutOfRangeException();
            }
            index = (index + 1) % in_Bosses.Count;
            return in_Bosses.ElementAt(index);
        }

        public EWeaponIndex ToWeaponIndex()
        {
            if (mBossToWeaponMap.TryGetValue(this, out EWeaponIndex? weaponIndex))
            {
                return weaponIndex;
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        private static readonly Dictionary<EBossIndex, EWeaponIndex> mBossToWeaponMap = new()
        {
            { EBossIndex.Heat, EWeaponIndex.Heat },
            { EBossIndex.Air, EWeaponIndex.Air },
            { EBossIndex.Wood, EWeaponIndex.Wood },
            { EBossIndex.Bubble, EWeaponIndex.Bubble },
            { EBossIndex.Quick, EWeaponIndex.Quick },
            { EBossIndex.Flash, EWeaponIndex.Flash },
            { EBossIndex.Metal, EWeaponIndex.Metal },
            { EBossIndex.Crash, EWeaponIndex.Crash },
        };

        private EBossIndex(Int32 in_Value, String in_name)
        {
            Offset = in_Value;
            Name = in_name;
            mAll.Add(this);
            // We can't use Dragon.Offset because it's not guaranteed to
            // be initialized yet
            if (in_Value < DragonValue)
            {
                mRobotMasters.Add(this);
            }
            else
            {
                mWilyBosses.Add(this);
            }
        }
    }
}
