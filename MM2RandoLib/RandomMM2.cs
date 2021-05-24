using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;
using MM2Randomizer.Randomizers;
using MM2Randomizer.Randomizers.Colors;
using MM2Randomizer.Randomizers.Enemies;
using MM2Randomizer.Randomizers.Stages;
using MM2Randomizer.Utilities;

namespace MM2Randomizer
{
    public static class RandomMM2
    {
        //
        // Public Data Members
        //

        //public static Random Random;
        //public static Random RNGCosmetic;
        //private static Patch Patch;
        //???public static RandoSettings Settings;
        private const String TEMPORARY_FILE_NAME = "temp.nes";
        //private static String RecentlyCreatedFileName = "";


        //================
        // "CORE" MODULES
        //================

        // NOTE: Just in case, link RStages, RWeaponGet, and RTeleporter into one "Core Randomizer" module
        // Their interdependencies are too risky to separate as options, and likely nobody will want to customize this part anyways.
        // Random portrait locations on stage select
        private static readonly RStages randomStages = new RStages();
        // Random weapon awarded from each stage
        // WARNING: May be dependent on RTeleporters, verify?
        // WARNING: May be dependent on RStages
        private static readonly RWeaponGet randomWeaponGet = new RWeaponGet();
        // Random teleporter destinations in Wily 5
        private static readonly RTeleporters randomTeleporters = new RTeleporters();


        ///=========================
        /// "GAMEPLAY SEED" MODULES
        ///=========================

        // Caution: RWeaknesses depends on this
        private static RWeaponBehavior randomWeaponBehavior = new RWeaponBehavior();
        // Depends on RWeaponBehavior (ammo), can use default values
        private static RWeaknesses randomWeaknesses = new RWeaknesses();
        // Independent
        private static RBossAI randomBossAI = new RBossAI();
        // Independent
        private static RItemGet randomItemGet = new RItemGet();
        // Independent
        private static REnemies randomEnemies = new REnemies();
        // Independent
        private static REnemyWeaknesses randomEnemyWeakness = new REnemyWeaknesses();
        // Caution: RText depends on this, but default values will be used if not enabled.
        private static RBossRoom randomBossInBossRoom;
        // Independent
        private static RTilemap randomTilemap = new RTilemap();


        ///==========================
        /// "COSMETIC SEED" MODULES
        ///==========================

        // Independent
        private static RColors randomColors = new RColors();
        // Independent
        private static RMusic randomMusic = new RMusic();
        // Caution: Depends on RBossRoom, but can use default values if its not enabled.
        private static RText rWeaponNames = new RText();


        /// <summary>
        /// Perform the randomization based on the seed and user-provided settings, and then
        /// generate the new ROM.
        /// </summary>
        public static void RandomizerCreate(Settings in_Settings, out RomInfo out_RomInfo)
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

            // List of randomizer modules to use; will add modules based on checkbox states
            List<IRandomizer> randomizers = new List<IRandomizer>();
            List<IRandomizer> cosmeticRandomizers = new List<IRandomizer>();


            // Add randomizers according to each flag
            ///==========================
            /// "GAMEPLAY SEED" MODULES
            ///==========================

            if (in_Settings.EnableRandomizationOfRobotMasterStageSelection)
            {
                randomizers.Add(randomStages);
            }

            if (in_Settings.EnableRandomizationOfSpecialWeaponReward)
            {
                randomizers.Add(randomWeaponGet);
            }

            if (in_Settings.EnableRandomizationOfSpecialWeaponBehavior)
            {
                randomizers.Add(randomWeaponBehavior);
            }

            if (in_Settings.EnableRandomizationOfBossWeaknesses)
            {
                randomizers.Add(randomWeaknesses);
            }

            if (in_Settings.EnableRandomizationOfRobotMasterBehavior)
            {
                randomizers.Add(randomBossAI);
            }

            if (in_Settings.EnableRandomizationOfSpecialItemLocations)
            {
                randomizers.Add(randomItemGet);
            }

            if (in_Settings.EnableRandomizationOfRefightTeleporters)
            {
                randomizers.Add(randomTeleporters);
            }

            if (in_Settings.EnableRandomizationOfEnemySpawns)
            {
                randomizers.Add(randomEnemies);
            }

            if (in_Settings.EnableRandomizationOfEnemyWeaknesses)
            {
                randomizers.Add(randomEnemyWeakness);
            }

            if (in_Settings.EnableRandomizationOfRobotMasterLocations)
            {
                randomizers.Add(randomBossInBossRoom);
            }

            if (in_Settings.EnableRandomizationOfFalseFloors)
            {
                randomizers.Add(randomTilemap);
            }

            ///==========================
            /// "COSMETIC SEED" MODULES
            ///==========================
            if (in_Settings.EnableRandomizationOfColorPalettes)
            {
                randomColors.DisableFlashingEffects = in_Settings.DisableFlashingEffects;
                cosmeticRandomizers.Add(randomColors);
            }

            if (in_Settings.EnableRandomizationOfMusicTracks)
            {
                cosmeticRandomizers.Add(randomMusic);
            }

            if (in_Settings.EnableRandomizationOfSpecialWeaponNames)
            {
                cosmeticRandomizers.Add(rWeaponNames);
            }


            // Create randomization patch
            Patch patch = new Patch();

            // In tournament mode, offset the seed by 1 call, making seeds mode-dependent
            if (in_Settings.EnableSpoilerFreeMode)
            {
                seed.Next();
            }

            // Conduct randomization of Gameplay Modules
            foreach (IRandomizer randomizer in randomizers)
            {
                randomizer.Randomize(patch, in_Settings, seed);
                Debug.WriteLine(randomizer);
            }

            // Conduct randomization of Cosmetic Modules
            foreach (IRandomizer cosmetic in cosmeticRandomizers)
            {
                cosmetic.Randomize(patch, in_Settings, seed);
                Debug.WriteLine(cosmetic);
            }

            // Apply additional required incidental modifications
            if (in_Settings.EnableRandomizationOfRobotMasterStageSelection || in_Settings.EnableRandomizationOfSpecialWeaponNames)
            {
                MiscHacks.FixPortraits(patch, in_Settings.EnableRandomizationOfRobotMasterStageSelection, randomStages, in_Settings.EnableRandomizationOfSpecialWeaponNames, randomWeaponGet);
                MiscHacks.FixWeaponLetters(patch, randomWeaponGet, randomStages, rWeaponNames);
            }

            if (in_Settings.EnableRandomizationOfEnemySpawns)
            {
                MiscHacks.FixM445PaletteGlitch(patch);
            }

            // Apply final optional gameplay modifications
            if (in_Settings.EnableFasterCutsceneText)
            {
                MiscHacks.SetFastWeaponGetText(patch);
                MiscHacks.SetFastReadyText(patch);
                MiscHacks.SetFastWilyMap(patch);
                MiscHacks.SkipItemGetPages(patch);
            }

            if (in_Settings.EnableBurstChaserMode)
            {
                MiscHacks.SetBurstChaser(patch);
            }

            if (in_Settings.DisableFlashingEffects)
            {
                MiscHacks.DisableScreenFlashing(patch, in_Settings);
            }

            MiscHacks.SetHitPointChargingSpeed(patch, in_Settings.HitPointRefillSpeed);
            MiscHacks.SetWeaponEnergyChargingSpeed(patch, in_Settings.WeaponEnergyRefillSpeed);
            MiscHacks.SetEnergyTankChargingSpeed(patch, in_Settings.EnergyTankRefillSpeed);
            MiscHacks.SetRobotMasterEnergyChargingSpeed(patch, in_Settings.RobotMasterEnergyRefillSpeed);
            MiscHacks.SetCastleBossEnergyChargingSpeed(patch, in_Settings.CastleBossEnergyRefillSpeed);

            MiscHacks.DrawTitleScreenChanges(patch, seed.Identifier, in_Settings);
            MiscHacks.SetWily5NoMusicChange(patch);
            MiscHacks.NerfDamageValues(patch);
            MiscHacks.SetETankKeep(patch);
            MiscHacks.PreventETankUseAtFullLife(patch);
            MiscHacks.SetFastBossDefeatTeleport(patch);

            if (in_Settings.EnableUnderwaterLagReduction)
            {
                MiscHacks.ReduceUnderwaterLag(patch);
            }

            if (in_Settings.DisableDelayScrolling)
            {
                MiscHacks.DisableDelayScroll(patch);
            }

            // Create file name based on seed and game region
            String newFileName = $"MM2-RNG-{seed.Identifier} ({seed.SeedString}).nes";

            //File.Copy(Settings.SourcePath, TempFileName, true);
            //using (Stream stream = assembly.GetManifestResourceStream("MM2Randomizer.Resources.MM2.nes"))
            // Load user provided ROM
            using (Stream stream = new FileStream(in_Settings.RomSourcePath, FileMode.Open, FileAccess.Read))
            {
                using (Stream output = File.OpenWrite(RandomMM2.TEMPORARY_FILE_NAME))
                {
                    stream.CopyTo(output);
                }
            }

            // Apply pre-patch changes via IPS patch (manual title screen, stage select, stage changes, player sprite)
            patch.ApplyIPSPatch(RandomMM2.TEMPORARY_FILE_NAME, Properties.Resources.mm2rng_musicpatch);
            patch.ApplyIPSPatch(RandomMM2.TEMPORARY_FILE_NAME, Properties.Resources.mm2rng_prepatch);
            MiscHacks.SetNewMegaManSprite(patch, RandomMM2.TEMPORARY_FILE_NAME, in_Settings.PlayerSprite);

            // Apply patch with randomized content
            patch.ApplyRandoPatch(RandomMM2.TEMPORARY_FILE_NAME);

            // If a file of the same seed already exists, delete it
            if (File.Exists(newFileName))
            {
                File.Delete(newFileName);
            }

            // Finish the copy/rename and open Explorer at that location
            File.Move(RandomMM2.TEMPORARY_FILE_NAME, newFileName);

            out_RomInfo = new RomInfo(seed, newFileName);
        }

        static public Version AssemblyVersion
        {
            get
            {
                return Assembly.GetAssembly(typeof(RandomMM2)).GetName().Version;
            }
        }
    }
}