using System;
using System.Collections.Generic;
using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;

namespace MM2Randomizer.Randomizers.Colors
{
    public class ColorSet
    {
        public Int32[]? addresses;
        public List<EColorsHex[]> ColorBytes;
        public Int32 Index;

        public ColorSet()
        {
            this.ColorBytes = new List<EColorsHex[]>();
            this.Index = 0;
        }

        public void RandomizeAndWrite(Patch in_Patch, ISeed in_Seed, Int32 setNumber)
        {
            if (null == this.addresses)
            {
                throw new NullReferenceException(@"Addresses has not been initialized");
            }

            this.Index = in_Seed.NextInt32(ColorBytes.Count);

            for (Int32 i = 0; i < this.addresses.Length; i++)
            {
                in_Patch.Add(
                    this.addresses[i],
                    (Byte)this.ColorBytes[this.Index][i],
                    String.Format("Color Set {0} (Index Chosen: {1}) Value #{2}", setNumber, Index, i));
            }
        }
    }
}
