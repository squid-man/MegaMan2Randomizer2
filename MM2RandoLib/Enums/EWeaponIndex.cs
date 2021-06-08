using System;
using System.Collections.Generic;
using System.Linq;

namespace MM2Randomizer.Enums
{
    public class EWeaponIndex
    {
        // These need to appear before other members in this file, otherwise we can't use them in the
        // constructor
        private static readonly List<EWeaponIndex> mAll = new();
        private static readonly List<EWeaponIndex> mSpecialWeapons = new();

        private const Int32 BusterValue = 0;
        public static readonly EWeaponIndex Buster = new(BusterValue);
        public static readonly EWeaponIndex Heat = new(1);
        public static readonly EWeaponIndex Air = new(2);
        public static readonly EWeaponIndex Wood = new(3);
        public static readonly EWeaponIndex Bubble = new(4);
        public static readonly EWeaponIndex Quick = new(5);
        public static readonly EWeaponIndex Flash = new(6);
        public static readonly EWeaponIndex Metal = new(7);
        public static readonly EWeaponIndex Crash = new(8);

        public Int32 Offset { get; private init; }

        public static IReadOnlyList<EWeaponIndex> All { get { return mAll.ToList(); } }
        public static IReadOnlyList<EWeaponIndex> SpecialWeapons { get { return mSpecialWeapons.ToList(); } }

        /// <summary>
        /// Gets the index of the next weapon in the list, in a cyclic way. That is,
        /// it wraps around if it's already at the last one.
        /// 
        /// Throws an exception if "this" is not in the list.
        /// </summary>
        /// <returns></returns>
        public EWeaponIndex NextWeapon(IList<EWeaponIndex> in_Weapons)
        {
            Int32 index = in_Weapons.IndexOf(this);
            if (-1 == index)
            {
                throw new IndexOutOfRangeException();
            }
            index = (index + 1) % in_Weapons.Count;
            return in_Weapons.ElementAt(index);
        }

        public EBossIndex ToBossIndex()
        {
            Dictionary<EWeaponIndex, EBossIndex> mapping = new()
            {
                { EWeaponIndex.Heat, EBossIndex.Heat },
                { EWeaponIndex.Air, EBossIndex.Air },
                { EWeaponIndex.Wood, EBossIndex.Wood },
                { EWeaponIndex.Bubble, EBossIndex.Bubble },
                { EWeaponIndex.Quick, EBossIndex.Quick },
                { EWeaponIndex.Flash, EBossIndex.Flash },
                { EWeaponIndex.Metal, EBossIndex.Metal },
                { EWeaponIndex.Crash, EBossIndex.Crash },
            };
            if (!mapping.ContainsKey(this))
            {
                throw new IndexOutOfRangeException();
            }
            return mapping[this];
        }

        private EWeaponIndex(Int32 in_Value)
        {
            Offset = in_Value;
            mAll.Add(this);
            // We can't use Buster.Offset yet because it hasn't been initialized yet
            if (BusterValue != in_Value)
            {
                mSpecialWeapons.Add(this);
            }
        }
    }

}
