using MM2Randomizer.Settings.Options;

namespace MM2Randomizer.Settings.OptionGroups
{
    public class QualityOfLifeOptions
    {
        public RandomizationOption<BooleanOption> DisableWaterfall { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> EnableLeftwardWallEjection { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> DisableFlashingEffects { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> EnableUnderwaterLagReduction { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> DisablePauseLock { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> EnableBirdEggFix { get; } = new RandomizationOption<BooleanOption>();
    }
}
