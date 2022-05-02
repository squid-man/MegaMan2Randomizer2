namespace MM2Randomizer.Settings.Options
{
    public class ChargingSpeedOptions
    {
        public RandomizationOption<ChargingSpeed> CastleBossEnergy { get; } = new RandomizationOption<ChargingSpeed>();

        public RandomizationOption<ChargingSpeed> EnergyTank { get; } = new RandomizationOption<ChargingSpeed>();

        public RandomizationOption<ChargingSpeed> HitPoints { get; } = new RandomizationOption<ChargingSpeed>();

        public RandomizationOption<ChargingSpeed> RobotMasterEnergy { get; } = new RandomizationOption<ChargingSpeed>();

        public RandomizationOption<ChargingSpeed> WeaponEnergy { get; } = new RandomizationOption<ChargingSpeed>();
    }
}
