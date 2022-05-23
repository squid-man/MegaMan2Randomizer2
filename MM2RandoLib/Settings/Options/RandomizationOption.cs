using System;
using MM2Randomizer.Random;

namespace MM2Randomizer.Settings.Options
{
    public class RandomizationOption<T> where T : Enum
    {
        public Boolean Randomize { get; set; }

        public T Value { get; set; } = default(T) ?? throw new Exception("Enums cannot be null");

        public T NextValue(ISeed in_Seed)
        {
            // TODO: Add option pools to this class so users can specify the
            // pool of values from which to pull random options

            if (true == this.Randomize)
            {
                return in_Seed.NextEnum<T>();
            }
            else
            {
                return this.Value;
            }
        }
    }
}
