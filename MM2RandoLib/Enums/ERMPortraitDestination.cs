using System;

namespace MM2Randomizer.Enums
{
    /// <summary>
    /// These values correspond to the stage numbers in the vanilla rom.
    /// For example, heat man is stored as the first stage. These values
    /// range from 0 to 7 and can safely be used as array indices.
    /// </summary>
    /// Please keep these constraints in mind when editing these values.
    public enum ERMPortraitDestination : Int32
    {
        HeatMan = 0,
        AirMan = 1,
        WoodMan = 2,
        BubbleMan = 3,
        QuickMan = 4,
        FlashMan = 5,
        MetalMan = 6,
        CrashMan = 7,
    }
}
