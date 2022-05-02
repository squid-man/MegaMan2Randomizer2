using System;

namespace MM2Randomizer.Settings.Options
{
    public class RandomizationOption<T> where T : struct
    {
        public Boolean Randomize { get; set; }

        public T Value { get; set; }
    }
}
