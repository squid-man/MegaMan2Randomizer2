using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;
using MM2Randomizer.Randomizers;
using MM2Randomizer.Randomizers.Colors;
using MM2Randomizer.Randomizers.Enemies;
using MM2Randomizer.Randomizers.Stages;
using MM2Randomizer.Resources.SpritePatches;
using MM2Randomizer.Settings;
using MM2Randomizer.Settings.OptionGroups;
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

            Settings.ActualizeBehaviorSettings(this.Seed);
            var gameplayOpts = Settings.GameplayOptions;
            var spriteOpts = Settings.SpriteOptions;
            var chargingOpts = Settings.ChargingSpeedOptions;
            var qolOpts = Settings.QualityOfLifeOptions;

            // List of randomizer modules to use; will add modules based on checkbox states
            List<IRandomizer> randomizers = new List<IRandomizer>();
            if (gameplayOpts.RandomizeRobotMasterStageSelection.Value)
            {
                randomizers.Add(this.RandomStages);
            }

            if (gameplayOpts.RandomizeSpecialWeaponReward.Value)
            {
                randomizers.Add(this.RandomWeaponGet);
            }

            if (gameplayOpts.RandomizeSpecialWeaponBehavior.Value)
            {
                randomizers.Add(this.RandomWeaponBehavior);
            }

            if (gameplayOpts.RandomizeBossWeaknesses.Value)
            {
                randomizers.Add(this.RandomWeaknesses);
            }

            if (gameplayOpts.RandomizeRobotMasterBehavior.Value)
            {
                randomizers.Add(this.RandomBossAI);
            }

            if (gameplayOpts.RandomizeSpecialItemLocations.Value)
            {
                randomizers.Add(this.RandomItemGet);
            }

            if (gameplayOpts.RandomizeRefightTeleporters.Value)
            {
                randomizers.Add(this.RandomTeleporters);
            }

            if (gameplayOpts.RandomizeEnemySpawns.Value)
            {
                randomizers.Add(this.RandomEnemies);
            }

            if (gameplayOpts.RandomizeEnemyWeaknesses.Value)
            {
                randomizers.Add(this.RandomEnemyWeakness);
            }

            if (gameplayOpts.RandomizeRobotMasterLocations.Value)
            {
                randomizers.Add(this.RandomBossInBossRoom);
            }

            if (gameplayOpts.RandomizeFalseFloors.Value)
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
            if (spriteOpts.RandomizeBossSprites.Value)
            {
                BossSpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (spriteOpts.RandomizeEnemySprites.Value)
            {
                EnemySpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (spriteOpts.RandomizeSpecialWeaponSprites.Value)
            {
                WeaponSpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (spriteOpts.RandomizeItemPickupSprites.Value)
            {
                PickupSpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (spriteOpts.RandomizeEnvironmentSprites.Value)
            {
                EnvironmentSpriteRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }


            ///==========================
            /// "COSMETIC SEED" MODULES
            ///==========================

            // NOTE: Reset the seed for cosmetic options
            this.Seed.Reset();

            this.Settings.ActualizeCosmeticSettings(this.Seed);
            var cosmOpts = Settings.CosmeticOptions;

            // List of randomizer modules to use; will add modules based on checkbox states
            List<IRandomizer> cosmeticRandomizers = new List<IRandomizer>();

            if (cosmOpts.RandomizeColorPalettes.Value)
            {
                cosmeticRandomizers.Add(this.RandomColors);
            }

            if (cosmOpts.RandomizeMusicTracks.Value)
            {
                cosmeticRandomizers.Add(this.RandomMusic);
            }

            if (cosmOpts.RandomizeInGameText.Value)
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
            if (cosmOpts.RandomizeMenusAndTransitionScreens.Value)
            {
                MenusAndTransitionScreenRandomizer.ApplySprites(this.Seed, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }


            // ================================================
            // No randomization after this point, only patching
            // ================================================

            // Apply additional required incidental modifications
            if (gameplayOpts.RandomizeRobotMasterStageSelection.Value ||
                cosmOpts.RandomizeInGameText.Value)
            {
                MiscHacks.FixPortraits(
                    this.Patch,
                    gameplayOpts.RandomizeRobotMasterStageSelection.Value,
                    this.RandomStages,
                    cosmOpts.RandomizeInGameText.Value,
                    this.RandomWeaponGet);

                MiscHacks.FixWeaponLetters(
                    this.Patch,
                    this.RandomWeaponGet,
                    this.RandomStages,
                    this.RandomInGameText);
            }

            if (gameplayOpts.RandomizeEnemySpawns.Value)
            {
                MiscHacks.FixM445PaletteGlitch(this.Patch);
            }

            // Apply final optional gameplay modifications
            if (gameplayOpts.FasterCutsceneText.Value)
            {
                MiscHacks.SetFastWeaponGetText(this.Patch);
                MiscHacks.SetFastReadyText(this.Patch);
                MiscHacks.SetFastWilyMap(this.Patch);
                MiscHacks.SkipItemGetPages(this.Patch);
            }

            if (gameplayOpts.BurstChaserMode.Value)
            {
                MiscHacks.SetBurstChaser(this.Patch);
            }

            if (qolOpts.DisableFlashingEffects.Value)
            {
                MiscHacks.DisableScreenFlashing(
                    this.Patch,
                    gameplayOpts.FasterCutsceneText.Value,
                    cosmOpts.RandomizeColorPalettes.Value);
            }

            MiscHacks.SetHitPointChargingSpeed(
                this.Patch,
                chargingOpts.HitPoints.Value);

            MiscHacks.SetWeaponEnergyChargingSpeed(
                this.Patch,
                chargingOpts.WeaponEnergy.Value);

            // PreventETankUseAtFullLife must be applied before SetEnergyTankChargingSpeed
            MiscHacks.PreventETankUseAtFullLife(this.Patch);
            MiscHacks.SetEnergyTankChargingSpeed(
                this.Patch,
                chargingOpts.EnergyTank.Value);

            MiscHacks.SetRobotMasterEnergyChargingSpeed(
                this.Patch,
                chargingOpts.RobotMasterEnergy.Value);

            MiscHacks.SetCastleBossEnergyChargingSpeed(
                this.Patch,
                chargingOpts.CastleBossEnergy.Value);

            MiscHacks.DrawTitleScreenChanges(this.Patch, this.Seed.Identifier, this.Settings);
            MiscHacks.SetWily5NoMusicChange(this.Patch);
            MiscHacks.NerfDamageValues(this.Patch);
            MiscHacks.SetETankKeep(this.Patch);
            MiscHacks.SetFastBossDefeatTeleport(this.Patch);


            if (qolOpts.EnableUnderwaterLagReduction.Value)
            {
                MiscHacks.ReduceUnderwaterLag(this.Patch);
            }

            if (qolOpts.DisableWaterfall.Value)
            {
                this.Patch.Add(0xFE10, (byte)1, "Disable Bubble Man stage palette animation");
            }

            if (qolOpts.EnableLeftwardWallEjection.Value)
            {
                MiscHacks.EnableLeftwardWallEjection(this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            // Apply pre-patch changes via IPS patch (manual title screen, stage select, stage changes, player sprite)
            this.Patch.ApplyIPSPatch(RandomizationContext.TEMPORARY_FILE_NAME, Properties.Resources.mm2ft, false);
            this.Patch.ApplyIPSPatch(RandomizationContext.TEMPORARY_FILE_NAME, Properties.Resources.mm2rng_prepatch);

            // IPS patches should/must come after mm2ft as IPS patches are applied immediately and may be overwritten by deferred patches
            if (qolOpts.DisablePauseLock.Value)
            {
                MiscHacks.DisablePauseLock(this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (gameplayOpts.MercilessMode.Value)
            {
                MiscHacks.EnableMercilessMode(this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (qolOpts.EnableBirdEggFix.Value)
            {
                MiscHacks.EnableBirdEggFix(this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            MiscHacks.EnableClownBotFix(this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);

            MiscHacks.SetNewMegaManSprite(
                this.Patch,
                RandomizationContext.TEMPORARY_FILE_NAME,
                cosmOpts.PlayerSprite.Value);

            MiscHacks.SetNewHudElement(
                this.Patch,
                RandomizationContext.TEMPORARY_FILE_NAME,
                cosmOpts.HudElement.Value);

            MiscHacks.SetNewFont(
                this.Patch,
                RandomizationContext.TEMPORARY_FILE_NAME,
                cosmOpts.Font.Value);


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
