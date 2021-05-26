using System;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;

namespace MM2Randomizer.Randomizers
{
    public class RTilemap : IRandomizer
    {
        public RTilemap() { }


        public void Randomize(Patch in_Patch, RandomizationContext in_Context)
        {
            //ReadLevelComponentJSON(p, r);

            RTilemap.ChangeW4FloorsBeforeSpikes(in_Patch, in_Context.Seed);
            RTilemap.ChangeW4FloorsSpikePit(in_Patch, in_Context.Seed);
        }

        /*
        private void ReadLevelComponentJSON(Patch p, Random r)
        {
            ComponentManager component = JsonConvert.DeserializeObject<ComponentManager>(Properties.Resources.level_components);

            foreach (var levelComponent in component.LevelComponents)
            {
                // Get a random variation
                var variation = levelComponent.Variations[r.Next(levelComponent.Variations.Count)];

                // Add patch for each element of the tsamap
                Int32 startAddress = Convert.ToInt32(levelComponent.StartAddress, 16);
                for (Int32 i = 0; i < variation.TsaMap.Length; i++)
                {
                    // Parse hex String
                    Byte tsaVal = Convert.ToByte(variation.TsaMap[i], 16);

                    p.Add(startAddress + i, tsaVal, $"Tilemap data for {levelComponent.Name} variation \"{variation.Name}\"");
                }
            }

            Console.WriteLine(component.LevelComponents);
        }
        */

        private static void ChangeW4FloorsBeforeSpikes(Patch in_Patch, ISeed in_Seed)
        {
            // Choose 2 of the 5 32x32 tiles to be fake
            Int32 tileA = in_Seed.NextInt32(5);
            Int32 tileB = in_Seed.NextInt32(4);

            // Make sure 2nd tile chosen is different
            if (tileB == tileA)
            {
                tileB++;
            }

            for (Int32 i = 0; i < 5; i++)
            {
                if (i == tileA || i == tileB)
                {
                    in_Patch.Add(0x00CB5C + i * 8, 0x94, String.Format("Wily 4 Room 4 Tile {0} (fake)", i));
                }
                else
                {
                    in_Patch.Add(0x00CB5C + i * 8, 0x85, String.Format("Wily 4 Room 4 Tile {0} (solid)", i));
                }
            }
        }

        private static void ChangeW4FloorsSpikePit(Patch in_Patch, ISeed in_Seed)
        {
            // 5 tiles, but since two adjacent must construct a gap, 4 possible gaps.  Choose 1 random gap.
            Int32 gap = in_Seed.NextInt32(4);

            for (Int32 i = 0; i < 4; i++)
            {
                if (i == gap)
                {
                    in_Patch.Add(0x00CB9A + i * 8, 0x9B, String.Format("Wily 4 Room 5 Tile {0} (gap on right)", i));
                    in_Patch.Add(0x00CB9A + i * 8 + 8, 0x9C, String.Format("Wily 4 Room 5 Tile {0} (gap on left)", i));
                    ++i; // skip next tile since we just drew it
                }
                else
                {
                    in_Patch.Add(0x00CB9A + i * 8, 0x9D, String.Format("Wily 4 Room 5 Tile {0} (solid)", i));
                }
            }
        }


    }
}
