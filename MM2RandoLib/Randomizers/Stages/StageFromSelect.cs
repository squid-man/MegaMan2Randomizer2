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
        public readonly String PortraitName;
        public readonly ERMPortraitText TextAddress;
        public readonly String TextValues;
        public readonly ERMPortraitAddress PortraitAddress;
        public readonly DestinationPair PortraitDestination;

        //
        // Constructors
        //

        public StageFromSelect(String in_PortraitName, ERMPortraitText in_TextAddress, String in_TextValues, ERMPortraitAddress in_PortraitAddress, ERMPortraitDestination in_InitialPortraitDestination)
        {
            this.PortraitName = in_PortraitName;
            this.TextAddress = in_TextAddress;
            this.TextValues = in_TextValues;
            this.PortraitAddress = in_PortraitAddress;
            this.PortraitDestination = new DestinationPair()
            {
                Old = in_InitialPortraitDestination,
                New = in_InitialPortraitDestination,
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
