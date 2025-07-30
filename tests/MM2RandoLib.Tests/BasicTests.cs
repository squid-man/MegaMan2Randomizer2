using MM2Randomizer;
using MM2Randomizer.Settings;
using System;
using System.IO;

namespace MM2RandoLib.Tests
{
    public class BasicTests
    {
        /// <summary>
        /// Verify that the randomizer can successfully create a ROM. This is necessary because assembly modules will not be compiled until a ROM is created.
        /// </summary>
        [Fact]
        public void CanBuildRom()
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

                File.WriteAllBytes(settings.RomSourcePath, new byte[0x80010]);

                RandomizationContext ctx;
                RandomMM2.RandomizerCreate(settings, out ctx);

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

            presets.ValidatePresets(settings.AllOptions);
        }
    }
}