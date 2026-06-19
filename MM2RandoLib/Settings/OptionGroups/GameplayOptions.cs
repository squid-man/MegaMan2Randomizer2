using MM2RandoLib.Settings.Options;
using MM2Randomizer.Settings.Options;
using System;
using System.ComponentModel;

namespace MM2Randomizer.Settings.OptionGroups
{
    public class GameplayOptions : OptionGroup
    {
        [NoCreateControl]
        public BoolOption RandomizeRefightTeleporters { get; } = new(true);
        [NoCreateControl]
        public BoolOption RandomizeRobotMasterStageSelection { get; } = new(true);
        [NoCreateControl]
        public BoolOption RandomizeSpecialWeaponReward { get; } = new(true);

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
        [PatchRom(0xe, 0x95ad, [0xea, 0xea, 0xea])] // M-445 Palette Glitch Fix
        public BoolOption RandomizeEnemySpawns { get; } = new(true);

        [Description("Randomize Enemy Weaknesses")]
        public BoolOption RandomizeEnemyWeaknesses { get; } = new(true);

        [Description("Randomize False Floors")]
        public BoolOption RandomizeFalseFloors { get; } = new(true);

        [Description("Enable Faster Cutscene Text")]
        [DefineSymbol("FASTER_CUTSCENE_TEXT")] // Needed in Options.disable_flashing.asm
        [AssembleFile("Options.faster_cutscenes.asm")]
        public BoolOption FasterCutsceneText { get; } = new(true);

        [Description("Enable Burst Chaser Mode")]
        [AssembleFile("Options.burst_chaser.asm")]
        public BoolOption BurstChaserMode { get; } = new(false);

        [Description("Hide Stage Names")]
        public BoolOption HideStageNames { get; } = new(false);

        [Description("Instant Death Ignores Invincibility")]
        [Tooltip("Instant death hazards such as spikes will be lethal regardless of invincibility frames like in Mega Man 1.")]
        [AssembleFile("Options.merciless.asm")]
        public BoolOption MercilessMode { get; } = new(false);

        [Description("Randomize PicoPico-kun Spawns")]
        public BoolOption RandomizePicoPicoSpawns { get; } = new(true);


        [Description("Disallow Bubble Barrier Vulnerability")]
        [Tooltip("Do not allow Bubble Lead to be the vulnerability of Crash barriers to avoid strange interactions especially during Boobeam fight.")]
        public BoolOption DisallowBubbleBarrierWeakness { get; } = new(true);

        [Description("Deterministic Heat Man Delays")]
        [Tooltip("Randomize Heat Man delays at seed generation, repeating the same pattern every life to ensure the same pattern occurs for all players.")]
        [AssembleFile("Options.fair_heat_man.asm")]
        public BoolOption FairHeatManDelays { get; } = new(false);
    }
}
