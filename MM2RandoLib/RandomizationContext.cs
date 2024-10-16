using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;
using MM2Randomizer.Randomizers;
using MM2Randomizer.Randomizers.Colors;
using MM2Randomizer.Randomizers.Enemies;
using MM2Randomizer.Randomizers.Stages;
using MM2Randomizer.Resources;
using MM2Randomizer.Settings;
using MM2Randomizer.Settings.OptionGroups;
using MM2Randomizer.Settings.Options;
using MM2Randomizer.Utilities;

namespace MM2Randomizer
{
    public class RandomizationContext
    {
        internal record WriteSpec(int Offs, IReadOnlyList<byte> Data);
        internal record BlockCopySpec(int SrcOffs, int TgtOffs, int Size);

        //
        // Constructors
        //

        internal RandomizationContext(RandomizationSettings in_Settings, ISeed in_Seed)
        {
            this.Seed = in_Seed;
            this.Settings = in_Settings;
            // Create file name based on seed and game region
            this.FileName = $"MM2-RNG-{in_Seed.Identifier} ({in_Seed.SeedString}).nes";
        }


        //
        // Properties
        //

        public ISeed Seed { get; private set; }

        public RandomizationSettings Settings { get; private set; }

        public String FileName { get; private set; }

        // Create randomization patch
        public Patch Patch { get; private set; } = new Patch();

        public ResourceTree ResourceTree { get; } = new(null, "Resources");

        public const String TEMPORARY_FILE_NAME = "temp.nes";

        /// <summary>
        /// Quality of life hack: if InvisiPico, do NOT randomize Pico movement.
        /// </summary>
        public bool IsInvisiPico = false;

        //================
        // "CORE" MODULES
        //================

        // NOTE: Just in case, link RStages, RWeaponGet, and RTeleporter into one "Core Randomizer" module
        // Their interdependencies are too risky to separate as options, and likely nobody will want to customize this part anyways.
        // Random portrait locations on stage select
        public RStages RandomStages { get; private set; } = new RStages();
        // Random weapon awarded from each stage
        // WARNING: May be dependent on RTeleporters, verify?
        // WARNING: May be dependent on RStages
        public RWeaponGet RandomWeaponGet { get; private set; } = new RWeaponGet();
        // Random teleporter destinations in Wily 5
        public RTeleporters RandomTeleporters { get; private set; } = new RTeleporters();


        ///=========================
        /// "GAMEPLAY SEED" MODULES
        ///=========================

        // Caution: RWeaknesses depends on this
        public RWeaponBehavior RandomWeaponBehavior { get; private set; } = new RWeaponBehavior();
        // Depends on RWeaponBehavior (ammo), can use default values
        public RWeaknesses RandomWeaknesses { get; private set; } = new RWeaknesses();
        // Independent
        public RBossAI RandomBossAI { get; private set; } = new RBossAI();
        // Independent
        public RItemGet RandomItemGet { get; private set; } = new RItemGet();
        // Independent
        public REnemies RandomEnemies { get; private set; } = new REnemies();
        // Independent
        public REnemyWeaknesses RandomEnemyWeakness { get; private set; } = new REnemyWeaknesses();
        // Caution: RText depends on this, but default values will be used if not enabled.
        public RBossRoom RandomBossInBossRoom { get; private set; } = new RBossRoom();
        // Independent
        public RTilemap RandomTilemap { get; private set; } = new RTilemap();


        ///==========================
        /// "COSMETIC SEED" MODULES
        ///==========================

        // Independent
        public RColors RandomColors { get; private set; } = new RColors();
        // Independent
        public RMusic RandomMusic { get; private set; } = new RMusic();
        // Caution: Depends on RBossRoom, but can use default values if its not enabled.
        public RText RandomInGameText { get; private set; } = new RText();


        //
        // Internal Methods
        //

        internal void Initialize()
        {
            CreateInitialRom(TEMPORARY_FILE_NAME);

            // In tournament mode, offset the seed by 1 call, making seeds mode-dependent
            /*
            if (this.Settings.EnableSpoilerFreeMode)
            {
                this.Seed.Next();
            }
            */


            // Add randomizers according to each flag
            ///==========================
            /// "GAMEPLAY SEED" MODULES
            ///==========================

            Settings.ActualizeBehaviorSettings(this.Seed);
            var gameplayOpts = Settings.GameplayOptions;
            var spriteOpts = Settings.SpriteOptions;
            var chargingOpts = Settings.ChargingSpeedOptions;
            var qolOpts = Settings.QualityOfLifeOptions;

#if DEBUG
            ResourceTree.TestFind();
            ResourceTree.TestPaths();
#endif

            Dictionary<ResourceNode, ResourceNode?>? bossPatches = null,
                enemySprites = null,
                envSprites = null,
                screenPatches = null,
                pickupPatches = null,
                weaponSprites = null;
            Dictionary<EBossIndex, ResourceNode?>? bossSprites = null;

            // List of randomizer modules to use; will add modules based on checkbox states
            List<IRandomizer> randomizers = new List<IRandomizer>();
            if (gameplayOpts.RandomizeRobotMasterStageSelection.Value)
            {
                randomizers.Add(this.RandomStages);
            }

            if (gameplayOpts.RandomizeSpecialWeaponReward.Value)
            {
                randomizers.Add(this.RandomWeaponGet);
            }

            if (gameplayOpts.RandomizeSpecialWeaponBehavior.Value)
            {
                randomizers.Add(this.RandomWeaponBehavior);
            }

            if (gameplayOpts.RandomizeBossWeaknesses.Value)
            {
                randomizers.Add(this.RandomWeaknesses);
            }

            if (gameplayOpts.RandomizeRobotMasterBehavior.Value)
            {
                randomizers.Add(this.RandomBossAI);
            }

            if (gameplayOpts.RandomizeSpecialItemLocations.Value)
            {
                randomizers.Add(this.RandomItemGet);
            }

            if (gameplayOpts.RandomizeRefightTeleporters.Value)
            {
                randomizers.Add(this.RandomTeleporters);
            }

            if (gameplayOpts.RandomizeEnemySpawns.Value)
            {
                randomizers.Add(this.RandomEnemies);
            }

            if (gameplayOpts.RandomizeEnemyWeaknesses.Value)
            {
                randomizers.Add(this.RandomEnemyWeakness);
            }

            if (gameplayOpts.RandomizeRobotMasterLocations.Value)
            {
                randomizers.Add(this.RandomBossInBossRoom);
            }

            if (gameplayOpts.RandomizeFalseFloors.Value)
            {
                randomizers.Add(this.RandomTilemap);
            }

            // Apply random sprite changes
            // Boss sprites need to be randomized before running normal randomizers because InvisiPico is a special case
            if (spriteOpts.RandomizeBossSprites.Value)
            {
                bossPatches = MiscHacks.ApplyOneIpsPerDir(
                    this, "SpritePatches.Bosses");
                bossSprites = bossPatches
                    .Where(kv => BossDirNames.ContainsKey(kv.Key.Name))
                    .ToDictionary(kv => BossDirNames[kv.Key.Name], kv => kv.Value);

                // Very hacky. But I'm not sure what a better way to do it would be.
                var picoNode = bossSprites[EBossIndex.Pico];
                IsInvisiPico = picoNode is not null && picoNode.Name.Contains(
                    "CheatMode", StringComparison.InvariantCultureIgnoreCase);
            }

            // Conduct randomization of behavior options
            foreach (IRandomizer randomizer in randomizers)
            {
                randomizer.Randomize(this.Patch, this);
                Debug.WriteLine(randomizer);
            }

            if (spriteOpts.RandomizeEnemySprites.Value)
            {
                enemySprites = MiscHacks.ApplyOneIpsPerDir(
                    this, "SpritePatches.Enemies");
            }

            if (spriteOpts.RandomizeSpecialWeaponSprites.Value)
            {
                weaponSprites = MiscHacks.ApplyOneIpsPerDir(
                    this, "SpritePatches.Weapons");
            }

            if (spriteOpts.RandomizeItemPickupSprites.Value)
            {
                pickupPatches = MiscHacks.ApplyOneIpsPerDir(
                    this, "SpritePatches.Pickups");
            }

            if (spriteOpts.RandomizeEnvironmentSprites.Value)
            {
                envSprites = MiscHacks.ApplyOneIpsPerDir(
                    this, "SpritePatches.Environment", rebasePatch: false);
            }


            ///==========================
            /// "COSMETIC SEED" MODULES
            ///==========================

            // NOTE: Reset the seed for cosmetic options
            this.Seed.Reset();

            this.Settings.ActualizeCosmeticSettings(this.Seed);
            var cosmOpts = Settings.CosmeticOptions;

            // List of randomizer modules to use; will add modules based on checkbox states
            List<IRandomizer> cosmeticRandomizers = new List<IRandomizer>();

            if (cosmOpts.RandomizeColorPalettes.Value)
            {
                cosmeticRandomizers.Add(this.RandomColors);
            }

            if (cosmOpts.RandomizeMusicTracks.Value)
            {
                cosmeticRandomizers.Add(this.RandomMusic);
            }

            if (cosmOpts.RandomizeInGameText.Value)
            {
                cosmeticRandomizers.Add(this.RandomInGameText);
            }

            // Conduct randomization of Cosmetic Modules
            foreach (IRandomizer cosmetic in cosmeticRandomizers)
            {
                cosmetic.Randomize(this.Patch, this);
                Debug.WriteLine(cosmetic);
            }

            // Apply random sprite changes
            if (cosmOpts.RandomizeMenusAndTransitionScreens.Value)
            {
                screenPatches = MiscHacks.ApplyOneIpsPerDir(
                    this, "SpritePatches.MenusAndTransitionScreens");
            }


            // ================================================
            // No randomization after this point, only patching
            // ================================================

            // Apply additional required incidental modifications
            if (gameplayOpts.RandomizeRobotMasterStageSelection.Value ||
                cosmOpts.RandomizeInGameText.Value)
            {
                MiscHacks.FixPortraits(
                    this.Patch,
                    gameplayOpts.RandomizeRobotMasterStageSelection.Value,
                    this.RandomStages,
                    cosmOpts.RandomizeInGameText.Value,
                    this.RandomWeaponGet);

                MiscHacks.FixWeaponLetters(
                    this.Patch,
                    this.RandomWeaponGet,
                    this.RandomStages,
                    this.RandomInGameText);
            }

            if (gameplayOpts.RandomizeEnemySpawns.Value)
            {
                MiscHacks.FixM445PaletteGlitch(this.Patch);
            }

            // Apply final optional gameplay modifications
            if (gameplayOpts.FasterCutsceneText.Value)
            {
                MiscHacks.SetFastWeaponGetText(this.Patch);
                MiscHacks.SetFastReadyText(this.Patch);
                MiscHacks.SetFastWilyMap(this.Patch);
                MiscHacks.SkipItemGetPages(this.Patch);
            }

            if (gameplayOpts.BurstChaserMode.Value)
            {
                MiscHacks.SetBurstChaser(this.Patch);
            }

            if (qolOpts.DisableFlashingEffects.Value)
            {
                MiscHacks.DisableScreenFlashing(
                    this.Patch,
                    gameplayOpts.FasterCutsceneText.Value,
                    cosmOpts.RandomizeColorPalettes.Value);
            }

            MiscHacks.SetHitPointChargingSpeed(
                this.Patch,
                chargingOpts.HitPoints.Value);

            MiscHacks.SetWeaponEnergyChargingSpeed(
                this.Patch,
                chargingOpts.WeaponEnergy.Value);

            // PreventETankUseAtFullLife must be applied before SetEnergyTankChargingSpeed
            MiscHacks.PreventETankUseAtFullLife(this.Patch);
            MiscHacks.SetEnergyTankChargingSpeed(
                this.Patch,
                chargingOpts.EnergyTank.Value);

            MiscHacks.SetRobotMasterEnergyChargingSpeed(
                this.Patch,
                chargingOpts.RobotMasterEnergy.Value);

            MiscHacks.SetCastleBossEnergyChargingSpeed(
                this.Patch,
                chargingOpts.CastleBossEnergy.Value);

            MiscHacks.DrawTitleScreenChanges(this.Patch, this.Seed.Identifier, this.Settings);
            MiscHacks.SetWily5NoMusicChange(this.Patch);
            MiscHacks.NerfDamageValues(this.Patch);
            MiscHacks.SetETankKeep(this.Patch);
            MiscHacks.SetFastBossDefeatTeleport(this.Patch);


            if (qolOpts.EnableUnderwaterLagReduction.Value)
            {
                MiscHacks.ReduceUnderwaterLag(this.Patch);
            }

            if (qolOpts.DisableWaterfall.Value)
            {
                this.Patch.Add(0xFE10, (byte)1, "Disable Bubble Man stage palette animation");
            }

            if (qolOpts.EnableLeftwardWallEjection.Value)
            {
                MiscHacks.EnableLeftwardWallEjection(ResourceTree, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (qolOpts.DisablePauseLock.Value)
            {
                MiscHacks.DisablePauseLock(ResourceTree, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (gameplayOpts.MercilessMode.Value)
            {
                MiscHacks.EnableMercilessMode(ResourceTree, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (qolOpts.EnableBirdEggFix.Value)
            {
                MiscHacks.EnableBirdEggFix(ResourceTree, this.Patch, RandomizationContext.TEMPORARY_FILE_NAME);
            }

            if (qolOpts.StageSelectDefault.Value)
            {
                MiscHacks.MakeStageSelectDefault(Patch);
            }

            MiscHacks.SetNewMegaManSprite(
                ResourceTree,
                this.Patch,
                RandomizationContext.TEMPORARY_FILE_NAME,
                cosmOpts.PlayerSprite.Value);

            MiscHacks.SetNewHudElement(
                ResourceTree,
                this.Patch,
                RandomizationContext.TEMPORARY_FILE_NAME,
                cosmOpts.HudElement.Value);

            MiscHacks.SetNewFont(
                ResourceTree,
                this.Patch,
                RandomizationContext.TEMPORARY_FILE_NAME,
                cosmOpts.Font.Value);


            // Modify the Wily 5 game loop so that large weapon energy refills
            // can be spawned in the refight teleporter room
            MiscHacks.AddWily5SubroutineWithItemSpawns(this.Patch);
            MiscHacks.AddLargeWeaponEnergyRefillPickupsToWily5TeleporterRoom(this.Patch);

            // Apply patch with randomized content
            this.Patch.ApplyRandoPatch(RandomizationContext.TEMPORARY_FILE_NAME);

            // If a file of the same seed already exists, delete it
            if (File.Exists(this.FileName))
            {
                File.Delete(this.FileName);
            }

            // Finish the copy/rename and open Explorer at that location
            File.Move(RandomizationContext.TEMPORARY_FILE_NAME, this.FileName);
        }

        /// <summary>
        /// Perform 0 or more copy and subsequent write actions to the ROM file. This is done outside the normal patching system because it must come before other IPS files which directly modify the ROM.
        /// </summary>
        /// <param name="in_RomPath"></param>
        /// <param name="in_CopySpecs">Copy operations to perform.</param>
        /// <param name="in_WriteSpecs">Write operations to perform.</param>
        internal void ModifyRomFile(
            string in_RomPath, 
            IEnumerable<BlockCopySpec>? in_CopySpecs = null,
            IEnumerable<WriteSpec>? in_WriteSpecs = null)
        {
            using (FileStream romFile = new(
                in_RomPath, FileMode.Open, FileAccess.ReadWrite))
            {
                var rom = new byte[romFile.Length];
                romFile.ReadExactly(rom, 0, rom.Length);

                if (in_CopySpecs is not null)
                {
                    foreach (var spec in in_CopySpecs)
                    {
                        romFile.Position = spec.TgtOffs;
                        romFile.Write(rom, spec.SrcOffs, spec.Size);
                    }
                }

                if (in_WriteSpecs is not null)
                {
                    foreach (var spec in in_WriteSpecs)
                    {
                        romFile.Position = spec.Offs;

                        byte[]? buff = spec.Data as byte[];
                        if (buff is null)
                            buff = spec.Data.ToArray();

                        romFile.Write(buff, 0, buff.Length);
                    }
                }
            }
        }

        /// <summary>
        /// Apply the base patches to the ROM that must come before anything else including other IPS files.
        /// </summary>
        private void CreateInitialRom(string in_RomPath)
        {
            File.Copy(this.Settings.RomSourcePath, in_RomPath, true);

            // Apply pre-patch changes via IPS patch (manual title screen, stage select, stage changes, player sprite)
            this.Patch.ApplyIPSPatch(
                in_RomPath, ResourceTree.LoadResource("mm2ft.ips"), false);
            this.Patch.ApplyIPSPatch(
                in_RomPath, ResourceTree.LoadResource("mm2rng_prepatch.ips"));

            CopyWilyTilesets(in_RomPath);
        }

        /// <summary>
        /// Duplicate the portions of the tileset that are shared between Wily 2-5. Makes the following modifications:
        /// Wily 3's copy of 6a10:6e10 (PPU 1600:1a00) is now located at 3f610.
        /// Wily 4's copy of 6a10:6e10 is now located at 3fa10.
        /// Wily 5's copy of ac10:ae10 (PPU 1200:1400) is now located at 3fe10.
        /// </summary>
        private void CopyWilyTilesets(string in_RomPath)
        {
            /* All stages have a list of regions to copy to VRAM at start (this includes both sprites at PPU 0:1000 and background at 1000:2000). For Wily 1-6 these lists are at bd00 of bank # - 1. The first byte of each list specifies the number of entries, and each entry is a byte triplet AA NN BB where A is the MSB of the ROM address to copy from, N is the number of 256-byte blocks to copy, and B is the 16 KB ROM bank number.
             * The vanilla values of these tables for the background portion, with * and # indicating the portions that need to be duplicated:
             * Wily 1 @+10: 80 06 09  AA 0A 00
             * Wily 2 @+13: 80 06 09  AA 0A 01*
             * WIly 3 @ +d: 80 06 09  AA 04 01* AA 05 02# AC 01 02#
             * Wily 4 @ +d: 80 06 09  AA 04 01* AA 06 03
             * Wily 5 @ +d: 80 02 09  AC 02 02# 84 01 09  AA 01 04  AA 0A 04
             * Wily 6 @ +d: 80 08 09  B0 08 02 */

            ModifyRomFile(in_RomPath,
                [
                    new(0x6a10, 0x3f610, 0x400), // Wily 3 copy of 2's data
                    new(0x6a10, 0x3fa10, 0x400), // Wily 4 copy of 2's data
                    new(0xac10, 0x3fe10, 0x200), // Wily 5 copy of 3's data
                ],
                [
                    new(0x8010 + 0x3d10, Convert.FromHexString("b6040f")), // Wily 3 ref
                    new(0xc010 + 0x3d10, Convert.FromHexString("ba040f")), // Wily 4 ref
                    new(0x10010 + 0x3d10, Convert.FromHexString("be020f")), // Wily 5 ref
                ]);
        }

        //
        // Private Data Members
        //

        private static readonly Dictionary<string, EBossIndex> BossDirNames = new()
        {
            { "AirMan", EBossIndex.Air },
            { "Alien", EBossIndex.Alien },
            { "BoobeamTrap", EBossIndex.Boobeam },
            { "BubbleMan", EBossIndex.Bubble },
            { "CrashMan", EBossIndex.Crash },
            { "FlashMan", EBossIndex.Flash },
            { "GutsTank", EBossIndex.Guts },
            { "HeatMan", EBossIndex.Heat },
            { "MechaDragon", EBossIndex.Dragon },
            { "MetalMan", EBossIndex.Metal },
            { "PicopicoKun", EBossIndex.Pico },
            { "QuickMan", EBossIndex.Quick },
            { "WoodMan", EBossIndex.Wood },
        };
    }
}
