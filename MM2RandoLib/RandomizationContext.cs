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
using MM2Randomizer.Resources.SpritePatches;
using MM2Randomizer.Settings;
using MM2Randomizer.Settings.Options;
using MM2Randomizer.Utilities;

namespace MM2Randomizer
{
    public class RandomizationContext
    {
        //
        // Constructors
        //

        internal RandomizationContext(RandomizationSettings in_Settings, ISeed in_Seed)
        {
            this.Seed = in_Seed;
            this.Settings = in_Settings;
            // Create file name based on seed and game region
            this.FileName = $"MM2-RNG-{in_Seed.Identifier} ({in_Seed.SeedString}).nes";
        }


        //
        // Properties
        //

        public ISeed Seed { get; private set; }

        public RandomizationSettings Settings { get; private set; }

        public String FileName { get; private set; }

        // Create randomization patch
        public Patch Patch { get; private set; } = new Patch();

        public dynamic? ActualizedBehaviorSettings { get; private set; }

        public dynamic? ActualizedCosmeticSettings { get; private set; }


        //================
        // "CORE" MODULES
        //================

        // NOTE: Just in case, link RStages, RWeaponGet, and RTeleporter into one "Core Randomizer" module
        // Their interdependencies are too risky to separate as options, and likely nobody will want to customize this part anyways.
        // Random portrait locations on stage select
        public RStages RandomStages { get; private set; } = new RStages();
        // Random weapon awarded from each stage
        // WARNING: May be dependent on RTeleporters, verify?
        // WARNING: May be dependent on RStages
        public RWeaponGet RandomWeaponGet { get; private set; } = new RWeaponGet();
        // Random teleporter destinations in Wily 5
        public RTeleporters RandomTeleporters { get; private set; } = new RTeleporters();


        ///=========================
        /// "GAMEPLAY SEED" MODULES
        ///=========================

        // Caution: RWeaknesses depends on this
        public RWeaponBehavior RandomWeaponBehavior { get; private set; } = new RWeaponBehavior();
        // Depends on RWeaponBehavior (ammo), can use default values
        public RWeaknesses RandomWeaknesses { get; private set; } = new RWeaknesses();
        // Independent
        public RBossAI RandomBossAI { get; private set; } = new RBossAI();
        // Independent
        public RItemGet RandomItemGet { get; private set; } = new RItemGet();
        // Independent
        public REnemies RandomEnemies { get; private set; } = new REnemies();
        // Independent
        public REnemyWeaknesses RandomEnemyWeakness { get; private set; } = new REnemyWeaknesses();
        // Caution: RText depends on this, but default values will be used if not enabled.
        public RBossRoom RandomBossInBossRoom { get; private set; } = new RBossRoom();
        // Independent
        public RTilemap RandomTilemap { get; private set; } = new RTilemap();


        ///==========================
        /// "COSMETIC SEED" MODULES
        ///==========================

        // Independent
        public RColors RandomColors { get; private set; } = new RColors();
        // Independent
        public RMusic RandomMusic { get; private set; } = new RMusic();
        // Caution: Depends on RBossRoom, but can use default values if its not enabled.
        public RText RandomInGameText { get; private set; } = new RText();


        //
        // Internal Methods
        //

        internal void Initialize()
        {
            // Load user provided ROM
            using (Stream stream = new FileStream(this.Settings.RomSourcePath, FileMode.Open, FileAccess.Read))
            {
                using (Stream output = File.OpenWrite(RandomizationContext.TEMPORARY_FILE_NAME))
                {
                    stream.CopyTo(output);
                }
            }

            // In tournament mode, offset the seed by 1 call, making seeds mode-dependent
            /*
            if (this.Settings.EnableSpoilerFreeMode)
            {
                this.Seed.Next();
            }
            */


            // Add randomizers according to each flag
            ///==========================
            /// "GAMEPLAY SEED" MODULES
            ///==========================

            this.ActualizedBehaviorSettings = this.Settings.ActualizeBehaviorSettings(this.Seed);

            // List of randomizer modules to use; will add modules based on checkbox states
            List<IRandomizer> randomizers = new List<IRandomizer>();

            if (BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.RandomizeRobotMasterStageSelection)
            {
                randomizers.Add(this.RandomStages);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.RandomizeSpecialWeaponReward)
            {
                randomizers.Add(this.RandomWeaponGet);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.RandomizeSpecialWeaponBehavior)
            {
                randomizers.Add(this.RandomWeaponBehavior);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.RandomizeBossWeaknesses)
            {
                randomizers.Add(this.RandomWeaknesses);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.RandomizeRobotMasterBehavior)
            {
                randomizers.Add(this.RandomBossAI);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.RandomizeSpecialItemLocations)
            {
                randomizers.Add(this.RandomItemGet);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.RandomizeRefightTeleporters)
            {
                randomizers.Add(this.RandomTeleporters);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.RandomizeEnemySpawns)
            {
                randomizers.Add(this.RandomEnemies);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.RandomizeEnemyWeaknesses)
            {
                randomizers.Add(this.RandomEnemyWeakness);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.RandomizeRobotMasterLocations)
            {
                randomizers.Add(this.RandomBossInBossRoom);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.RandomizeFalseFloors)
            {
                randomizers.Add(this.RandomTilemap);
            }

            // Conduct randomization of behavior options
            foreach (IRandomizer randomizer in randomizers)
            {
                randomizer.Randomize(this.Patch, this);
                Debug.WriteLine(randomizer);
            }

            // Apply random sprite changes
            if (BooleanOption.True == this.ActualizedBehaviorSettings.SpriteOption.RandomizeBossSprites)
            {
                BossSpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.SpriteOption.RandomizeEnemySprites)
            {
                EnemySpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.SpriteOption.RandomizeSpecialWeaponSprites)
            {
                WeaponSpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.SpriteOption.RandomizeItemPickupSprites)
            {
                PickupSpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.SpriteOption.RandomizeEnvironmentSprites)
            {
                EnvironmentSpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }


            ///==========================
            /// "COSMETIC SEED" MODULES
            ///==========================

            // NOTE: Reset the seed for cosmetic options
            this.Seed.Reset();

            this.ActualizedCosmeticSettings = this.Settings.ActualizeCosmeticSettings(this.Seed);

            // List of randomizer modules to use; will add modules based on checkbox states
            List<IRandomizer> cosmeticRandomizers = new List<IRandomizer>();

            if (BooleanOption.True == this.ActualizedCosmeticSettings.CosmeticOption.RandomizeColorPalettes)
            {
                cosmeticRandomizers.Add(this.RandomColors);
            }

            if (BooleanOption.True == this.ActualizedCosmeticSettings.CosmeticOption.RandomizeMusicTracks)
            {
                cosmeticRandomizers.Add(this.RandomMusic);
            }

            if (BooleanOption.True == this.ActualizedCosmeticSettings.CosmeticOption.RandomizeInGameText)
            {
                cosmeticRandomizers.Add(this.RandomInGameText);
            }

            // Conduct randomization of Cosmetic Modules
            foreach (IRandomizer cosmetic in cosmeticRandomizers)
            {
                cosmetic.Randomize(this.Patch, this);
                Debug.WriteLine(cosmetic);
            }

            // Apply random sprite changes
            if (BooleanOption.True == this.ActualizedCosmeticSettings.CosmeticOption.RandomizeMenusAndTransitionScreens)
            {
                MenusAndTransitionScreenRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }


            // ================================================
            // No randomization after this point, only patching
            // ================================================

            // Apply additional required incidental modifications
            if (BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.RandomizeRobotMasterStageSelection ||
                BooleanOption.True == this.ActualizedCosmeticSettings.CosmeticOption.RandomizeInGameText)
            {
                MiscHacks.FixPortraits(
                    this.Patch,
                    BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.RandomizeRobotMasterStageSelection,
                    this.RandomStages,
                    BooleanOption.True == this.ActualizedCosmeticSettings.CosmeticOption.RandomizeInGameText,
                    this.RandomWeaponGet);

                MiscHacks.FixWeaponLetters(
                    this.Patch,
                    this.RandomWeaponGet,
                    this.RandomStages,
                    this.RandomInGameText);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.RandomizeEnemySpawns)
            {
                MiscHacks.FixM445PaletteGlitch(this.Patch);
            }

            // Apply final optional gameplay modifications
            if (BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.FasterCutsceneText)
            {
                MiscHacks.SetFastWeaponGetText(this.Patch);
                MiscHacks.SetFastReadyText(this.Patch);
                MiscHacks.SetFastWilyMap(this.Patch);
                MiscHacks.SkipItemGetPages(this.Patch);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.BurstChaserMode)
            {
                MiscHacks.SetBurstChaser(this.Patch);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.QualityOfLifeOption.DisableFlashingEffects)
            {
                MiscHacks.DisableScreenFlashing(
                    this.Patch,
                    BooleanOption.True == this.ActualizedBehaviorSettings.GameplayOption.FasterCutsceneText,
                    BooleanOption.True == this.ActualizedCosmeticSettings.CosmeticOption.RandomizeColorPalettes);
            }

            MiscHacks.SetHitPointChargingSpeed(
                this.Patch,
                this.ActualizedBehaviorSettings.ChargingSpeedOption.HitPoints);

            MiscHacks.SetWeaponEnergyChargingSpeed(
                this.Patch,
                this.ActualizedBehaviorSettings.ChargingSpeedOption.WeaponEnergy);

            MiscHacks.SetEnergyTankChargingSpeed(
                this.Patch,
                this.ActualizedBehaviorSettings.ChargingSpeedOption.EnergyTank);

            MiscHacks.SetRobotMasterEnergyChargingSpeed(
                this.Patch,
                this.ActualizedBehaviorSettings.ChargingSpeedOption.RobotMasterEnergy);

            MiscHacks.SetCastleBossEnergyChargingSpeed(
                this.Patch,
                this.ActualizedBehaviorSettings.ChargingSpeedOption.CastleBossEnergy);

            MiscHacks.DrawTitleScreenChanges(this.Patch, this.Seed.Identifier, this.Settings);
            MiscHacks.SetWily5NoMusicChange(this.Patch);
            MiscHacks.NerfDamageValues(this.Patch);
            MiscHacks.SetETankKeep(this.Patch);
            MiscHacks.PreventETankUseAtFullLife(this.Patch);
            MiscHacks.SetFastBossDefeatTeleport(this.Patch);


            if (BooleanOption.True == this.ActualizedBehaviorSettings.QualityOfLifeOption.EnableUnderwaterLagReduction)
            {
                MiscHacks.ReduceUnderwaterLag(this.Patch);
            }

            if (BooleanOption.True == this.ActualizedBehaviorSettings.QualityOfLifeOption.DisableDelayScrolling)
            {
                MiscHacks.DisableDelayScroll(this.Patch);
            }


            // Apply pre-patch changes via IPS patch (manual title screen, stage select, stage changes, player sprite)
            this.Patch.ApplyIPSPatch(RandomizationContext.TEMPORARY_FILE_NAME, Properties.Resources.mm2rng_musicpatch);
            this.Patch.ApplyIPSPatch(RandomizationContext.TEMPORARY_FILE_NAME, Properties.Resources.mm2rng_prepatch);


            MiscHacks.SetNewMegaManSprite(
                this.Patch,
                RandomizationContext.TEMPORARY_FILE_NAME,
                this.ActualizedCosmeticSettings.CosmeticOption.PlayerSprite);

            MiscHacks.SetNewHudElement(
                this.Patch,
                RandomizationContext.TEMPORARY_FILE_NAME,
                this.ActualizedCosmeticSettings.CosmeticOption.HudElement);

            MiscHacks.SetNewFont(
                this.Patch,
                RandomizationContext.TEMPORARY_FILE_NAME,
                this.ActualizedCosmeticSettings.CosmeticOption.Font);


            // Modify the Wily 5 game loop so that large weapon energy refills
            // can be spawned in the refight teleporter room
            MiscHacks.AddWily5SubroutineWithItemSpawns(this.Patch);
            MiscHacks.AddLargeWeaponEnergyRefillPickupsToWily5TeleporterRoom(this.Patch);

            // Apply patch with randomized content
            this.Patch.ApplyRandoPatch(RandomizationContext.TEMPORARY_FILE_NAME);

            // If a file of the same seed already exists, delete it
            if (File.Exists(this.FileName))
            {
                File.Delete(this.FileName);
            }

            // Finish the copy/rename and open Explorer at that location
            File.Move(RandomizationContext.TEMPORARY_FILE_NAME, this.FileName);
        }


        //
        // Private Data Members
        //

        private const String TEMPORARY_FILE_NAME = "temp.nes";
    }
}
