using System;
using System.Collections.Generic;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;

namespace MM2Randomizer.Resources.SpritePatches
{
    public static class WeaponSpriteRandomizer
    {
        public static void ApplySprites(ISeed in_Seed, Patch in_Patch, String in_TargetFileName)
        {
            foreach (List<Byte[]?> weaponSpriteList in WeaponSpriteRandomizer.WEAPON_SPRITE_COLLECTION)
            {
                Byte[]? weaponSpritePatch = in_Seed.NextElement(weaponSpriteList);

                // Each boss sprite list contians a null entry, which is used
                // to indicate the default sprite will be retained
                if (null != weaponSpritePatch)
                {
                    in_Patch.ApplyIPSPatch(in_TargetFileName, weaponSpritePatch);
                }
            }
        }

        private static readonly List<Byte[]?> WEAPON_SPRITE_LIST_AIR_SHOOTER = new List<Byte[]?>()
        {
            null,
            Properties.WeaponSpriteResources.AirShooter_DeusExMachina,
            Properties.WeaponSpriteResources.AirShooter_FinalMix,
            Properties.WeaponSpriteResources.AirShooter_GodWorld,
            Properties.WeaponSpriteResources.AirShooter_JavaIslandIndonesia,
            Properties.WeaponSpriteResources.AirShooter_NoConstancy,
            Properties.WeaponSpriteResources.AirShooter_RockMan2Plus,
            Properties.WeaponSpriteResources.AirShooter_VineMan,
        };

        private static readonly List<Byte[]?> WEAPON_SPRITE_LIST_ATOMIC_FIRE = new List<Byte[]?>()
        {
            null,
            Properties.WeaponSpriteResources.AtomicFire_DeusExMachina,
            Properties.WeaponSpriteResources.AtomicFire_FinalMix,
            Properties.WeaponSpriteResources.AtomicFire_GodWorld,
            Properties.WeaponSpriteResources.AtomicFire_JavaIslandIndonesia,
            Properties.WeaponSpriteResources.AtomicFire_NoConstancy,
            Properties.WeaponSpriteResources.AtomicFire_RockMan2Plus,
        };

        private static readonly List<Byte[]?> WEAPON_SPRITE_LIST_BUBBLE_LEAD = new List<Byte[]?>()
        {
            null,
            Properties.WeaponSpriteResources.BubbleLead_GodWorld,
            Properties.WeaponSpriteResources.BubbleLead_JavaIslandIndonesia,
            Properties.WeaponSpriteResources.BubbleLead_NoConstancy,
            Properties.WeaponSpriteResources.BubbleLead_RockMan2E,
            Properties.WeaponSpriteResources.BubbleLead_RockMan2Plus,
            Properties.WeaponSpriteResources.BubbleLead_SearchSnakeV1,
            Properties.WeaponSpriteResources.BubbleLead_SearchSnakeV1,
            Properties.WeaponSpriteResources.BubbleLead_SparkShock,
        };

        private static readonly List<Byte[]?> WEAPON_SPRITE_LIST_CRASH_BOMBER = new List<Byte[]?>()
        {
            null,
            Properties.WeaponSpriteResources.CrashBomber_DeusExMachina,
            Properties.WeaponSpriteResources.CrashBomber_FinalMix,
            Properties.WeaponSpriteResources.CrashBomber_GodWorld,
            Properties.WeaponSpriteResources.CrashBomber_JavaIslandIndonesia,
            Properties.WeaponSpriteResources.CrashBomber_NoConstancy,
            Properties.WeaponSpriteResources.CrashBomber_RockMan2E,
            Properties.WeaponSpriteResources.CrashBomber_RockMan2Plus,
        };

        private static readonly List<Byte[]?> WEAPON_SPRITE_LIST_LEAF_SHIELD = new List<Byte[]?>()
        {
            null,
            Properties.WeaponSpriteResources.LeafShield_FinalMix,
            Properties.WeaponSpriteResources.LeafShield_GodWorld,
            Properties.WeaponSpriteResources.LeafShield_JavaIslandIndonesia,
            Properties.WeaponSpriteResources.LeafShield_JewelSatelliteV1,
            Properties.WeaponSpriteResources.LeafShield_JewelSatelliteV2,
            Properties.WeaponSpriteResources.LeafShield_NoConstancy,
            Properties.WeaponSpriteResources.LeafShield_RockMan2E,
            Properties.WeaponSpriteResources.LeafShield_RockMan2Plus,
            Properties.WeaponSpriteResources.LeafShield_SkullBarrier,
        };

        private static readonly List<Byte[]?> WEAPON_SPRITE_LIST_METAL_BLADE = new List<Byte[]?>()
        {
            null,
            Properties.WeaponSpriteResources.MetalBlade_DeusExMachina,
            Properties.WeaponSpriteResources.MetalBlade_FinalMix,
            Properties.WeaponSpriteResources.MetalBlade_GodWorld,
            Properties.WeaponSpriteResources.MetalBlade_JavaIslandIndonesia,
            Properties.WeaponSpriteResources.MetalBlade_HypnoShot,
            Properties.WeaponSpriteResources.MetalBlade_NoConstancy,
            Properties.WeaponSpriteResources.MetalBlade_RockMan2E,
            Properties.WeaponSpriteResources.MetalBlade_RockMan2Plus,
            Properties.WeaponSpriteResources.MetalBlade_ShadowBlade,
            Properties.WeaponSpriteResources.MetalBlade_Star,
        };

        private static readonly List<Byte[]?> WEAPON_SPRITE_LIST_QUICK_BOOMERANG = new List<Byte[]?>()
        {
            null,
            Properties.WeaponSpriteResources.QuickBoomerang_DeusExMachina,
            Properties.WeaponSpriteResources.QuickBoomerang_FinalMix,
            Properties.WeaponSpriteResources.QuickBoomerang_GodWorld,
            Properties.WeaponSpriteResources.QuickBoomerang_JavaIslandIndonesia,
            Properties.WeaponSpriteResources.QuickBoomerang_NoConstancy,
            Properties.WeaponSpriteResources.QuickBoomerang_RockMan2E,
            Properties.WeaponSpriteResources.QuickBoomerang_RockMan2Plus,
            Properties.WeaponSpriteResources.QuickBoomerang_VineMan,
        };

        private static readonly List<Byte[]?> WEAPON_SPRITE_LIST_TIME_STOPPER = new List<Byte[]?>()
        {
            null,
            Properties.WeaponSpriteResources.TimeStopper_DeusExMachina,
            Properties.WeaponSpriteResources.TimeStopper_JavaIslandIndonesia,
            Properties.WeaponSpriteResources.TimeStopper_MegaMan5FlashStopper,
            Properties.WeaponSpriteResources.TimeStopper_NoConstancy,
            Properties.WeaponSpriteResources.TimeStopper_RockMan2Plus,
        };

        private static readonly List<Byte[]?> WEAPON_SPRITE_LIST_ITEM1 = new List<Byte[]?>()
        {
            null,
            Properties.WeaponSpriteResources.Item1_MegaMan2Remix,
            Properties.WeaponSpriteResources.Item1_MegaRan,
            Properties.WeaponSpriteResources.Item1_MetCarrier,
            Properties.WeaponSpriteResources.Item1_Peercast,
            Properties.WeaponSpriteResources.Item1_Rollchan2,
            Properties.WeaponSpriteResources.Item2_BulletBill,
        };

        private static readonly List<Byte[]?> WEAPON_SPRITE_LIST_ITEM2 = new List<Byte[]?>()
        {
            null,
            Properties.WeaponSpriteResources.Item2_BulletBill,
            Properties.WeaponSpriteResources.Item2_CutMansBadScissorsDay,
            Properties.WeaponSpriteResources.Item2_MegaMan2Remix,
            Properties.WeaponSpriteResources.Item2_MegaRan,
            Properties.WeaponSpriteResources.Item2_Peercast,
            Properties.WeaponSpriteResources.Item2_RedSuperArrow,
            Properties.WeaponSpriteResources.Item2_Rollchan2Broom,
            Properties.WeaponSpriteResources.Item2_RushJet1,
            Properties.WeaponSpriteResources.Item2_RushJet2,
        };

        private static readonly List<Byte[]?> WEAPON_SPRITE_LIST_ITEM3 = new List<Byte[]?>()
        {
            null,
            Properties.WeaponSpriteResources.Item3_MegaMan2Remix,
            Properties.WeaponSpriteResources.Item3_MegaRan,
            Properties.WeaponSpriteResources.Item3_Met,
            Properties.WeaponSpriteResources.Item3_Peercast,
            Properties.WeaponSpriteResources.Item3_Rollchan2,
        };

        private static readonly List<List<Byte[]?>> WEAPON_SPRITE_COLLECTION = new List<List<Byte[]?>>()
        {
            WEAPON_SPRITE_LIST_AIR_SHOOTER,
            WEAPON_SPRITE_LIST_ATOMIC_FIRE,
            WEAPON_SPRITE_LIST_BUBBLE_LEAD,
            WEAPON_SPRITE_LIST_CRASH_BOMBER,
            WEAPON_SPRITE_LIST_LEAF_SHIELD,
            WEAPON_SPRITE_LIST_METAL_BLADE,
            WEAPON_SPRITE_LIST_QUICK_BOOMERANG,
            WEAPON_SPRITE_LIST_TIME_STOPPER,
            WEAPON_SPRITE_LIST_ITEM1,
            WEAPON_SPRITE_LIST_ITEM2,
            WEAPON_SPRITE_LIST_ITEM3,
        };
    }
}
