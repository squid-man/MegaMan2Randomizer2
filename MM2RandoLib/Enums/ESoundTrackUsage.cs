using System;
using System.Collections.Generic;

namespace MM2Randomizer.Enums
{
    [Flags]
    public enum ESoundTrackUsage
    {
        Intro = 1,
        Title = 2,
        StageSelect = 4,
        Stage = 8,
        Boss = 16,
        Ending = 32,
        Credits = 64,
    }
}
