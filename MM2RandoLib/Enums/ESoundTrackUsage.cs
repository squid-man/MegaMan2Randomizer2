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

    public struct SoundTrackUsage
    {
        public static ESoundTrackUsage FromStrings(IEnumerable<String> in_Uses)
        {
            ESoundTrackUsage usage = (ESoundTrackUsage)0;
            foreach (String str in in_Uses)
                usage |= Enum.Parse<ESoundTrackUsage>(str);

            return usage;
        }
    }
}
