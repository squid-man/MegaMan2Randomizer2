using System;
using System.Collections.Generic;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;

namespace MM2Randomizer.Resources.SpritePatches
{
    public static class MenusAndTransitionScreenRandomizer
    {
        public static void ApplySprites(ISeed in_Seed, Patch in_Patch, String in_TargetFileName)
        {
            foreach (List<Byte[]?> menusAndTransitionScreensSpriteList in MenusAndTransitionScreenRandomizer.MENUS_AND_TRANSITION_SCREENS_SPRITE_COLLECTION)
            {
                Byte[]? menusAndTransitionScreensSpritePatch = in_Seed.NextElement(menusAndTransitionScreensSpriteList);

                // Each boss sprite list contians a null entry, which is used
                // to indicate the default sprite will be retained
                if (null != menusAndTransitionScreensSpritePatch)
                {
                    in_Patch.ApplyIPSPatch(in_TargetFileName, menusAndTransitionScreensSpritePatch);
                }
            }
        }


        //
        // Constants
        //

        private static readonly List<Byte[]?> MENUS_AND_TRANSITION_SCREENS_SPRITE_LIST_CASTLE = new List<Byte[]?>()
        {
            null,
            Properties.MenusAndTransitionScreenRandomizer.Castle_FinalFantasy,
            Properties.MenusAndTransitionScreenRandomizer.Castle_MegaMan5MiniCastle,
            Properties.MenusAndTransitionScreenRandomizer.Castle_None,
            Properties.MenusAndTransitionScreenRandomizer.Castle_OnlyHills,
            Properties.MenusAndTransitionScreenRandomizer.Castle_ProtoMan,
            Properties.MenusAndTransitionScreenRandomizer.Castle_What,
        };

        private static readonly List<Byte[]?> MENUS_AND_TRANSITION_SCREENS_SPRITE_LIST_ROBOT_MASTER_INTRO_SCREEN = new List<Byte[]?>()
        {
            null,
            Properties.MenusAndTransitionScreenRandomizer.RobotMasterIntroScreen_Background1,
            Properties.MenusAndTransitionScreenRandomizer.RobotMasterIntroScreen_Background2,
        };

        private static readonly List<Byte[]?> MENUS_AND_TRANSITION_SCREENS_SPRITE_LIST_STAGE_SELECT_SCREEN = new List<Byte[]?>()
        {
            null,
            Properties.MenusAndTransitionScreenRandomizer.StageSelectScreen_BasicMaster,
            Properties.MenusAndTransitionScreenRandomizer.StageSelectScreen_Claw,
            Properties.MenusAndTransitionScreenRandomizer.StageSelectScreen_Exile,
            Properties.MenusAndTransitionScreenRandomizer.StageSelectScreen_None,
            Properties.MenusAndTransitionScreenRandomizer.StageSelectScreen_Ultra,
        };

        private static readonly List<Byte[]?> MENUS_AND_TRANSITION_SCREENS_SPRITE_LIST_WEAPON_GET_SCREEN = new List<Byte[]?>()
        {
            null,
            Properties.MenusAndTransitionScreenRandomizer.WeaponGetScreen_Claw,
            Properties.MenusAndTransitionScreenRandomizer.WeaponGetScreen_FinalMix,
            Properties.MenusAndTransitionScreenRandomizer.WeaponGetScreen_MissingTile,
            Properties.MenusAndTransitionScreenRandomizer.WeaponGetScreen_None,
            Properties.MenusAndTransitionScreenRandomizer.WeaponGetScreen_Remix,
            Properties.MenusAndTransitionScreenRandomizer.WeaponGetScreen_Ultra,
        };

        private static readonly List<Byte[]?> MENUS_AND_TRANSITION_SCREENS_SPRITE_LIST_WEAPON_MENU_BORDER = new List<Byte[]?>()
        {
            null,
            Properties.MenusAndTransitionScreenRandomizer.WeaponMenuBorder_Battletoads,
            Properties.MenusAndTransitionScreenRandomizer.WeaponMenuBorder_CB,
            Properties.MenusAndTransitionScreenRandomizer.WeaponMenuBorder_DestructibleBlock,
            Properties.MenusAndTransitionScreenRandomizer.WeaponMenuBorder_Kirby,
            Properties.MenusAndTransitionScreenRandomizer.WeaponMenuBorder_MegaMan,
            Properties.MenusAndTransitionScreenRandomizer.WeaponMenuBorder_MegaManX,
            Properties.MenusAndTransitionScreenRandomizer.WeaponMenuBorder_Pacman,
            Properties.MenusAndTransitionScreenRandomizer.WeaponMenuBorder_Paperboy,
            Properties.MenusAndTransitionScreenRandomizer.WeaponMenuBorder_SmallRefill1,
            Properties.MenusAndTransitionScreenRandomizer.WeaponMenuBorder_SmallRefill2,
            Properties.MenusAndTransitionScreenRandomizer.WeaponMenuBorder_SmallRefill3,
            Properties.MenusAndTransitionScreenRandomizer.WeaponMenuBorder_ZeldaCandle,
            Properties.MenusAndTransitionScreenRandomizer.WeaponMenuBorder_ZeldaShield,
        };

        private static readonly List<List<Byte[]?>> MENUS_AND_TRANSITION_SCREENS_SPRITE_COLLECTION = new List<List<Byte[]?>>()
        {
            MENUS_AND_TRANSITION_SCREENS_SPRITE_LIST_CASTLE,
            MENUS_AND_TRANSITION_SCREENS_SPRITE_LIST_ROBOT_MASTER_INTRO_SCREEN,
            MENUS_AND_TRANSITION_SCREENS_SPRITE_LIST_STAGE_SELECT_SCREEN,
            MENUS_AND_TRANSITION_SCREENS_SPRITE_LIST_WEAPON_GET_SCREEN,
            MENUS_AND_TRANSITION_SCREENS_SPRITE_LIST_WEAPON_MENU_BORDER,
        };
    }
}

