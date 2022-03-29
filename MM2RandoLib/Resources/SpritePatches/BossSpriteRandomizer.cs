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
            foreach (List<Byte[]> bossSpriteList in BossSpriteRandomizer.BOSS_SPRITE_COLLECTION)
            {
                Byte[] bossSpritePatch = in_Seed.NextElement(bossSpriteList);

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

        private static readonly List<Byte[]> BOSS_SPRITE_LIST_AIR_MAN = new List<Byte[]>()
        {
            null,
            Properties.BossSpriteResources.AirMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.AirMan_GrayZone,
            Properties.BossSpriteResources.AirMan_Peercast,
        };

        private static readonly List<Byte[]> BOSS_SPRITE_LIST_BUBBLE_MAN = new List<Byte[]>()
        {
            null,
            Properties.BossSpriteResources.BubbleMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.BubbleMan_GrayZone,
            Properties.BossSpriteResources.BubbleMan_NoConstancy,
            Properties.BossSpriteResources.BubbleMan_Peercast,
        };

        private static readonly List<Byte[]> BOSS_SPRITE_LIST_CRASH_MAN = new List<Byte[]>()
        {
            null,
            Properties.BossSpriteResources.CrashMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.CrashMan_GrayZone,
            Properties.BossSpriteResources.CrashMan_Peercast,
        };

        private static readonly List<Byte[]> BOSS_SPRITE_LIST_FLASH_MAN = new List<Byte[]>()
        {
            null,
            Properties.BossSpriteResources.FlashMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.FlashMan_GrayZone,
            Properties.BossSpriteResources.FlashMan_Peercast,
        };

        private static readonly List<Byte[]> BOSS_SPRITE_LIST_HEAT_MAN = new List<Byte[]>()
        {
            null,
            Properties.BossSpriteResources.HeatMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.HeatMan_GrayZone,
            Properties.BossSpriteResources.HeatMan_Peercast,
        };

        private static readonly List<Byte[]> BOSS_SPRITE_LIST_METAL_MAN = new List<Byte[]>()
        {
            null,
            Properties.BossSpriteResources.MetalMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.MetalMan_GrayZone,
            Properties.BossSpriteResources.MetalMan_Peercast,
        };

        private static readonly List<Byte[]> BOSS_SPRITE_LIST_QUICK_MAN = new List<Byte[]>()
        {
            null,
            Properties.BossSpriteResources.QuickMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.QuickMan_GrayZone,
            Properties.BossSpriteResources.QuickMan_Peercast,
        };

        private static readonly List<Byte[]> BOSS_SPRITE_LIST_WOOD_MAN = new List<Byte[]>()
        {
            null,
            Properties.BossSpriteResources.WoodMan_CutMansBadScissorsDay,
            Properties.BossSpriteResources.WoodMan_GrayZone,
            Properties.BossSpriteResources.WoodMan_Peercast,
            Properties.BossSpriteResources.WoodMan_ToadMan,
        };

        private static readonly List<Byte[]> BOSS_SPRITE_LIST_MECHA_DRAGON = new List<Byte[]>()
        {
            null,
            Properties.BossSpriteResources.MechaDragon_CoolDraga,
            Properties.BossSpriteResources.MechaDragon_CutMansBadScissorsDay,
            Properties.BossSpriteResources.MechaDragon_Dragocorn,
        };

        private static readonly List<Byte[]> BOSS_SPRITE_LIST_PICOPICO_KUN = new List<Byte[]>()
        {
            null,
            Properties.BossSpriteResources.PicopicoKun_CheatMode,
        };

        private static readonly List<Byte[]> BOSS_SPRITE_LIST_GUTS_TANK = new List<Byte[]>()
        {
            null,
            Properties.BossSpriteResources.GutsTank_CutMansBadScissorsDay,
        };

        private static readonly List<Byte[]> BOSS_SPRITE_LIST_WILY = new List<Byte[]>()
        {
            null,
            Properties.BossSpriteResources.Wily_DrLight,
        };

        private static readonly List<Byte[]> BOSS_SPRITE_LIST_ALIEN = new List<Byte[]>()
        {
            null,
            Properties.BossSpriteResources.Alien_CutMansBadScissorsDay,
            Properties.BossSpriteResources.Alien_Exhaust,
        };

        private static readonly List<List<Byte[]>> BOSS_SPRITE_COLLECTION = new List<List<Byte[]>>()
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
            BOSS_SPRITE_LIST_WILY,
            BOSS_SPRITE_LIST_ALIEN,
        };
    }
}
