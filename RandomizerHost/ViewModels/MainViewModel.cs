using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;
using MM2RandoLib;
using MM2RandoLib.Settings.Options;
using MM2RandoLib.Utilities;
using MM2Randomizer;
using MM2Randomizer.Extensions;
using MM2Randomizer.Settings;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using RandomizerHost.Settings;
using RandomizerHost.Views;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Hashing;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RandomizerHost.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        //
        // Constructor
        //

        public MainViewModel(IHostPlatformServices libPlatformServices)
        {
            // Need to defer loading settings until an async context

            PlatformServices = libPlatformServices;
            AppConfigurationSettings = new();
            Settings = AppConfigurationSettings.RandomizationSettings;
            SettingsPresets = new(Settings);

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            string verStr = version?.ToString() ?? "";

            if (!GitInfo.IsOfficialBuild)
            {
                string branch = GitInfo.Branch, cmtSuff = "", dbgSuff = "";
                if (!GitInfo.IsDirty)
                    cmtSuff = $":{GitInfo.Commit}";

#if DEBUG
                dbgSuff += " (Debug)";
#endif

                verStr += $" EXPERIMENTAL [{branch}{cmtSuff}]{dbgSuff}";
            }
            else if (verStr.Length == 0)
                verStr = "Unknown";

            Version = verStr;

            // These need to use Switch/WhenAnyValue because AppConfigurationSettings will change when settings are imported
            var cfgObs = this.WhenAnyValue(vm => vm.AppConfigurationSettings);
            cfgObs.Subscribe(OnAppConfigurationSettingsChanged);
            cfgObs.SwitchSubscribe(
                c => c.WhenAnyValue(c => c.SettingsPresetIndex),
                OnSettingsPresetIndexChanged);
            cfgObs.SwitchSelect(c => c.WhenAnyValue(c => c.SeedString))
                .Select(s => IsValidSeed(s))
                .ToProperty(this, vm => vm.IsSeedValid, out _isSeedValid);
        }

        public async Task OnViewCreated(Visual view)
        {
            // First time there is a storage provider that can be used
            var top = TopLevel.GetTopLevel(view);
            var stg = top!.StorageProvider;

            var settingsData = await PlatformServices.LoadSettingsData();
            if (settingsData != null)
            {
                AppConfigurationSettings = AppConfigurationSettings.Deserialize(settingsData);
                Settings = AppConfigurationSettings.RandomizationSettings;
            }

            // Check for a cached ROM in the browser
            string? romPath;
            byte[]? romData;
            if (PlatformServices.TryLoadCachedRom(
                out romPath, out romData))
            {
                await SetRomFile(
                    romPath,
                    () => Task.FromResult((Stream)new MemoryStream(romData)),
                    false);

                if (mRom != null)
                    // I bet I'm going to regret this
                    AppConfigurationSettings.RomSourceBookmark = "";
            }

            // See if there's a bookmark in settings
            if (mRom == null)
            {
                // If the application configuration settings does not have a saved value,
                // try to load the Mega Man 2 rom from the executable path
                if (AppConfigurationSettings.RomSourceBookmark == "")
                {
                    romPath = await PlatformServices.GetInitialRomPath();
                    if (romPath != null)
                    {
                        var stgFile = await stg.TryGetFileFromPathAsync(romPath);
                        if (stgFile != null && stgFile.CanBookmark)
                            AppConfigurationSettings.RomSourceBookmark
                                = await stgFile.SaveBookmarkAsync() ?? "";
                    }
                }

                await SetRomFile(
                    stg, AppConfigurationSettings.RomSourceBookmark, false);
            }
        }

        private void OnAppConfigurationSettingsChanged(AppConfigurationSettings settings)
        {
            Settings = settings.RandomizationSettings;

            if (settings.SettingsPresetIndex >= SettingsPresets.Presets.Count)
                settings.SettingsPresetIndex = 0;

            this.AppConfigurationSettings.PropertyChanged += this.AppConfigurationSettings_PropertyChanged;

            foreach (var opt in AppConfigurationSettings.RandomizationSettings.AllOptions)
                opt.PropertyChanged += this.AppConfigurationSettings_PropertyChanged;
        }

        private void OnSettingsPresetIndexChanged(int newIndex)
        {
            SettingsPreset = SettingsPresets.Presets[newIndex];
            IsTournament = !string.IsNullOrEmpty(SettingsPreset.TournamentTitleScreenString);
            Settings.SettingsPreset = SettingsPreset;
        }

        private void AppConfigurationSettings_PropertyChanged(Object? sender, System.ComponentModel.PropertyChangedEventArgs? e)
        {
            if (e is null)
                return;

            if (sender is IOption opt
                && e.PropertyName != nameof(IOption.BaseValue)
                && e.PropertyName != nameof(IOption.Randomize))
                return;

            var optsData = AppConfigurationSettings.Serialize();
            //Trace.WriteLine(Encoding.UTF8.GetString(optsData));

            PlatformServices.SaveSettingsData(optsData);
        }

        private async Task SetRomFile(IStorageProvider stg, string? bmString, bool forceClear)
        {
            IStorageBookmarkFile? file = null;
            if (!string.IsNullOrEmpty(bmString))
                file = await stg.OpenFileBookmarkAsync(bmString);

            await SetRomFile(file, forceClear);
        }

        private async Task SetRomFile(IStorageFile? file, bool forceClear)
        {
            string path = "";
            Func<Task<Stream>>? OpenStream = null;

            if (file != null)
            {
                path = file.TryGetLocalPath() ?? file.Name;
                OpenStream = () => file!.OpenReadAsync();
            }

            await SetRomFile(path, OpenStream, forceClear);
        }

        private async Task SetRomFile(
            string path, 
            Func<Task<Stream>>? OpenStream, 
            bool forceClear)
        {
            if (OpenStream == null && !forceClear)
                return;

            mRom = null;

            IsRomSourcePathValid = false;
            IsRomValid = false;
            RomSourcePath = "Invalid File";
            IsRomValidText = "";
            RomStatusTooltip = "";
            HashValidationMessage = String.Empty;

            if (OpenStream == null)
                return;

            IsRomValidText = "❌";

            try
            {
                using (Stream stream = await OpenStream())
                {
                    RomSourcePath = path;

                    var data = new byte[SOURCE_ROM_SIZE];
                    var fileSize = await stream.ReadAtLeastAsync(data, data.Length, false);

                    if (fileSize != SOURCE_ROM_SIZE
                        || await stream.ReadAsync(data) != 0)
                    {
                        HashValidationMessage = "Incorrect ROM size";
                        return;
                    }

                    var rom = data[ROM_HEADER_SIZE..];

                    // Don't need to explicitly check file size because the hash will fail if it was only partially read
                    Dictionary<string, Func<byte[], byte[]>> romHashAlgs = new()
                    {
                        { "CRC", rom => Crc32.Hash(rom).Reverse().ToArray() },
                        //{ "MD5", MD5.HashData},
                    };
                    Dictionary<string, Func<byte[], byte[]>> fileHashAlgs = new()
                    {
                        { "CRC", rom => Crc32.Hash(rom).Reverse().ToArray() },
                        //{ "MD5", MD5.HashData },
                        { "SHA-1", SHA1.HashData },
                        { "SHA-256", SHA256.HashData },
                    };

                    var ByteToString = (byte[] b) => BitConverter.ToString(b)
                        .Replace("-", String.Empty).ToLowerInvariant();
                    var romHashes = romHashAlgs.ToDictionary(nf => nf.Key,
                        nf => ByteToString(nf.Value(rom)));
                    var fileHashes = fileHashAlgs.ToDictionary(nf => nf.Key,
                        nf => ByteToString(nf.Value(data)));

                    StringBuilder tipSb = new();
                    tipSb.AppendLine("ROM");
                    foreach (var (name, fn) in romHashAlgs)
                        tipSb.AppendLine($"{name}: {ByteToString(fn(rom))}");

                    tipSb.AppendLine();
                    tipSb.AppendLine("File");
                    foreach (var (name, fn) in fileHashAlgs)
                        tipSb.AppendLine($"{name}: {ByteToString(fn(data))}");

                    // Check that the hash matches a supported hash
                    IsRomValid = EXPECTED_SHA256_HASH_LIST.Contains(
                        fileHashes["SHA-256"]);
                    RomStatusTooltip = tipSb.ToString();

                    if (IsRomValid)
                    {
                        PlatformServices.SaveRomCache(path, data);

                        mRom = data;
                        HashValidationMessage = "ROM checksum is valid.";
                        IsRomValidText = "✅";
                        IsRomSourcePathValid = true;
                    }
                    else
                        HashValidationMessage = "ROM checksum is INVALID.";
                }
            }
            catch (Exception e)
            {
                IsRomValid = false;
                HashValidationMessage = e.ToString();
            }
        }

        //
        // Properties
        //

        public IHostPlatformServices PlatformServices{ get; }

        [Reactive]
        public partial AppConfigurationSettings AppConfigurationSettings { get; private set; }

        [Reactive]
        public partial RandomizationSettings Settings { get; private set; }

        public SettingsPresets SettingsPresets { get; }

        public string Version { get; }

        [Reactive]
        public partial string RomSourcePath { get; private set; } = "";

        [Reactive]
        public partial bool IsRomSourcePathValid { get; private set; } = false;

        private readonly ObservableAsPropertyHelper<bool> _isSeedValid;
        public bool IsSeedValid => _isSeedValid.Value;

        [Reactive]
        public partial bool IsRomValid { get; private set; } = false;

        [Reactive]
        public partial string IsRomValidText { get; private set; } = "";

        [Reactive]
        public partial string RomStatusTooltip { get; private set; } = "";

        [Reactive]
        public partial string HashValidationMessage { get; private set; } = "";

        [Reactive]
        public partial bool CanOpenContainingFolder { get; private set; } = false;

        public Boolean IsCoreModulesChecked
        {
            get
            {
                Debug.Assert(this.AppConfigurationSettings is not null);

                return this.AppConfigurationSettings.RandomizationSettings.GameplayOptions.RandomizeRobotMasterStageSelection.Value &&
                   this.AppConfigurationSettings.RandomizationSettings.GameplayOptions.RandomizeSpecialWeaponReward.Value &&
                   this.AppConfigurationSettings.RandomizationSettings.GameplayOptions.RandomizeRefightTeleporters.Value;
            }
        }

        [Reactive]
        public partial SettingsPreset? SettingsPreset { get; private set; } = null;

        [Reactive]
        public partial bool IsTournament { get; private set; } = false;

        // Add this property to bind to your slider
        [Reactive]
        public partial int RandomSeedCount { get; set; } = 1;

        //
        // Commands
        //

        [RelayCommand]
        protected async Task OpenRomFile(Visual in_View)
        {
            string? exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var storage = TopLevel.GetTopLevel(in_View)!.StorageProvider;
            var initDir = exeDir != null
                ? await storage.TryGetFolderFromPathAsync(exeDir)
                : null;

            var stgFiles = await storage.OpenFilePickerAsync(new()
            {
                Title = "Open Mega Man 2 (US) NES ROM File",
                FileTypeFilter = mNesRomFileTypes,
                SuggestedStartLocation = initDir,
                SuggestedFileType = mNesRomFileTypes[0],
                AllowMultiple = false,
            });

            // Process input if the user clicked OK.
            if (stgFiles.Count != 1)
                return;

            try
            {
                await SetRomFile(stgFiles[0], true);

                AppConfigurationSettings.RomSourceBookmark = await stgFiles[0].SaveBookmarkAsync() ?? "";
            }
            catch
            {
                AppConfigurationSettings.RomSourceBookmark = "";
            }
        }

        [RelayCommand]
        protected async Task CreateFromGivenSeed(Visual in_View)
        {
            if (true == String.IsNullOrEmpty(this.AppConfigurationSettings?.SeedString))
            {
                await this.CreateFromRandomSeedMultiple(in_View);
            }
            else
            {
                try
                {
                    using (var romSaver = PlatformServices.CreateRomSaver(
                        Path.GetDirectoryName(RomSourcePath)))
                    {
                        await this.PerformRandomization(in_View, false, romSaver);
                        this.AppConfigurationSettings.SeedString = this.mCurrentRandomizationContext!.Seed.SeedString;

                        await romSaver.Commit();
                    }
                }
                catch (Exception e)
                {
                    await MessageBox.ShowAsync(in_View, e.ToString(), "Error", ButtonEnum.Ok);
                }
            }
        }


        [RelayCommand]
        protected async Task CreateFromRandomSeedMultiple(Visual in_View)
        {
            try
            {
                using (var romSaver = PlatformServices.CreateRomSaver(
                    Path.GetDirectoryName(RomSourcePath)))
                {
                    for (int i = 1; i <= this.RandomSeedCount; i++)
                    {
                        await this.PerformRandomization(in_View, true, romSaver);
                        this.AppConfigurationSettings!.SeedString = this.mCurrentRandomizationContext!.Seed.SeedString;
                        HashValidationMessage = $"Successfully copied and patched {i} of {this.RandomSeedCount} ROMs!";
                    }

                    await romSaver.Commit();
                }
            }
            catch (Exception e)
            {
                await MessageBox.ShowAsync(in_View, e.ToString(), "Error", ButtonEnum.Ok);
            }
        }


        [RelayCommand]
        protected async Task OpenContainingFolder(Visual in_View)
        {
            var launcher = TopLevel.GetTopLevel(in_View)?.Launcher;
            if (launcher == null)
                return;

            if (!string.IsNullOrEmpty(this.mCurrentRandomizationContext?.FileName))
            {
                try
                {
                    if (await launcher.LaunchDirectoryInfoAsync(new(Path.TrimEndingDirectorySeparator(Path.GetDirectoryName(Path.GetFullPath(mCurrentRandomizationContext!.FileName))!))))
                        return;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            await launcher.LaunchDirectoryInfoAsync(new(Path.TrimEndingDirectorySeparator(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!)));
        }

        async Task PerformRandomization(Visual in_View, Boolean in_DefaultSeed, IRomSaver in_RomSaver)
        {
            // Perform randomization based on settings, then generate the ROM.
            var settings = AppConfigurationSettings;
            var rndOpts = AppConfigurationSettings.RandomizationSettings;

            rndOpts.SeedString = (true == in_DefaultSeed) ? null : settings.SeedString;
            rndOpts.RomSourcePath = RomSourcePath;
            rndOpts.CreateLogFile = settings.CreateLogFile && !rndOpts.IsTournament;

            //Settings.SettingsPreset = AppConfigurationSettings.SettingsPresetIndex != 0 ? SettingsPreset : null;

            RandomizationContext context = await RandomMM2.RandomizerCreate(Settings, PlatformServices, mRom!);
            HashValidationMessage = "Successfully copied and patched! File: " + context.FileName;

            // Get A-Z representation of seed
            String seedBase26 = context.Seed.Identifier;

            this.mCurrentRandomizationContext = context;

            Debug.WriteLine("\nSeed: " + seedBase26 + "\n");

            // Create log file if left shift is pressed while clicking
            if (true == this.AppConfigurationSettings.CreateLogFile &&
                !IsTournament)
            {
                String logFileName = $"MM2RNG-{seedBase26}.log";

                using (StreamWriter sw = new StreamWriter(logFileName, false))
                {
                    sw.WriteLine("Mega Man 2 Randomizer");
                    sw.WriteLine($"Version {RandomMM2.AssemblyVersion}");
                    sw.WriteLine($"Seed {seedBase26}\n");
                    sw.WriteLine(context.RandomStages.ToString());
                    sw.WriteLine(context.RandomWeaponBehavior.ToString());
                    sw.WriteLine(context.RandomEnemyWeakness.ToString());
                    sw.WriteLine(context.RandomWeaknesses.ToString());
                    sw.Write(context.Patch.GetStringSortedByAddress());
                }
            }

            in_RomSaver.AddFile(
                Path.GetFileName(context.FileName), context.Rom);

            // Flag UI as having created a ROM, enabling the "open folder" button
            CanOpenContainingFolder = TopLevel.GetTopLevel(in_View)?.Launcher != null;
        }

        [RelayCommand]
        protected async Task ImportSettings(Visual in_View)
        {
            string? exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var storage = TopLevel.GetTopLevel(in_View)!.StorageProvider;
            var initDir = exeDir != null
                ? await storage.TryGetFolderFromPathAsync(exeDir)
                : null;

            var stgFiles = await storage.OpenFilePickerAsync(new()
            {
                Title = "Import Settings",
                FileTypeFilter = mJsonSettingsFileTypes,
                SuggestedStartLocation = initDir,
                SuggestedFileType = mJsonSettingsFileTypes[0],
                AllowMultiple = false,
            });

            // Process input if the user clicked OK.
            if (stgFiles.Count != 1)
                return;

            using (var stream = await stgFiles[0].OpenReadAsync())
            {
                var data = new byte[stream.Length];
                await stream.ReadExactlyAsync(data);

                AppConfigurationSettings = AppConfigurationSettings.Deserialize(data);
            }
        }

        [RelayCommand]
        protected async Task ExportSettings(Visual in_View)
        {
            string? exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var storage = TopLevel.GetTopLevel(in_View)!.StorageProvider;
            var initDir = exeDir != null
                ? await storage.TryGetFolderFromPathAsync(exeDir)
                : null;

            var stgFile = await storage.SaveFilePickerAsync(new()
            {
                Title = "Export Settings",
                FileTypeChoices = mJsonSettingsFileTypes,
                SuggestedStartLocation = initDir,
                SuggestedFileType = mJsonSettingsFileTypes[0],
                ShowOverwritePrompt = true,
            });

            // Process input if the user clicked OK.
            if (stgFile == null)
                return;

            var data = AppConfigurationSettings.Serialize();
            using (var stream = await stgFile.OpenWriteAsync())
                await stream.WriteAsync(data);
        }


        //
        // Public Methods
        //

        public void SetTheme()
        {
            Application.Current!.RequestedThemeVariant = AppConfigurationSettings.EnableAppUiDarkTheme
                ? ThemeVariant.Dark
                : ThemeVariant.Light;
        }

        public async Task<bool> TryDrop(IStorageProvider storage, IDataTransfer transfer)
        {
            Console.WriteLine("TryDrop");

            var file = GetDragDropFile(transfer);
            if (file == null)
                return false;

            await SetRomFile(file, true);

            return true;
        }


        //
        // Constants
        //

        private const Double BYTES_PER_MEGABYTE = 1024d * 1024d;
        private const Int64 ONE_MEGABYTE = 1024 * 1024;
        private const int ROM_HEADER_SIZE = 0x10;
        private const int SOURCE_ROM_SIZE = 0x40000 + ROM_HEADER_SIZE;

        private readonly List<String> EXPECTED_SHA256_HASH_LIST = new List<String>()
        {
            "27b5a635df33ed57ed339dfc7fd62fc603b39c1d1603adb5cdc3562a0b0d555b", // Mega Man 2 (U)
            "49136b412ff61beac6e40d0bbcd8691a39a50cd2744fdcdde3401eed53d71edf", // Mega Man 2 (USA)
        };

        //
        // Private Data Members
        //

        private static readonly FilePickerFileType[] mNesRomFileTypes = [
            new("NES ROMs") { Patterns = ["*.nes"] }
        ];

        private static readonly FilePickerFileType[] mJsonSettingsFileTypes = [
            new("JSON Settings") { Patterns = ["*.json", "*.jsn"] }
        ];

        private byte[]? mRom = null;

        private RandomizationContext? mCurrentRandomizationContext = null;

        private static IStorageFile? GetDragDropFile(IDataTransfer transfer)
        {
            Console.WriteLine("GetDragDropFile");

            // Only allow if the dragged data contains text or filenames
            IStorageFile? file = null;
            var files = transfer.TryGetFiles();
            if (files != null)
                Console.WriteLine(files.Length);
            if (files != null && files.Length == 1)
            {
                file = files[0] as IStorageFile;
                Console.WriteLine(file);
            }

            if (file != null)
                Console.WriteLine(file.Name);

            if (file == null
                || !file.Name.ToLowerInvariant().EndsWith(".nes"))
                return null;

            return file;
        }

        private bool IsValidSeed(string seed)
        {
            // First, clean the seed of non-alphanumerics.  This isn't for the
            // seed generation code, but to maintain safe file names
            seed = seed.Trim().ToUpperInvariant().RemoveNonAlphanumericCharacters()!;

            return !string.IsNullOrWhiteSpace(seed);
        }
    }
}
