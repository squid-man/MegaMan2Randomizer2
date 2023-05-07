using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MM2Randomizer.Settings
{
    internal class RandomizationFlags
    {
        //
        // Constructors
        //

        public RandomizationFlags(Int32 in_FlagCharacters)
        {
            if (in_FlagCharacters <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(in_FlagCharacters), "Cannot be less than or equal to zero");
            }

            Int32 length = RandomizationFlags.BITS_PER_FLAG_CHARACTER * in_FlagCharacters;

            this.mMaxLength = length;
            this.mMaxFlagCharacters = in_FlagCharacters;
            this.mBitArray = new BitArray(length);
            this.mCurrentIndex = 0;
        }


        //
        // Public Methods
        //

        public void PushValue(Boolean in_Value)
        {
            if (this.mCurrentIndex >= this.mMaxLength)
            {
                throw new IndexOutOfRangeException();
            }

            this.mBitArray[this.mCurrentIndex++] = in_Value;
        }

        public void PushValue<T>(T in_Value) where T : Enum
        {
            Array enumValues = Enum.GetValues(typeof(T));

            Int32 newIndex = this.mCurrentIndex + enumValues.Length;

            if (newIndex > this.mMaxLength)
            {
                throw new IndexOutOfRangeException();
            }

            foreach (T enumValue in enumValues)
            {
                this.mBitArray[this.mCurrentIndex++] = in_Value.Equals(enumValue);
            }
        }

        public Boolean PopValue()
        {
            if (this.mCurrentIndex <= 0)
            {
                throw new IndexOutOfRangeException();
            }

            return this.mBitArray[this.mCurrentIndex--];
        }

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


        public String ToFlagString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            Int32 flagCharacterIndex = 0;

            while (flagCharacterIndex < this.mMaxLength)
            {
                Byte flagFieldValue = 0;

                for (Int32 count = 0; count < RandomizationFlags.BITS_PER_FLAG_CHARACTER; count++)
                {
                    flagFieldValue <<= 1;
                    flagFieldValue |= (this.mBitArray[flagCharacterIndex++] ? (Byte)0b_0000_0001 : (Byte)0b_0000_0000);
                }

                stringBuilder.Append(RandomizationFlags.FlagValueToAsciiCharacterLookup[flagFieldValue]);
            }

            return stringBuilder.ToString();
        }

        public void FromFlagString(String in_FlagString)
        {
            if (in_FlagString.Length > this.mMaxFlagCharacters)
            {
                throw new ArgumentException(nameof(in_FlagString), $"The flag string cannot be longer than the maximum length of {this.mMaxFlagCharacters}");
            }

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


        //
        // Private Data Members
        //

        private readonly BitArray mBitArray;
        private readonly Int32 mMaxLength;
        private readonly Int32 mMaxFlagCharacters;
        private Int32 mCurrentIndex;


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
            { 0x1A, '1' },
            { 0x1B, '2' },
            { 0x1C, '3' },
            { 0x1D, '4' },
            { 0x1E, '5' },
            { 0x1F, '6' },
            { 0x20, '7' },
            { 0x21, '8' },
            { 0x22, '9' },
            { 0x23, '0' },
            { 0x24, 'a' },
            { 0x25, 'b' },
            { 0x26, 'c' },
            { 0x27, 'd' },
            { 0x28, 'e' },
            { 0x29, 'f' },
            { 0x2A, 'g' },
            { 0x2B, 'h' },
            { 0x2C, 'i' },
            { 0x2D, 'j' },
            { 0x2E, 'k' },
            { 0x2F, 'l' },
            { 0x30, 'm' },
            { 0x31, 'n' },
            { 0x32, 'o' },
            { 0x33, 'p' },
            { 0x34, 'q' },
            { 0x35, 'r' },
            { 0x36, 's' },
            { 0x37, 't' },
            { 0x38, 'u' },
            { 0x39, 'v' },
            { 0x3A, 'w' },
            { 0x3B, 'x' },
            { 0x3C, 'y' },
            { 0x3D, 'z' },
            { 0x3E, '!' },
            { 0x3F, '?' },
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
            { '1', 0x1A },
            { '2', 0x1B },
            { '3', 0x1C },
            { '4', 0x1D },
            { '5', 0x1E },
            { '6', 0x1F },
            { '7', 0x20 },
            { '8', 0x21 },
            { '9', 0x22 },
            { '0', 0x23 },
            { 'a', 0x24 },
            { 'b', 0x25 },
            { 'c', 0x26 },
            { 'd', 0x27 },
            { 'e', 0x28 },
            { 'f', 0x29 },
            { 'g', 0x2A },
            { 'h', 0x2B },
            { 'i', 0x2C },
            { 'j', 0x2D },
            { 'k', 0x2E },
            { 'l', 0x2F },
            { 'm', 0x30 },
            { 'n', 0x31 },
            { 'o', 0x32 },
            { 'p', 0x33 },
            { 'q', 0x34 },
            { 'r', 0x35 },
            { 's', 0x36 },
            { 't', 0x37 },
            { 'u', 0x38 },
            { 'v', 0x39 },
            { 'w', 0x3A },
            { 'x', 0x3B },
            { 'y', 0x3C },
            { 'z', 0x3D },
            { '!', 0x3E },
            { '?', 0x3F },
        };

    }
}
