using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM2Randomizer.Extensions;

namespace MM2Randomizer.Random
{
    /// <summary>
    /// Provides basic randomization methods
    /// </summary>
    public sealed class DefaultSeed : ISeed
    {
        //
        // Constructors
        //

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Private constructors are used by the SeedFactory.")]
        private DefaultSeed(String in_SeedString)
        {
            if (null == in_SeedString)
            {
                throw new ArgumentNullException(nameof(in_SeedString));
            }

            this.mSeedString = in_SeedString;

            Int32 int32Seed = in_SeedString.ToInt32Hash();
            this.mSeed = int32Seed;

            String alpha26Seed = int32Seed.ToAlphaBase26();
            this.mSeedAlphaBase26 = alpha26Seed;

            System.Random random = new System.Random(int32Seed);
            this.mRandom = random;
        }

        private DefaultSeed()
        {
            // Get a random number for the seed
            System.Random r = new System.Random();
            StringBuilder sb = new StringBuilder();

            const Int32 SEED_STRING_LENGTH = 20;
            const String SEED_STRING_CHARACTERS = "1234567890";

            for (Int32 index = 0; index < SEED_STRING_LENGTH; ++index)
            {
                sb.Append(SEED_STRING_CHARACTERS[r.Next(0, SEED_STRING_CHARACTERS.Length)]);
            }

            String seedString = sb.ToString();
            this.mSeedString = seedString;

            Int32 int32Seed = seedString.ToInt32Hash();
            this.mSeed = int32Seed;

            String alpha26Seed = int32Seed.ToAlphaBase26();
            this.mSeedAlphaBase26 = alpha26Seed;

            System.Random random = new System.Random(int32Seed);
            this.mRandom = random;
        }


        //
        // Properties
        //

        public String SeedString
        {
            get
            {
                return this.mSeedString;
            }
        }


        public String Identifier
        {
            get
            {
                return this.mSeedAlphaBase26;
            }
        }


        //
        // Public Methods
        //

        public void Reset()
        {
            System.Random random = new System.Random(this.mSeed);
            this.mRandom = random;
        }

        public void Next()
        {
            this.mRandom.Next();
        }

        // Boolean Methods
        public Boolean NextBoolean()
        {
            return (this.mRandom.Next() & 1) > 0;
        }

        // UInt8 Methods
        public Byte NextUInt8()
        {
            return (Byte)this.mRandom.Next(0, Byte.MaxValue + 1);
        }

        public Byte NextUInt8(Int32 in_MaxValue)
        {
            if (in_MaxValue <= 0 || in_MaxValue > Byte.MaxValue)
            {
                return this.NextUInt8();
            }
            else
            {
                return (Byte)this.mRandom.Next(in_MaxValue);
            }
        }

        public Byte NextUInt8(Int32 in_MinValue, Int32 in_MaxValue)
        {
            if (in_MinValue > in_MaxValue)
            {
                throw new ArgumentOutOfRangeException($@"{in_MaxValue} must be greater than or equal to {in_MinValue}");
            }

            in_MinValue = Math.Max(0, in_MinValue);
            in_MaxValue = Math.Min(Byte.MaxValue + 1, in_MaxValue);

            return (Byte)this.mRandom.Next(in_MinValue, in_MaxValue);
        }


        // Int32 Methods
        public Int32 NextInt32()
        {
            return this.mRandom.Next();
        }

        public Int32 NextInt32(Int32 in_MaxValue)
        {
            return this.mRandom.Next(in_MaxValue);
        }

        public Int32 NextInt32(Int32 in_MinValue, Int32 in_MaxValue)
        {
            return this.mRandom.Next(in_MinValue, in_MaxValue);
        }

        public Double NextDouble()
        {
            return this.mRandom.NextDouble();
        }


        // IEnumerator Methods
        public Object? NextArrayElement(Array in_Array)
        {
            Int32 count = in_Array.Length;
            Int32 index = this.mRandom.Next(count);
            return in_Array.GetValue(index);
        }


        // IEnumerable Methods
        public T NextElement<T>(IEnumerable<T> in_Elements)
        {
            Int32 count = in_Elements.Count();
            Int32 index = this.mRandom.Next(count);
            return in_Elements.ElementAt(index);
        }

        public IList<T> Shuffle<T>(IEnumerable<T> in_List)
        {
            return in_List.OrderBy(x => this.mRandom.Next()).ToList();
        }


        //
        // Private Data Members
        //

        private System.Random mRandom;
        private String mSeedAlphaBase26;
        private String mSeedString;
        private Int32 mSeed;
    }
}
