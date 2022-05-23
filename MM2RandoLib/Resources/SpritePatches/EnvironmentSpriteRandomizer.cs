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

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_CASTLE = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.Castle_MegaMan5MiniCastle,
            Properties.EnvironmentSpriteResources.Castle_None,
            Properties.EnvironmentSpriteResources.Castle_OnlyHills,
            Properties.EnvironmentSpriteResources.Castle_ProtoMan,
            Properties.EnvironmentSpriteResources.Castle_What,
        };

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

        private static readonly List<List<Byte[]?>> ENVIRONMENT_SPRITE_COLLECTION = new List<List<Byte[]?>>()
        {
            ENVIRONMENT_SPRITE_LIST_CASTLE,
            ENVIRONMENT_SPRITE_LIST_DESTRUCTIBLE_BLOCK,
            ENVIRONMENT_SPRITE_LIST_DRAGON_BLOCK,
            ENVIRONMENT_SPRITE_LIST_SPIKES,
        };
    }
}

