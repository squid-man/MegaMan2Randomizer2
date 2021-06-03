using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Enums
{
    public enum ERMPortraitAddress
    {
        /// <summary>
        /// See https://datacrystal.romhacking.net/wiki/Mega_Man_2:ROM_map#Stage_Select
        ///
        /// The details of how to render each portait on the stage select screen is stored
        /// around 0x0346XX in the rom. The addresses in this enum point to a value that
        /// determines which stage will be selected when choosing that portrait.
        ///
        /// See also ERMPortraitDestination.
        ///
        /// These are the default values from vanilla:
        ///
        /// StageSelect  Address    Value
        /// -----------------------------
        /// Bubble Man   0x034670   3
        /// Air Man      0x034671   1
        /// Quick Man    0x034672   4
        /// Wood Man     0x034673   2
        /// Crash Man    0x034674   7
        /// Flash Man    0x034675   5
        /// Metal Man    0x034676   6
        /// Heat Man     0x034677   0
        /// </summary>

        HeatMan = 0x034677,
        
        AirMan = 0x034671,
        
        WoodMan = 0x034673,
        
        BubbleMan = 0x034670,
        
        QuickMan = 0x034672,
        
        FlashMan = 0x034675,
        
        MetalMan = 0x034676,
        
        CrashMan = 0x034674
    }
}
