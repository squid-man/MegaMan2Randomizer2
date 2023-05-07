using System;
using MM2Randomizer.Random;
using MM2Randomizer.Settings.OptionGroups;

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

        public Boolean SetTheme { get; set; }



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

        public dynamic ActualizeBehaviorSettings(ISeed in_Seed)
        {
            // TODO: This can be simplified with an ExpandoObject

            return new
            {
                GameplayOption = new
                {
                    BurstChaserMode = this.GameplayOption.BurstChaserMode.NextValue(in_Seed),
                    FasterCutsceneText = this.GameplayOption.FasterCutsceneText.NextValue(in_Seed),
                    HideStageNames = this.GameplayOption.HideStageNames.NextValue(in_Seed),
                    RandomizeBossWeaknesses = this.GameplayOption.RandomizeBossWeaknesses.NextValue(in_Seed),
                    RandomizeEnemySpawns = this.GameplayOption.RandomizeEnemySpawns.NextValue(in_Seed),
                    RandomizeEnemyWeaknesses = this.GameplayOption.RandomizeEnemyWeaknesses.NextValue(in_Seed),
                    RandomizeFalseFloors = this.GameplayOption.RandomizeFalseFloors.NextValue(in_Seed),
                    RandomizeRefightTeleporters = this.GameplayOption.RandomizeRefightTeleporters.NextValue(in_Seed),
                    RandomizeRobotMasterBehavior = this.GameplayOption.RandomizeRobotMasterBehavior.NextValue(in_Seed),
                    RandomizeRobotMasterLocations = this.GameplayOption.RandomizeRobotMasterLocations.NextValue(in_Seed),
                    RandomizeRobotMasterStageSelection = this.GameplayOption.RandomizeRobotMasterStageSelection.NextValue(in_Seed),
                    RandomizeSpecialItemLocations = this.GameplayOption.RandomizeSpecialItemLocations.NextValue(in_Seed),
                    RandomizeSpecialWeaponBehavior = this.GameplayOption.RandomizeSpecialWeaponBehavior.NextValue(in_Seed),
                    RandomizeSpecialWeaponReward = this.GameplayOption.RandomizeSpecialWeaponReward.NextValue(in_Seed),
                },
                ChargingSpeedOption = new
                {
                    CastleBossEnergy = this.ChargingSpeedOption.CastleBossEnergy.NextValue(in_Seed),
                    EnergyTank = this.ChargingSpeedOption.EnergyTank.NextValue(in_Seed),
                    HitPoints = this.ChargingSpeedOption.HitPoints.NextValue(in_Seed),
                    RobotMasterEnergy = this.ChargingSpeedOption.RobotMasterEnergy.NextValue(in_Seed),
                    WeaponEnergy = this.ChargingSpeedOption.WeaponEnergy.NextValue(in_Seed),
                },
                SpriteOption = new
                {
                    RandomizeBossSprites = this.SpriteOption.RandomizeBossSprites.NextValue(in_Seed),
                    RandomizeEnemySprites = this.SpriteOption.RandomizeEnemySprites.NextValue(in_Seed),
                    RandomizeEnvironmentSprites = this.SpriteOption.RandomizeEnvironmentSprites.NextValue(in_Seed),
                    RandomizeItemPickupSprites = this.SpriteOption.RandomizeItemPickupSprites.NextValue(in_Seed),
                    RandomizeSpecialWeaponSprites = this.SpriteOption.RandomizeSpecialWeaponSprites.NextValue(in_Seed),
                },
                QualityOfLifeOption = new
                {
                    DisableWaterfall = this.QualityOfLifeOption.DisableWaterfall.NextValue(in_Seed),
                    DisableFlashingEffects = this.QualityOfLifeOption.DisableFlashingEffects.NextValue(in_Seed),
                    EnableUnderwaterLagReduction = this.QualityOfLifeOption.EnableUnderwaterLagReduction.NextValue(in_Seed),
                },
            };
        }

        public dynamic ActualizeCosmeticSettings(ISeed in_Seed)
        {
            // TODO: This can be simplified with an ExpandoObject

            return new
            {
                CosmeticOption = new
                {
                    Font = this.CosmeticOption.Font.NextValue(in_Seed),
                    HudElement = this.CosmeticOption.HudElement.NextValue(in_Seed),
                    PlayerSprite = this.CosmeticOption.PlayerSprite.NextValue(in_Seed),
                    RandomizeColorPalettes = this.CosmeticOption.RandomizeColorPalettes.NextValue(in_Seed),
                    RandomizeInGameText = this.CosmeticOption.RandomizeInGameText.NextValue(in_Seed),
                    RandomizeMusicTracks = this.CosmeticOption.RandomizeMusicTracks.NextValue(in_Seed),
                    RandomizeMenusAndTransitionScreens = this.CosmeticOption.RandomizeMenusAndTransitionScreens.NextValue(in_Seed),
                },
            };
        }

        public void SetFromFlagString(String in_OptionsFlagString, String in_CosmeticFlagString)
        {
        }

        public String GetBehaviorFlagsString()
        {
            // TODO: The settings class can inherit from the flags class. Flags
            // can be dynamically tracked by reflecting the properties of the class.
            // Flag properties can be differentiated with Attributes
            RandomizationFlags flags = new RandomizationFlags(28);

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
            flags.PushValue(this.QualityOfLifeOption.DisableWaterfall.Randomize);
            flags.PushValue(this.QualityOfLifeOption.DisableWaterfall.Value);
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
            flags.PushValue(this.CosmeticOption.Font.Randomize);
            flags.PushValue(this.CosmeticOption.Font.Value);
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
