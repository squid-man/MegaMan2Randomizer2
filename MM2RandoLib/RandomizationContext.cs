using js65;
using MM2RandoLib;
using MM2RandoLib.Settings.Options;
using MM2RandoLib.Utilities;
using MM2Randomizer.Enums;
using MM2Randomizer.Extensions;
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MM2Randomizer
{
    public class RandomizationContext
    {
        internal record OptionAction(IOption Option, OptionActionAttribute Action);

        internal record WriteSpec(int Offs, IReadOnlyList<byte> Data);
        internal record BlockCopySpec(int SrcOffs, int TgtOffs, int Size);

        //
        // Constructors
        //

        internal RandomizationContext(
            RandomizationSettings in_Settings, 
            ISeed in_Seed, 
            IPlatformServices in_PlatformServices, 
            byte[] rom,
            IProgress<string?> in_Progress,
            CancellationToken in_CancellationToken)
        {
            this.Seed = in_Seed;
            this.Settings = in_Settings;
            // Create file name based on seed and game region
            this.FileName = $"MM2-RNG-{in_Seed.Identifier} ({in_Seed.SeedString}).nes";

            AsmRoot = ResourceTree.Find("Asm");
            AsmIncBase = ResourceTree.LoadUtf8Resource(
                AsmRoot.Find("mm2r_base.inc"));

            PlatformServices = in_PlatformServices;
            Assembler = CreateAssemblyEngine();

            SourceRom = rom;
            PrepatchRom = Array.Empty<byte>();
            Rom = rom.ToArray();

            Progress = in_Progress;
            CancellationToken = in_CancellationToken;
        }

        //
        // Properties
        //

        public IPlatformServices PlatformServices { get; }

        public ISeed Seed { get; private set; }

        public RandomizationSettings Settings { get; private set; }

        public String FileName { get; private set; }

        public byte[] SourceRom { get; }
        public byte[] PrepatchRom { get; private set; }
        public byte[] Rom { get; private set; }

        public IProgress<string?> Progress { get; }

        public CancellationToken CancellationToken { get; }

        // Create randomization patch
        public Patch Patch { get; private set; } = new Patch();

        public ResourceTree ResourceTree { get; } = new(null, "Resources");

        /// <summary>
        /// Quality of life hack: if InvisiPico, do NOT randomize Pico movement.
        /// </summary>
        public bool IsInvisiPico = false;

        //
        // Options System and Assembler Properties
        //

        private readonly Js65Options AssemblerOptions = new()
        {
            // Must exist for the FileResolve callback to be called
            includePaths = [""],
        };

        public readonly Assembler Assembler;
        public readonly ResourceNode AsmRoot;
        public readonly string AsmIncBase;
        public readonly List<string> DefineSymbolLines = new();

        public readonly IReadOnlySet<string> SpecialAsmPaths = new HashSet<string> { "mm2r.inc", "original.nes", "prepatch.nes" };


        public readonly Dictionary<IOption, Dictionary<ResourceNode, ResourceNode?>> OneIpsPerDirSelections = new(ReferenceEqualityComparer.Instance);

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

        internal async Task Initialize()
        {
            // These steps complete so fast only two messages ever have time to display
            UpdateProgressAndCheckCancellation("Performing randomization...");

            await CreateInitialRom();

            CheckCancellation();

            // Not certain whether this must come first
            AsmModuleFromResource("config.asm");

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

            MiscHacks.FindAllPatchesWithCommonBankChanges(ResourceTree);
#endif

            ParseOptionActions(false);

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

            // Boss sprites need to be randomized before running normal randomizers because InvisiPico is a special case
            if (spriteOpts.RandomizeBossSprites.Value)
            {
                var bossPatches = MiscHacks.ApplyOneIpsPerDir(
                    this, "SpritePatches.Bosses");
                var bossSprites = bossPatches
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

            ApplyOptionActions();


            ///==========================
            /// "COSMETIC SEED" MODULES
            ///==========================

            // NOTE: Reset the seed for cosmetic options
            this.Seed.Reset();

            this.Settings.ActualizeCosmeticSettings(this.Seed);
            var cosmOpts = Settings.CosmeticOptions;

            ParseOptionActions(true);

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


            // ================================================
            // No randomization after this point, only patching
            // ================================================

            UpdateProgressAndCheckCancellation("Applying changes...");

            // Apply additional required incidental modifications
            if (gameplayOpts.RandomizeRobotMasterStageSelection.Value ||
                gameplayOpts.RandomizeSpecialWeaponReward.Value)
            {
                MiscHacks.FixPortraits(
                    this.Patch,
                    gameplayOpts.RandomizeRobotMasterStageSelection.Value,
                    this.RandomStages,
                    gameplayOpts.RandomizeSpecialWeaponReward.Value,
                    this.RandomWeaponGet);
            }

            // Apply final optional gameplay modifications
            MiscHacks.DrawTitleScreenChanges(this.Patch, this.Seed.Identifier, this.Settings);

            MiscHacks.SetNewMegaManSprite(
                ResourceTree,
                this.Patch,
                Rom,
                cosmOpts.PlayerSprite.Value);

            MiscHacks.SetNewCannonShot(
                ResourceTree,
                this.Patch,
                Rom,
                cosmOpts.CannonShot.Value);

            MiscHacks.SetNewHudElement(
                ResourceTree,
                this.Patch,
                Rom,
                cosmOpts.HudElement.Value);

            MiscHacks.SetNewFont(
                ResourceTree,
                this.Patch,
                Rom,
                cosmOpts.Font.Value);
            MiscHacks.AddLargeWeaponEnergyRefillPickupsToWily5TeleporterRoom(this.Patch);

            ApplyOptionActions();

            await CompileAssembly();

            // Apply patch with randomized content
            this.Patch.ApplyRandoPatch(Rom);
        }

        /// <summary>
        /// Perform 0 or more copy and subsequent write actions to the ROM. This is done outside the normal patching system because it must come before other IPS files which directly modify the ROM.
        /// </summary>
        /// <param name="in_CopySpecs">Copy operations to perform.</param>
        /// <param name="in_WriteSpecs">Write operations to perform.</param>
        internal void ModifyRomFile(
            IEnumerable<BlockCopySpec>? in_CopySpecs = null,
            IEnumerable<WriteSpec>? in_WriteSpecs = null)
        {
            var srcRom = Rom.ToArray();

            if (in_CopySpecs is not null)
            {
                foreach (var spec in in_CopySpecs)
                    Array.Copy(srcRom, spec.SrcOffs, Rom, spec.TgtOffs, spec.Size);
            }

            if (in_WriteSpecs is not null)
            {
                foreach (var spec in in_WriteSpecs)
                {
                    byte[]? buff = spec.Data as byte[];
                    if (buff is null)
                        buff = spec.Data.ToArray();

                    Array.Copy(buff, 0, Rom, spec.Offs, buff.Length);
                }
            }
        }

        /// <summary>
        /// Apply the base patches to the ROM that must come before anything else including other IPS files.
        /// </summary>
        private async Task CreateInitialRom()
        {
            // Apply pre-patch changes via IPS patch (manual title screen, stage select, stage changes, player sprite)
            using (MemoryStream stream = new())
            {
                stream.Write(Rom);

                this.Patch.ApplyIPSPatch(
                    stream, ResourceTree.LoadResource("mm2ft.ips"), false);

                Rom = stream.ToArray();
            }

            CopyWilyTilesets();

            var asm = CreateAssemblyEngine();

            AsmModuleFromResource("config.asm", asm);
            AsmModuleFromResource("prepatch.asm", asm);

            PrepatchRom = await asm.ApplyAsync(Rom).WaitAsync(CancellationToken);
            Rom = PrepatchRom.ToArray();
        }

    /// <summary>
    /// Duplicate the portions of the tileset that are shared between Wily 2-5. Makes the following modifications:
    /// Wily 3's copy of 6a10:6e10 (PPU 1600:1a00) is now located at 3f610.
    /// Wily 4's copy of 6a10:6e10 is now located at 3fa10.
    /// Wily 5's copy of ac10:ae10 (PPU 1200:1400) is now located at 3fe10.
    /// </summary>
    private void CopyWilyTilesets()
        {
            /* All stages have a list of regions to copy to VRAM at start (this includes both sprites at PPU 0:1000 and background at 1000:2000). For Wily 1-6 these lists are at bd00 of bank # - 1. The first byte of each list specifies the number of entries, and each entry is a byte triplet AA NN BB where A is the MSB of the ROM address to copy from, N is the number of 256-byte blocks to copy, and B is the 16 KB ROM bank number.
             * The vanilla values of these tables for the background portion, with * and # indicating the portions that need to be duplicated:
             * Wily 1 @+10: 80 06 09  AA 0A 00
             * Wily 2 @+13: 80 06 09  AA 0A 01*
             * WIly 3 @ +d: 80 06 09  AA 04 01* AA 05 02# AC 01 02#
             * Wily 4 @ +d: 80 06 09  AA 04 01* AA 06 03
             * Wily 5 @ +d: 80 02 09  AC 02 02# 84 01 09  AA 01 04  AA 0A 04
             * Wily 6 @ +d: 80 08 09  B0 08 02 */

            ModifyRomFile(
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

        /// <summary>
        /// Process the list of option actions and, for all applicable to the selected settings, either execute them if they can be executed immediately with no adverse effects or queue them for execution at the proper time. The primary purpose of this is to delay assembly as long as possible to allow the most symbols to be defined, until js65's implementation of .include is fixed.
        /// </summary>
        private void ParseOptionActions(bool cosmOpts)
        {
            foreach (var opt in Settings.OptionsWithActions)
            {
                if (opt.Info!.IsCosmetic != cosmOpts)
                    continue;

                foreach (var act in opt.Info!.Actions)
                {
                    if (act is OptionValueActionAttribute valAct)
                    {
                        object? cmpValue = valAct.OptionValue;
                        if (cmpValue is null)
                        {
                            Debug.Assert(opt is BoolOption);

                            cmpValue = true;
                        }

                        if (!cmpValue.Equals(opt.Value))
                            continue;

                        if (act is DefineSymbolAttribute symAct)
                        {
                            // Define can be executed immediately
                            string valueStr = symAct.Value is not null
                                ? $" ${symAct.Value:x}"
                                : "";
                            DefineSymbolLines.Add(
                                $".define {symAct.SymbolName}{valueStr}");
                        }
                        else if (act is AssembleFileAttribute asmAct)
                            AsmModuleFromResource(AsmRoot.Find(asmAct.Path));
                        else if (act is PatchRomAttribute patchAct)
                        {
                            // Patches are inherently deferred
                            Patch.Add(
                                patchAct.RomOffset,
                                patchAct.Data,
                                $"Option '{opt.Info.Name}' '{opt.Value}' PatchRom action");
                        }
                        else
                            OptActsQueue.Add(new(opt, act));
                    }
                    else
                    {
                        if (act is DefineValueSymbolAttribute symAct)
                        {
                            DefineSymbolLines.Add($".define {symAct.SymbolName} ${(int)opt.Value:x}");
                        }
                        else if (act is WriteValueToRomAttribute writeAct)
                        {
                            // Patches are inherently deferred
                            var type = opt.Info.Type;
                            int value = (int)opt.Value;
                            Patch.Add(
                                writeAct.RomOffset,
                                checked((byte)value),
                                $"Option '{opt.Info.Name}' '{opt.Value}' WriteValueToRom action");
                        }
                        else
                            OptActsQueue.Add(new(opt, act));
                    }
                }
            }

            return;
        }

        private void ApplyOptionActions()
        {
            foreach (var (opt, act) in OptActsQueue)
            {
                if (act is OptionValueActionAttribute valAct)
                {
                    if (act is ApplyOneIpsPerDirAttribute opdAct)
                    {
                        var selFiles = MiscHacks.ApplyOneIpsPerDir(
                            ResourceTree,
                            Seed,
                            Patch,
                            opdAct.RootPath,
                            opdAct.AllowNone,
                            Rom,
                            opdAct.RebaseIps is not null ? (bool)opdAct.RebaseIps : null);
                        OneIpsPerDirSelections[opt] = selFiles;
                    }
                    else
                        Debug.Assert(false);
                }
                else
                {
                    Debug.Assert(false);
                }
            }

            OptActsQueue.Clear();

            return;
        }

        private Assembler CreateAssemblyEngine()
        {
            var asm = PlatformServices.CreateAssembler(AssemblerOptions, false);
            asm.Callbacks = new Js65Callbacks
            {
                OnFileReadText = AsmFileReadTextCallback,
                OnFileReadBinary = AsmFileReadBinaryCallback
            };

            return asm;
        }

        private async Task CompileAssembly()
        {
            // Setup the obligatory assembly modules
            foreach (var node in AsmRoot.Find("Auto").Files)
                AsmModuleFromResource(node);

            // And compile
            Rom = await Assembler.ApplyAsync(Rom).WaitAsync(CancellationToken);
        }

        private string AsmFileReadTextCallback(string basePath, string path)
        {
            if (basePath == "")
            {
                if (path == "mm2r.inc")
                    return string.Join(
                        "\n", DefineSymbolLines.Append(AsmIncBase));
            }

            throw new FileNotFoundException();
        }

        private byte[] AsmFileReadBinaryCallback(string basePath, string path)
        {
            if (basePath == "")
            {
                if (path == "original.nes")
                    return SourceRom;
                //// Not sure this is a good idea
                else if (path == "prepatch.nes")
                    return PrepatchRom;
            }

            throw new FileNotFoundException();
        }

        public AsmModule AsmModuleFromResource(
            ResourceNode node, Assembler? asm = null)
        {
            if (asm is null)
                asm = Assembler;

            var mod = asm.Module();
            mod.Code(ResourceTree.LoadUtf8Resource(node), node.Path);

            return mod;
        }

        public AsmModule AsmModuleFromResource(
            string path, 
            Assembler? asm = null)
            => AsmModuleFromResource(AsmRoot.Find(path), asm);

        private void CheckCancellation()
        {
            if (CancellationToken.IsCancellationRequested)
                throw new OperationCanceledException();
        }

        private void UpdateProgressAndCheckCancellation(string progress)
        {
            Progress.Report(progress);

            CheckCancellation();
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

        private readonly List<OptionAction> OptActsQueue = new();
    }
}
