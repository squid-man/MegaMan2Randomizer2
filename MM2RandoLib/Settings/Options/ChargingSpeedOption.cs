using System.ComponentModel;

namespace MM2Randomizer.Settings.Options
{
    public enum ChargingSpeedOption
    {
        [Description("Fastest")]
        Fastest = 0x0,
        [Description("Faster")]
        Faster = 0x1,
        [Description("Faster (Irregular)")]
        FasterIrregular = 0x2,
        [Description("Normal")]
        Normal = 0x3,
        [Description("Fast (Irregular)")]
        FastIrregular = 0x4,
        [Description("Slow")]
        Slow = 0x7,
    }
}
