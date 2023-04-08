using MM2Randomizer.Settings.Options;

namespace MM2Randomizer.Settings.OptionGroups
{
    public class ChargingSpeedOptions
    {
        public RandomizationOption<ChargingSpeedOption> CastleBossEnergy { get; } = new RandomizationOption<ChargingSpeedOption>();

        public RandomizationOption<ChargingSpeedOption> EnergyTank { get; } = new RandomizationOption<ChargingSpeedOption>();

        public RandomizationOption<ChargingSpeedOption> HitPoints { get; } = new RandomizationOption<ChargingSpeedOption>();

        public RandomizationOption<ChargingSpeedOption> RobotMasterEnergy { get; } = new RandomizationOption<ChargingSpeedOption>();

        public RandomizationOption<ChargingSpeedOption> WeaponEnergy { get; } = new RandomizationOption<ChargingSpeedOption>();
    }
}
