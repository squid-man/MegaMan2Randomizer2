using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;
using MM2Randomizer.Randomizers;
using MM2Randomizer.Randomizers.Colors;
using MM2Randomizer.Randomizers.Enemies;
using MM2Randomizer.Settings;
using MM2Randomizer.Randomizers.Stages;
using MM2Randomizer.Utilities;
using MM2Randomizer.Resources.SpritePatches;

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


            // Add randomizers according to each flag
            ///==========================
            /// "GAMEPLAY SEED" MODULES
            ///==========================

            Boolean randomizeRobotMasterStageSelection = false;

            if (true == this.Settings.GameplayOption.RandomizeRobotMasterStageSelection.Randomize)
            {
                if (true == this.Seed.NextBoolean())
                {
                    randomizers.Add(this.RandomStages);

                    // For later use in fixing text/portraits
                    randomizeRobotMasterStageSelection = true;
                }
            }
            else if (true == this.Settings.GameplayOption.RandomizeRobotMasterStageSelection.Value)
            {
                randomizers.Add(this.RandomStages);

                // For later use in fixing text/portraits
                randomizeRobotMasterStageSelection = true;
            }

            if (true == this.Settings.GameplayOption.RandomizeSpecialWeaponReward.Randomize)
            {
                if (true == this.Seed.NextBoolean())
                {
                    randomizers.Add(this.RandomWeaponGet);
                }
            }
            else if (true == this.Settings.GameplayOption.RandomizeSpecialWeaponReward.Value)
            {
                randomizers.Add(this.RandomWeaponGet);
            }

            if (true == this.Settings.GameplayOption.RandomizeSpecialWeaponBehavior.Randomize)
            {
                if (true == this.Seed.NextBoolean())
                {
                    randomizers.Add(this.RandomWeaponBehavior);
                }
            }
            else if (true == this.Settings.GameplayOption.RandomizeSpecialWeaponBehavior.Value)
            {
                randomizers.Add(this.RandomWeaponBehavior);
            }

            if (true == this.Settings.GameplayOption.RandomizeBossWeaknesses.Randomize)
            {
                if (true == this.Seed.NextBoolean())
                {
                    randomizers.Add(this.RandomWeaknesses);
                }
            }
            else if (true == this.Settings.GameplayOption.RandomizeBossWeaknesses.Value)
            {
                randomizers.Add(this.RandomWeaknesses);
            }

            if (true == this.Settings.GameplayOption.RandomizeRobotMasterBehavior.Randomize)
            {
                if (true == this.Seed.NextBoolean())
                {
                    randomizers.Add(this.RandomBossAI);
                }
            }
            else if (true == this.Settings.GameplayOption.RandomizeRobotMasterBehavior.Value)
            {
                randomizers.Add(this.RandomBossAI);
            }

            if (true == this.Settings.GameplayOption.RandomizeSpecialItemLocations.Randomize)
            {
                if (true == this.Seed.NextBoolean())
                {
                    randomizers.Add(this.RandomItemGet);
                }
            }
            else if (true == this.Settings.GameplayOption.RandomizeSpecialItemLocations.Value)
            {
                randomizers.Add(this.RandomItemGet);
            }

            if (true == this.Settings.GameplayOption.RandomizeRefightTeleporters.Randomize)
            {
                if (true == this.Seed.NextBoolean())
                {
                    randomizers.Add(this.RandomTeleporters);
                }
            }
            else if (true == this.Settings.GameplayOption.RandomizeRefightTeleporters.Value)
            {
                randomizers.Add(this.RandomTeleporters);
            }

            Boolean randomizeEnemySpawns = false;

            if (true == this.Settings.GameplayOption.RandomizeEnemySpawns.Randomize)
            {
                if (true == this.Seed.NextBoolean())
                {
                    randomizers.Add(this.RandomEnemies);

                    // For later use in fixing text/portraits
                    randomizeEnemySpawns = true;
                }
            }
            else if (true == this.Settings.GameplayOption.RandomizeEnemySpawns.Value)
            {
                randomizers.Add(this.RandomEnemies);

                // For later use in fixing text/portraits
                randomizeEnemySpawns = true;
            }

            if (true == this.Settings.GameplayOption.RandomizeEnemyWeaknesses.Randomize)
            {
                if (true == this.Seed.NextBoolean())
                {
                    randomizers.Add(this.RandomEnemyWeakness);
                }
            }
            else if (true == this.Settings.GameplayOption.RandomizeEnemyWeaknesses.Value)
            {
                randomizers.Add(this.RandomEnemyWeakness);
            }

            if (true == this.Settings.GameplayOption.RandomizeRobotMasterLocations.Randomize)
            {
                if (true == this.Seed.NextBoolean())
                {
                    randomizers.Add(this.RandomBossInBossRoom);
                }
            }
            else if (true == this.Settings.GameplayOption.RandomizeRobotMasterLocations.Value)
            {
                randomizers.Add(this.RandomBossInBossRoom);
            }

            if (true == this.Settings.GameplayOption.RandomizeFalseFloors.Randomize)
            {
                if (true == this.Seed.NextBoolean())
                {
                    randomizers.Add(this.RandomTilemap);
                }
            }
            else if (true == this.Settings.GameplayOption.RandomizeFalseFloors.Value)
            {
                randomizers.Add(this.RandomTilemap);
            }


            ///==========================
            /// "COSMETIC SEED" MODULES
            ///==========================

            Boolean randomizeColorPalettes = false;

            if (true == this.Settings.CosmeticOption.RandomizeColorPalettes.Randomize)
            {
                if (true == this.Seed.NextBoolean())
                {
                    randomizers.Add(this.RandomColors);

                    // For later use in disabling flashing effects
                    randomizeColorPalettes = true;
                }
            }
            else if (true == this.Settings.CosmeticOption.RandomizeColorPalettes.Value)
            {
                randomizers.Add(this.RandomColors);

                // For later use in disabling flashing effects
                randomizeColorPalettes = true;
            }

            if (true == this.Settings.CosmeticOption.RandomizeMusicTracks.Randomize)
            {
                if (true == this.Seed.NextBoolean())
                {
                    randomizers.Add(this.RandomMusic);
                }
            }
            else if (true == this.Settings.CosmeticOption.RandomizeMusicTracks.Value)
            {
                randomizers.Add(this.RandomMusic);
            }


            Boolean randomizeInGameText = false;

            if (true == this.Settings.CosmeticOption.RandomizeInGameText.Randomize)
            {
                if (true == this.Seed.NextBoolean())
                {
                    randomizers.Add(this.RandomInGameText);

                    // For later use in fixing text/portraits
                    randomizeInGameText = true;
                }
            }
            else if (true == this.Settings.CosmeticOption.RandomizeInGameText.Value)
            {
                randomizers.Add(this.RandomInGameText);

                // For later use in fixing text/portraits
                randomizeInGameText = true;
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
            if (randomizeRobotMasterStageSelection || randomizeInGameText)
            {
                MiscHacks.FixPortraits(
                    this.Patch,
                    randomizeRobotMasterStageSelection,
                    this.RandomStages,
                    randomizeInGameText,
                    this.RandomWeaponGet);

                MiscHacks.FixWeaponLetters(
                    this.Patch,
                    this.RandomWeaponGet,
                    this.RandomStages,
                    this.RandomInGameText);
            }

            if (randomizeEnemySpawns)
            {
                MiscHacks.FixM445PaletteGlitch(this.Patch);
            }


            //
            // Apply final optional gameplay modifications
            //

            Boolean fasterCutsceneText = this.Settings.GameplayOption.FasterCutsceneText.Randomize ?
                this.Seed.NextBoolean() :
                this.Settings.GameplayOption.FasterCutsceneText.Value;

            if (fasterCutsceneText)
            {
                MiscHacks.SetFastWeaponGetText(this.Patch);
                MiscHacks.SetFastReadyText(this.Patch);
                MiscHacks.SetFastWilyMap(this.Patch);
                MiscHacks.SkipItemGetPages(this.Patch);
            }

            Boolean burstChaserMode = this.Settings.GameplayOption.BurstChaserMode.Randomize ?
                this.Seed.NextBoolean() :
                this.Settings.GameplayOption.BurstChaserMode.Value;

            if (burstChaserMode)
            {
                MiscHacks.SetBurstChaser(this.Patch);
            }

            {
                Boolean disableFlashingEffects = this.Settings.QualityOfLifeOption.DisableFlashingEffects.Randomize ?
                    this.Seed.NextBoolean() :
                    this.Settings.QualityOfLifeOption.DisableFlashingEffects.Value;

                if (disableFlashingEffects)
                {
                    MiscHacks.DisableScreenFlashing(
                        this.Patch,
                        fasterCutsceneText,
                        randomizeColorPalettes);
                }
            }

            MiscHacks.SetHitPointChargingSpeed(
                this.Patch,
                this.Settings.ChargingSpeedOption.HitPoints.Randomize ?
                    this.Seed.NextEnum<ChargingSpeed>() :
                    this.Settings.ChargingSpeedOption.HitPoints.Value);

            MiscHacks.SetWeaponEnergyChargingSpeed(
                this.Patch,
                this.Settings.ChargingSpeedOption.WeaponEnergy.Randomize ?
                    this.Seed.NextEnum<ChargingSpeed>() :
                    this.Settings.ChargingSpeedOption.WeaponEnergy.Value);

            MiscHacks.SetEnergyTankChargingSpeed(
                this.Patch,
                this.Settings.ChargingSpeedOption.EnergyTank.Randomize ?
                    this.Seed.NextEnum<ChargingSpeed>() :
                    this.Settings.ChargingSpeedOption.EnergyTank.Value);

            MiscHacks.SetRobotMasterEnergyChargingSpeed(
                this.Patch,
                this.Settings.ChargingSpeedOption.RobotMasterEnergy.Randomize ?
                    this.Seed.NextEnum<ChargingSpeed>() :
                    this.Settings.ChargingSpeedOption.RobotMasterEnergy.Value);

            MiscHacks.SetCastleBossEnergyChargingSpeed(
                this.Patch,
                this.Settings.ChargingSpeedOption.CastleBossEnergy.Randomize ?
                    this.Seed.NextEnum<ChargingSpeed>() :
                    this.Settings.ChargingSpeedOption.CastleBossEnergy.Value);

            MiscHacks.DrawTitleScreenChanges(this.Patch, this.Seed.Identifier, this.Settings);
            MiscHacks.SetWily5NoMusicChange(this.Patch);
            MiscHacks.NerfDamageValues(this.Patch);
            MiscHacks.SetETankKeep(this.Patch);
            MiscHacks.PreventETankUseAtFullLife(this.Patch);
            MiscHacks.SetFastBossDefeatTeleport(this.Patch);

            Boolean enableUnderwaterLagReduction =
                this.Settings.QualityOfLifeOption.EnableUnderwaterLagReduction.Randomize ?
                    this.Seed.NextBoolean() :
                    this.Settings.QualityOfLifeOption.EnableUnderwaterLagReduction.Value;

            if (enableUnderwaterLagReduction)
            {
                MiscHacks.ReduceUnderwaterLag(this.Patch);
            }

            Boolean disableDelayScrolling =
                this.Settings.QualityOfLifeOption.DisableDelayScrolling.Randomize ?
                    this.Seed.NextBoolean() :
                    this.Settings.QualityOfLifeOption.DisableDelayScrolling.Value;

            if (disableDelayScrolling)
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
                this.Settings.CosmeticOption.PlayerSprite.Randomize ?
                    this.Seed.NextEnum<PlayerSprite>() :
                    this.Settings.CosmeticOption.PlayerSprite.Value);

            MiscHacks.SetNewHudElement(
                this.Patch,
                RandomizationContext.TEMPORARY_FILE_NAME,
                this.Settings.CosmeticOption.HudElement.Randomize ?
                    this.Seed.NextEnum<HudElement>() :
                    this.Settings.CosmeticOption.HudElement.Value);

            Boolean randomizeBossSprites =
                this.Settings.SpriteOption.RandomizeBossSprites.Randomize ?
                    this.Seed.NextBoolean() :
                    this.Settings.SpriteOption.RandomizeBossSprites.Value;

            if (true == randomizeBossSprites)
            {
                BossSpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            Boolean randomizeEnemySprites =
                this.Settings.SpriteOption.RandomizeEnemySprites.Randomize ?
                    this.Seed.NextBoolean() :
                    this.Settings.SpriteOption.RandomizeEnemySprites.Value;

            if (true == randomizeEnemySprites)
            {
                EnemySpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            Boolean randomizeSpecialWeaponSprites =
                this.Settings.SpriteOption.RandomizeSpecialWeaponSprites.Randomize ?
                    this.Seed.NextBoolean() :
                    this.Settings.SpriteOption.RandomizeSpecialWeaponSprites.Value;

            if (true == randomizeSpecialWeaponSprites)
            {
                WeaponSpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            Boolean randomizeItemPickupSprites =
                this.Settings.SpriteOption.RandomizeItemPickupSprites.Randomize ?
                    this.Seed.NextBoolean() :
                    this.Settings.SpriteOption.RandomizeItemPickupSprites.Value;

            if (true == randomizeItemPickupSprites)
            {
                PickupSpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            Boolean randomizeEnvironmentSprites =
                this.Settings.SpriteOption.RandomizeEnvironmentSprites.Randomize ?
                    this.Seed.NextBoolean() :
                    this.Settings.SpriteOption.RandomizeEnvironmentSprites.Value;

            if (true == randomizeEnvironmentSprites)
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
