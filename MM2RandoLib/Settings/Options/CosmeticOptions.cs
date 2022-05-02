using System;

namespace MM2Randomizer.Settings.Options
{
    public class CosmeticOptions
    {
        public RandomizationOption<HudElement> HudElement { get; } = new RandomizationOption<HudElement>();

        public RandomizationOption<PlayerSprite> PlayerSprite { get; } = new RandomizationOption<PlayerSprite>();

        public RandomizationOption<Boolean> RandomizeColorPalettes { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeInGameText { get; } = new RandomizationOption<Boolean>();

        public RandomizationOption<Boolean> RandomizeMusicTracks { get; } = new RandomizationOption<Boolean>();
    }
}
