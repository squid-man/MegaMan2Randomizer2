using MM2RandoLib;
using MM2RandoLib.Utilities;
using MM2Randomizer.Random;
using MM2Randomizer.Settings;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MM2Randomizer
{
    public static class RandomMM2
    {
        static RandomMM2()
        {
            BasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            AssemblyVersion = Assembly.GetExecutingAssembly().GetVersion() ?? new Version();
            AssemblyVersionString = Assembly.GetExecutingAssembly().GetVersionString();

            int endIdx = AssemblyVersionString.IndexOfAny(['-', '+']);
            if (endIdx < 0)
                endIdx = AssemblyVersionString.Length;

            BaseAssemblyVersionString = AssemblyVersionString[..endIdx];
        }

        /// <summary>
        /// Perform the randomization based on the seed and user-provided settings, and then
        /// generate the new ROM.
        /// </summary>
        public static async Task<RandomizationContext> RandomizerCreate(
            RandomizationSettings in_Settings, 
            IPlatformServices in_PlatformServices, 
            byte[] in_Rom,
            IProgress<string?> in_Progress,
            CancellationToken in_CancellationToken)
        {
            ISeed seed = new PcgSeed(in_Settings.SeedString);
            var ctx = new RandomizationContext(in_Settings, seed, in_PlatformServices, in_Rom, in_Progress, in_CancellationToken);
            await ctx.Initialize();

            return ctx;
        }

        static public string BasePath { get; }

        static public Version AssemblyVersion { get; }

        static public string AssemblyVersionString { get; }

        static public string BaseAssemblyVersionString { get; }
    }
}