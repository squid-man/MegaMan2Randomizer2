using System;
using MM2Randomizer.Enums;

namespace MM2Randomizer.Randomizers.Stages
{
    /// <summary>
    /// This Object encapsulates the relevant ROM offsets for properties of each 
    /// selectable Robot Master portrait on the Stage Select screen.
    /// </summary>
    public class StageFromSelect
    {
        public String PortraitName;
        public ERMPortraitText TextAddress;
        public String TextValues;
        public ERMPortraitAddress PortraitAddress;
        public DestinationPair PortraitDestination;

        public ERMPortraitDestination InitialPortraitDestination
        {
            set => PortraitDestination = new DestinationPair()
            {
                Old = value,
                New = value
            };
        }

        public static EBossIndex GetBossIndex(ERMPortraitDestination in_Dest)
        {
            return in_Dest switch
            {
                ERMPortraitDestination.HeatMan => EBossIndex.Heat,
                ERMPortraitDestination.AirMan => EBossIndex.Air,
                ERMPortraitDestination.WoodMan => EBossIndex.Wood,
                ERMPortraitDestination.BubbleMan => EBossIndex.Bubble,
                ERMPortraitDestination.QuickMan => EBossIndex.Quick,
                ERMPortraitDestination.FlashMan => EBossIndex.Flash,
                ERMPortraitDestination.MetalMan => EBossIndex.Metal,
                ERMPortraitDestination.CrashMan => EBossIndex.Crash,
                _ => throw new IndexOutOfRangeException(),
            };
        }
    }

    public class DestinationPair
    {
        public ERMPortraitDestination Old { get; set; }
        public ERMPortraitDestination New { get; set; }
    }

}
