using System;
using System.Collections.Generic;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;

namespace MM2Randomizer.Resources.SpritePatches
{
    public static class PickupSpriteRandomizer
    {
        public static void ApplySprites(ISeed in_Seed, Patch in_Patch, String in_TargetFileName)
        {
            foreach (List<Byte[]?> pickupSpriteList in PickupSpriteRandomizer.PICKUP_SPRITE_COLLECTION)
            {
                Byte[]? pickupSpritePatch = in_Seed.NextElement(pickupSpriteList);

                // Each boss sprite list contians a null entry, which is used
                // to indicate the default sprite will be retained
                if (null != pickupSpritePatch)
                {
                    in_Patch.ApplyIPSPatch(in_TargetFileName, pickupSpritePatch);
                }
            }
        }


        //
        // Constants
        //
        private static readonly List<Byte[]?> PICKUP_SPRITE_LIST_ETANK = new List<Byte[]?>()
        {
            null,
            Properties.PickupSpriteResources.ETank_FinalFantasy,
            Properties.PickupSpriteResources.ETank_JavaIslandIndonesia,
            Properties.PickupSpriteResources.ETank_MegaManX,
            Properties.PickupSpriteResources.ETank_Metroid,
        };

        private static readonly List<Byte[]?> PICKUP_SPRITE_LIST_HEALTH_AND_WEAPON_ENERGY = new List<Byte[]?>()
        {
            null,
            Properties.PickupSpriteResources.HealthAndWeaponEnergy_Byte,
            Properties.PickupSpriteResources.HealthAndWeaponEnergy_CharlieboyV1,
            Properties.PickupSpriteResources.HealthAndWeaponEnergy_CharlieboyV2,
            Properties.PickupSpriteResources.HealthAndWeaponEnergy_CharlieboyV3,
            Properties.PickupSpriteResources.HealthAndWeaponEnergy_CharlieboyV4,
            Properties.PickupSpriteResources.HealthAndWeaponEnergy_MegaMan1,
            Properties.PickupSpriteResources.HealthAndWeaponEnergy_MegaManX,
            Properties.PickupSpriteResources.HealthAndWeaponEnergy_Quickman,
        };

        private static readonly List<List<Byte[]?>> PICKUP_SPRITE_COLLECTION = new List<List<Byte[]?>>()
        {
            PICKUP_SPRITE_LIST_ETANK,
            PICKUP_SPRITE_LIST_HEALTH_AND_WEAPON_ENERGY,
        };
    }
}
