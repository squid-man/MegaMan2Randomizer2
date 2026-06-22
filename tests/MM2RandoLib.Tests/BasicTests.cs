using js65;
using MM2RandoLib;
using MM2Randomizer;
using MM2Randomizer.Settings;
using RandomizerHost.Desktop;
using System;
using System.IO;

namespace MM2RandoLib.Tests
{
    public class DummyProgress : IProgress<string?>
    {
        public void Report(string? value)
        { }
    }

    public class BasicTests
    {
        /// <summary>
        /// Verify that the randomizer can successfully create a ROM. This is necessary because assembly modules will not be compiled until a ROM is created.
        /// </summary>
        [Fact]
        public async Task CanBuildRom()
        {
            var curDir = Directory.GetCurrentDirectory();
            var tempDir = Directory.CreateTempSubdirectory("mm2r");
            try
            {
                Directory.SetCurrentDirectory(tempDir.FullName);

                RandomizationSettings settings = new();
                settings.RomSourcePath = "mm2.nes";
                settings.SeedString = null;

                // Enable as many assembly modules as possible
                settings.GameplayOptions.MercilessMode.BaseValue = true;
                settings.GameplayOptions.BurstChaserMode.BaseValue = true;
                settings.GameplayOptions.FasterCutsceneText.BaseValue = true;
                settings.QualityOfLifeOptions.DisablePauseLock.BaseValue = true;
                settings.QualityOfLifeOptions.EnableBirdEggFix.BaseValue = true;
                settings.QualityOfLifeOptions.DisableFlashingEffects.BaseValue = true;

                RandomizationContext ctx = await RandomMM2.RandomizerCreate(
                    settings, 
                    new DesktopPlatformServices(),
                    new byte[0x80010],
                    new DummyProgress(),
                    new CancellationToken());

                return;
            }
            finally
            {
                try
                {
                    Directory.SetCurrentDirectory(curDir);
                    tempDir.Delete(true);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Verify that all settings presets are complete.
        /// </summary>
        [Fact]
        public void ValidatePresets()
        {
            RandomizationSettings settings = new();
            SettingsPresets presets = new(settings);

            presets.ValidatePresets(settings);
        }
    }
}