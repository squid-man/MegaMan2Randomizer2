using System;
using MM2Randomizer.Settings.Options;

namespace MM2Randomizer.Settings
{
    public class RandomizationSettings
    {
        //
        // Constructor
        //

        public RandomizationSettings()
        {
        }


        //
        // Variable Properties
        //

        public String? SeedString { get; set; }

        public String RomSourcePath { get; set; } = String.Empty;


        //
        // Flag Options
        //

        public Boolean CreateLogFile { get; set; }

        public Boolean EnableSpoilerFreeMode { get; set; }



        //
        // Option Categories
        //

        public GameplayOptions GameplayOption { get; } = new GameplayOptions();

        public ChargingSpeedOptions ChargingSpeedOption { get; } = new ChargingSpeedOptions();

        public CosmeticOptions CosmeticOption { get; } = new CosmeticOptions();

        public SpriteOptions SpriteOption { get; } = new SpriteOptions();

        public QualityOfLifeOptions QualityOfLifeOption { get; } = new QualityOfLifeOptions();


        //
        // Public Methods
        //

        public void SetFromFlagString(String in_OptionsFlagString, String in_CosmeticFlagString)
        {
        }

        public String GetBehaviorFlagsString()
        {
            // TODO: Work out a way to bind options to a randomization flags
            // instance such that updating the property will automatically
            // update the flags value
            RandomizationFlags flags = new RandomizationFlags(14);

            // Gameplay options
            flags.PushValue(this.GameplayOption.BurstChaserMode.Randomize);
            flags.PushValue(this.GameplayOption.BurstChaserMode.Value);
            flags.PushValue(this.GameplayOption.FasterCutsceneText.Randomize);
            flags.PushValue(this.GameplayOption.FasterCutsceneText.Value);
            flags.PushValue(this.GameplayOption.HideStageNames.Randomize);
            flags.PushValue(this.GameplayOption.HideStageNames.Value);
            flags.PushValue(this.GameplayOption.RandomizeBossWeaknesses.Randomize);
            flags.PushValue(this.GameplayOption.RandomizeBossWeaknesses.Value);
            flags.PushValue(this.GameplayOption.RandomizeEnemySpawns.Randomize);
            flags.PushValue(this.GameplayOption.RandomizeEnemySpawns.Value);
            flags.PushValue(this.GameplayOption.RandomizeEnemyWeaknesses.Randomize);
            flags.PushValue(this.GameplayOption.RandomizeEnemyWeaknesses.Value);
            flags.PushValue(this.GameplayOption.RandomizeFalseFloors.Randomize);
            flags.PushValue(this.GameplayOption.RandomizeFalseFloors.Value);
            flags.PushValue(this.GameplayOption.RandomizeRefightTeleporters.Randomize);
            flags.PushValue(this.GameplayOption.RandomizeRefightTeleporters.Value);
            flags.PushValue(this.GameplayOption.RandomizeRobotMasterBehavior.Randomize);
            flags.PushValue(this.GameplayOption.RandomizeRobotMasterBehavior.Value);
            flags.PushValue(this.GameplayOption.RandomizeRobotMasterLocations.Randomize);
            flags.PushValue(this.GameplayOption.RandomizeRobotMasterLocations.Value);
            flags.PushValue(this.GameplayOption.RandomizeRobotMasterStageSelection.Randomize);
            flags.PushValue(this.GameplayOption.RandomizeRobotMasterStageSelection.Value);
            flags.PushValue(this.GameplayOption.RandomizeSpecialItemLocations.Randomize);
            flags.PushValue(this.GameplayOption.RandomizeSpecialItemLocations.Value);
            flags.PushValue(this.GameplayOption.RandomizeSpecialWeaponBehavior.Randomize);
            flags.PushValue(this.GameplayOption.RandomizeSpecialWeaponBehavior.Value);
            flags.PushValue(this.GameplayOption.RandomizeSpecialWeaponReward.Randomize);
            flags.PushValue(this.GameplayOption.RandomizeSpecialWeaponReward.Value);

            // Charging speed options
            flags.PushValue(this.ChargingSpeedOption.CastleBossEnergy.Randomize);
            flags.PushValue(this.ChargingSpeedOption.CastleBossEnergy.Value);
            flags.PushValue(this.ChargingSpeedOption.EnergyTank.Randomize);
            flags.PushValue(this.ChargingSpeedOption.EnergyTank.Value);
            flags.PushValue(this.ChargingSpeedOption.HitPoints.Randomize);
            flags.PushValue(this.ChargingSpeedOption.HitPoints.Value);
            flags.PushValue(this.ChargingSpeedOption.RobotMasterEnergy.Randomize);
            flags.PushValue(this.ChargingSpeedOption.RobotMasterEnergy.Value);
            flags.PushValue(this.ChargingSpeedOption.WeaponEnergy.Randomize);
            flags.PushValue(this.ChargingSpeedOption.WeaponEnergy.Value);

            // Sprite options
            flags.PushValue(this.SpriteOption.RandomizeBossSprites.Randomize);
            flags.PushValue(this.SpriteOption.RandomizeBossSprites.Value);
            flags.PushValue(this.SpriteOption.RandomizeEnemySprites.Randomize);
            flags.PushValue(this.SpriteOption.RandomizeEnemySprites.Value);
            flags.PushValue(this.SpriteOption.RandomizeEnvironmentSprites.Randomize);
            flags.PushValue(this.SpriteOption.RandomizeEnvironmentSprites.Value);
            flags.PushValue(this.SpriteOption.RandomizeItemPickupSprites.Randomize);
            flags.PushValue(this.SpriteOption.RandomizeItemPickupSprites.Value);
            flags.PushValue(this.SpriteOption.RandomizeSpecialWeaponSprites.Randomize);
            flags.PushValue(this.SpriteOption.RandomizeSpecialWeaponSprites.Value);

            // Quality of life options
            flags.PushValue(this.QualityOfLifeOption.DisableDelayScrolling.Randomize);
            flags.PushValue(this.QualityOfLifeOption.DisableDelayScrolling.Value);
            flags.PushValue(this.QualityOfLifeOption.DisableFlashingEffects.Randomize);
            flags.PushValue(this.QualityOfLifeOption.DisableFlashingEffects.Value);
            flags.PushValue(this.QualityOfLifeOption.EnableUnderwaterLagReduction.Randomize);
            flags.PushValue(this.QualityOfLifeOption.EnableUnderwaterLagReduction.Value);

            return flags.ToFlagString();
        }


        public String GetCosmeticFlagsString()
        {
            // TODO: Work out a way to bind options to a randomization flags
            // instance such that updating the property will automatically
            // update the flags value
            RandomizationFlags flags = new RandomizationFlags(14);

            // Cosmetic options
            flags.PushValue(this.CosmeticOption.HudElement.Randomize);
            flags.PushValue(this.CosmeticOption.HudElement.Value);
            flags.PushValue(this.CosmeticOption.PlayerSprite.Randomize);
            flags.PushValue(this.CosmeticOption.PlayerSprite.Value);
            flags.PushValue(this.CosmeticOption.RandomizeColorPalettes.Randomize);
            flags.PushValue(this.CosmeticOption.RandomizeColorPalettes.Value);
            flags.PushValue(this.CosmeticOption.RandomizeInGameText.Randomize);
            flags.PushValue(this.CosmeticOption.RandomizeInGameText.Value);
            flags.PushValue(this.CosmeticOption.RandomizeMusicTracks.Randomize);
            flags.PushValue(this.CosmeticOption.RandomizeMusicTracks.Value);

            return flags.ToFlagString();
        }
    }
}
