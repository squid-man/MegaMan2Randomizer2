using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using MM2Randomizer.Random;

namespace MM2Randomizer.Settings
{
    internal class RandomizationFlags
    {
        //
        // Constructors
        //

        public RandomizationFlags(Int32 in_FlagCharacters, bool in_BgFill = false)
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

            if (in_BgFill)
            {
                ISeed seed = SeedFactory.Create(GeneratorType.MT19937, "");
                for (Int32 i = 0; i < length; i++)
                    this.mBitArray[i] = seed.NextBoolean();
            }
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

        public void PushValue(Int32 in_Value, Int32 in_NumValues)
        {
            Debug.Assert(in_Value >= 0);
            Debug.Assert(in_Value < in_NumValues);

            Int32 numBits = (Int32)Math.Ceiling(Math.Log2((double)in_NumValues));
            if (mCurrentIndex + numBits > this.mMaxLength)
            {
                throw new IndexOutOfRangeException();
            }

            for (Int32 i = 0; i < numBits; i++)
            {
                this.mBitArray[this.mCurrentIndex++] = (in_Value & 1) != 0;
                in_Value >>= 1;
            }
        }

        public void PushValue<T>(T in_Value) where T : Enum
        {
            PushEnum((object)in_Value);
        }

        public void PushEnum(Object in_Value)
        {
            Array enumValues = Enum.GetValues(in_Value.GetType());
            Int32 valIdx = 0;
            for (Int32 i = 0; i < enumValues.Length; i++)
            {
                if (in_Value.Equals(enumValues.GetValue(i)))
                {
                    valIdx = i;
                    break;
                }
            }

            PushValue(valIdx, enumValues.Length);
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


        public String ToFlagString(char? in_FillChar = null)
        {
            StringBuilder stringBuilder = new StringBuilder();

            Int32 flagCharacterIndex = 0;
            Int32 maxIndex = in_FillChar is not null ? this.mCurrentIndex : this.mMaxLength;
            while (flagCharacterIndex < maxIndex)
            {
                Byte flagFieldValue = 0;

                for (Int32 count = 0; count < RandomizationFlags.BITS_PER_FLAG_CHARACTER; count++)
                {
                    flagFieldValue <<= 1;
                    flagFieldValue |= (this.mBitArray[flagCharacterIndex++] ? (Byte)0b_0000_0001 : (Byte)0b_0000_0000);
                }

                stringBuilder.Append(RandomizationFlags.FlagValueToAsciiCharacterLookup[flagFieldValue]);
            }

            stringBuilder.Append(
                in_FillChar ?? RandomizationFlags.FlagValueToAsciiCharacterLookup[0],
                (this.mMaxLength - flagCharacterIndex) / BITS_PER_FLAG_CHARACTER);

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

        private const Int32 BITS_PER_FLAG_CHARACTER = 5;

        private static readonly string FlagValueToAsciiCharacterLookup = "ABCDEFGHIJKLMNOPQRSTUVWXYZ345679";
        private static readonly Dictionary<Char, Byte> AsciiCharacterToFlagValueLookup = FlagValueToAsciiCharacterLookup.ToDictionary(x => x, x => (Byte)FlagValueToAsciiCharacterLookup.IndexOf(x));
    }
}
