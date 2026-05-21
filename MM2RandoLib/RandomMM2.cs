using MM2RandoLib;
using MM2RandoLib.Utilities;
using MM2Randomizer.Random;
using MM2Randomizer.Settings;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MM2Randomizer
{
    public static class RandomMM2
    {
        /// <summary>
        /// Perform the randomization based on the seed and user-provided settings, and then
        /// generate the new ROM.
        /// </summary>
        public static async Task<RandomizationContext> RandomizerCreate(
            RandomizationSettings in_Settings, 
            IPlatformServices in_PlatformServices, 
            byte[] in_Rom)
        {
            ISeed seed = new PcgSeed(in_Settings.SeedString);
            var ctx = new RandomizationContext(in_Settings, seed, in_PlatformServices, in_Rom);
            await ctx.Initialize();

            return ctx;
        }

        static public Version AssemblyVersion
            => Assembly.GetExecutingAssembly().GetVersion() ?? new Version();

        static public string AssemblyVersionString
            => Assembly.GetExecutingAssembly().GetVersionString();
    }
}