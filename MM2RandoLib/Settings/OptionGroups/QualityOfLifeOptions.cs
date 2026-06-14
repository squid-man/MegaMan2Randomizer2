using MM2RandoLib.Settings.Options;
using MM2Randomizer.Enums;
using MM2Randomizer.Settings.Options;
using System;
using System.ComponentModel;

namespace MM2Randomizer.Settings.OptionGroups
{
    public class QualityOfLifeOptions : OptionGroup
    {
        [Description("Disable Flashing Effects")]
        [AssembleFile("Options.disable_flashing.asm")]
        public BoolOption DisableFlashingEffects { get; } = new(true);

        [Description("Reduce Underwater Lag")]
        // Reduces lag in various places (underwater, end of boss fight, and possibly other places) by disabling a subroutine that just delays until an NMI occurs.
        [PatchRom((Int32)ESubroutineAddress.WasteAFrame, Opcode6502.RTS)]
        public BoolOption EnableUnderwaterLagReduction { get; } = new(true);

        [Description("Disable Waterfall")]
        [PatchRom(0xFE10, 1)] // Set Bubble Man number of palettes to 1
        public BoolOption DisableWaterfall { get; } = new(true);

        [Description("Allow Pause During Item Use")]
        [AssembleFile("Options.pause_with_items.asm")]
        public BoolOption DisablePauseLock { get; } = new(true);

        [Description("Enable Leftward Wall Ejection")]
        [PatchRom(0xe, 0x8986, 0x18)] // Change SEC to CLC
        public BoolOption EnableLeftwardWallEjection { get; } = new(false);

        [Description("Fix Bird Object Despawn")]
        [AssembleFile("Options.bird_egg_fix.asm")]
        public BoolOption EnableBirdEggFix { get; } = new(true);

        [Description("Make Stage Select Default")]
        [Tooltip("Make Stage Select the default choice after obtaining weapons rather than Password.")]
        [PatchRom(0x37bea, 1)]
        public BoolOption StageSelectDefault { get; } = new(true);

        [Description("Allow Self-Destruct")]
        [Tooltip("Pressing Up + A + Select instantly kills Mega Man. Good for getting out of soft-locks.")]
        [AssembleFile("Options.self_destruct.asm")]
        public BoolOption AllowSelfDestruct { get; } = new(true);

        [Description("E-Tank Health Threshold")]
        [Tooltip("Prevents e-tanks from being used at this health and above. To force e-tank use at this level hold Select when Start is pressed.")]
        [DefineValueSymbol("ETANK_PROTECTION_LEVEL")]
        public EnumOption<PercentOption> AccidentalEtankProtectionLevel { get; } = new(PercentOption.Percent100);

        [Description("Weapons Regained on Death")]
        [Tooltip("Regains this amount of energy for all weapons on death.")]
        [DefineValueSymbol("REGAIN_WEAPON_ENERGY_ON_DEATH")]
        public EnumOption<PercentOption> AddWeaponEnergyOnDeath { get; } = new(PercentOption.Percent0);
    }
}
