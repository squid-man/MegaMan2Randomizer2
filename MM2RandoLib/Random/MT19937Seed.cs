using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM2Randomizer.Extensions;
using Troschuetz.Random.Generators;
using System.Security.Cryptography;

namespace MM2Randomizer.Random
{
    /// <summary>
    /// Provides basic randomization methods
    /// </summary>
    public sealed class MT19937Seed : ISeed
    {
        //
        // Constructors
        //

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Private constructors are used by the SeedFactory.")]
        private MT19937Seed(String in_SeedString)
        {
            if (null == in_SeedString)
            {
                throw new ArgumentNullException(nameof(in_SeedString));
            }

            this.mSeedString = in_SeedString;

            Byte[] seedStringArray = Encoding.ASCII.GetBytes(in_SeedString);
            Byte[] hashArray = MT19937Seed.mSha512.ComputeHash(seedStringArray);

            UInt32[]? seed = hashArray.ToUInt32Array();
            this.mSeed = seed;

            String alpha26Seed = in_SeedString.ToUInt64Hash().ToAlphaBase26();
            this.mSeedAlphaBase26 = alpha26Seed;

            MT19937Generator random = new MT19937Generator(seed);
            this.mRandom = random;
        }

        private MT19937Seed()
        {
            // Get a random number for the seed
            MT19937Generator r = new MT19937Generator();
            StringBuilder sb = new StringBuilder();

            const Int32 SEED_STRING_LENGTH = 20;
            const String SEED_STRING_CHARACTERS = "1234567890";

            for (Int32 index = 0; index < SEED_STRING_LENGTH; ++index)
            {
                sb.Append(SEED_STRING_CHARACTERS[r.Next(0, SEED_STRING_CHARACTERS.Length)]);
            }

            String seedString = sb.ToString();
            this.mSeedString = seedString;

            Byte[] seedStringArray = Encoding.ASCII.GetBytes(seedString);
            Byte[] hashArray = MT19937Seed.mSha512.ComputeHash(seedStringArray);

            UInt32[]? seed = hashArray.ToUInt32Array();
            this.mSeed = seed;

            String alpha26Seed = seedString.ToUInt64Hash().ToAlphaBase26();
            this.mSeedAlphaBase26 = alpha26Seed;

            MT19937Generator random = new MT19937Generator(seed);
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
            MT19937Generator random = new MT19937Generator(this.mSeed);
            this.mRandom = random;
        }

        public void Next()
        {
            this.mRandom.Next();
        }

        // Boolean Methods
        public Boolean NextBoolean()
        {
            return this.mRandom.NextBoolean();
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

        private MT19937Generator mRandom;
        private String mSeedAlphaBase26;
        private String mSeedString;
        private UInt32[]? mSeed;

        private static readonly SHA512 mSha512 = SHA512.Create();
    }
}
