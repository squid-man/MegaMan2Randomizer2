using MM2Randomizer.Settings.Options;

namespace MM2Randomizer.Settings.OptionGroups
{
    public class GameplayOptions
    {
        public RandomizationOption<BooleanOption> BurstChaserMode { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> FasterCutsceneText { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> HideStageNames { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> RandomizeBossWeaknesses { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> RandomizeEnemySpawns { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> RandomizeEnemyWeaknesses { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> RandomizeFalseFloors { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> RandomizeRefightTeleporters { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> RandomizeRobotMasterBehavior { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> RandomizeRobotMasterLocations { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> RandomizeRobotMasterStageSelection { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> RandomizeSpecialItemLocations { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> RandomizeSpecialWeaponBehavior { get; set; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> RandomizeSpecialWeaponReward { get; set; } = new RandomizationOption<BooleanOption>();
    }
}
