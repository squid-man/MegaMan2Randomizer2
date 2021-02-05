using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MM2Randomizer.Randomizers
{
    public class WeaponName
    {
        //
        // Static Constructors
        //

        static WeaponName()
        {
            // Initialize the weapon first name set
            WeaponName.mFirstNameLookup = new Dictionary<Char, HashSet<String>>();

            foreach (String name in WeaponName.WEAPON_FIRST_NAME_LIST)
            {
                if (false == String.IsNullOrEmpty(name) &&
                    name.Length < WeaponName.WEAPON_FIRST_NAME_MAXLENGTH)
                {
                    String upperName = name.ToUpperInvariant();

                    Char key = upperName[0];

                    if (false == WeaponName.mFirstNameLookup.ContainsKey(key))
                    {
                        WeaponName.mFirstNameLookup.Add(key, new HashSet<String>());
                    }

                    WeaponName.mFirstNameLookup[key].Add(upperName);
                }
            }

            // Initialize the weapon second name set
            WeaponName.mSecondNameLookup = new HashSet<String>();

            foreach (String name in WeaponName.WEAPON_SECOND_NAME_LIST)
            {
                if (false == String.IsNullOrEmpty(name))
                {
                    String upperName = name.ToUpperInvariant();
                    WeaponName.mSecondNameLookup.Add(upperName);
                }
            }
        }


        //
        // Constructors
        //

        public WeaponName(String in_FirstName, String in_SecondName, Char in_WeaponLetter)
        {
            this.mFirstName = in_FirstName ?? throw new ArgumentNullException(nameof(in_FirstName));
            this.mSecondName = in_SecondName ?? throw new ArgumentNullException(nameof(in_SecondName));
            this.mLetter = in_WeaponLetter;

            if (this.mFirstName.Length + this.mSecondName.Length + 1 > WeaponName.WEAPON_NAME_MAXLENGTH)
            {
                this.mName = $"{in_FirstName} {in_SecondName[0]}.";
            }
            else
            {
                this.mName = $"{in_FirstName} {in_SecondName}";
            }
        }


        //
        // Properties
        //

        public String FirstName
        {
            get
            {
                return this.mFirstName;
            }
        }

        public String SecondName
        {
            get
            {
                return this.mSecondName;
            }
        }

        public String Name
        {
            get
            {
                return this.mName;
            }
        }

        public Char Letter
        {
            get
            {
                return this.mLetter;
            }
        }

        //
        // Public Static Methods
        //

        public static IEnumerable<WeaponName> GenerateUniqueWeaponNames(Random in_Random, Int32 in_Count)
        {
            List<WeaponName> weaponNames = new List<WeaponName>();

            // Randomize a list of the alphabet to create weapons with
            // unique first letters
            List<Char> randomLetters = WeaponName.ALPHABET.OrderBy(x => in_Random.Next()).ToList();

            // Randomize the list of second names
            IEnumerator<String> secondNameEnumerator = WeaponName.mSecondNameLookup.OrderBy(x => in_Random.Next()).GetEnumerator();

            // Loop over the count of weapon names requested
            for (Int32 weaponIndex = 0; weaponIndex < in_Count; ++weaponIndex)
            {
                // Think about re-shuffling the list if the first run through is exhaused
                Char key = randomLetters[weaponIndex % randomLetters.Count];
                HashSet<String> firstNameSet = WeaponName.mFirstNameLookup[key];

                // Choose a random weapon first name from the list of names
                // based on the first character
                String firstName = firstNameSet.ElementAt(in_Random.Next(firstNameSet.Count));

                // Choose a weapon second name
                if (false == secondNameEnumerator.MoveNext())
                {
                    secondNameEnumerator.Reset();
                    secondNameEnumerator.MoveNext();
                }

                String secondName = secondNameEnumerator.Current;

                // Add the new name to the list
                weaponNames.Add(new WeaponName(firstName, secondName, firstName[0]));
            }

            return weaponNames;
        }


        public static Char? GetUnusedWeaponLetter(Random in_Random, IEnumerable<WeaponName> in_WeaponNames)
        {
            HashSet<Char> weaponLetters = new HashSet<Char>(in_WeaponNames.Select(x => x.Letter));
            HashSet<Char> alphabetSet = new HashSet<Char>(WeaponName.ALPHABET.Select(x => x));

            alphabetSet.ExceptWith(weaponLetters);

            if (alphabetSet.Count > 0)
            {
                return alphabetSet.ElementAt(in_Random.Next(alphabetSet.Count));
            }
            else
            {
                return null;
            }
        }


        //
        // Private Data Members
        //

        private String mFirstName;
        private String mSecondName;
        private String mName;
        private Char mLetter;
        private Int32 mMaxLength;


        //
        // Private Static Members
        //

        private static readonly Dictionary<Char, HashSet<String>> mFirstNameLookup = new Dictionary<Char, HashSet<String>>();
        private static readonly HashSet<String> mSecondNameLookup = new HashSet<String>();

        private const Int32 WEAPON_FIRST_NAME_MAXLENGTH = 9;
        private const Int32 WEAPON_NAME_MAXLENGTH = 12;
        public const String ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private static readonly String[] WEAPON_FIRST_NAME_LIST = new String[]
        {
            //
            // Generic Mega Man
            //
            "Proto",        // DLN.000 Proto Man
            "Mega",         // DLN.001 Mega Man
            "Roll",         // DLN.002 Roll

            //
            // Mega Man 1
            //
            "Rolling",      // DLN.003 Cut Man
            "Super",        // DLN.004 Guts Man
            "Ice",          // DLN.005 Ice Man
            "Hyper",        // DLN.006 Bomb Man
            "Fire",         // DLN.007 Fire Man
            "Thunder",      // DLN.008 Elec Man

            //
            // Mega Man 2
            //
            "Metal",        // DWN.009 Metal Man
            "Air",          // DWN.010 Air Man
            "Bubble",       // DWN.011 Bubble Man
            "Quick",        // DWN.012 Quick Man
            "Crash",        // DWN.013 Crash Man
            "Time",         // DWN.014 Flash Man
            "Atomic",       // DWN.015 Heat Man
            "Leaf",         // DWN.016 Wood Man

            //
            // Mega Man 3
            //
            "Needle",       // DWN.017 Needle Man
            "Magnet",       // DWN.018 Magnet Man
            "Gemini",       // DWN.019 Gemini Man
            "Hard",         // DWN.020 Hard Man
            "Top",          // DWN.021 Top Man
            "Search",       // DWN.022 Snake Man
            "Spark",        // DWN.023 Spark Man
            "Shadow",       // DWN.024 Shadow Man

            //
            // Mega Man 4
            //
            "Flash",        // DWN.025 Bright Man
            "Rain",         // DWN.026 Toad Man
            "Drill",        // DWN.027 Drill Man
            "Pharaoh",      // DWN.028 Pharaoh Man
            "Ring",         // DWN.029 Ring Man
            "Dust",         // DWN.030 Dust Man
            "Dive",         // DWN.031 Dive Man
            "Skull",        // DWN.032 Skull Man

            //
            // Mega Man 5
            //
            "Gravity",      // DWN.033 Gravity Man
            "Water",        // DWN.034 Wave Man
            "Power",        // DWN.035 Stone Man
            "Gyro",         // DWN.036 Gyro Man
            "Star",         // DWN.037 Star Man
            "Charge",       // DWN.038 Charge Man
            "Napalm",       // DWN.039 Napalm Man
            "Crystal",      // DWN.040 Crystal Man

            //
            // Mega Man 6
            //
            "Blizzard",     // DWN.041 Blizzard Man
            "Centaur",      // DWN.042 Centaur Man
            "Flame",        // DWN.043 Flame Man
            "Knight",       // DWN.044 Knight Man
            "Plant",        // DWN.045 Plant Man
            "Silver",       // DWN.046 Tomahawk Man
            "Wind",         // DWN.047 Wind Man
            "Yamato",       // DWN.048 Yamato Man

            //
            // Mega Man 7
            //
            "Freeze",       // DWN.049 Freeze Man
            "Junk",         // DWN.050 Junk Man
            "Danger",       // DWN.051 Burst Man
            "Thunder",      // DWN.052 Cloud Man
            "Wild",         // DWN.053 Spring Man
            "Slash",        // DWN.054 Slash Man
            "Noise",        // DWN.055 Shade Man
            "Scorch",       // DWN.056 Turbo Man

            //
            // Mega Man 8
            //
            "Tornado",      // DWN.057 Tengu Man
            "Astro",        // DWN.058 Astro Man
            "Copy",         // DWN.058 Astro Man
            "Flame",        // DWN.059 Sword Man
            "Thunder",      // DWN.060 Clown Man
            "Homing",       // DWN.061 Search Man
            "Ice",          // DWN.062 Frost Man
            "Flash",        // DWN.063 Grenade Man
            "Water",        // DWN.064 Aqua Man

            //
            // Mega Man 9
            //
            "Concrete",     // DLN.065 Concrete Man
            "Tornado",      // DLN.066 Tornado Man
            "Laser",        // DLN.067 Splash Woman
            "Plug",         // DLN.068 Plug Man
            "Jewel",        // DLN.069 Jewel Man
            "Hornet",       // DLN.070 Hornet Man
            "Magma",        // DLN.071 Magma Man
            "Black Hole",   // DLN.072 Galaxy Man

            //
            // Mega Man 10
            //
            "Triple",       // DWN.073 Blade Man
            "Water",        // DWN.074 Pump Man
            "Commando",     // DWN.075 Commando Man
            "Chill",        // DWN.076 Chill Man
            "Thunder",      // DWN.077 Sheep Man
            "Rebound",      // DWN.078 Strike Man
            "Wheel",        // DWN.079 Nitro Man
            "Solar",        // DWN.080 Solar Man

            //
            // Mega Man 11
            //
            "Block",        // DWN.081 Block Man
            "Scramble",     // DWN.082 Fuse Man
            "Chain",        // DWN.083 Blast Man
            "Acid",         // DWN.084 Acid Man
            "Tundra",       // DWN.085 Tundra Man
            "Blazing",      // DWN.086 Torch Man
            "Pile",         // DWN.087 Impact Man
            "Bounce",       // DWN.088 Bounce Man

            //
            // Mega Man Killer
            //
            "Mirror",       // MKN.001 Enker
            "Screw",        // MKN.002 Punk
            "Ballade",      // MKN.003 Ballade

            //
            // Quint
            //
            "Sakugarne",    // ???.??? Quint

            //
            // Mega Man V
            //
            "Spark",        // SRN.001 Terra
            "Grab",         // SRN.002 Mercury
            "Bubble",       // SRN.003 Venus
            "Photon",       // SRN.004 Mars
            "Electric",     // SRN.005 Jupiter
            "Black Hole",   // SRN.006 Saturn
            "Deep",         // SRN.007 Uranus
            "Break",        // SRN.008 Pluto
            "Salt",         // SRN.009 Neptune

            //
            // Mega Man & Bass
            //
            "Lightning",    // KGN.001 Dynamo Man
            "Ice",          // KGN.002 Cold Man
            "Spread",       // KGN.003 Ground Man
            "Remote",       // KGN.004 Pirate Man
            "Wave",         // KGN.005 Burner Man
            "Magic",        // KGN.006 Magic Man
            "Bass",         // SWN.001 Bass

            //
            // Mega Man DOS
            //
            "Sonic",        // ???.??? Sonic Man
            "Force",        // ???.??? Volt Man
            "Nuclear",      // ???.??? Dyna Man

            //
            // Mega Man 3 DOS
            //
            "Bit",          // ???.??? Bit Man
            "Blade",        // ???.??? Blade Man
            "Oil",          // ???.??? Oil Man
            "Shark",        // ???.??? Shark Man
            "Water",        // ???.??? Wave Man
            "Torch",        // ???.??? Torch Man

            //
            // Speacial Weapons
            //
            "Rush",
            "Beat",
            "Wire",
            "Super",

            //
            // Other
            //
            "Toad",
            "Wily",
            "Cossack",
            "Guts",
            "Elec",
            "Cut",
            "Clown",
            "Rta",
            "Mash",
            "Turbo",
            "Tas",
            "Big",
            "Urn",

            "Nudua",
            "Joka",
            "Ello",
            "Coolkid",
            "Cyghfer",
            "Zoda",
            "Shoka",
            "Twitch",
            "Pro",
            "Ion",
            "Auto",
            "Beat",
            "Lag",
        };

        private static readonly String[] WEAPON_SECOND_NAME_LIST = new String[]
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
    }
}
