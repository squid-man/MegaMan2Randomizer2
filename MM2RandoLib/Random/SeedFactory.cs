using System;
using System.Reflection;

namespace MM2Randomizer.Random
{
    public static class SeedFactory
    {
        /// <summary>
        /// Create a new Seed from the given input String.
        /// </summary>
        public static ISeed Create(GeneratorType in_Generator, String in_SeedString)
        {
            if (null == in_SeedString)
            {
                throw new ArgumentNullException(nameof(in_SeedString));
            }

            switch (in_Generator)
            {
                case GeneratorType.Default:
                {
                    return Activator.CreateInstance(
                        typeof(DefaultSeed),
                        BindingFlags.NonPublic | BindingFlags.Instance,
                        null,
                        new Object[] { in_SeedString },
                        null) as ISeed ?? throw new NullReferenceException(@"Unable to create instance of DefaultSeed");
                }

                case GeneratorType.MT19937:
                {
                    return Activator.CreateInstance(
                        typeof(MT19937Seed),
                        BindingFlags.NonPublic | BindingFlags.Instance,
                        null,
                        new Object[] { in_SeedString },
                        null) as ISeed ?? throw new NullReferenceException(@"Unable to create instance of MT19937Seed");
                }

                default:
                {
                    throw new ArgumentException(@"Invalid GeneratorType", nameof(in_Generator));
                }
            }
        }


        /// <summary>
        /// Create a new random Seed.
        /// </summary>
        public static ISeed Create(GeneratorType in_Generator)
        {
            switch (in_Generator)
            {
                case GeneratorType.Default:
                {
                    return Activator.CreateInstance(typeof(DefaultSeed), true) as ISeed ?? throw new NullReferenceException(@"Unable to create instance of DefaultSeed"); ;
                }

                case GeneratorType.MT19937:
                {
                    return Activator.CreateInstance(typeof(MT19937Seed), true) as ISeed ?? throw new NullReferenceException(@"Unable to create instance of MT19937Seed"); ;
                }

                default:
                {
                    throw new ArgumentException(@"Invalid GeneratorType", nameof(in_Generator));
                }
            }
        }
    }
}
