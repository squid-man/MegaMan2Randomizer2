using System;
using MM2Randomizer.Random;

namespace MM2Randomizer
{
    public class RomInfo
    {
        //
        // Constructors
        //
        public RomInfo(ISeed in_Seed, String in_FileName)
        {
            this.mSeed = in_Seed;
            this.mFileName = in_FileName;
        }


        //
        // Properties
        //

        public ISeed Seed
        {
            get
            {
                return this.mSeed;
            }
        }

        public String FileName
        {
            get
            {
                return this.mFileName;
            }
        }


        //
        // Private Data Members
        //

        private ISeed mSeed;
        private String mFileName;
    }
}
