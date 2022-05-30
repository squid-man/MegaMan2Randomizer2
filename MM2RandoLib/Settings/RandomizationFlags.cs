using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MM2Randomizer.Extensions;

namespace MM2Randomizer.Settings
{
    internal class RandomizationFlags
    {
        //
        // Constructors
        //

        public RandomizationFlags()
        {
            this.mBitArray = new BitArray(0);
        }


        //
        // Public Methods
        //

        public void PushValue(Boolean in_Value)
        {
            Int32 currentIndex = this.mBitArray.Length;
            this.mBitArray.Length = currentIndex + 1;
            this.mBitArray[currentIndex] = in_Value;
        }

        public void PushValue<T>(T in_Value) where T : Enum
        {
            Array enumValues = Enum.GetValues(typeof(T));

            Int32 currentIndex = this.mBitArray.Length;
            this.mBitArray.Length = currentIndex + enumValues.Length;

            foreach (T enumValue in enumValues)
            {
                this.mBitArray[currentIndex++] = in_Value.Equals(enumValue);
            }
        }

        /*
        public Boolean PopValue()
        {
            if (this.mCurrentIndex <= 0)
            {
                throw new IndexOutOfRangeException();
            }

            return this.mBitArray[this.mCurrentIndex--];
        }
        */

        /*
        public T PopValue<T>() where T : Enum
        {
            Array enumValues = Enum.GetValues(typeof(T));

            Int32 newIndex = this.mCurrentIndex - enumValues.Length;

            if (newIndex < 0)
            {
                throw new IndexOutOfRangeException();
            }

            T retval = (T)(enumValues.GetValue(0) ?? throw new Exception("Enum values should never be null"));
            Int32 assignedCount = 0;

            for (Int32 count = 0; count < enumValues.Length; ++count)
            {
                if (true == this.mBitArray[this.mCurrentIndex--])
                {
                    assignedCount++;
                    retval = (T)(enumValues.GetValue(count) ?? throw new Exception("Enum values should never be null"));
                }
            }

            return (1 == assignedCount) ? retval : throw new Exception($"More than one value was read for {typeof(T)}");
        }
        */


        public String ToFlagString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            Byte flagFieldValue = 0;
            Int32 currentCharacterBit = 0;

            for (Int32 index = 0; index < this.mBitArray.Length; ++index)
            {
                flagFieldValue |= (this.mBitArray[index] ? (Byte)0b_0000_0001 : (Byte)0b_0000_0000);
                currentCharacterBit++;

                if (RandomizationFlags.BITS_PER_FLAG_CHARACTER == currentCharacterBit)
                {
                    stringBuilder.Append(RandomizationFlags.FlagValueToAsciiCharacterLookup[flagFieldValue]);
                    currentCharacterBit = 0;
                    flagFieldValue = 0;
                }
                else
                {
                    flagFieldValue <<= 1;
                }
            }

            if (currentCharacterBit > 0)
            {
                stringBuilder.Append(RandomizationFlags.FlagValueToAsciiCharacterLookup[flagFieldValue]);
            }

            return stringBuilder.ToString();
        }

        public String ToHashString()
        {
            return this.ToFlagString().ToUInt64Hash().ToAlphaBase26();
        }

        /*
        public void FromFlagString(String in_FlagString)
        {
            BitArray bitArray = new BitArray(this.mMaxLength);

            Int32 index = 0;

            foreach (Char c in in_FlagString)
            {
                if (true == RandomizationFlags.AsciiCharacterToFlagValueLookup.TryGetValue(c, out Byte flagValue))
                {
                    for (Int32 count = RandomizationFlags.BITS_PER_FLAG_CHARACTER - 1; count >= 0; --count)
                    {
                        bitArray[index + count] = Convert.ToBoolean(flagValue & 1);
                        flagValue >>= 1;
                    }
                }
                else
                {
                    throw new ArgumentException(nameof(in_FlagString), "The string contains invalid flag characters");
                }
            }
        }
        */


        //
        // Private Data Members
        //

        private readonly BitArray mBitArray;
        //private readonly Int32 mCurrentLength;
        //private Int32 mCurrentIndex;


        //
        // Constants
        //

        private const Int32 BITS_PER_FLAG_CHARACTER = 6;

        private static readonly Dictionary<Byte, Char> FlagValueToAsciiCharacterLookup = new Dictionary<Byte, Char>()
        {
            { 0x00, 'A' },
            { 0x01, 'B' },
            { 0x02, 'C' },
            { 0x03, 'D' },
            { 0x04, 'E' },
            { 0x05, 'F' },
            { 0x06, 'G' },
            { 0x07, 'H' },
            { 0x08, 'I' },
            { 0x09, 'J' },
            { 0x0A, 'K' },
            { 0x0B, 'L' },
            { 0x0C, 'M' },
            { 0x0D, 'N' },
            { 0x0E, 'O' },
            { 0x0F, 'P' },
            { 0x10, 'Q' },
            { 0x11, 'R' },
            { 0x12, 'S' },
            { 0x13, 'T' },
            { 0x14, 'U' },
            { 0x15, 'V' },
            { 0x16, 'W' },
            { 0x17, 'X' },
            { 0x18, 'Y' },
            { 0x19, 'Z' },
            { 0x1A, 'a' },
            { 0x1B, 'b' },
            { 0x1C, 'c' },
            { 0x1D, 'd' },
            { 0x1E, 'e' },
            { 0x1F, 'f' },
            { 0x20, 'g' },
            { 0x21, 'h' },
            { 0x22, 'i' },
            { 0x23, 'j' },
            { 0x24, 'k' },
            { 0x25, 'l' },
            { 0x26, 'm' },
            { 0x27, 'n' },
            { 0x28, 'o' },
            { 0x29, 'p' },
            { 0x2A, 'q' },
            { 0x2B, 'r' },
            { 0x2C, 's' },
            { 0x2D, 't' },
            { 0x2E, 'u' },
            { 0x2F, 'v' },
            { 0x30, 'w' },
            { 0x31, 'x' },
            { 0x32, 'y' },
            { 0x33, 'z' },
            { 0x34, '0' },
            { 0x35, '1' },
            { 0x36, '2' },
            { 0x37, '3' },
            { 0x38, '4' },
            { 0x39, '4' },
            { 0x3A, '6' },
            { 0x3B, '7' },
            { 0x3C, '8' },
            { 0x3D, '9' },
            { 0x3E, '+' },
            { 0x3F, '/' },
        };

        private static readonly Dictionary<Char, Byte> AsciiCharacterToFlagValueLookup = new Dictionary<Char, Byte>()
        {
            { 'A', 0x00 },
            { 'B', 0x01 },
            { 'C', 0x02 },
            { 'D', 0x03 },
            { 'E', 0x04 },
            { 'F', 0x05 },
            { 'G', 0x06 },
            { 'H', 0x07 },
            { 'I', 0x08 },
            { 'J', 0x09 },
            { 'K', 0x0A },
            { 'L', 0x0B },
            { 'M', 0x0C },
            { 'N', 0x0D },
            { 'O', 0x0E },
            { 'P', 0x0F },
            { 'Q', 0x10 },
            { 'R', 0x11 },
            { 'S', 0x12 },
            { 'T', 0x13 },
            { 'U', 0x14 },
            { 'V', 0x15 },
            { 'W', 0x16 },
            { 'X', 0x17 },
            { 'Y', 0x18 },
            { 'Z', 0x19 },
            { 'a', 0x1A },
            { 'b', 0x1B },
            { 'c', 0x1C },
            { 'd', 0x1D },
            { 'e', 0x1E },
            { 'f', 0x1F },
            { 'g', 0x20 },
            { 'h', 0x21 },
            { 'i', 0x22 },
            { 'j', 0x23 },
            { 'k', 0x24 },
            { 'l', 0x25 },
            { 'm', 0x26 },
            { 'n', 0x27 },
            { 'o', 0x28 },
            { 'p', 0x29 },
            { 'q', 0x2A },
            { 'r', 0x2B },
            { 's', 0x2C },
            { 't', 0x2D },
            { 'u', 0x2E },
            { 'v', 0x2F },
            { 'w', 0x30 },
            { 'x', 0x31 },
            { 'y', 0x32 },
            { 'z', 0x33 },
            { '0', 0x34 },
            { '1', 0x35 },
            { '2', 0x36 },
            { '3', 0x37 },
            { '4', 0x38 },
            { '5', 0x39 },
            { '6', 0x3A },
            { '7', 0x3B },
            { '8', 0x3C },
            { '9', 0x3D },
            { '+', 0x3E },
            { '/', 0x3F },
        };

    }
}
