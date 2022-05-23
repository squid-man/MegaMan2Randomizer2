using MM2Randomizer.Settings.Options;

namespace MM2Randomizer.Settings.OptionGroups
{
    public class SpriteOptions
    {
        public RandomizationOption<BooleanOption> RandomizeBossSprites { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> RandomizeEnemySprites { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> RandomizeEnvironmentSprites { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> RandomizeItemPickupSprites { get; } = new RandomizationOption<BooleanOption>();

        public RandomizationOption<BooleanOption> RandomizeSpecialWeaponSprites { get; } = new RandomizationOption<BooleanOption>();
    }
}
