using System;
using System.Collections.Generic;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;

namespace MM2Randomizer.Resources.SpritePatches
{
    public static class BossSpriteRandomizer
    {
        public static void ApplySprites(ISeed in_Seed, Patch in_Patch, String in_TargetFileName)
        {
            foreach (List<Byte[]?> bossSpriteList in BossSpriteRandomizer.BOSS_SPRITE_COLLECTION)
            {
                Byte[]? bossSpritePatch = in_Seed.NextElement(bossSpriteList);

                // Each boss sprite list contians a null entry, which is used
                // to indicate the default sprite will be retained
                if (null != bossSpritePatch)
                {
                    in_Patch.ApplyIPSPatch(in_TargetFileName, bossSpritePatch);
                }
            }
        }


        //
        // Constants
        //

        private static readonly List<Byte[]?> BOSS_SPRITE_LIST_AIR_MAN = new List<Byte[]?>()
        {
            null,
            Properties.BossSpriteResources.AirMan_Another,
            Properties.BossSpriteResources.AirMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.AirMan_GrayZone,
            Properties.BossSpriteResources.Airman_HornetMan,
            Properties.BossSpriteResources.AirMan_Peercast,
            Properties.BossSpriteResources.Airman_RockMan2E,
            Properties.BossSpriteResources.Airman_RockMan2Plus,
        };

        private static readonly List<Byte[]?> BOSS_SPRITE_LIST_BUBBLE_MAN = new List<Byte[]?>()
        {
            null,
            Properties.BossSpriteResources.BubbleMan_Another,
            Properties.BossSpriteResources.BubbleMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.BubbleMan_GrayZone,
            Properties.BossSpriteResources.BubbleMan_NoConstancy,
            Properties.BossSpriteResources.BubbleMan_Peercast,
            Properties.BossSpriteResources.BubbleMan_RockMan2E,
            Properties.BossSpriteResources.BubbleMan_RockMan2Plus,
        };

        private static readonly List<Byte[]?> BOSS_SPRITE_LIST_CRASH_MAN = new List<Byte[]?>()
        {
            null,
            Properties.BossSpriteResources.CrashMan_Another,
            Properties.BossSpriteResources.CrashMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.CrashMan_DX,
            Properties.BossSpriteResources.CrashMan_GrayZone,
            Properties.BossSpriteResources.CrashMan_Peercast,
            Properties.BossSpriteResources.CrashMan_RockMan2E,
            Properties.BossSpriteResources.CrashMan_RockMan2Plus,
        };

        private static readonly List<Byte[]?> BOSS_SPRITE_LIST_FLASH_MAN = new List<Byte[]?>()
        {
            null,
            Properties.BossSpriteResources.FlashMan_Another,
            Properties.BossSpriteResources.FlashMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.FlashMan_GrayZone,
            Properties.BossSpriteResources.FlashMan_Peercast,
            Properties.BossSpriteResources.FlashMan_RockMan2E,
            Properties.BossSpriteResources.FlashMan_RockMan2Plus,
        };

        private static readonly List<Byte[]?> BOSS_SPRITE_LIST_HEAT_MAN = new List<Byte[]?>()
        {
            null,
            Properties.BossSpriteResources.HeatMan_Another,
            Properties.BossSpriteResources.HeatMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.HeatMan_DX,
            Properties.BossSpriteResources.HeatMan_GrayZone,
            Properties.BossSpriteResources.HeatMan_Peercast,
            Properties.BossSpriteResources.HeatMan_RockMan2E,
            Properties.BossSpriteResources.HeatMan_RockMan2Plus,
        };

        private static readonly List<Byte[]?> BOSS_SPRITE_LIST_METAL_MAN = new List<Byte[]?>()
        {
            null,
            Properties.BossSpriteResources.MetalMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.MetalMan_ElecMan,
            Properties.BossSpriteResources.MetalMan_GrayZone,
            Properties.BossSpriteResources.MetalMan_Peercast,
            Properties.BossSpriteResources.MetalMan_RockMan2E,
            Properties.BossSpriteResources.MetalMan_RockMan2Plus,
        };

        private static readonly List<Byte[]?> BOSS_SPRITE_LIST_QUICK_MAN = new List<Byte[]?>()
        {
            null,
            Properties.BossSpriteResources.QuickMan_Another,
            Properties.BossSpriteResources.QuickMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.QuickMan_GrayZone,
            Properties.BossSpriteResources.QuickMan_Peercast,
            Properties.BossSpriteResources.QuickMan_RockMan2E,
            Properties.BossSpriteResources.QuickMan_RockMan2Plus,
            Properties.BossSpriteResources.QuickMan_Sonic,
        };

        private static readonly List<Byte[]?> BOSS_SPRITE_LIST_WOOD_MAN = new List<Byte[]?>()
        {
            null,
            Properties.BossSpriteResources.WoodMan_Another,
            Properties.BossSpriteResources.WoodMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.WoodMan_DK,
            Properties.BossSpriteResources.WoodMan_DX,
            Properties.BossSpriteResources.WoodMan_GrayZone,
            Properties.BossSpriteResources.WoodMan_Peercast,
            Properties.BossSpriteResources.WoodMan_RockMan2E,
            Properties.BossSpriteResources.WoodMan_RockMan2Plus,
            Properties.BossSpriteResources.WoodMan_ToadMan,
        };

        private static readonly List<Byte[]?> BOSS_SPRITE_LIST_MECHA_DRAGON = new List<Byte[]?>()
        {
            null,
            Properties.BossSpriteResources.MechaDragon_CoolDraga,
            Properties.BossSpriteResources.MechaDragon_CutMansBadScissorsDay,
            Properties.BossSpriteResources.MechaDragon_Dragocorn,
            Properties.BossSpriteResources.MechaDragon_Shades,
            Properties.BossSpriteResources.MechaDragon_Swag,
        };

        private static readonly List<Byte[]?> BOSS_SPRITE_LIST_PICOPICO_KUN = new List<Byte[]?>()
        {
            null,
            Properties.BossSpriteResources.PicopicoKun_CheatMode,
            Properties.BossSpriteResources.PicopicoKun_RockMan2Plus,
            Properties.BossSpriteResources.PicopicoKun_VineMan,
        };

        private static readonly List<Byte[]?> BOSS_SPRITE_LIST_GUTS_TANK = new List<Byte[]?>()
        {
            null,
            Properties.BossSpriteResources.GutsTank_CutMansBadScissorsDay,
            Properties.BossSpriteResources.GutsTank_Cray,
        };

        private static readonly List<Byte[]?> BOSS_SPRITE_LIST_BOOBEAM_TRAP = new List<Byte[]?>()
        {
            null,
            Properties.BossSpriteResources.BoobeamTrap_MegaMan1TurretV1,
            Properties.BossSpriteResources.BoobeamTrap_MegaMan1TurretV2,
            Properties.BossSpriteResources.BoobeamTrap_MegaMan5Turret,
        };

        private static readonly List<Byte[]?> BOSS_SPRITE_LIST_WILY = new List<Byte[]?>()
        {
            null,
            Properties.BossSpriteResources.Wily_Abobo,
            Properties.BossSpriteResources.Wily_Cossack,
            Properties.BossSpriteResources.Wily_CutMansBadScissorsDay,
            Properties.BossSpriteResources.Wily_DrLight,
            Properties.BossSpriteResources.Wily_DrM,
            Properties.BossSpriteResources.Wily_MegaMan3,
            Properties.BossSpriteResources.Wily_RiverCityMan,
        };

        private static readonly List<Byte[]?> BOSS_SPRITE_LIST_ALIEN = new List<Byte[]?>()
        {
            null,
            Properties.BossSpriteResources.Alien_Cray,
            Properties.BossSpriteResources.Alien_CutMansBadScissorsDay,
            Properties.BossSpriteResources.Alien_Exhaust,
            Properties.BossSpriteResources.Alien_Grayzone,
            Properties.BossSpriteResources.Alien_Gyotot,
            Properties.BossSpriteResources.Alien_NoConstancy,
            Properties.BossSpriteResources.Alien_RockMan2E,
            Properties.BossSpriteResources.Alien_RockMan2Hardcore,
        };

        private static readonly List<Byte[]?> BOSS_SPRITE_LIST_BOSS_DOOR_SIGN = new List<Byte[]?>()
        {
            null,
            Properties.BossSpriteResources.BossDoorSign_April,
            Properties.BossSpriteResources.BossDoorSign_BurgerTime,
            Properties.BossSpriteResources.BossDoorSign_CapnA,
            Properties.BossSpriteResources.BossDoorSign_Charlieboy,
            Properties.BossSpriteResources.BossDoorSign_CutMansBadScissorsDay,
            Properties.BossSpriteResources.BossDoorSign_Don,
            Properties.BossSpriteResources.BossDoorSign_DoubleDragon,
            Properties.BossSpriteResources.BossDoorSign_DrCossack,
            Properties.BossSpriteResources.BossDoorSign_DrLight,
            Properties.BossSpriteResources.BossDoorSign_Leo,
            Properties.BossSpriteResources.BossDoorSign_LilMac,
            Properties.BossSpriteResources.BossDoorSign_MegaMan4Squid,
            Properties.BossSpriteResources.BossDoorSign_Mikey,
            Properties.BossSpriteResources.BossDoorSign_MortalKombat,
            Properties.BossSpriteResources.BossDoorSign_MrX,
            Properties.BossSpriteResources.BossDoorSign_NoReset,
            Properties.BossSpriteResources.BossDoorSign_NothinButSpeedruns,
            Properties.BossSpriteResources.BossDoorSign_PizzaHut,
            Properties.BossSpriteResources.BossDoorSign_Raph,
            Properties.BossSpriteResources.BossDoorSign_Shredder,
            Properties.BossSpriteResources.BossDoorSign_SiberianBull,
            Properties.BossSpriteResources.BossDoorSign_SpeedGaming,
            Properties.BossSpriteResources.BossDoorSign_Squidman,
            Properties.BossSpriteResources.BossDoorSign_WWF,
        };

        private static readonly List<List<Byte[]?>> BOSS_SPRITE_COLLECTION = new List<List<Byte[]?>>()
        {
            BOSS_SPRITE_LIST_AIR_MAN,
            BOSS_SPRITE_LIST_BUBBLE_MAN,
            BOSS_SPRITE_LIST_CRASH_MAN,
            BOSS_SPRITE_LIST_FLASH_MAN,
            BOSS_SPRITE_LIST_HEAT_MAN,
            BOSS_SPRITE_LIST_METAL_MAN,
            BOSS_SPRITE_LIST_QUICK_MAN,
            BOSS_SPRITE_LIST_WOOD_MAN,
            BOSS_SPRITE_LIST_MECHA_DRAGON,
            BOSS_SPRITE_LIST_PICOPICO_KUN,
            BOSS_SPRITE_LIST_GUTS_TANK,
            BOSS_SPRITE_LIST_BOOBEAM_TRAP,
            BOSS_SPRITE_LIST_WILY,
            BOSS_SPRITE_LIST_ALIEN,
            BOSS_SPRITE_LIST_BOSS_DOOR_SIGN,
        };
    }
}
