using System;
using System.Collections.Generic;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;

namespace MM2Randomizer.Resources.SpritePatches
{
    public static class EnvironmentSpriteRandomizer
    {
        public static void ApplySprites(ISeed in_Seed, Patch in_Patch, String in_TargetFileName)
        {
            foreach (List<Byte[]?> environmentSpriteList in EnvironmentSpriteRandomizer.ENVIRONMENT_SPRITE_COLLECTION)
            {
                Byte[]? environmentSpritePatch = in_Seed.NextElement(environmentSpriteList);

                // Each boss sprite list contians a null entry, which is used
                // to indicate the default sprite will be retained
                if (null != environmentSpritePatch)
                {
                    in_Patch.ApplyIPSPatch(in_TargetFileName, environmentSpritePatch);
                }
            }
        }


        //
        // Constants
        //

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_DESTRUCTIBLE_BLOCK = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.DestructibleBlock_DoctorMarioV1,
            Properties.EnvironmentSpriteResources.DestructibleBlock_DoctorMarioV2,
            Properties.EnvironmentSpriteResources.DestructibleBlock_SuperMarioBros3,
            Properties.EnvironmentSpriteResources.DestructibleBlock_SuperMarioBros3V2,
            Properties.EnvironmentSpriteResources.DestructibleBlock_SuperMarioBros3V3,
            Properties.EnvironmentSpriteResources.DestructibleBlock_SuperMarioBros3V4,
            Properties.EnvironmentSpriteResources.DestructibleBlock_SuperMarioBrosBrick,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_DRAGON_BLOCK = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.DragonBlock_Ash,
            Properties.EnvironmentSpriteResources.DragonBlock_Charlieboy,
            Properties.EnvironmentSpriteResources.DragonBlock_Cloud,
            Properties.EnvironmentSpriteResources.DragonBlock_DoctorMarioPill,
            Properties.EnvironmentSpriteResources.DragonBlock_DoctorMarioVirus,
            Properties.EnvironmentSpriteResources.DragonBlock_Note,
            Properties.EnvironmentSpriteResources.DragonBlock_NyanCat,
            Properties.EnvironmentSpriteResources.DragonBlock_Question,
            Properties.EnvironmentSpriteResources.DragonBlock_SuperMarioBros2V1,
            Properties.EnvironmentSpriteResources.DragonBlock_SuperMarioBros2V2,
            Properties.EnvironmentSpriteResources.DragonBlock_SuperMarioBrosBrick,
            Properties.EnvironmentSpriteResources.DragonBlock_WB,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_SPIKES = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.Spikes_1UP,
            Properties.EnvironmentSpriteResources.Spikes_Ash,
            Properties.EnvironmentSpriteResources.Spikes_Diamond,
            Properties.EnvironmentSpriteResources.Spikes_DIE,
            Properties.EnvironmentSpriteResources.Spikes_JavaIslandIndonesia,
            Properties.EnvironmentSpriteResources.Spikes_Jelectro,
            Properties.EnvironmentSpriteResources.Spikes_MarioStar,
            Properties.EnvironmentSpriteResources.Spikes_MegaMan1,
            Properties.EnvironmentSpriteResources.Spikes_MegaMan3V1,
            Properties.EnvironmentSpriteResources.Spikes_MegaMan3V2,
            Properties.EnvironmentSpriteResources.Spikes_MegaMan3V3,
            Properties.EnvironmentSpriteResources.Spikes_MegaMan3V4,
            Properties.EnvironmentSpriteResources.Spikes_MegaMan4V1,
            Properties.EnvironmentSpriteResources.Spikes_MegaMan4V2,
            Properties.EnvironmentSpriteResources.Spikes_MegaMan4V3,
            Properties.EnvironmentSpriteResources.Spikes_Muncher,
            Properties.EnvironmentSpriteResources.Spikes_SkullV1,
            Properties.EnvironmentSpriteResources.Spikes_SkullV2,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_AIR_MAN_BLOCK = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_AirMan_Block_Clear,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_BUBBLE_MAN_WATERFALL = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_BubbleMan_Waterfall_CBv1,
            Properties.EnvironmentSpriteResources.StageTile_BubbleMan_Waterfall_CBv2,
            Properties.EnvironmentSpriteResources.StageTile_BubbleMan_Waterfall_CoinfallV1,
            Properties.EnvironmentSpriteResources.StageTile_BubbleMan_Waterfall_CoinfallV2,
            Properties.EnvironmentSpriteResources.StageTile_BubbleMan_Waterfall_None,
            Properties.EnvironmentSpriteResources.StageTile_BubbleMan_Waterfall_QBlock,
            Properties.EnvironmentSpriteResources.StageTile_BubbleMan_Waterfall_Rain,
            Properties.EnvironmentSpriteResources.StageTile_BubbleMan_Waterfall_Sand,
            Properties.EnvironmentSpriteResources.StageTile_BubbleMan_Waterfall_SuperMarioBros2,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_CRASH_MAN_BLOCK = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_CrashMan_Block_SuperMarioBrosBlock,
            Properties.EnvironmentSpriteResources.StageTile_CrashMan_Block_SuperMarioBrosBrick,
            Properties.EnvironmentSpriteResources.StageTile_CrashMan_Block_TMNT2,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_CRASH_MAN_DAYLIGHT_BACKGROUND = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_CrashMan_DaylightBackground_SuperMarioBrosClouds,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_CRASH_MAN_GLOBE = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_CrashMan_Globe_TMNT2,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_CRASH_MAN_NIGHT_BACKGROUND = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_CrashMan_NightBackground_SuperMarioBros3Stars,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_CRASH_MAN_PIPE = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_CrashMan_Pipe_SuperMarioBros,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_FLASH_MAN_BACKGROUND = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_FlashMan_Background_DDBrick,
            Properties.EnvironmentSpriteResources.StageTile_FlashMan_Background_FinalMix,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_FLASH_MAN_BLOCK = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_FlashMan_Block_SuperMarioBrosBlock,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_HEAT_MAN_BACKGROUND = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_HeatMan_Background_None,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_HEAT_MAN_BLOCK = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_HeatMan_Block_JavaIslandIndonesia,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_HEAT_MAN_LAVA = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_HeatMan_Lava_Axe,
            Properties.EnvironmentSpriteResources.StageTile_HeatMan_Lava_CBv1,
            Properties.EnvironmentSpriteResources.StageTile_HeatMan_Lava_CBv2,
            Properties.EnvironmentSpriteResources.StageTile_HeatMan_Lava_Sand,
            Properties.EnvironmentSpriteResources.StageTile_HeatMan_Lava_SpikeV1,
            Properties.EnvironmentSpriteResources.StageTile_HeatMan_Lava_SpikeV2,
            Properties.EnvironmentSpriteResources.StageTile_HeatMan_Lava_SuperMarioBros,
            Properties.EnvironmentSpriteResources.StageTile_HeatMan_Lava_TMNT2,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_METAL_MAN_BACKGROUND = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_MetalMan_Background_None,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_METAL_MAN_BLOCK = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_MetalMan_Block_Sand,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_METAL_MAN_CONVEYOR = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_MetalMan_Conveyor_CB,
            Properties.EnvironmentSpriteResources.StageTile_MetalMan_Conveyor_CB2,
            Properties.EnvironmentSpriteResources.StageTile_MetalMan_Conveyor_CB3,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_QUICK_MAN_BACKGROUND = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_QuickMan_Background_None,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_QUICK_MAN_BLOCK = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_QuickMan_Block_HeatBrick,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_WOOD_MAN_BLOCK = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_WoodMan_Block_SuperMarioBros,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_WOOD_MAN_CAVE_BACKGROUND = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_WoodMan_CaveBackground_None,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_WOOD_MAN_CAVE_GROUND = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_WoodMan_CaveGround_Sand,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_STAGE_TILE_WOOD_MAN_GRASS = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.StageTile_WoodMan_Grass_SuperMarioBros2V1,
            Properties.EnvironmentSpriteResources.StageTile_WoodMan_Grass_SuperMarioBros2V1,
            Properties.EnvironmentSpriteResources.StageTile_WoodMan_Grass_SuperMarioBros3V1,
            Properties.EnvironmentSpriteResources.StageTile_WoodMan_Grass_SuperMarioBros3V2,
            Properties.EnvironmentSpriteResources.StageTile_WoodMan_Grass_SuperMarioBros3V3,
        };

        private static readonly List<List<Byte[]?>> ENVIRONMENT_SPRITE_COLLECTION = new List<List<Byte[]?>>()
        {
            ENVIRONMENT_SPRITE_LIST_DESTRUCTIBLE_BLOCK,
            ENVIRONMENT_SPRITE_LIST_DRAGON_BLOCK,
            ENVIRONMENT_SPRITE_LIST_SPIKES,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_AIR_MAN_BLOCK,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_BUBBLE_MAN_WATERFALL,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_CRASH_MAN_DAYLIGHT_BACKGROUND,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_CRASH_MAN_GLOBE,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_CRASH_MAN_NIGHT_BACKGROUND,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_CRASH_MAN_PIPE,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_CRASH_MAN_BLOCK,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_FLASH_MAN_BACKGROUND,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_FLASH_MAN_BLOCK,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_HEAT_MAN_BACKGROUND,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_HEAT_MAN_LAVA,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_HEAT_MAN_BLOCK,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_METAL_MAN_BACKGROUND,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_METAL_MAN_BLOCK,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_METAL_MAN_CONVEYOR,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_QUICK_MAN_BACKGROUND,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_QUICK_MAN_BLOCK,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_WOOD_MAN_CAVE_BACKGROUND,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_WOOD_MAN_CAVE_GROUND,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_WOOD_MAN_GRASS,
            ENVIRONMENT_SPRITE_LIST_STAGE_TILE_WOOD_MAN_BLOCK,
        };
    }
}

