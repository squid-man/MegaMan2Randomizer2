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
            EBossIndex index;
            switch (in_Dest)
            {
                case ERMPortraitDestination.HeatMan:
                    index = EBossIndex.Heat;
                    break;
                case ERMPortraitDestination.AirMan:
                    index = EBossIndex.Air;
                    break;
                case ERMPortraitDestination.WoodMan:
                    index = EBossIndex.Wood;
                    break;
                case ERMPortraitDestination.BubbleMan:
                    index = EBossIndex.Bubble;
                    break;
                case ERMPortraitDestination.QuickMan:
                    index = EBossIndex.Quick;
                    break;
                case ERMPortraitDestination.FlashMan:
                    index = EBossIndex.Flash;
                    break;
                case ERMPortraitDestination.MetalMan:
                    index = EBossIndex.Metal;
                    break;
                case ERMPortraitDestination.CrashMan:
                    index = EBossIndex.Clash;
                    break;
                default:
                    throw new IndexOutOfRangeException();
            };
            return index;
        }
    }

    public class DestinationPair
    {
        public ERMPortraitDestination Old { get; set; }
        public ERMPortraitDestination New { get; set; }
    }

}
