using MM2RandoLib.Settings.Options;
using MM2Randomizer.Settings.Options;
using System.ComponentModel;

namespace MM2Randomizer.Settings.OptionGroups
{
    [CosmeticOptionGroup]
    public class CosmeticOptions : OptionGroup
    {
        [Description("Randomize Color Palettes")]
        public BoolOption RandomizeColorPalettes { get; } = new(true);

        [Description("Randomize Music Tracks")]
        public BoolOption RandomizeMusicTracks { get; } = new(true);

        [Description("Omit Unfriendly Music Tracks")]
        public BoolOption OmitUnsafeMusicTracks { get; } = new(false);

        [Description("Randomize In Game Text")]
        public BoolOption RandomizeInGameText { get; } = new(true);

        [Description("Randomize Menus and Transition Screens")]
        public BoolOption RandomizeMenusAndTransitionScreens { get; } = new(true);

        [Description("Character")]
        public EnumOption<PlayerSpriteOption> PlayerSprite { get; } = new(PlayerSpriteOption.MegaMan);

        [Description("Cannon Shot")]
        public EnumOption<CannonShotOption> CannonShot { get; } = new(CannonShotOption.PlasmaCannon);

        [Description("HUD")]
        public EnumOption<HudElementOption> HudElement { get; } = new(HudElementOption.Default);

        [Description("Font")]
        public EnumOption<FontOption> Font { get; } = new(FontOption.Default);
    }
}
