using System;
using System.Text;

namespace MM2Randomizer.Extensions
{
    public static class IntegerExtensions
    {
        public static String ToAlphaBase26(this Int32 in_Value)
        {
            const String DIGITS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const Int32 RADIX = 26;

            if (0 == in_Value)
            {
                return DIGITS[0].ToString();
            }

            StringBuilder sb = new StringBuilder();

            Int32 currentNumber = Math.Abs(in_Value);

            while (currentNumber > 0)
            {
                Int32 remainder = currentNumber % RADIX;

                sb.Insert(0, DIGITS[remainder]);
                currentNumber = currentNumber / RADIX;
            }

            return sb.ToString();
        }

        public static String ToAlphaBase26(this UInt64 in_Value)
        {
            const String DIGITS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const UInt64 RADIX = 26;

            if (0 == in_Value)
            {
                return DIGITS[0].ToString();
            }

            StringBuilder sb = new StringBuilder();

            UInt64 currentNumber = in_Value;

            while (currentNumber > 0)
            {
                UInt64 remainder = currentNumber % RADIX;

                sb.Insert(0, DIGITS[(Int32)remainder]);
                currentNumber = currentNumber / RADIX;
            }

            return sb.ToString();
        }
    }
}
