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

        public static Dictionary<Int32, EDmgVsBoss> Addresses;

        //Japanese
        public static readonly EDmgVsBoss Buster          = new EDmgVsBoss(EWeaponIndex.Buster, 0x02E933, "Buster");
        public static readonly EDmgVsBoss AtomicFire      = new EDmgVsBoss(EWeaponIndex.Heat, 0x02E941, "Atomic Fire");
        public static readonly EDmgVsBoss AirShooter      = new EDmgVsBoss(EWeaponIndex.Air, 0x02E94F, "Air Shooter");
        public static readonly EDmgVsBoss LeafShield      = new EDmgVsBoss(EWeaponIndex.Wood, 0x02E95D, "Leaf Shield");
        public static readonly EDmgVsBoss BubbleLead      = new EDmgVsBoss(EWeaponIndex.Bubble, 0x02E96B, "Bubble Lead");
        public static readonly EDmgVsBoss QuickBoomerang  = new EDmgVsBoss(EWeaponIndex.Quick, 0x02E979, "Quick Boomerang");
        public static readonly EDmgVsBoss TimeStopper     = new EDmgVsBoss(EWeaponIndex.Flash, 0x02C049, "Time Stopper");
        public static readonly EDmgVsBoss MetalBlade      = new EDmgVsBoss(EWeaponIndex.Metal, 0x02E995, "Metal Blade");
        public static readonly EDmgVsBoss ClashBomber     = new EDmgVsBoss(EWeaponIndex.Clash, 0x02E987, "Clash Bomber");
        
        //English
        public static readonly EDmgVsBoss U_DamageP = new EDmgVsBoss(EWeaponIndex.Buster, 0x2e952, "Buster");
        public static readonly EDmgVsBoss U_DamageH = new EDmgVsBoss(EWeaponIndex.Heat, 0x2e960, "Atomic Fire");
        public static readonly EDmgVsBoss U_DamageA = new EDmgVsBoss(EWeaponIndex.Air, 0x2e96e, "Air Shooter");
        public static readonly EDmgVsBoss U_DamageW = new EDmgVsBoss(EWeaponIndex.Wood, 0x2e97c, "Leaf Shield");
        public static readonly EDmgVsBoss U_DamageB = new EDmgVsBoss(EWeaponIndex.Bubble, 0x2e98a, "Bubble Lead");
        public static readonly EDmgVsBoss U_DamageQ = new EDmgVsBoss(EWeaponIndex.Quick, 0x2e998, "Quick Boomerang");
        public static readonly EDmgVsBoss U_DamageF = new EDmgVsBoss(EWeaponIndex.Flash, 0x2C049, "Time Stopper");
        public static readonly EDmgVsBoss U_DamageM = new EDmgVsBoss(EWeaponIndex.Metal, 0x2e9b4, "Metal Blade");
        public static readonly EDmgVsBoss U_DamageC = new EDmgVsBoss(EWeaponIndex.Clash, 0x2e9a6, "Clash Bomber");

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
            Dictionary<EWeaponIndex, EDmgVsBoss> tables = new Dictionary<EWeaponIndex, EDmgVsBoss>();

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
            tables.Add(EWeaponIndex.Clash, U_DamageC);
            return tables;
        }

        public static EBossIndex GetBossIndexFromWeaponIndex(EWeaponIndex in_WeaponIndex)
        {
            EBossIndex index;
            switch (in_WeaponIndex)
            {
                case EWeaponIndex.Heat:
                    index = EBossIndex.Heat;
                    break;
                case EWeaponIndex.Air:
                    index = EBossIndex.Air;
                    break;
                case EWeaponIndex.Wood:
                    index = EBossIndex.Wood;
                    break;
                case EWeaponIndex.Bubble:
                    index = EBossIndex.Bubble;
                    break;
                case EWeaponIndex.Quick:
                    index = EBossIndex.Quick;
                    break;
                case EWeaponIndex.Flash:
                    index = EBossIndex.Flash;
                    break;
                case EWeaponIndex.Metal:
                    index = EBossIndex.Metal;
                    break;
                case EWeaponIndex.Clash:
                    index = EBossIndex.Clash;
                    break;
                default:
                    throw new IndexOutOfRangeException();
            };
            return index;
        }

        public static EWeaponIndex GetWeaponIndexFromBossIndex(EBossIndex in_BossIndex)
        {
            EWeaponIndex index;
            switch (in_BossIndex)
            {
                case EBossIndex.Heat:
                    index = EWeaponIndex.Heat;
                    break;
                case EBossIndex.Air:
                    index = EWeaponIndex.Air;
                    break;
                case EBossIndex.Wood:
                    index = EWeaponIndex.Wood;
                    break;
                case EBossIndex.Bubble:
                    index = EWeaponIndex.Bubble;
                    break;
                case EBossIndex.Quick:
                    index = EWeaponIndex.Quick;
                    break;
                case EBossIndex.Flash:
                    index = EWeaponIndex.Flash;
                    break;
                case EBossIndex.Metal:
                    index = EWeaponIndex.Metal;
                    break;
                case EBossIndex.Clash:
                    index = EWeaponIndex.Clash;
                    break;
                default:
                    throw new IndexOutOfRangeException();
            };
            return index;
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

            public EBossIndex Value
            {
                get; private set;
            }

            public static Dictionary<EBossIndex, Offset> Offsets;

            public static readonly Offset Dragon    = new Offset(EBossIndex.Dragon, "Dragon");
            public static readonly Offset Guts      = new Offset(EBossIndex.Guts, "Guts");
            public static readonly Offset Machine   = new Offset(EBossIndex.Machine, "Machine");
            public static readonly Offset Alien     = new Offset(EBossIndex.Alien, "Alien");
            public static readonly Offset Heat      = new Offset(EBossIndex.Heat, "Heat");
            public static readonly Offset Air       = new Offset(EBossIndex.Air, "Air");
            public static readonly Offset Wood      = new Offset(EBossIndex.Wood, "Wood");
            public static readonly Offset Bubble    = new Offset(EBossIndex.Bubble, "Bubble");
            public static readonly Offset Quick     = new Offset(EBossIndex.Quick, "Quick");
            public static readonly Offset Flash     = new Offset(EBossIndex.Flash, "Flash");
            public static readonly Offset Metal     = new Offset(EBossIndex.Metal, "Metal");
            public static readonly Offset Clash     = new Offset(EBossIndex.Clash, "Clash");

        static Offset()
            {
                Offsets = new Dictionary<EBossIndex, Offset>()
                {
                    { Dragon.Value  , Dragon  },
                    { Guts.Value    , Guts    },
                    { Machine.Value , Machine },
                    { Alien.Value   , Alien   },
                    { Heat.Value    , Heat    },
                    { Air.Value     , Air     },
                    { Wood.Value    , Wood    },
                    { Bubble.Value  , Bubble  },
                    { Quick.Value   , Quick   },
                    { Flash.Value   , Flash   },
                    { Metal.Value   , Metal   },
                    { Clash.Value   , Clash   },
                };
            }

            private Offset(EBossIndex offset, String name)
            {
                this.Name = name;
                this.Value = offset;
            }

            public static implicit operator EBossIndex (Offset offset)
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
