using System;
using System.Collections.Generic;

namespace MM2Randomizer.Extensions
{
    public static class StringExtensions
    {
        //
        // Public Methods
        //

        public static Byte[] AsIntroString(this String in_String)
        {
            if (null == in_String)
            {
                return null;
            }

            Byte[] convertedString = new Byte[in_String.Length];
            Int32 index = 0;

            foreach (Char c in in_String)
            {
                if (true == StringExtensions.IntroCharacterLookup.TryGetValue(c, out Byte convertedCharacter))
                {
                    convertedString[index] = convertedCharacter;
                }
                else
                {
                    convertedString[index] = StringExtensions.IntroCharacterLookup['?'];
                }

                index++;
            }

            return convertedString;
        }


        public static Byte AsIntroCharacter(this Char in_Char)
        {
            if (true == StringExtensions.IntroCharacterLookup.TryGetValue(in_Char, out Byte convertedCharacter))
            {
                return convertedCharacter;
            }
            else
            {
                return StringExtensions.IntroCharacterLookup['?'];
            }
        }


        public static Byte[] AsCreditsString(this String in_String)
        {
            if (null == in_String)
            {
                return null;
            }

            Byte[] convertedString = new Byte[in_String.Length];
            Int32 index = 0;

            foreach (Char c in in_String)
            {
                if (true == StringExtensions.CreditsCharacterLookup.TryGetValue(c, out Byte convertedCharacter))
                {
                    convertedString[index] = convertedCharacter;
                }
                else
                {
                    convertedString[index] = StringExtensions.CreditsCharacterLookup['?'];
                }

                index++;
            }

            return convertedString;
        }


        public static Byte AsCreditsCharacter(this Char in_Char)
        {
            if (true == StringExtensions.CreditsCharacterLookup.TryGetValue(in_Char, out Byte convertedCharacter))
            {
                return convertedCharacter;
            }
            else
            {
                return StringExtensions.CreditsCharacterLookup['?'];
            }
        }


        public static String PadCenter(this String in_String, Int32 in_TotalWidth)
        {
            Int32 totalPadding = in_TotalWidth - in_String.Length;

            if (totalPadding > 1)
            {
                Int32 leftPadding = in_String.Length + (totalPadding / 2);

                return in_String.PadLeft(leftPadding).PadRight(in_TotalWidth);
            }
            else
            {
                return in_String.PadRight(in_TotalWidth);
            }
        }


        //
        // Private Constants
        //

        private static readonly Dictionary<Char, Byte> IntroCharacterLookup = new Dictionary<Char, Byte>()
        {
            { ' ',  0x00 },
            { '0',  0xA0 },
            { '8',  0xA1 },
            { '2',  0xA2 },
            { '©',  0xA3 },  // Copyright symbol
            { '™',  0xA4 },  // Trademark symbol
            { '9',  0xA5 },
            { '7',  0xA6 },
            { '1',  0xA7 },
            { '3',  0xA8 },
            { '4',  0xA9 },
            { '5',  0xAA },
            { '6',  0xAB },
            { '|',  0xC0 },  // Blank space character
            { 'a',  0xC1 },
            { 'A',  0xC1 },
            { 'b',  0xC2 },
            { 'B',  0xC2 },
            { 'c',  0xC3 },
            { 'C',  0xC3 },
            { 'd',  0xC4 },
            { 'D',  0xC4 },
            { 'e',  0xC5 },
            { 'E',  0xC5 },
            { 'f',  0xC6 },
            { 'F',  0xC6 },
            { 'g',  0xC7 },
            { 'G',  0xC7 },
            { 'h',  0xC8 },
            { 'H',  0xC8 },
            { 'i',  0xC9 },
            { 'I',  0xC9 },
            { 'j',  0xCA },
            { 'J',  0xCA },
            { 'k',  0xCB },
            { 'K',  0xCB },
            { 'l',  0xCC },
            { 'L',  0xCC },
            { 'm',  0xCD },
            { 'M',  0xCD },
            { 'n',  0xCE },
            { 'N',  0xCE },
            { 'o',  0xCF },
            { 'O',  0xCF },
            { 'p',  0xD0 },
            { 'P',  0xD0 },
            { 'q',  0xD1 },
            { 'Q',  0xD1 },
            { 'r',  0xD2 },
            { 'R',  0xD2 },
            { 's',  0xD3 },
            { 'S',  0xD3 },
            { 't',  0xD4 },
            { 'T',  0xD4 },
            { 'u',  0xD5 },
            { 'U',  0xD5 },
            { 'v',  0xD6 },
            { 'V',  0xD6 },
            { 'w',  0xD7 },
            { 'W',  0xD7 },
            { 'x',  0xD8 },
            { 'X',  0xD8 },
            { 'y',  0xD9 },
            { 'Y',  0xD9 },
            { 'z',  0xDA },
            { 'Z',  0xDA },
            { '?',  0xDB },
            { '.',  0xDC },
            { ',',  0xDD },
            { '\'', 0xDE },
            { '!',  0xDF },
        };


        public static Dictionary<char, byte> CreditsCharacterLookup = new Dictionary<char, byte>()
        {
            { ' ',  0x00},
            { 'a',  0x01},
            { 'A',  0x01},
            { 'b',  0x02},
            { 'B',  0x02},
            { 'c',  0x03},
            { 'C',  0x03},
            { 'd',  0x04},
            { 'D',  0x04},
            { 'e',  0x05},
            { 'E',  0x05},
            { 'f',  0x06},
            { 'F',  0x06},
            { 'g',  0x07},
            { 'G',  0x07},
            { 'h',  0x08},
            { 'H',  0x08},
            { 'i',  0x09},
            { 'I',  0x09},
            { 'j',  0x0A},
            { 'J',  0x0A},
            { 'k',  0x0B},
            { 'K',  0x0B},
            { 'l',  0x0C},
            { 'L',  0x0C},
            { 'm',  0x0D},
            { 'M',  0x0D},
            { 'n',  0x0E},
            { 'N',  0x0E},
            { 'o',  0x0F},
            { 'O',  0x0F},
            { 'p',  0x10},
            { 'P',  0x10},
            { 'q',  0x11},
            { 'Q',  0x11},
            { 'r',  0x12},
            { 'R',  0x12},
            { 's',  0x13},
            { 'S',  0x13},
            { 't',  0x14},
            { 'T',  0x14},
            { 'u',  0x15},
            { 'U',  0x15},
            { 'v',  0x16},
            { 'V',  0x16},
            { 'w',  0x17},
            { 'W',  0x17},
            { 'x',  0x18},
            { 'X',  0x18},
            { 'y',  0x19},
            { 'Y',  0x19},
            { 'z',  0x1A},
            { 'Z',  0x1A},
            { '.',  0x1C},
            { ',',  0x1D},
            { '\'', 0x1E},
            { '!',  0x1F},
            { '`',  0x20}, // Blank line
            { '0',  0x30},
            { '1',  0x31},
            { '2',  0x32},
            { '3',  0x33},
            { '4',  0x34},
            { '5',  0x35},
            { '6',  0x36},
            { '7',  0x37},
            { '8',  0x38},
            { '9',  0x39},
            { '=',  0x23},
        };
    }
}
