using System;
using System.Reflection;
using MM2Randomizer.Random;
using MM2Randomizer.Settings;

namespace MM2Randomizer
{
    public static class RandomMM2
    {
        /// <summary>
        /// Perform the randomization based on the seed and user-provided settings, and then
        /// generate the new ROM.
        /// </summary>
        public static void RandomizerCreate(RandomizationSettings in_Settings, out RandomizationContext out_Context)
        {
            ISeed seed;

            // Initialize the seed
            if (null == in_Settings.SeedString)
            {
                seed = SeedFactory.Create(GeneratorType.MT19937);
            }
            else
            {
                seed = SeedFactory.Create(GeneratorType.MT19937, in_Settings.SeedString);
            }

            out_Context = new RandomizationContext(in_Settings, seed);
            out_Context.Initialize();
        }


        static public Version AssemblyVersion
        {
            get
            {
                return Assembly.GetAssembly(typeof(RandomMM2))?.GetName().Version ?? new Version(0, 0, 0, 0);
            }
        }
    }
}