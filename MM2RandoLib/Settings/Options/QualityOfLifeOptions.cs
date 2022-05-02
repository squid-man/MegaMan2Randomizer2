using System;

namespace MM2Randomizer.Settings.Options
{
    public class QualityOfLifeOptions
    {
        public RandomizationOption<Boolean> DisableDelayScrolling { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> DisableFlashingEffects { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> EnableUnderwaterLagReduction { get; } = new RandomizationOption<Boolean>();
    }
}
