using System;

namespace MM2Randomizer.Settings.Options
{
    public class SpriteOptions
    {
        public RandomizationOption<Boolean> RandomizeBossSprites { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeEnemySprites { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeEnvironmentSprites { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeItemPickupSprites { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeSpecialWeaponSprites { get; } = new RandomizationOption<Boolean>();
    }
}
