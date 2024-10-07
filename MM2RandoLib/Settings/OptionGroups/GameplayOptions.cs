using MM2RandoLib.Settings.Options;
using MM2Randomizer.Settings.Options;
using System.ComponentModel;

namespace MM2Randomizer.Settings.OptionGroups
{
    public class GameplayOptions : OptionGroup
    {
        [Description("Randomize Special Weapon Behavior")]
        public BoolOption RandomizeSpecialWeaponBehavior { get; } = new(true);

        [Description("Randomize Boss Weaknesses")]
        public BoolOption RandomizeBossWeaknesses { get; } = new(true);

        [Description("Randomize Robot Master Locations")]
        public BoolOption RandomizeRobotMasterLocations { get; } = new(true);

        [Description("Randomize Robot Master Behavior")]
        public BoolOption RandomizeRobotMasterBehavior { get; } = new(true);

        [Description("Randomize Special Item Locations")]
        public BoolOption RandomizeSpecialItemLocations { get; } = new(true);

        [Description("Randomize Enemy Spawns")]
        public BoolOption RandomizeEnemySpawns { get; } = new(true);

        [Description("Randomize Enemy Weaknesses")]
        public BoolOption RandomizeEnemyWeaknesses { get; } = new(true);

        [Description("Randomize False Floors")]
        public BoolOption RandomizeFalseFloors { get; } = new(true);

        [Description("Enable Faster Cutscene Text")]
        public BoolOption FasterCutsceneText { get; } = new(true);

        [Description("Enable Burst Chaser Mode")]
        public BoolOption BurstChaserMode { get; } = new(false);

        [Description("Hide Stage Names")]
        public BoolOption HideStageNames { get; } = new(false);

        [Description("Instant Death Ignores Invincibility")]
        [Tooltip("Instant death hazards such as spikes will be lethal regardless of invincibility frames like in Mega Man 1.")]
        public BoolOption MercilessMode { get; } = new(false);

        [NoCreateControl]
        public BoolOption RandomizeRefightTeleporters { get; } = new(true);
        [NoCreateControl]
        public BoolOption RandomizeRobotMasterStageSelection { get; } = new(true);
        [NoCreateControl]
        public BoolOption RandomizeSpecialWeaponReward { get; } = new(true);
    }
}
