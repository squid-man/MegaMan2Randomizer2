using System;

namespace MM2Randomizer.Settings.Options
{
    public class GameplayOptions
    {
        public RandomizationOption<Boolean> BurstChaserMode { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> FasterCutsceneText { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> HideStageNames { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeBossWeaknesses { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeEnemySpawns { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeEnemyWeaknesses { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeFalseFloors { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeRefightTeleporters { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeRobotMasterBehavior { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeRobotMasterLocations { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeRobotMasterStageSelection { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeSpecialItemLocations { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeSpecialWeaponBehavior { get; set; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeSpecialWeaponReward { get; set; } = new RandomizationOption<Boolean>();
    }
}
