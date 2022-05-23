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

        public dynamic? ActualizedSettings { get; private set; }


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
            // List of randomizer modules to use; will add modules based on checkbox states
            List<IRandomizer> randomizers = new List<IRandomizer>();
            List<IRandomizer> cosmeticRandomizers = new List<IRandomizer>();

            this.ActualizedSettings = this.Settings.ActualizeSettings(this.Seed);


            // Add randomizers according to each flag
            ///==========================
            /// "GAMEPLAY SEED" MODULES
            ///==========================

            if (BooleanOption.True == this.ActualizedSettings.GameplayOption.RandomizeRobotMasterStageSelection)
            {
                randomizers.Add(this.RandomStages);
            }

            if (BooleanOption.True == this.ActualizedSettings.GameplayOption.RandomizeSpecialWeaponReward)
            {
                randomizers.Add(this.RandomWeaponGet);
            }

            if (BooleanOption.True == this.ActualizedSettings.GameplayOption.RandomizeSpecialWeaponBehavior)
            {
                randomizers.Add(this.RandomWeaponBehavior);
            }

            if (BooleanOption.True == this.ActualizedSettings.GameplayOption.RandomizeBossWeaknesses)
            {
                randomizers.Add(this.RandomWeaknesses);
            }

            if (BooleanOption.True == this.ActualizedSettings.GameplayOption.RandomizeRobotMasterBehavior)
            {
                randomizers.Add(this.RandomBossAI);
            }

            if (BooleanOption.True == this.ActualizedSettings.GameplayOption.RandomizeSpecialItemLocations)
            {
                randomizers.Add(this.RandomItemGet);
            }

            if (BooleanOption.True == this.ActualizedSettings.GameplayOption.RandomizeRefightTeleporters)
            {
                randomizers.Add(this.RandomTeleporters);
            }

            if (BooleanOption.True == this.ActualizedSettings.GameplayOption.RandomizeEnemySpawns)
            {
                randomizers.Add(this.RandomEnemies);
            }

            if (BooleanOption.True == this.ActualizedSettings.GameplayOption.RandomizeEnemyWeaknesses)
            {
                randomizers.Add(this.RandomEnemyWeakness);
            }

            if (BooleanOption.True == this.ActualizedSettings.GameplayOption.RandomizeRobotMasterLocations)
            {
                randomizers.Add(this.RandomBossInBossRoom);
            }

            if (BooleanOption.True == this.ActualizedSettings.GameplayOption.RandomizeFalseFloors)
            {
                randomizers.Add(this.RandomTilemap);
            }


            ///==========================
            /// "COSMETIC SEED" MODULES
            ///==========================

            if (BooleanOption.True == this.ActualizedSettings.CosmeticOption.RandomizeColorPalettes)
            {
                randomizers.Add(this.RandomColors);
            }

            if (BooleanOption.True == this.ActualizedSettings.CosmeticOption.RandomizeMusicTracks)
            {
                randomizers.Add(this.RandomMusic);
            }

            if (BooleanOption.True == this.ActualizedSettings.CosmeticOption.RandomizeInGameText)
            {
                randomizers.Add(this.RandomInGameText);
            }

            // In tournament mode, offset the seed by 1 call, making seeds mode-dependent
            if (this.Settings.EnableSpoilerFreeMode)
            {
                this.Seed.Next();
            }

            // Conduct randomization of Gameplay Modules
            foreach (IRandomizer randomizer in randomizers)
            {
                randomizer.Randomize(this.Patch, this);
                Debug.WriteLine(randomizer);
            }

            // Conduct randomization of Cosmetic Modules
            foreach (IRandomizer cosmetic in cosmeticRandomizers)
            {
                cosmetic.Randomize(this.Patch, this);
                Debug.WriteLine(cosmetic);
            }

            // Apply additional required incidental modifications
            if (BooleanOption.True == this.ActualizedSettings.GameplayOption.RandomizeRobotMasterStageSelection ||
                BooleanOption.True == this.ActualizedSettings.CosmeticOption.RandomizeInGameText)
            {
                MiscHacks.FixPortraits(
                    this.Patch,
                    BooleanOption.True == this.ActualizedSettings.GameplayOption.RandomizeRobotMasterStageSelection,
                    this.RandomStages,
                    BooleanOption.True == this.ActualizedSettings.CosmeticOption.RandomizeInGameText,
                    this.RandomWeaponGet);

                MiscHacks.FixWeaponLetters(
                    this.Patch,
                    this.RandomWeaponGet,
                    this.RandomStages,
                    this.RandomInGameText);
            }

            if (BooleanOption.True == this.ActualizedSettings.GameplayOption.RandomizeEnemySpawns)
            {
                MiscHacks.FixM445PaletteGlitch(this.Patch);
            }


            //
            // Apply final optional gameplay modifications
            //

            if (BooleanOption.True == this.ActualizedSettings.GameplayOption.FasterCutsceneText)
            {
                MiscHacks.SetFastWeaponGetText(this.Patch);
                MiscHacks.SetFastReadyText(this.Patch);
                MiscHacks.SetFastWilyMap(this.Patch);
                MiscHacks.SkipItemGetPages(this.Patch);
            }

            if (BooleanOption.True == this.ActualizedSettings.GameplayOption.BurstChaserMode)
            {
                MiscHacks.SetBurstChaser(this.Patch);
            }

            if (BooleanOption.True == this.ActualizedSettings.QualityOfLifeOption.DisableFlashingEffects)
            {
                MiscHacks.DisableScreenFlashing(
                    this.Patch,
                    BooleanOption.True == this.ActualizedSettings.GameplayOption.FasterCutsceneText,
                    BooleanOption.True == this.ActualizedSettings.CosmeticOption.RandomizeColorPalettes);
            }

            MiscHacks.SetHitPointChargingSpeed(
                this.Patch,
                this.ActualizedSettings.ChargingSpeedOption.HitPoints);

            MiscHacks.SetWeaponEnergyChargingSpeed(
                this.Patch,
                this.ActualizedSettings.ChargingSpeedOption.WeaponEnergy);

            MiscHacks.SetEnergyTankChargingSpeed(
                this.Patch,
                this.ActualizedSettings.ChargingSpeedOption.EnergyTank);

            MiscHacks.SetRobotMasterEnergyChargingSpeed(
                this.Patch,
                this.ActualizedSettings.ChargingSpeedOption.RobotMasterEnergy);

            MiscHacks.SetCastleBossEnergyChargingSpeed(
                this.Patch,
                this.ActualizedSettings.ChargingSpeedOption.CastleBossEnergy);

            MiscHacks.DrawTitleScreenChanges(this.Patch, this.Seed.Identifier, this.Settings);
            MiscHacks.SetWily5NoMusicChange(this.Patch);
            MiscHacks.NerfDamageValues(this.Patch);
            MiscHacks.SetETankKeep(this.Patch);
            MiscHacks.PreventETankUseAtFullLife(this.Patch);
            MiscHacks.SetFastBossDefeatTeleport(this.Patch);

            if (BooleanOption.True == this.ActualizedSettings.QualityOfLifeOption.EnableUnderwaterLagReduction)
            {
                MiscHacks.ReduceUnderwaterLag(this.Patch);
            }

            if (BooleanOption.True == this.ActualizedSettings.QualityOfLifeOption.DisableDelayScrolling)
            {
                MiscHacks.DisableDelayScroll(this.Patch);
            }

            // Load user provided ROM
            using (Stream stream = new FileStream(this.Settings.RomSourcePath, FileMode.Open, FileAccess.Read))
            {
                using (Stream output = File.OpenWrite(RandomizationContext.TEMPORARY_FILE_NAME))
                {
                    stream.CopyTo(output);
                }
            }

            // Apply pre-patch changes via IPS patch (manual title screen, stage select, stage changes, player sprite)
            this.Patch.ApplyIPSPatch(RandomizationContext.TEMPORARY_FILE_NAME, Properties.Resources.mm2rng_musicpatch);
            this.Patch.ApplyIPSPatch(RandomizationContext.TEMPORARY_FILE_NAME, Properties.Resources.mm2rng_prepatch);


            MiscHacks.SetNewMegaManSprite(
                this.Patch,
                RandomizationContext.TEMPORARY_FILE_NAME,
                this.ActualizedSettings.CosmeticOption.PlayerSprite);

            MiscHacks.SetNewHudElement(
                this.Patch,
                RandomizationContext.TEMPORARY_FILE_NAME,
                this.ActualizedSettings.CosmeticOption.HudElement);

            if (BooleanOption.True == this.ActualizedSettings.SpriteOption.RandomizeBossSprites)
            {
                BossSpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (BooleanOption.True == this.ActualizedSettings.SpriteOption.RandomizeEnemySprites)
            {
                EnemySpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (BooleanOption.True == this.ActualizedSettings.SpriteOption.RandomizeSpecialWeaponSprites)
            {
                WeaponSpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (BooleanOption.True == this.ActualizedSettings.SpriteOption.RandomizeItemPickupSprites)
            {
                PickupSpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (BooleanOption.True == this.ActualizedSettings.SpriteOption.RandomizeEnvironmentSprites)
            {
                EnvironmentSpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

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
