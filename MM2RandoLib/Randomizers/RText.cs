using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM2Randomizer.Data;
using MM2Randomizer.Enums;
using MM2Randomizer.Extensions;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;
using MM2Randomizer.Utilities;

namespace MM2Randomizer.Randomizers
{
    public class RText : Randomizer
    {
        //
        // Constructor
        //

        public RText()
            : base([nameof(RandomizationContext.BossStages)])
        {
        }


        //
        // IRandomizer Methods
        //

        public override void Randomize(Patch in_Patch, RandomizationContext in_Context)
        {
            CompanyNameSet companyNameSet = Properties.Resources.CompanyNameConfig.Deserialize<CompanyNameSet>();
            IEnumerable<CompanyName> enabledCompanyNames = companyNameSet.Where(x => true == x.Enabled);
            CompanyName companyName = in_Context.Seed.NextElement(enabledCompanyNames);

            // Write the intro text
            RText.PatchSplashScreen(in_Patch, companyName, in_Context);

            RText.PatchIntroStory(in_Patch, in_Context.Seed);

            // Write the new weapons names
            RText.PatchWeaponNames(in_Patch, in_Context.Seed, out Dictionary<EWeaponIndex, Char> newWeaponLetters);

            // This is a hack to get around the strange interdependency that
            // the randomizer interfaces have
            this.mNewWeaponLetters = newWeaponLetters;

            // Write the credits
            RText.PatchCredits(in_Patch, companyName, in_Context);
        }

        //
        // Private Static Methods
        //

        public static void PatchSplashScreenLine(
            Patch in_Patch, 
            int in_LineIndex, 
            string in_Text)
        {
            if (in_Text.Length > 32)
                in_Text = in_Text.Substring(0, 32);

            in_Patch.Add(
                SplashScreenDataOffset + in_LineIndex * 32,
                in_Text.PadCenter(32).AsIntroString(),
                $"Splash Text @ {in_LineIndex}: {in_Text}");
        }

        public static void PatchSplashScreen(
            Patch in_Patch,
            CompanyName in_CompanyName, 
            RandomizationContext in_Context)
        {
            //       ©1988 CAPCOM CO.LTD
            // TM AND ©1989 CAPCOM U.S.A.,INC.
            //   MEGA MAN 2 RANDOMIZER 0.3.2
            //           LICENSED BY
            //    NINTENDO OF AMERICA. INC.

            PatchSplashScreenLine(
                in_Patch,
                10,
                $"©{DateTime.Now.Year} {in_CompanyName.GetCompanyName()}");

            PatchSplashScreenLine(in_Patch, 12, "Mega Man 2 Randomizer");

            PatchSplashScreenLine(
                in_Patch,
                14,
                "V" + RandomMM2.AssemblyVersionString);

            CountryNameSet countryNameSet = Properties.Resources.CountryNameConfig.Deserialize<CountryNameSet>();
            IEnumerable<CountryName> countryNames = countryNameSet.Where(x => true == x.Enabled);
            CountryName countryName = in_Context.Seed.NextElement(countryNames);

            PatchSplashScreenLine(
                in_Patch, 16, "FOR USE " + countryName.GetFormattedPrefix());
            PatchSplashScreenLine(in_Patch, 18, countryName.GetFormattedName());
        }


        /// <summary>
        /// This method patches the intro story text.
        /// </summary>
        /// <remarks>
        /// Intro Story: 0x036D56 - 0x036E64 (270 chars)
        /// 27 characters per line
        /// 10 lines
        /// </remarks>
        public static void PatchIntroStory(Patch in_Patch, ISeed in_Seed)
        {
            const Int32 INTRO_STORY_PAGE1_ADDRESS = 0x036D56;

            IntroStorySet introStorySet = Properties.Resources.IntroStoryConfig.Deserialize<IntroStorySet>();
            IEnumerable<IntroStory> introStories = introStorySet.Where(x => true == x.Enabled);
            IntroStory introStory = in_Seed.NextElement(introStories);

            in_Patch.Add(
                INTRO_STORY_PAGE1_ADDRESS,
                introStory.GetFormattedString(),
                $"Intro Text: {introStory.Title}");
        }


        public static void PatchWeaponNames(Patch in_Patch, ISeed in_Seed, out Dictionary<EWeaponIndex, Char> out_NewWeaponLetters)
        {
            const Int32 WEAPON_GET_LETTERS_ADDRESS = 0x037E22;
            const Int32 WEAPON_GET_NAME_ADDRESS = 0x037E2C;
            const Int32 WEAPON_GET_EXTENDED_NAME_ADDRESS = 0x037F5C;
            EWeaponIndex WEAPON_GET_EXTENDED_NAME_INDEX = EWeaponIndex.Quick;     // Quick Boomerang has an extended name

            WeaponNameGenerator weaponNameGenerator = new(in_Seed);

            Dictionary<EWeaponIndex, WeaponName> weaponNames = new();
            List<EWeaponIndex> onlySpecialWeapons = EWeaponIndex.SpecialWeapons.ToList();

            // Write in new weapon names
            Int32 offset = 0; // incremented at the end of this loop
            foreach (EWeaponIndex weaponIndex in onlySpecialWeapons)
            {
                // Each weapon get name is 14 bytes long with a 2 Byte header
                Int32 offsetAddress = WEAPON_GET_NAME_ADDRESS + offset;

                if (WEAPON_GET_EXTENDED_NAME_INDEX == weaponIndex)
                {
                    WeaponName weaponName = weaponNameGenerator.GenerateWeaponName(true);
                    weaponNames.Add(weaponIndex, weaponName);

                    Int32 characterIndex = 0;
                    foreach (Char c in weaponName.Name)
                    {
                        in_Patch.Add(
                            offsetAddress + characterIndex,
                            c.AsPrintCharacter(),
                            String.Format("Weapon Name {0} Char #{1}: {2}", ((EDmgVsBoss.Offset)weaponIndex.ToBossIndex()).Name, characterIndex, c.ToString()));

                        characterIndex++;
                    }

                    if (null != weaponName.ExtendedName)
                    {
                        characterIndex = 0;
                        foreach (Char c in weaponName.ExtendedName)
                        {
                            in_Patch.Add(
                                WEAPON_GET_EXTENDED_NAME_ADDRESS + characterIndex,
                                c.AsPrintCharacter(),
                                String.Format("Extended Weapon Name {0} Char #{1}: {2}", ((EDmgVsBoss.Offset)weaponIndex.ToBossIndex()).Name, characterIndex, c.ToString()));

                            characterIndex++;
                        }
                    }
                }
                else
                {
                    WeaponName weaponName = weaponNameGenerator.GenerateWeaponName(false);
                    weaponNames.Add(weaponIndex, weaponName);

                    Int32 characterIndex = 0;
                    foreach (Char c in weaponName.Name)
                    {
                        in_Patch.Add(
                            offsetAddress + characterIndex,
                            c.AsPrintCharacter(),
                            String.Format("Weapon Name {0} Char #{1}: {2}", ((EDmgVsBoss.Offset)weaponIndex.ToBossIndex()).Name, characterIndex, c.ToString()));

                        characterIndex++;
                    }
                }
                offset += 0x10;
            }

            // Get a list of the weapon letters
            Dictionary<EWeaponIndex, Char> weaponLetters = weaponNames
                .ToDictionary(x => x.Key, x => x.Value.Letter);


            // Write in "Weapon Get" letters
            //
            // NOTE! There is not a space for buster because
            // there is never a "Weapon Get" for buster
            Dictionary<EWeaponIndex, Int32> weaponGetLetterAddress = new()
            {
                { EWeaponIndex.Heat, WEAPON_GET_LETTERS_ADDRESS + 0},
                { EWeaponIndex.Air, WEAPON_GET_LETTERS_ADDRESS + 1},
                { EWeaponIndex.Wood, WEAPON_GET_LETTERS_ADDRESS + 2},
                { EWeaponIndex.Bubble, WEAPON_GET_LETTERS_ADDRESS + 3},
                { EWeaponIndex.Quick, WEAPON_GET_LETTERS_ADDRESS + 4},
                { EWeaponIndex.Flash, WEAPON_GET_LETTERS_ADDRESS + 5},
                { EWeaponIndex.Metal, WEAPON_GET_LETTERS_ADDRESS + 6},
                { EWeaponIndex.Crash, WEAPON_GET_LETTERS_ADDRESS + 7},
            };

            foreach (KeyValuePair<EWeaponIndex, Char> weaponLetter in weaponLetters)
            {
                in_Patch.Add(
                    weaponGetLetterAddress[weaponLetter.Key],
                    weaponLetter.Value.AsPrintCharacter(),
                    $"Weapon Get {((EDmgVsBoss.Offset)weaponLetter.Key.ToBossIndex()).Name} Letter: {weaponLetter}");
            }

            // Pick a new weapon letter for buster
            weaponLetters.Add(EWeaponIndex.Buster, weaponNameGenerator.GetNextLetter(false));

            // Write in weapon pause menu letters
            foreach (KeyValuePair<EWeaponIndex, Char> weaponLetter in weaponLetters)
            {
                // Write to pause menu
                in_Patch.Add(
                    PauseScreenWpnAddress[weaponLetter.Key],
                    weaponLetter.Value.AsPauseScreenString(),
                    $"Pause menu weapon letter GFX for \'{weaponLetter.Value}\'");
            }

            out_NewWeaponLetters = weaponLetters;
        }


        public static void PatchCredits(Patch in_Patch, CompanyName in_CompanyName, RandomizationContext in_Context)
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

            Int32 startChar = 0x024D36; // First Byte of credits text

            for (Int32 i = 0; i < creditsSb.Length; i++)
            {
                in_Patch.Add(startChar, creditsSb[i].AsCreditsCharacter(), $"Credits Char #{i}");
                startChar++;
            }

            // Last line "Capcom Co.Ltd."
            String companyName = in_CompanyName.GetCompanyName();

            for (Int32 i = 0; i < companyName.Length; i++)
            {
                in_Patch.Add(startChar, companyName[i].AsCreditsCharacter(), $"Credits company Char #{i}");
                startChar++;
            }

            in_Patch.Add(0x024CA4, (Byte)companyName.Length, "Credits Company Line Length");

            Dictionary<EBossIndex, Int32> txtRobos = new()
            {
                { EBossIndex.Heat, 0x024D6B }, // Heat
                { EBossIndex.Air, 0x024D83 }, // Air
                { EBossIndex.Wood, 0x024D9C }, // Wood
                { EBossIndex.Bubble, 0x024DB7 }, // Bubble
                { EBossIndex.Quick, 0x024DD1 }, // Quick
                { EBossIndex.Flash, 0x024DEB }, // Flash
                { EBossIndex.Metal, 0x024E05 }, // Metal
                { EBossIndex.Crash, 0x024E1F }, // Crash
            };

            Dictionary<EBossIndex, Int32> txtWilys = new()
            {
                { EBossIndex.Dragon, 0x024E54 }, // Dragon
                { EBossIndex.Pico, 0x024E6C }, // Picopico
                { EBossIndex.Guts, 0x024E80 }, // Guts
                { EBossIndex.Boobeam, 0x024E97 }, // Boobeam
                { EBossIndex.Machine, 0x024EAE }, // Machine
                { EBossIndex.Alien, 0x024EC3 }, // Alien
            };

            // Write Robot Master damage table
            StringBuilder sb;
            foreach (EBossIndex i in txtRobos.Keys)
            {
                sb = new StringBuilder();

                var stgIdx = EBossIndex.All[(int)in_Context.BossStages[i.Offset]];
                foreach (EWeaponIndex j in EWeaponIndex.All)
                {
                    Int32 dmg = RWeaknesses.BotWeaknesses[stgIdx][j];
                    sb.Append($"{RText.GetBossWeaknessDamageChar(dmg)} ");
                }

                String rowString = sb.ToString().Trim();

                for (Int32 j = 0; j < rowString.Length; j++)
                {
                    in_Patch.Add(txtRobos[i] + j,
                        rowString[j].AsCreditsCharacter(),
                        $"Credits robo weakness table Char #{j + i.Offset * rowString.Length}");
                }
            }

            // Write Wily Boss damage table
            List<EWeaponIndex> weaponsExcludingFlash = EWeaponIndex.All
                .Where(x => EWeaponIndex.Flash != x)
                .ToList();
            foreach (EBossIndex i in txtWilys.Keys)
            {
                sb = new StringBuilder();

                foreach (EWeaponIndex j in weaponsExcludingFlash)
                {
                    Int32 dmg = RWeaknesses.WilyWeaknesses[i][j];
                    sb.Append($"{RText.GetBossWeaknessDamageChar(dmg)} ");
                }

                sb.Remove(sb.Length - 1, 1);
                String rowString = sb.ToString();

                for (Int32 j = 0; j < rowString.Length; j++)
                {
                    in_Patch.Add(txtWilys[i] + j,
                        rowString[j].AsCreditsCharacter(),
                        $"Credits wily weakness table Char #{j + i.Offset * rowString.Length}");
                }
            }
        }


        private static Char GetBossWeaknessDamageChar(Int32 dmg)
        {
            Char c;

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


        /// <summary>
        /// These ROM addresses point to the graphical data of the sprites in the pause menu, namely the weapon 
        /// letters. Use the data at <see cref="PauseScreenCipher"/> to write new values at these locations to
        /// change the weapon letter graphics.
        /// </summary>
        public static Dictionary<EWeaponIndex, Int32> PauseScreenWpnAddress = new()
        {
            { EWeaponIndex.Buster, 0x001B00 }, // "P"
            { EWeaponIndex.Heat, 0x001A00 }, // "H"
            { EWeaponIndex.Air, 0x0019C0 }, // "A"
            { EWeaponIndex.Wood, 0x0019A0 }, // "W"
            { EWeaponIndex.Bubble, 0x0019E0 }, // "B"
            { EWeaponIndex.Quick, 0x0019D0 }, // "Q"
            { EWeaponIndex.Flash, 0x0019B0 }, // "F"
            { EWeaponIndex.Metal, 0x0019F0 }, // "M"
            { EWeaponIndex.Crash, 0x001A10 }, // "C"
        };


        //
        // Private Data Members
        //

        const int SplashScreenDataOffset = 0x1f * 0x2000 + 0x1200 + 0x10;

        Dictionary<EWeaponIndex, Char> mNewWeaponLetters = new()
        {
            { EWeaponIndex.Buster, 'P' },
            { EWeaponIndex.Heat, 'H' },
            { EWeaponIndex.Air, 'A' },
            { EWeaponIndex.Wood, 'W' },
            { EWeaponIndex.Bubble, 'B' },
            { EWeaponIndex.Quick, 'Q' },
            { EWeaponIndex.Flash, 'F' },
            { EWeaponIndex.Metal, 'M' },
            { EWeaponIndex.Crash, 'C' },
        };
    }
}
