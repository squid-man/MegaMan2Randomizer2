using MM2RandoLib.Settings.Options;
using MM2Randomizer.Settings.Options;
using System.ComponentModel;

namespace MM2Randomizer.Settings.OptionGroups
{
    public class ChargingSpeedOptions : OptionGroup
    {
        [Description("Hit Point Charging Speed")]
        public EnumOption<ChargingSpeedOption> HitPoints { get; } = new(ChargingSpeedOption.Fastest);

        [Description("Weapon Energy Charging Speed")]
        public EnumOption<ChargingSpeedOption> WeaponEnergy { get; } = new(ChargingSpeedOption.Fastest);

        [Description("Energy Tank Charging Speed")]
        public EnumOption<ChargingSpeedOption> EnergyTank { get; } = new(ChargingSpeedOption.Fastest);

        [Description("Robot Master Energy Charging Speed")]
        public EnumOption<ChargingSpeedOption> RobotMasterEnergy { get; } = new(ChargingSpeedOption.Fastest);

        [Description("Castle Boss Energy Charging Speed")]
        public EnumOption<ChargingSpeedOption> CastleBossEnergy { get; } = new(ChargingSpeedOption.Fastest);
    }
}
