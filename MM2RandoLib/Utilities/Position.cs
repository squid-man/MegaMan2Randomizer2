using System;

namespace MM2Randomizer.Utilities
{
    public struct Position
    {
        public Position(Byte in_X, Byte in_Y)
        {
            this.X = in_X;
            this.Y = in_Y;
        }

        public Byte X { get; set; }
        public Byte Y { get; set; }
    }
}
