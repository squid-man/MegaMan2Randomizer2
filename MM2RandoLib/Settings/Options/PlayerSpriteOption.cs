using System;
using System.ComponentModel;

namespace MM2Randomizer
{
    using PathAttribute = PlayerSpritePathAttribute;
    using DirAttribute = PlayerSpriteParentDirectoryAttribute;

    /*  Logic for finding sprite IPS files based on enum values (all listed paths are relative to "MM2RandoLib.Resources.SpritePatches.Characters."):
        - Path(""): No IPS (vanilla)
        - Path(path): "{0}.ips"
        - Dir(dir): Try "{dir}.{value}.ips"
        - Dir(dir): Try "{dir}.PlayerCharacter_{value}.ips"
        - Neither: Try "{value}.{value}.ips"
        - Neither: Try "{value}.PlayerCharacter_{value}.ips"
    */

    public class PlayerSpritePathAttribute : Attribute
    {
        public string Path { get; }

        public PlayerSpritePathAttribute(string path)
        {
            Path = path;
        }
    }

    public class PlayerSpriteParentDirectoryAttribute : Attribute
    {
        public string Name { get; }

        public PlayerSpriteParentDirectoryAttribute(string name)
        {
            Name = name;
        }
    }

    public enum PlayerSpriteOption
    {
        [Description("Mega Man")]
        [Path("")] // No resource
        MegaMan,

        [Description("AVGN")]
        AVGN,

        [Description("Bad Box Art")]
        [Dir("MegaMan")]
        BadBoxArt,

        [Description("Bass")]
        Bass,

        [Description("Bass Reloaded")]
        [Dir("Bass")]
        BassReloaded,

        [Description("Batman")]
        BatMan,

        [Description("Break Man")]
        BreakMan,

        [Description("Byte (Blue)")]
        [Dir("MegaMan")]
        Byte,

        [Description("Byte (Red)")]
        [Dir("MegaMan")]
        ByteRed,

        [Description("CasualTom")]
        CasualTom,

        [Description("Charlieboy")]
        [Dir("Charlieboy")]
        Charlieboy,

        [Description("Charlieboy Alt")]
        [Dir("Charlieboy")]
        CharlieboyAlt,

        [Description("Coda")]
        Coda,

        [Description("Cut Man's Bad Scissors Day")]
        CutMansBadScissorsDay,

        [Description("FF Black Belt")]
        [Dir("FinalFantasy")]
        FinalFantasyBlackBelt,

        [Description("FF Black Mage")]
        [Dir("FinalFantasy")]
        FinalFantasyBlackMage,

        [Description("FF Fighter (Red)")]
        [Dir("FinalFantasy")]
        FinalFantasyFighter,

        [Description("FF Fighter (Blue)")]
        [Dir("FinalFantasy")]
        FinalFantasyFighterBlue,

        [Description("FF White Mage")]
        [Dir("FinalFantasy")]
        FinalFantasyWhiteMage,

        [Description("Francesca")]
        [Dir("KrionConquest")]
        Francesca,

        [Description("Guard")]
        [Dir("MegaMan")]
        Guard,

        [Description("Hatsune Miku")]
        HatsuneMiku,

        [Description("Java Island Indonesia")]
        [Dir("MegaMan")]
        JavaIslandIndonesia,

        [Description("Justin Bailey")]
        [Dir("Metroid")]
        JustinBailey,

        [Description("Link")]
        [Dir("Zelda")]
        Link,

        [Description("Lucky Man")]
        LuckyMan,

        [Description("Luigi (Arcade)")]
        [Dir("Mario")]
        LuigiArcade,

        [Description("Man II")]
        [Dir("MegaMan")]
        ManII,

        [Description("Mario (Arcade)")]
        [Dir("Mario")]
        MarioArcade,

        [Description("Mega Man X")]
        MegaManX,

        [Description("Mega Claus")]
        MegaClaus,

        [Description("Mega Mari")]
        [Dir("MegaMan")]
        MegaMari,

        [Description("Mega Ran")]
        [Dir("MegaRan")]
        MegaRan2Remix,

        [Description("My Little Pony")]
        MyLittlePony,

        [Description("New Lands")]
        NewLands,

        [Description("Pikachu")]
        Pikachu,

        [Description("Pit")]
        Pit,

        [Description("Proto Man")]
        ProtoMan,

        [Description("Prototype Tom")]
        [Dir("CasualTom")]
        PrototypeTom,

        [Description("Quick Man")]
        QuickMan,

        [Description("Quint")]
        Quint,

        [Description("Remix")]
        Remix,

        [Description("Rock")]
        [Dir("MegaMan")]
        Rock,

        [Description("Roll")]
        Roll,

        [Description("Roll (Mega Man 8)")]
        [Dir("Roll")]
        RollFromMegaMan8,

        [Description("Samus")]
        [Dir("Metroid")]
        Samus,

        [Description("Stantler")]
        Stantler,

        [Description("Vine Man")]
        VineMan,
    }
}
