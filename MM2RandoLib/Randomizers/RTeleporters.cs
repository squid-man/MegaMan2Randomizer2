using System;
using System.Collections.Generic;
using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;
using MM2Randomizer.Utilities;

namespace MM2Randomizer.Randomizers
{
    public class RTeleporters : IRandomizer
    {
        public RTeleporters() { }

        public void Randomize(Patch in_Patch, RandomizationContext in_Context)
        {
            // Create list of default teleporter position values
            List<Position> DEFAULT_TELEPORTER_POSITIONS = new List<Position>
            {
                new Position(0x20, 0x3B), // Teleporter X, Y (top-left)
                new Position(0x20, 0x7B),
                new Position(0x20, 0xBB),
                new Position(0x70, 0xBB),
                new Position(0x90, 0xBB),
                new Position(0xE0, 0x3B),
                new Position(0xE0, 0x7B),
                new Position(0xE0, 0xBB),
            };

            IList<Position> newTeleporterPositions = in_Context.Seed.Shuffle(DEFAULT_TELEPORTER_POSITIONS);

            // Write the new x-coordinates
            for (Int32 index = 0; index < newTeleporterPositions.Count; ++index)
            {
                in_Patch.Add(
                    (Int32)(EMiscAddresses.WarpXCoordinateStartAddress + index),
                    newTeleporterPositions[index].X,
                    String.Format("Teleporter {0} X-Pos", index));

                in_Patch.Add(
                    (Int32)(EMiscAddresses.WarpYCoordinateStartAddress + index),
                    newTeleporterPositions[index].Y,
                    String.Format("Teleporter {0} Y-Pos", index));
            }

            // These values will be copied over to $04b0 (y) and $0470 (x), which will be checked
            // for in real time to determine where Mega will teleport to
        }
    }
}
