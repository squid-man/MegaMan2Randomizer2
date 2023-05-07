using System;
using System.Collections.Generic;

namespace MM2Randomizer.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EDmgVsEnemy
    {
        public Int32 Address
        {
            get; private set;
        }

        public String WeaponName
        {
            get; private set;
        }

        public EWeaponIndex Index
        {
            get; private set;
        }

        public static Dictionary<Int32, EDmgVsEnemy> Addresses { get; }

        public static readonly EDmgVsEnemy DamageP = new(EWeaponIndex.Buster, 0x03E9A8, "Buster");
        public static readonly EDmgVsEnemy DamageH = new(EWeaponIndex.Heat, 0x03EA24, "Atomic Fire");
        public static readonly EDmgVsEnemy DamageA = new(EWeaponIndex.Air, 0x03EA9C, "Air Shooter");
        public static readonly EDmgVsEnemy DamageW = new(EWeaponIndex.Wood, 0x03EB14, "Leaf Shield");
        public static readonly EDmgVsEnemy DamageB = new(EWeaponIndex.Bubble, 0x03EB8C, "Bubble Lead");
        public static readonly EDmgVsEnemy DamageQ = new(EWeaponIndex.Quick, 0x03EC04, "Quick Boomerang");
        public static readonly EDmgVsEnemy DamageM = new(EWeaponIndex.Metal, 0x03ECF4, "Metal Blade");
        public static readonly EDmgVsEnemy DamageC = new(EWeaponIndex.Crash, 0x03EC7C, "Crash Bomber");

        static EDmgVsEnemy()
        {
            Addresses = new Dictionary<Int32, EDmgVsEnemy>()
            {
                { DamageP.Address, DamageP },
                { DamageH.Address, DamageH },
                { DamageA.Address, DamageA },
                { DamageW.Address, DamageW },
                { DamageB.Address, DamageB },
                { DamageQ.Address, DamageQ },
                { DamageM.Address, DamageM },
                { DamageC.Address, DamageC },
            };
        }

        private EDmgVsEnemy(EWeaponIndex index, Int32 address, String name)
        {
            this.Index = index;
            this.Address = address;
            this.WeaponName = name;
        }

        public static implicit operator Int32 (EDmgVsEnemy eDmgVsEnemy)
        {
            return eDmgVsEnemy.Address;
        }

        public static implicit operator EDmgVsEnemy(Int32 eDmgVsEnemy)
        {
            return Addresses[eDmgVsEnemy];
        }

        public static Boolean operator ==(EDmgVsEnemy a, EDmgVsEnemy b)
        {
            return (a.Address == b.Address);
        }

        public static Boolean operator !=(EDmgVsEnemy a, EDmgVsEnemy b)
        {
            return (a.Address != b.Address);
        }
        public static Boolean operator ==(Int32 a, EDmgVsEnemy b)
        {
            return (a == b.Address);
        }

        public static Boolean operator !=(Int32 a, EDmgVsEnemy b)
        {
            return (a != b.Address);
        }

        public override Boolean Equals(Object? obj)
        {
            return base.Equals(obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Dictionary<EWeaponIndex, EDmgVsEnemy> GetTables(Boolean includeBuster)
        {
            Dictionary<EWeaponIndex, EDmgVsEnemy> dict = new()
            {
                { EWeaponIndex.Heat, DamageH },
                { EWeaponIndex.Air, DamageA },
                { EWeaponIndex.Wood, DamageW },
                { EWeaponIndex.Bubble, DamageB },
                { EWeaponIndex.Quick, DamageQ },
                { EWeaponIndex.Metal, DamageM },
                { EWeaponIndex.Crash, DamageC },
            };

            if (includeBuster)
            {
                dict.Add(EWeaponIndex.Buster, DamageP);
            }

            return dict;
        }

        /// <summary>
        /// 
        /// </summary>
        public class Offset
        {
            public Int32 Value
            {
                get; private set;
            }

            public static readonly Offset PicopicoKun;
            public static readonly Offset Press;
            public static readonly Offset CrashBarrier_Other;
            public static readonly Offset CrashBarrier_W4;
            public static readonly Offset Boobeam;

            static Offset()
            {
                CrashBarrier_Other  = new Offset(0x2D);
                Press               = new Offset(0x30);
                CrashBarrier_W4     = new Offset(0x57);
                PicopicoKun         = new Offset(0x6A);
                Boobeam             = new Offset(0x6D);
            }

            private Offset(Int32 offset)
            {
                this.Value = offset;
            }

            public static implicit operator Int32 (Offset offset)
            {
                return offset.Value;
            }

            public static Boolean operator ==(Offset a, Offset b)
            {
                return (a.Value == b.Value);
            }

            public static Boolean operator !=(Offset a, Offset b)
            {
                return (a.Value != b.Value);
            }

            public static Boolean operator ==(Int32 a, Offset b)
            {
                return (a == b.Value);
            }

            public static Boolean operator !=(Int32 a, Offset b)
            {
                return (a != b.Value);
            }

            public override Boolean Equals(Object? obj)
            {
                return base.Equals(obj);
            }

            public override Int32 GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}
