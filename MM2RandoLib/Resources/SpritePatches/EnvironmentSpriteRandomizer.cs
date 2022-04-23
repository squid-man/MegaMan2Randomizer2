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
            Properties.EnvironmentSpriteResources.Castle_None,
            Properties.EnvironmentSpriteResources.Castle_What,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_DESTRUCTIBLE_BLOCK = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.DestructibleBlock_DoctorMarioV1,
            Properties.EnvironmentSpriteResources.DestructibleBlock_DoctorMarioV2,
        };

        private static readonly List<Byte[]?> ENVIRONMENT_SPRITE_LIST_DRAGON_BLOCK = new List<Byte[]?>()
        {
            null,
            Properties.EnvironmentSpriteResources.DragonBlock_Charlieboy,
            Properties.EnvironmentSpriteResources.DragonBlock_Cloud,
            Properties.EnvironmentSpriteResources.DragonBlock_DoctorMarioPill,
            Properties.EnvironmentSpriteResources.DragonBlock_DoctorMarioVirus,
            Properties.EnvironmentSpriteResources.DragonBlock_Note,
            Properties.EnvironmentSpriteResources.DragonBlock_NyanCat,
            Properties.EnvironmentSpriteResources.DragonBlock_Question,
            Properties.EnvironmentSpriteResources.DragonBlock_SuperMarioBros2V1,
            Properties.EnvironmentSpriteResources.DragonBlock_SuperMarioBros2V2,
        };

        private static readonly List<List<Byte[]?>> ENVIRONMENT_SPRITE_COLLECTION = new List<List<Byte[]?>>()
        {
            ENVIRONMENT_SPRITE_LIST_CASTLE,
            ENVIRONMENT_SPRITE_LIST_DESTRUCTIBLE_BLOCK,
            ENVIRONMENT_SPRITE_LIST_DRAGON_BLOCK,
        };
    }
}

