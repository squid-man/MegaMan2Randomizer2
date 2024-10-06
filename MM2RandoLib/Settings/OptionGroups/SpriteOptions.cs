using MM2RandoLib.Settings.Options;
using MM2Randomizer.Settings.Options;
using System.ComponentModel;

namespace MM2Randomizer.Settings.OptionGroups
{
    public class SpriteOptions : OptionGroup
    {
        [Description("Randomize Boss Sprites")]
        public BoolOption RandomizeBossSprites { get; } = new(true);

        [Description("Randomize Enemy Sprites")]
        public BoolOption RandomizeEnemySprites { get; } = new(true);

        [Description("Randomize Special Weapon Sprites")]
        public BoolOption RandomizeSpecialWeaponSprites { get; } = new(true);

        [Description("Randomize Item Pickup Sprites")]
        public BoolOption RandomizeItemPickupSprites { get; } = new(true);

        [Description("Randomize Environment Sprites")]
        public BoolOption RandomizeEnvironmentSprites { get; } = new(true);
    }
}
