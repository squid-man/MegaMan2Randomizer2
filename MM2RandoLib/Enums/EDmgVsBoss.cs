using System;
using System.Collections.Generic;

namespace MM2Randomizer.Enums
{
    public sealed class EDmgVsBoss
    {
        public String WeaponName
        {
            get; private set;
        }

        public Int32 Address
        {
            get; private set;
        }

        public EWeaponIndex Index
        {
            get; private set;
        }

        public static readonly Dictionary<Int32, EDmgVsBoss> Addresses;

        //Japanese
        public static readonly EDmgVsBoss Buster          = new(EWeaponIndex.Buster, 0x02E933, "Buster");
        public static readonly EDmgVsBoss AtomicFire      = new(EWeaponIndex.Heat, 0x02E941, "Atomic Fire");
        public static readonly EDmgVsBoss AirShooter      = new(EWeaponIndex.Air, 0x02E94F, "Air Shooter");
        public static readonly EDmgVsBoss LeafShield      = new(EWeaponIndex.Wood, 0x02E95D, "Leaf Shield");
        public static readonly EDmgVsBoss BubbleLead      = new(EWeaponIndex.Bubble, 0x02E96B, "Bubble Lead");
        public static readonly EDmgVsBoss QuickBoomerang  = new(EWeaponIndex.Quick, 0x02E979, "Quick Boomerang");
        public static readonly EDmgVsBoss TimeStopper     = new(EWeaponIndex.Flash, 0x02C049, "Time Stopper");
        public static readonly EDmgVsBoss MetalBlade      = new(EWeaponIndex.Metal, 0x02E995, "Metal Blade");
        public static readonly EDmgVsBoss CrashBomber     = new(EWeaponIndex.Crash, 0x02E987, "Crash Bomber");
        
        //English
        public static readonly EDmgVsBoss U_DamageP = new(EWeaponIndex.Buster, 0x2e952, "Buster");
        public static readonly EDmgVsBoss U_DamageH = new(EWeaponIndex.Heat, 0x2e960, "Atomic Fire");
        public static readonly EDmgVsBoss U_DamageA = new(EWeaponIndex.Air, 0x2e96e, "Air Shooter");
        public static readonly EDmgVsBoss U_DamageW = new(EWeaponIndex.Wood, 0x2e97c, "Leaf Shield");
        public static readonly EDmgVsBoss U_DamageB = new(EWeaponIndex.Bubble, 0x2e98a, "Bubble Lead");
        public static readonly EDmgVsBoss U_DamageQ = new(EWeaponIndex.Quick, 0x2e998, "Quick Boomerang");
        public static readonly EDmgVsBoss U_DamageF = new(EWeaponIndex.Flash, 0x2C049, "Time Stopper");
        public static readonly EDmgVsBoss U_DamageM = new(EWeaponIndex.Metal, 0x2e9b4, "Metal Blade");
        public static readonly EDmgVsBoss U_DamageC = new(EWeaponIndex.Crash, 0x2e9a6, "Crash Bomber");

        static EDmgVsBoss()
        {
            Addresses = new Dictionary<Int32, EDmgVsBoss>()
            {
                { U_DamageP.Address, U_DamageP },
                { U_DamageH.Address, U_DamageH },
                { U_DamageA.Address, U_DamageA },
                { U_DamageW.Address, U_DamageW },
                { U_DamageB.Address, U_DamageB },
                { U_DamageQ.Address, U_DamageQ },
                { U_DamageF.Address, U_DamageF },
                { U_DamageM.Address, U_DamageM },
                { U_DamageC.Address, U_DamageC },
            };
        }

        private EDmgVsBoss(EWeaponIndex index, Int32 address, String name)
        {
            this.Address = address;
            this.WeaponName = name;
            this.Index = index;
        }

        public static implicit operator Int32 (EDmgVsBoss eDmgVsBoss)
        {
            return eDmgVsBoss.Address;
        }

        public static implicit operator EDmgVsBoss (Int32 eDmgVsBoss)
        {
            return Addresses[eDmgVsBoss];
        }

        public override String ToString()
        {
            return WeaponName;
        }

        /// <summary>
        /// Get a list of pointers to weapon damage tables against bosses, sorted by boss order
        /// </summary>
        /// <param name="includeBuster"></param>
        /// <param name="includeTimeStopper"></param>
        /// <returns></returns>
        public static Dictionary<EWeaponIndex, EDmgVsBoss> GetTables(Boolean includeBuster, Boolean includeTimeStopper)
        {
            Dictionary<EWeaponIndex, EDmgVsBoss> tables = new();

            if (includeBuster)
            {
                tables.Add(EWeaponIndex.Buster, U_DamageP);
            }

            tables.Add(EWeaponIndex.Heat, U_DamageH);
            tables.Add(EWeaponIndex.Air, U_DamageA);
            tables.Add(EWeaponIndex.Wood, U_DamageW);
            tables.Add(EWeaponIndex.Bubble, U_DamageB);
            tables.Add(EWeaponIndex.Quick, U_DamageQ);

            if (includeTimeStopper)
            {
                tables.Add(EWeaponIndex.Flash, U_DamageF);
            }

            tables.Add(EWeaponIndex.Metal, U_DamageM);
            tables.Add(EWeaponIndex.Crash, U_DamageC);
            return tables;
        }

        /// <summary>
        /// 
        /// </summary>
        public class Offset
        {
            public String Name
            {
                get; private set;
            }

            public Int32 Value
            {
                get { return mValue.Offset; }
            }

            private readonly EBossIndex mValue;

            private static readonly Dictionary<EBossIndex, Offset> Offsets;

            public static readonly Offset Dragon    = new(EBossIndex.Dragon, "Dragon");
            // Note: Pico is handled in EDmgVsEnemy instead of here
            public static readonly Offset Guts      = new(EBossIndex.Guts, "Guts");
            public static readonly Offset Machine   = new(EBossIndex.Machine, "Machine");
            public static readonly Offset Alien     = new(EBossIndex.Alien, "Alien");
            public static readonly Offset Heat      = new(EBossIndex.Heat, "Heat");
            public static readonly Offset Air       = new(EBossIndex.Air, "Air");
            public static readonly Offset Wood      = new(EBossIndex.Wood, "Wood");
            public static readonly Offset Bubble    = new(EBossIndex.Bubble, "Bubble");
            public static readonly Offset Quick     = new(EBossIndex.Quick, "Quick");
            public static readonly Offset Flash     = new(EBossIndex.Flash, "Flash");
            public static readonly Offset Metal     = new(EBossIndex.Metal, "Metal");
            public static readonly Offset Crash     = new(EBossIndex.Crash, "Crash");

        static Offset()
            {
                Offsets = new Dictionary<EBossIndex, Offset>()
                {
                    { EBossIndex.Dragon  , Dragon  },
                    { EBossIndex.Guts    , Guts    },
                    { EBossIndex.Machine , Machine },
                    { EBossIndex.Alien   , Alien   },
                    { EBossIndex.Heat    , Heat    },
                    { EBossIndex.Air     , Air     },
                    { EBossIndex.Wood    , Wood    },
                    { EBossIndex.Bubble  , Bubble  },
                    { EBossIndex.Quick   , Quick   },
                    { EBossIndex.Flash   , Flash   },
                    { EBossIndex.Metal   , Metal   },
                    { EBossIndex.Crash   , Crash   },
                };
            }

            private Offset(EBossIndex offset, String name)
            {
                this.Name = name;
                this.mValue = offset;
            }

            public static implicit operator Int32 (Offset offset)
            {
                return offset.Value;
            }

            public static implicit operator Offset(EBossIndex offset)
            {
                return Offsets[offset];
            }

            public override String ToString()
            {
                return Name;
            }
        }


    }
}
