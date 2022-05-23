using System;
using System.Collections.Generic;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;

namespace MM2Randomizer.Resources.SpritePatches
{
    public static class EnemySpriteRandomizer
    {
        public static void ApplySprites(ISeed in_Seed, Patch in_Patch, String in_TargetFileName)
        {
            foreach (List<Byte[]?> enemySpriteList in EnemySpriteRandomizer.ENEMY_SPRITE_COLLECTION)
            {
                Byte[]? enemySpritePatch = in_Seed.NextElement(enemySpriteList);

                // Each boss sprite list contians a null entry, which is used
                // to indicate the default sprite will be retained
                if (null != enemySpritePatch)
                {
                    in_Patch.ApplyIPSPatch(in_TargetFileName, enemySpritePatch);
                }
            }
        }

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_BATTON = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Batton_Ghost,
            Properties.EnemySpriteResources.Batton_Vader,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_BLOCKY = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Blocky_Rocky,
            Properties.EnemySpriteResources.Blocky_TotemPolen,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_CHANGKEY_MAKER = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.ChangkeyMaker_ConstructionJoe,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_CLAW = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Claw_Chiko,
            Properties.EnemySpriteResources.Claw_MarioFish,
            Properties.EnemySpriteResources.Claw_Peercast,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_FLY_BOY = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.FlyBoy_CutMansBadScissorsDay,
            Properties.EnemySpriteResources.FlyBoy_VineMan,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_FORCE_BEAM_BODY = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.ForceBeamBody_Block,
            Properties.EnemySpriteResources.ForceBeamBody_Invisible,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_FORCE_BEAM_END = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.ForceBeamEnd_Invisible,
            Properties.EnemySpriteResources.ForceBeamEnd_NyanCat,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_FRIENDER = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Friender_DeusExMachina,
            Properties.EnemySpriteResources.Friender_Evil,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_KAMINARI_GORO = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.KaminariGoro_CutMansBadScissorsDay,
            Properties.EnemySpriteResources.KaminariGoro_MetLord,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_KEROG_AND_PETIT_KEROG = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.KerogAndPetitKerog_Hearts,
            Properties.EnemySpriteResources.KerogAndPetitKerog_Komasaburo,
            Properties.EnemySpriteResources.KerogAndPetitKerog_MetMom,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_KUKKU = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Kukku_DX,
            Properties.EnemySpriteResources.Kukku_VineCock,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_M445 = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.M445_M422A,
            Properties.EnemySpriteResources.M445_Metroid,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_MATASABURO = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Matasaburo_EnhancedBlow,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_MOLE = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Mole_MegaMan9Drill
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_MONKING = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Monking_VineMan
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_NEO_METALL = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.NeoMetall_1UpMet1,
            Properties.EnemySpriteResources.NeoMetall_1UpMet2,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_PIEROBOT = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Pierobot_VineMan,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_PIPI_AND_COPIPI = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.PipiAndCopipi_Beat,
            Properties.EnemySpriteResources.PipiAndCopipi_BomberPipi,
            Properties.EnemySpriteResources.PipiAndCopipi_CutMansBadScissorsDay,
            Properties.EnemySpriteResources.PipiAndCopipi_NotTheBees,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_PRESS = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Press_Crusher2,
            Properties.EnemySpriteResources.Press_FlatCrusher,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_ROBBIT = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Robbit_MegaMan1Joe,
            Properties.EnemySpriteResources.Robbit_MetalManRabbit,
            Properties.EnemySpriteResources.Robbit_Shrek,
            Properties.EnemySpriteResources.Robbit_Turtle,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_SCWORM = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Scworm_CutMansBadScissorsDay,
            Properties.EnemySpriteResources.Scworm_FlameThrow,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_SHOTMAN = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Shotman_CutMansBadScissorsDay,
            Properties.EnemySpriteResources.Shotman_Turret,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_SHRINK = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Shrink_MegaMan1Watcher,
            Properties.EnemySpriteResources.Shrink_Metall,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_SNIPER_JOE_AND_SNIPER_ARMOR = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.SniperJoeAndSniperArmor_MegaMan1ContructionJoe,
            Properties.EnemySpriteResources.SniperJoeAndSniperArmor_ProtoJoe,
            Properties.EnemySpriteResources.SniperJoeAndSniperArmor_ProtoJoeAndMech,
            Properties.EnemySpriteResources.SniperJoeAndSniperArmor_SniperMet,
            Properties.EnemySpriteResources.SniperJoeAndSniperArmor_SpecialJoe,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_SPRINGER = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Springer_BMSpringer,
            Properties.EnemySpriteResources.Springer_CuteSpringer,
            Properties.EnemySpriteResources.Springer_MegaMan6Springer,
            Properties.EnemySpriteResources.Springer_Saw,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_TANISHI = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Tanishi_HatCrab,
            Properties.EnemySpriteResources.Tanishi_MetCrab,
        };

        private static readonly List<Byte[]?> ENEMY_SPRITE_LIST_TELLY = new List<Byte[]?>()
        {
            null,
            Properties.EnemySpriteResources.Telly_EyeTelly,
            Properties.EnemySpriteResources.Telly_Telly2,
        };

        private static readonly List<List<Byte[]?>> ENEMY_SPRITE_COLLECTION = new List<List<Byte[]?>>()
        {
            ENEMY_SPRITE_LIST_BATTON,
            ENEMY_SPRITE_LIST_BLOCKY,
            ENEMY_SPRITE_LIST_CHANGKEY_MAKER,
            ENEMY_SPRITE_LIST_CLAW,
            ENEMY_SPRITE_LIST_FLY_BOY,
            ENEMY_SPRITE_LIST_FORCE_BEAM_BODY,
            ENEMY_SPRITE_LIST_FORCE_BEAM_END,
            ENEMY_SPRITE_LIST_FRIENDER,
            ENEMY_SPRITE_LIST_KAMINARI_GORO,
            ENEMY_SPRITE_LIST_KEROG_AND_PETIT_KEROG,
            ENEMY_SPRITE_LIST_KUKKU,
            ENEMY_SPRITE_LIST_M445,
            ENEMY_SPRITE_LIST_MATASABURO,
            ENEMY_SPRITE_LIST_MOLE,
            ENEMY_SPRITE_LIST_MONKING,
            ENEMY_SPRITE_LIST_NEO_METALL,
            ENEMY_SPRITE_LIST_PIEROBOT,
            ENEMY_SPRITE_LIST_PIPI_AND_COPIPI,
            ENEMY_SPRITE_LIST_PRESS,
            ENEMY_SPRITE_LIST_ROBBIT,
            ENEMY_SPRITE_LIST_SCWORM,
            ENEMY_SPRITE_LIST_SHOTMAN,
            ENEMY_SPRITE_LIST_SHRINK,
            ENEMY_SPRITE_LIST_SNIPER_JOE_AND_SNIPER_ARMOR,
            ENEMY_SPRITE_LIST_SPRINGER,
            ENEMY_SPRITE_LIST_TANISHI,
            ENEMY_SPRITE_LIST_TELLY,
        };
    }
}
