using System;
using System.Linq;
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
        Boss = 0x10,
        Refights = 0x20,
        Ending = 0x40,
        Credits = 0x80,
    }

    public struct SoundTrackUsage
    {
        public static readonly Dictionary<String, ESoundTrackUsage> Values
            = Enumerable.ToDictionary(
                Enum.GetValues<ESoundTrackUsage>(),
                val => (Enum.GetName<ESoundTrackUsage>(val) ?? ""),
                StringComparer.InvariantCultureIgnoreCase);

        public static ESoundTrackUsage FromStrings(IEnumerable<String> in_Uses)
        {
            ESoundTrackUsage usage = (ESoundTrackUsage)0;
            foreach (String str in in_Uses)
                usage |= Values[str];

            return usage;
        }
    }
}
