using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;
using MM2Randomizer.Utilities;

using MM2Randomizer.Extensions;

namespace MM2Randomizer.Randomizers
{
    public class RText : IRandomizer
    {
        //
        // Constructor
        //

        public RText()
        {
        }


        //
        // IRandomizer Methods
        //

        public void Randomize(Patch in_Patch, Random in_Random)
        {
            CompanyNameSet companyNameSet = Properties.Resources.CompanyNameConfig.Deserialize<CompanyNameSet>();
            IEnumerable<CompanyName> enabledCompanyNames = companyNameSet.Where(x => true == x.Enabled);
            CompanyName companyName = enabledCompanyNames.ElementAt(in_Random.Next(enabledCompanyNames.Count()));

            // Write the intro text

            //       ©1988 CAPCOM CO.LTD
            // TM AND ©1989 CAPCOM U.S.A.,INC.
            //   MEGA MAN 2 RANDOMIZER 0.3.2
            //           LICENSED BY
            //    NINTENDO OF AMERICA. INC.

            RText.PatchCompanyName(in_Patch, companyName);
            RText.PatchIntroVersion(in_Patch);
            RText.PatchForUse(in_Patch, in_Random);
            RText.PatchIntroStroy(in_Patch, in_Random);

            // Generate Weapon Names
            IEnumerable<WeaponName> weaponNames = WeaponName.GenerateUniqueWeaponNames(in_Random, 8);

            // Write the new weapons names
            RText.PatchWeaponNames(in_Patch, in_Random, weaponNames);

            // Write the credits
            RText.PatchCredits(in_Patch, in_Random, companyName);
        }

        //
        // Private Static Methods
        //

        /// <summary>
        /// This method patches the company name in the intro screen.
        /// </summary>
        /// <remarks>
        /// Intro Screen Line 1: 0x036EA8 - 0x036EBA (20 chars)
        /// ©2017 <company name> (13 chars for company, 19 total)
        /// </remarks>
        public static void PatchCompanyName(Patch in_Patch, CompanyName in_CompanyName)
        {
            const Int32 MAX_LINE_LENGTH = 20;
            const Int32 INTRO_LINE1_ADDRESS = 0x036EA8;

            String line = $"©{DateTime.Now.Year} {in_CompanyName.Name}".PadCenter(MAX_LINE_LENGTH);

            in_Patch.Add(
                INTRO_LINE1_ADDRESS,
                line.AsIntroString(),
                $"Splash Text: {line}");
        }


        /// <summary>
        /// This method patches the second line in the intro text.
        /// </summary>
        /// <remarks>
        /// Intro Screen Line 2: 0x036EBE - 0x036EDC (31 chars)
        /// </remarks>
        public static void PatchIntroVersion(Patch in_Patch)
        {
            const String APP_NAME = "Mega Man 2 Randomizer";
            const Int32 INTRO_LINE2_OFFSET = 0x036EBE;
            const Int32 INTRO_LINE2_MAXLENGTH = 31;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Version appVersion = assembly.GetName().Version;
            String version = appVersion.ToString(2);

            String line = APP_NAME + " " + version;
            line = line.PadCenter(INTRO_LINE2_MAXLENGTH);

            in_Patch.Add(INTRO_LINE2_OFFSET, line.AsIntroString(), $"Splash Text: {line}");
        }


        /// <summary>
        /// This method patches the third and fourth lines in the intro text.
        /// </summary>
        /// <remarks>
        /// Intro Screen Line 3: 0x036EE0 - 0x036EEA (11 chars)
        /// Intro Screen Line 4: 0x036EEE - 0x036F06 (25 chars)
        /// </remarks>
        public static void PatchForUse(Patch in_Patch, Random in_Random)
        {
            const String INTRO_LINE3_PREFIX = "FOR USE ";
            const Int32 INTRO_LINE3_ADDRESS = 0x036EE0;
            const Int32 INTRO_LINE4_ADDRESS = 0x036EEE;

            CountryNameSet countryNameSet = Properties.Resources.CountryNameConfig.Deserialize<CountryNameSet>();
            IEnumerable<CountryName> countryNames = countryNameSet.Where(x => true == x.Enabled);
            CountryName countryName = countryNames.ElementAt(in_Random.Next(countryNames.Count()));

            Int32 line3NextCharacterAddress = in_Patch.Add(
                INTRO_LINE3_ADDRESS,
                INTRO_LINE3_PREFIX.AsIntroString(),
                $"Splash Text: {INTRO_LINE3_PREFIX}");

            in_Patch.Add(
                line3NextCharacterAddress,
                countryName.GetFormattedPrefix(),
                $"Splash Text: {countryName.Prefix}");

            in_Patch.Add(
                INTRO_LINE4_ADDRESS,
                countryName.GetFormattedName(),
                $"Splash Text: {countryName.Name}");
        }


        /// <summary>
        /// This method patches the intro story text.
        /// </summary>
        /// <remarks>
        /// Intro Story: 0x036D56 - 0x036E64 (270 chars)
        /// 27 characters per line
        /// 10 lines
        /// </remarks>
        public static void PatchIntroStroy(Patch in_Patch, Random in_Random)
        {
            const Int32 INTRO_STORY_PAGE1_ADDRESS = 0x036D56;

            IntroStorySet introStorySet = Properties.Resources.IntroStoryConfig.Deserialize<IntroStorySet>();
            IEnumerable<IntroStory> introStories = introStorySet.Where(x => true == x.Enabled);
            IntroStory introStory = introStories.ElementAt(in_Random.Next(introStories.Count()));

            in_Patch.Add(
                INTRO_STORY_PAGE1_ADDRESS,
                introStory.GetFormattedString(),
                $"Intro Text: {introStory.Title}");
        }


        public static void PatchWeaponNames(Patch in_Patch, Random in_Random, IEnumerable<WeaponName> in_WeaponNames)
        {
            const Int32 WEAPON_GET_LETTERS_ADDRESS = 0x037E22;
            const Int32 WEAPON_GET_NAME_ADDRESS = 0x037E2E;

            if (8 != in_WeaponNames.Count())
            {
                throw new ArgumentException("Incorrect weapon name count", nameof(in_WeaponNames));
            }

            List<WeaponName> weaponNames = in_WeaponNames.ToList();

            //String[] newWeaponNames = new String[8];
            //Char[] newWeaponLetters = new Char[9]; // Original order: P H A W B Q F M C

            // Write in new weapon names
            for (Int32 weaponIndex = 0; weaponIndex < weaponNames.Count; ++weaponIndex)
            {
                // Magic numbers?
                Int32 offsetAddress = WEAPON_GET_NAME_ADDRESS + (weaponIndex * 0x10);
                String weaponName = weaponNames[weaponIndex].Name.PadRight(12, '@');

                //newWeaponNames[i] = name;
                Int32 characterIndex = 0;
                foreach (Char c in weaponName)
                {
                    Byte b = Convert.ToByte(c);

                    in_Patch.Add(
                        offsetAddress + characterIndex,
                        b,
                        String.Format("Weapon Name {0} Char #{1}: {2}", ((EDmgVsBoss.Offset)weaponIndex).Name, characterIndex, c.ToString()));

                    characterIndex++;
                }
            }

            // Erase "Boomerang" for now
            // TODO: why?
            for (Int32 i = 0; i < 10; i++)
            {
                in_Patch.Add(0x037f5e + i, Convert.ToByte('@'), $"Quick Boomerang Name Erase Char #{i}: @");
            }

            // Pick a new weapon letter for buster
            Char? busterWeaponLetter = WeaponName.GetUnusedWeaponLetter(in_Random, weaponNames);

            List<Char> weaponLetters = weaponNames.Select(x => x.Letter).ToList();
            weaponLetters.Insert(0, busterWeaponLetter ?? 'P');

            // Write in new weapon letters
            for (Int32 weaponLetterIndex = 0; weaponLetterIndex < weaponLetters.Count; ++weaponLetterIndex)
            {
                // Write to Weapon Get screen (note: Buster value is unused here)
                Char weaponLetter = weaponLetters[weaponLetterIndex];

                in_Patch.Add(
                    WEAPON_GET_LETTERS_ADDRESS + weaponLetterIndex,
                    weaponLetter.AsAsciiByte(),
                    $"Weapon Get {((EDmgVsBoss.Offset)weaponLetterIndex).Name} Letter: {weaponLetter}");

                // Write to pause menu
                Byte[] pauseLetterBytes = weaponLetter.AsPauseScreenString();
                Int32 weaponLetterAddress = PauseScreenWpnAddressByBossIndex[weaponLetterIndex];

                in_Patch.Add(
                    weaponLetterAddress,
                    pauseLetterBytes,
                    $"Pause menu weapon letter GFX for \'{weaponLetter}\'");
            }
        }


        public static void PatchCredits(Patch in_Patch, Random in_Random, CompanyName in_CompanyName)
        {
            // Credits: Text content and line lengths (Starting with "Special Thanks")
            CreditTextSet creditTextSet = Properties.Resources.CreditTextConfig.Deserialize<CreditTextSet>();

            StringBuilder creditsSb = new StringBuilder();

            Int32 k = 0;
            foreach (CreditText creditText in creditTextSet)
            {
                if (true == creditText.Enabled)
                {
                    in_Patch.Add(0x024C78 + k, (Byte)creditText.Text.Length, $"Credits Line {k} Length");
                    Byte value = Convert.ToByte(creditText.Value, 16);
                    in_Patch.Add(0x024C3C + k, value, $"Credits Line {k} X-Pos");

                    k++;

                    // Content of line of text
                    creditsSb.Append(creditText.Text);
                }
            }

            Int32 startChar = 0x024D36; // First byte of credits text

            for (Int32 i = 0; i < creditsSb.Length; i++)
            {
                in_Patch.Add(startChar, creditsSb[i].AsCreditsCharacter(), $"Credits char #{i}");
                startChar++;
            }

            // Last line "Capcom Co.Ltd."
            String companyName = in_CompanyName.GetCompanyName();

            for (Int32 i = 0; i < companyName.Length; i++)
            {
                in_Patch.Add(startChar, companyName[i].AsCreditsCharacter(), $"Credits company char #{i}");
                startChar++;
            }

            in_Patch.Add(0x024CA4, (Byte)companyName.Length, "Credits Company Line Length");

            Int32[] txtRobos = new Int32[8]
            {
                0x024D6B, // Heat
                0x024D83, // Air
                0x024D9C, // Wood
                0x024DB7, // Bubble
                0x024DD1, // Quick
                0x024DEB, // Flash
                0x024E05, // Metal
                0x024E1F, // Clash
            };

            int[] txtWilys = new int[6]
            {
                0x024E54, // Dragon
                0x024E6C, // Picopico
                0x024E80, // Guts
                0x024E97, // Boobeam
                0x024EAE, // Machine
                0x024EC3, // Alien
            };

            // Write Robot Master damage table
            StringBuilder sb;
            for (Int32 i = 0; i < txtRobos.Length; i++)
            {
                sb = new StringBuilder();

                // Since weaknesses are for the "Room", and the room bosses were shuffled,
                // obtain the weakness for the boss at this room
                // TODO: Optimize this mess; when the bossroom is shuffled it should save
                // a mapping that could be reused here.
                Int32 newIndex = 0;
                for (Int32 m = 0; m < RandomMM2.randomBossInBossRoom.Components.Count; m++)
                {
                    RBossRoom.BossRoomRandomComponent room = RandomMM2.randomBossInBossRoom.Components[m];

                    if (room.OriginalBossIndex == i)
                    {
                        newIndex = m;
                        break;
                    }
                }

                for (Int32 j = 0; j < 9; j++)
                {
                    Int32 dmg = RWeaknesses.BotWeaknesses[newIndex, j];
                    sb.Append($"{GetBossWeaknessDamageChar(dmg)} ");
                }

                String rowString = sb.ToString().Trim();

                for (Int32 j = 0; j < rowString.Length; j++)
                {
                    in_Patch.Add(txtRobos[i] + j,
                        rowString[j].AsCreditsCharacter(),
                        $"Credits robo weakness table char #{j + i * rowString.Length}");
                }
            }

            // Write Wily Boss damage table
            for (Int32 i = 0; i < txtWilys.Length; i++)
            {
                sb = new StringBuilder();

                for (Int32 j = 0; j < 8; j++)
                {
                    Int32 dmg = RWeaknesses.WilyWeaknesses[i, j];
                    sb.Append($"{GetBossWeaknessDamageChar(dmg)} ");
                }

                sb.Remove(sb.Length - 1, 1);
                String rowString = sb.ToString();

                for (Int32 j = 0; j < rowString.Length; j++)
                {
                    in_Patch.Add(txtWilys[i] + j,
                        rowString[j].AsCreditsCharacter(),
                        $"Credits wily weakness table char #{j + i * rowString.Length}");
                }
            }
        }



        public void FixWeaponLetters(Patch p, Int32[] permutation)
        {
            // Re-order the letters array to match the ordering of the shuffled weapons
            Char[] newLettersPermutation = new Char[9];
            newLettersPermutation[0] = newWeaponLetters[0];

            for (Int32 i = 0; i < 8; i++)
            {
                newLettersPermutation[i + 1] = newWeaponLetters[permutation[i] + 1];
            }

            // Write new weapon letters to weapon get screen
            for (Int32 i = 1; i < 9; i++)
            {
                // Write to Weapon Get screen (note: Buster value is unused here)
                Int32 newLetter = 0x41 + Alphabet.IndexOf(newLettersPermutation[i]); // unicode
                p.Add(offsetWpnGetLetters + i - 1, (Byte)newLetter, $"Weapon Get {((EDmgVsBoss.Offset)i).Name} Letter: {newWeaponLetters[i]}");
            }

            //// Write new weapon letters to pause menu
            //for (int i = 0; i < 9; i++)
            //{
                //int[] pauseLetterBytes = PauseScreenCipher[newWeaponLetters[i + 1]];
                //int wpnLetterAddress = PauseScreenWpnAddressByBossIndex[permutedIndex + 1];
                //for (int j = 0; j < pauseLetterBytes.Length; j++)
                //{
                //    p.Add(wpnLetterAddress + j, (byte)pauseLetterBytes[j], $"Pause menu weapon letter GFX for \'{newWeaponLetters[permutedIndex]}\', byte #{j}");
                //}
            //}
        }

        private static Char GetBossWeaknessDamageChar(Int32 dmg)
        {
            char c;

            if (dmg == 0 || dmg == 255)
            {
                c = ' ';
            }
            else if (dmg < 10)
            {
                c = dmg.ToString()[0];
            }
            else if (dmg >= 10 && dmg < 20)
            {
                c = 'A';
            }
            else
            {
                c = 'B';
            }

            return c;
        }

        public static String Alphabet { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// These arrays represent the raw graphical data for each letter, A-Z, to be rendered on the pause menu. To 
        /// replace a weapon letter's graphic on the pause screen, use <see cref="PauseScreenWpnAddressByBossIndex"/>
        /// to get the address of the desired weapon letter to replace, and then write in the 16 bytes for the desired
        /// new letter from this dictionary.
        /// </summary>
        public static Dictionary<Char, Int32[]> PauseScreenCipher = new Dictionary<Char, Int32[]>()
        {
            // Kept as int, to prevent a thousand (byte) casts clogging this up. Cast to byte in a loop later.
            { 'A', new Int32[] { 0x02, 0x39, 0x21, 0x21, 0x01, 0x39, 0x21, 0xE7, 0x7C, 0xC6, 0xC6, 0xC6, 0xFE, 0xC6, 0xC6, 0x00 } },
            { 'B', new Int32[] { 0x02, 0x39, 0x21, 0x02, 0x39, 0x21, 0x02, 0xFC, 0xFC, 0xC6, 0xC6, 0xFC, 0xC6, 0xC6, 0xFC, 0x00 } },
            { 'C', new Int32[] { 0x00, 0x38, 0x26, 0x20, 0x20, 0x20, 0x82, 0x7C, 0x7C, 0xC6, 0xC0, 0xC0, 0xC0, 0xC6, 0x7C, 0x00 } },
            { 'D', new Int32[] { 0x02, 0x39, 0x21, 0x21, 0x21, 0x21, 0x02, 0xFC, 0xFC, 0xC6, 0xC6, 0xC6, 0xC6, 0xC6, 0xFC, 0x00 } },
            { 'E', new Int32[] { 0x00, 0x3E, 0x20, 0x00, 0x3C, 0x20, 0x00, 0xFE, 0xFE, 0xC0, 0xC0, 0xFC, 0xC0, 0xC0, 0xFE, 0x00 } },
            { 'F', new Int32[] { 0x00, 0x3E, 0x20, 0x00, 0x3C, 0x20, 0x20, 0xE0, 0xFE, 0xC0, 0xC0, 0xFC, 0xC0, 0xC0, 0xC0, 0x00 } },
            { 'G', new Int32[] { 0x02, 0x39, 0x27, 0x21, 0x39, 0x21, 0x83, 0x7E, 0x7C, 0xC6, 0xC0, 0xDE, 0xC6, 0xC6, 0x7C, 0x00 } },
            { 'H', new Int32[] { 0x21, 0x21, 0x21, 0x01, 0x39, 0x21, 0x21, 0xE7, 0xC6, 0xC6, 0xC6, 0xFE, 0xC6, 0xC6, 0xC6, 0x00 } },
            { 'I', new Int32[] { 0x02, 0xCE, 0x08, 0x08, 0x08, 0x08, 0x02, 0xFE, 0xFC, 0x30, 0x30, 0x30, 0x30, 0x30, 0xFC, 0x00 } },
            { 'J', new Int32[] { 0x02, 0x02, 0x02, 0x02, 0x12, 0x12, 0x46, 0x3C, 0x0C, 0x0C, 0x0C, 0x0C, 0x6C, 0x6C, 0x38, 0x00 } },
            { 'K', new Int32[] { 0x22, 0x26, 0x0C, 0x18, 0x08, 0x24, 0x32, 0xEE, 0xCC, 0xD8, 0xF0, 0xE0, 0xF0, 0xD8, 0xCC, 0x00 } },
            { 'L', new Int32[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x02, 0xFE, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xC0, 0xFC, 0x00 } },
            { 'M', new Int32[] { 0x01, 0x01, 0x01, 0x29, 0x31, 0x21, 0x21, 0xE7, 0xC6, 0xEE, 0xFE, 0xD6, 0xC6, 0xC6, 0xC6, 0x00 } },
            { 'N', new Int32[] { 0x21, 0x01, 0x01, 0x21, 0x31, 0x29, 0x21, 0xE7, 0xC6, 0xE6, 0xF6, 0xDE, 0xCE, 0xC6, 0xC6, 0x00 } },
            { 'O', new Int32[] { 0x02, 0x39, 0x21, 0x21, 0x21, 0x21, 0x82, 0x7C, 0x7C, 0xC6, 0xC6, 0xC6, 0xC6, 0xC6, 0x7C, 0x00 } },
            { 'P', new Int32[] { 0x02, 0x39, 0x21, 0x21, 0x02, 0x3C, 0x20, 0xE0, 0xFC, 0xC6, 0xC6, 0xC6, 0xFC, 0xC0, 0xC0, 0x00 } },
            { 'Q', new Int32[] { 0x02, 0x39, 0x21, 0x21, 0x21, 0x31, 0x82, 0x7C, 0x7C, 0xC6, 0xC6, 0xC6, 0xDE, 0xCE, 0x7C, 0x00 } },
            { 'R', new Int32[] { 0x02, 0x39, 0x21, 0x21, 0x02, 0x32, 0x09, 0xC7, 0xFC, 0xC6, 0xC6, 0xC6, 0xFC, 0xCC, 0xC6, 0x00 } },
            { 'S', new Int32[] { 0x02, 0x39, 0x27, 0x82, 0x79, 0x21, 0x83, 0x7E, 0x7C, 0xC6, 0xC0, 0x7C, 0x06, 0xC6, 0x7C, 0x00 } },
            { 'T', new Int32[] { 0x02, 0xCE, 0x08, 0x08, 0x08, 0x08, 0x08, 0x38, 0xFC, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x00 } },
            { 'U', new Int32[] { 0x21, 0x21, 0x21, 0x21, 0x21, 0x21, 0x82, 0x7C, 0xC6, 0xC6, 0xC6, 0xC6, 0xC6, 0xC6, 0x7C, 0x00 } },
            { 'V', new Int32[] { 0x21, 0x21, 0x21, 0x21, 0x93, 0x46, 0x2C, 0x18, 0xC6, 0xC6, 0xC6, 0xC6, 0x6C, 0x38, 0x10, 0x00 } },
            { 'W', new Int32[] { 0x21, 0x21, 0x21, 0x21, 0x01, 0x11, 0x21, 0xC7, 0xC6, 0xC6, 0xC6, 0xD6, 0xFE, 0xEE, 0xC6, 0x00 } },
            { 'X', new Int32[] { 0x22, 0x22, 0x84, 0x48, 0x00, 0x32, 0x22, 0xCE, 0xCC, 0xCC, 0x78, 0x30, 0x78, 0xCC, 0xCC, 0x00 } },
            { 'Y', new Int32[] { 0x22, 0x22, 0x22, 0x84, 0x48, 0x08, 0x08, 0x38, 0xCC, 0xCC, 0xCC, 0x78, 0x30, 0x30, 0x30, 0x00 } },
            { 'Z', new Int32[] { 0x02, 0x72, 0x84, 0x08, 0x10, 0x22, 0x02, 0xFE, 0xFC, 0x8C, 0x18, 0x30, 0x60, 0xC4, 0xFC, 0x00 } },
        };

        /// <summary>
        /// These ROM addresses point to the graphical data of the sprites in the pause menu, namely the weapon 
        /// letters. Use the data at <see cref="PauseScreenCipher"/> to write new values at these locations to
        /// change the weapon letter graphics.
        /// </summary>
        public static Int32[] PauseScreenWpnAddressByBossIndex = new Int32[]
        {
            0x001B00, // "P"
            0x001A00, // "H"
            0x0019C0, // "A"
            0x0019A0, // "W"
            0x0019E0, // "B"
            0x0019D0, // "Q"
            0x0019B0, // "F"
            0x0019F0, // "M"
            0x001A10, // "C"
        };

        // STAFF == D3 D4 C1 C6 C6

        /*
        private String GetRandomName(Random r)
        {
            // Start with random list
            Int32 l = r.Next(1);
            String name0, name1;

            if (l == 0)
            {
                // Get random name from first list
                Int32 random = r.Next(Names0.Length);
                name0 = Names0[random];

                // From second list, get subset of names with valid character count
                Int32 charsLeft = MAX_CHARS - name0.Length - 1; // 1 space
                List<String> names1Left = new List<String>();

                for (int j = 0; j < Names1.Length; j++)
                {
                    if (Names1[j].Length <= charsLeft)
                    {
                        names1Left.Add(Names1[j]);
                    }
                }

                // Get random name from modified second list
                if (names1Left.Count > 0)
                {
                    random = r.Next(names1Left.Count);
                    name1 = names1Left[random];
                }
                else
                {
                    name1 = "";
                }
            }
            else
            {
                // Get random name from second list
                Int32 random = r.Next(Names1.Length);
                name1 = Names1[random];

                // From first list, get subset of names with valid character count
                Int32 charsLeft = MAX_CHARS - name1.Length - 1; // 1 space
                List<String> names0Left = new List<String>();
                for (Int32 j = 0; j < Names0.Length; j++)
                {
                    if (Names0[j].Length <= charsLeft)
                    {
                        names0Left.Add(Names0[j]);
                    }
                }

                // Get random name from modified first list
                if (names0Left.Count > 0)
                {
                    random = r.Next(names0Left.Count);
                    name0 = names0Left[random];
                }
                else
                {
                    name0 = "";
                }
            }

            // Handle cases for only one name
            String finalName;

            if (name0.Length == 0)
            {
                finalName = name1;
            }
            else if (name1.Length == 0)
            {
                finalName = name0;
            }
            // Concatenate final name
            else
            {
                finalName = name0 + "@" + name1;
            }

            return finalName;
        }
        */

        /*
        private static String[] Names0 = new String[]
        {
            "TIME",
            "MEGA",
            "SUPER",
            "METAL",
            "ATOMIC",
            "AIR",
            "WILY",
            "ROLL",
            "RUSH",
            "FRANKERZ",
            "WATER",
            "GUTS",
            "ELEC",
            "GEMINI",
            "COSSACK",
            "TOAD",
            "HYPER",
            "TIME",
            "CRASH",
            "LEAF",
            "QUICK",
            "DRILL",
            "FLAME",
            "PLANT",
            "KNIGHT",
            "SILVER",
            "JUNK",
            "THUNDER",
            "WILD",
            "NOISE",
            "SLASH",
            "TORNADO",
            "ASTRO",
            "CLOWN",
            "SOLAR",
            "CHILL",
            "TRIPLE",
            "REBOUND",
            "NUDUA",
            "JOKA",
            "ELLO",
            "COOLKID",
            "CYGHFER",
            "ZODA",
            "SHOKA",
            "PROTO",
            "PLUG",
            "RTA",
            "MASH",
            "TURBO",
            "TAS",
            "BIG",
            "CUT",
            "URN",
            "TWITCH",
            "PRO",
            "ION",
            "AUTO",
            "BEAT",
            "LAG",
        };

        private static String[] Names1 = new String[]
        {
            "BLAST",
            "BLASTER",
            "FIRE",
            "CUTTER",
            "BLADE",
            "STOPPER",
            "GUN",
            "CANNON",
            "HIT",
            "SHOT",
            "COIL",
            "SHOOTER",
            "BOMB",
            "BOMBER",
            "LASER",
            "FLUSH",
            "BEAM",
            "DASH",
            "MISSILE",
            "STORM",
            "CUTTER",
            "SHIELD",
            "KNUCKLE",
            "SNAKE",
            "SHOCK",
            "SPIN",
            "CRUSHER",
            "HOLD",
            "EYE",
            "ATTACK",
            "KICK",
            "STONE",
            "WAVE",
            "SPEAR",
            "CLAW",
            "BALL",
            "TRIDENT",
            "WOOL",
            "SPIKE",
            "BLAZE",
            "STRIKER",
            "WALL",
            "BALLOON",
            "MARINE",
            "WIRE",
            "BURNER",
            "BUSTER",
            "ZIP",
            "GLITCH",
            "ADAPTER",
            "RAID",
            "DEVICE",
            "BOX",
            "AXE",
            "ARC",
            "JAB",
            "RESET",
            "STRAT",
        };
        */
    }
}
