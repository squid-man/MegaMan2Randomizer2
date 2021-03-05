using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        // Properties
        //

        public static ISeed Seed
        {
            get
            {
                return RandomMM2.mSeed;
            }
        }


        //
        // Public Data Members
        //

        //public static Random Random;
        //public static Random RNGCosmetic;
        public static Patch Patch;
        public static RandoSettings Settings;
        public static readonly String TempFileName = "temp.nes";
        public static String RecentlyCreatedFileName = "";

        public static RStages randomStages;
        public static RWeaponGet randomWeaponGet;
        public static RWeaponBehavior randomWeaponBehavior;
        public static RWeaknesses randomWeaknesses;
        public static RBossAI randomBossAI;
        public static RItemGet randomItemGet;
        public static RTeleporters randomTeleporters;
        public static REnemies randomEnemies;
        public static REnemyWeaknesses randomEnemyWeakness;
        public static RBossRoom randomBossInBossRoom;
        public static RTilemap randomTilemap;
        public static RColors randomColors;
        public static RMusic randomMusic;
        public static RText rWeaponNames;
        public static List<IRandomizer> Randomizers;
        public static List<IRandomizer> CosmeticRandomizers;

        /// <summary>
        /// Perform the randomization based on the seed and user-provided settings, and then
        /// generate the new ROM.
        /// </summary>
        public static String RandomizerCreate(String in_SeedString)
        {
            // Initialize the seed
            if (null == in_SeedString)
            {
                RandomMM2.mSeed = SeedFactory.Create(GeneratorType.MT19937);
            }
            else
            {
                RandomMM2.mSeed = SeedFactory.Create(GeneratorType.MT19937, in_SeedString);
            }

            //Random = new Random(RandomMM2.mSeed);
            //RNGCosmetic = new Random(RandomMM2.mSeed);

            // List of randomizer modules to use; will add modules based on checkbox states
            Randomizers = new List<IRandomizer>();
            CosmeticRandomizers = new List<IRandomizer>();


            ///==========================
            /// "CORE" MODULES
            ///==========================
            // NOTE: Just in case, link RStages, RWeaponGet, and RTeleporter into one "Core Randomizer" module
            // Their interdependencies are too risky to separate as options, and likely nobody will want to customize this part anyways.
            // Random portrait locations on stage select
            randomStages = new RStages();
            // Random weapon awarded from each stage
            // WARNING: May be dependent on RTeleporters, verify?
            // WARNING: May be dependent on RStages
            randomWeaponGet = new RWeaponGet();
            // Random teleporter destinations in Wily 5
            randomTeleporters = new RTeleporters();


            ///==========================
            /// "GAMEPLAY SEED" MODULES
            ///==========================
            // Caution: RWeaknesses depends on this
            randomWeaponBehavior = new RWeaponBehavior();

            // Depends on RWeaponBehavior (ammo), can use default values
            randomWeaknesses = new RWeaknesses();

            // Caution: RText depends on this, but default values will be used if not enabled.
            randomBossInBossRoom = new RBossRoom();

            // Independent
            randomBossAI = new RBossAI();

            // Independent
            randomItemGet = new RItemGet();

            // Independent
            randomEnemies = new REnemies();

            // Independent
            randomEnemyWeakness = new REnemyWeaknesses();

            // Independent
            randomTilemap = new RTilemap();


            ///==========================
            /// "COSMETIC SEED" MODULES
            ///==========================
            // Caution: Depends on RBossRoom, but can use default values if its not enabled.
            rWeaponNames = new RText();

            // Independent
            randomColors = new RColors(Settings.IsFlashingDisabled);

            // Independent
            randomMusic = new RMusic();



            // Add randomizers according to each flag
            ///==========================
            /// "GAMEPLAY SEED" MODULES
            ///==========================
            if (Settings.Is8StagesRandom)
            {
                Randomizers.Add(randomStages);
            }
            if (Settings.IsWeaponsRandom)
            {
                Randomizers.Add(randomWeaponGet);
            }
            if (Settings.IsWeaponBehaviorRandom)
            {
                Randomizers.Add(randomWeaponBehavior);
            }
            if (Settings.IsWeaknessRandom)
            {
                Randomizers.Add(randomWeaknesses);
            }
            if (Settings.IsBossAIRandom)
            {
                Randomizers.Add(randomBossAI);
            }
            if (Settings.IsItemsRandom)
            {
                Randomizers.Add(randomItemGet);
            }
            if (Settings.IsTeleportersRandom)
            {
                Randomizers.Add(randomTeleporters);
            }
            if (Settings.IsEnemiesRandom)
            {
                Randomizers.Add(randomEnemies);
            }
            if (Settings.IsEnemyWeaknessRandom)
            {
                Randomizers.Add(randomEnemyWeakness);
            }
            if (Settings.IsBossInBossRoomRandom)
            {
                Randomizers.Add(randomBossInBossRoom);
            }
            if (Settings.IsTilemapChangesEnabled)
            {
                Randomizers.Add(randomTilemap);
            }

            ///==========================
            /// "COSMETIC SEED" MODULES
            ///==========================
            if (Settings.IsColorsRandom)
            {
                CosmeticRandomizers.Add(randomColors);
            }
            if (Settings.IsBGMRandom)
            {
                CosmeticRandomizers.Add(randomMusic);
            }
            if (Settings.IsWeaponNamesRandom)
            {
                CosmeticRandomizers.Add(rWeaponNames);
            }


            // Create randomization patch
            Patch = new Patch();

            // In tournament mode, offset the seed by 1 call, making seeds mode-dependent
            if (Settings.IsSpoilerFree)
            {
                RandomMM2.mSeed.Next();
            }

            // Conduct randomization of Gameplay Modules
            foreach (IRandomizer randomizer in Randomizers)
            {
                randomizer.Randomize(Patch, RandomMM2.mSeed);
                Debug.WriteLine(randomizer);
            }

            // Conduct randomization of Cosmetic Modules
            foreach (IRandomizer cosmetic in CosmeticRandomizers)
            {
                cosmetic.Randomize(Patch, RandomMM2.mSeed);
                Debug.WriteLine(cosmetic);
            }

            // Apply additional required incidental modifications
            if (Settings.Is8StagesRandom || Settings.IsWeaponsRandom)
            {
                MiscHacks.FixPortraits(Patch, Settings.Is8StagesRandom, randomStages, Settings.IsWeaponsRandom, randomWeaponGet);
                MiscHacks.FixWeaponLetters(Patch, randomWeaponGet, randomStages, rWeaponNames);
            }
            if (Settings.IsEnemiesRandom)
            {
                MiscHacks.FixM445PaletteGlitch(Patch);
            }

            // Apply final optional gameplay modifications
            if (Settings.FastText)
            {
                MiscHacks.SetFastWeaponGetText(Patch);
                MiscHacks.SetFastReadyText(Patch);
                MiscHacks.SetFastWilyMap(Patch);
                MiscHacks.SkipItemGetPages(Patch);
            }

            if (Settings.BurstChaserMode)
            {
                MiscHacks.SetBurstChaser(Patch);
            }

            if (Settings.IsFlashingDisabled)
            {
                MiscHacks.DisableScreenFlashing(Patch, Settings);
            }

            MiscHacks.SetHitPointChargingSpeed(Patch, Settings.HitPointChargingSpeed);
            MiscHacks.SetWeaponEnergyChargingSpeed(Patch, Settings.WeaponEnergyChargingSpeed);
            MiscHacks.SetEnergyTankChargingSpeed(Patch, Settings.EnergyTankChargingSpeed);
            MiscHacks.SetRobotMasterEnergyChargingSpeed(Patch, Settings.RobotMasterEnergyChargingSpeed);
            MiscHacks.SetCastleBossEnergyChargingSpeed(Patch, Settings.CastleBossEnergyChargingSpeed);

            MiscHacks.DrawTitleScreenChanges(Patch, RandomMM2.mSeed.Identifier, Settings);
            MiscHacks.SetWily5NoMusicChange(Patch);
            MiscHacks.NerfDamageValues(Patch);
            MiscHacks.SetETankKeep(Patch);
            MiscHacks.PreventETankUseAtFullLife(Patch);
            MiscHacks.SetFastBossDefeatTeleport(Patch);

            if (Settings.ReduceUnderwaterLag)
            {
                MiscHacks.ReduceUnderwaterLag(Patch);
            }

            if (Settings.DisableDelayScrolling)
            {
                MiscHacks.DisableDelayScroll(Patch);
            }

            // Create file name based on seed and game region
            String newFileName = $"MM2-RNG-{RandomMM2.mSeed.Identifier} ({RandomMM2.mSeed.SeedString}).nes";

            //File.Copy(Settings.SourcePath, TempFileName, true);
            //using (Stream stream = assembly.GetManifestResourceStream("MM2Randomizer.Resources.MM2.nes"))
            // Load user provided ROM
            using (Stream stream = new FileStream(Settings.SourcePath, FileMode.Open, FileAccess.Read))
            {
                using (Stream output = File.OpenWrite(TempFileName))
                {
                    stream.CopyTo(output);
                }
            }

            // Apply pre-patch changes via IPS patch (manual title screen, stage select, stage changes, player sprite)
            Patch.ApplyIPSPatch(TempFileName, Properties.Resources.mm2rng_musicpatch);
            Patch.ApplyIPSPatch(TempFileName, Properties.Resources.mm2rng_prepatch);
            MiscHacks.SetNewMegaManSprite(Patch, TempFileName, Settings.SelectedPlayer);

            // Apply patch with randomized content
            Patch.ApplyRandoPatch(TempFileName);

            // If a file of the same seed already exists, delete it
            if (File.Exists(newFileName))
            {
                File.Delete(newFileName);
            }

            // Finish the copy/rename and open Explorer at that location
            File.Move(TempFileName, newFileName);
            RecentlyCreatedFileName = newFileName;
            Settings.HashValidationMessage = "Successfully copied and patched! File: " + newFileName;
            return newFileName;
        }


        //
        // Private Data Members
        //

        private static ISeed mSeed = null;
    }
}