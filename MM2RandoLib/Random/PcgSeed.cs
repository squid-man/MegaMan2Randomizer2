using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MM2Randomizer.Extensions;

namespace MM2Randomizer.Random
{
    /// <summary>
    /// Provides basic randomization methods
    /// </summary>
    public sealed class PcgSeed : ISeed
    {
        //
        // Constructors
        //

        public PcgSeed(string? seedString = null)
        {
            // "" is a valid seed string
            if (seedString == null)
                seedString = new System.Random().GetHexString(20);

            _seed = seedString.ToUInt64Hash();
            _state = 0;

            SeedString = seedString;
            Identifier = _seed.ToAlphaBase26();

            Reset();
        }


        //
        // Properties
        //

        public string SeedString { get; }

        public string Identifier { get; }


        //
        // Public Methods
        //

        public void Reset()
        {
            _state = _seed + Increment;
            Next();
        }

        void ISeed.Next()
            => Next();

        // Boolean Methods
        public bool NextBoolean()
            => (Next() & 1) > 0;

        // UInt8 Methods
        public byte NextUInt8()
            => (byte)(Next() & 0xff);

        public byte NextUInt8(int maxValue)
            => (byte)Next(checked((uint)maxValue));

        public byte NextUInt8(int minValue, int maxValue)
            => (byte)(Next(checked((uint)(maxValue - minValue))) + minValue);


        // UInt32 Methods
        public uint NextUInt32()
            => Next();

        public uint NextUInt32(uint maxValue)
            => (uint)(NextUInt64() % maxValue);

        public uint NextUInt32(uint minValue, uint maxValue)
            => NextUInt32(checked(maxValue - minValue)) + minValue;

        // Int32 Methods
        public int NextInt32()
            => (int)NextUInt32() + int.MinValue;

        public int NextInt32(int maxValue)
            => (int)NextUInt32(checked((uint)maxValue));

        public int NextInt32(int minValue, int maxValue)
        {
            ulong range = checked((ulong)((long)maxValue - minValue));
            return (int)(NextUInt64() % range) + minValue;
        }

        public double NextDouble()
            => (double)(NextUInt64() >> 11) / (1ul << 53);


        // UInt64 Methods
        public ulong NextUInt64()
            => ((ulong)Next() << 32) + Next();


        // IEnumerator Methods
        public object? NextArrayElement(Array array)
            => array.GetValue((int)Next((uint)array.Length));


        // IEnumerable Methods
        public T NextElement<T>(IEnumerable<T> elements)
            => elements.ElementAt((int)Next((uint)elements.Count()));

        public IList<T> Shuffle<T>(IEnumerable<T> list)
            => list.OrderBy(x => Next()).ToList();


        //
        // Private Data Members
        //

        const ulong Multiplier = 6364136223846793005u;
        const ulong Increment = 1442695040888963407u;

        ulong _seed;
        ulong _state = 0x4d595df4d0f33173;

        uint Next()
        {
            ulong x = _state;
            int shiftCount = (int)(x >> 59);

            _state = x * Multiplier + Increment;
            x ^= x >> 18;

            return BitOperations.RotateRight((uint)(x >> 27), shiftCount);
        }

        uint Next(uint maxValue)
            => Next() % maxValue;
    }
}
