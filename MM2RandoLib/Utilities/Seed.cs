using System;
using System.Collections.Generic;
using System.Text;
using MM2Randomizer.Extensions;
using System.Linq;

namespace MM2Randomizer.Utilities
{
    /// <summary>
    /// Provides basic randomization methods
    /// </summary>
    public sealed class Seed
    {
        //
        // Constructors
        //

        private Seed()
        {
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

        /// <summary>
        /// Shuffle the elements of the specified list.
        /// </summary>
        public IList<T> Shuffle<T>(IList<T> in_List)
        {
            return in_List.OrderBy(x => this.mRandom.Next()).ToList();
        }



        //
        // Public Static Methods
        //

        /// <summary>
        /// Create a new Seed from the given input String.
        /// </summary>
        public static Seed Create(String in_SeedString)
        {
            if (null == in_SeedString)
            {
                throw new ArgumentNullException(nameof(in_SeedString));
            }

            Int32 int32Seed = in_SeedString.ToInt32Hash();
            String alpha26Seed = int32Seed.ToAlphaBase26();
            Random random = new Random(int32Seed);

            Seed seed = new Seed();
            seed.mRandom = random;
            seed.mSeed = int32Seed;
            seed.mSeedAlphaBase26 = alpha26Seed;
            seed.mSeedString = in_SeedString;

            return seed;
        }


        /// <summary>
        /// Create a new random Seed.
        /// </summary>
        public static Seed Create()
        {
            // Get a random number for the seed
            Random r = new Random();
            String seedString = r.Next(Int32.MaxValue).ToString();

            Int32 int32Seed = seedString.ToInt32Hash();
            String alpha26Seed = int32Seed.ToAlphaBase26();
            Random random = new Random(int32Seed);

            Seed seed = new Seed();
            seed.mRandom = random;
            seed.mSeed = int32Seed;
            seed.mSeedAlphaBase26 = alpha26Seed;
            seed.mSeedString = seedString;

            return seed;
        }


        //
        // Private Data Members
        //

        private Random mRandom;
        private Int32 mSeed;
        private String mSeedAlphaBase26;
        private String mSeedString;
    }
}
