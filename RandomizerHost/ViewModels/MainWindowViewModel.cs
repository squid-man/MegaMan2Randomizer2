using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;
using MM2RandoLib.Settings.Options;
using MM2Randomizer;
using MM2Randomizer.Extensions;
using MM2Randomizer.Settings;
using RandomizerHost.Settings;
using RandomizerHost.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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
    public partial class MainWindowViewModel : ViewModelBase
    {
        //
        // Constructor
        //

        public MainWindowViewModel(AppConfigurationSettings settings, Action<byte[]> saveSettings)
        {
            AppConfigurationSettings = settings;
            Settings = settings.RandomizationSettings;
            SettingsPresets = new(Settings);
            SaveSettings = saveSettings;

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
                c => c.WhenAnyValue(c => c.RomSourcePath),
                OnRomSourcePathChanged);
            cfgObs.SwitchSubscribe(
                c => c.WhenAnyValue(c => c.SettingsPresetIndex),
                OnSettingsPresetIndexChanged);
            cfgObs.SwitchSelect(c => c.WhenAnyValue(c => c.SeedString))
                .Select(s => IsValidSeed(s))
                .ToProperty(this, vm => vm.IsSeedValid, out _isSeedValid);

            // If the application configuration settings does not have a saved value,
            // try to load the Mega Man 2 rom from the executable path
            if (true == String.IsNullOrEmpty(this.AppConfigurationSettings.RomSourcePath))
            {
                string? curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                Debug.Assert(curDir is not null);

                String[] tryNames = new String[]
                {
                    "MM2.nes",
                    "MegaMan2.nes",
                    "Mega Man 2.nes",
                    "Megaman II (U) [!].nes",
                    "Mega Man 2 (USA).nes",
                };
                foreach (var path in tryNames.Select(name => Path.Combine(curDir, name)))
                {
                    if (!File.Exists(path))
                        continue;

                    AppConfigurationSettings.RomSourcePath = path;
                    IsShowingHint = false;
                }
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
            Trace.WriteLine(Encoding.UTF8.GetString(optsData));

            SaveSettings(optsData);
        }

        private void OnRomSourcePathChanged(string path)
        {
            IsShowingHint = false;

            if (true == String.IsNullOrWhiteSpace(path))
            {
                IsRomSourcePathValid = false;
                IsRomValid = false;
                IsRomValidText = "";
                RomStatusTooltip = "";
                HashValidationMessage = String.Empty;

                return;
            }

            IsRomValidText = "❌";
            IsRomSourcePathValid = File.Exists(path);

            if (true == IsRomSourcePathValid)
            {
                // Ensure file size is small so that we can take the hash
                FileInfo info = new FileInfo(path);
                Int64 fileSize = info.Length;

                if (fileSize > ONE_MEGABYTE)
                {
                    Double sizeInMegabytes = fileSize / BYTES_PER_MEGABYTE;

                    HashValidationMessage = $"File is too large! {sizeInMegabytes:0.00} MB";
                    IsRomValid = false;
                }
                else
                {
                    byte[]? file = File.ReadAllBytes(path),
                        rom = file[0x10..(file.Length - 1)];

                    Dictionary<string, Func<byte[], byte[]>> romHashAlgs = new()
                    {
                        { "CRC", rom => Crc32.Hash(rom).Reverse().ToArray() },
                        { "MD5", MD5.HashData},
                    };
                    Dictionary<string, Func<byte[], byte[]>> fileHashAlgs = new()
                    {
                        { "CRC", rom => Crc32.Hash(rom).Reverse().ToArray() },
                        { "MD5", MD5.HashData },
                        { "SHA-1", SHA1.HashData },
                        { "SHA-256", SHA256.HashData },
                    };

                    var ByteToString = (byte[] b) => BitConverter.ToString(b)
                        .Replace("-", String.Empty).ToLowerInvariant();
                    var romHashes = romHashAlgs.ToDictionary(nf => nf.Key,
                        nf => ByteToString(nf.Value(rom)));
                    var fileHashes = fileHashAlgs.ToDictionary(nf => nf.Key,
                        nf => ByteToString(nf.Value(file)));

                    StringBuilder tipSb = new();
                    tipSb.AppendLine("ROM");
                    foreach (var (name, fn) in romHashAlgs)
                        tipSb.AppendLine($"{name}: {ByteToString(fn(rom))}");

                    tipSb.AppendLine();
                    tipSb.AppendLine("File");
                    foreach (var (name, fn) in fileHashAlgs)
                        tipSb.AppendLine($"{name}: {ByteToString(fn(file))}");

                    // Check that the hash matches a supported hash
                    IsRomValid = EXPECTED_SHA256_HASH_LIST.Contains(
                        fileHashes["SHA-256"]);
                    RomStatusTooltip = tipSb.ToString();

                    if (IsRomValid)
                    {
                        HashValidationMessage = "ROM checksum is valid.";
                        IsRomValidText = "✅";
                    }
                    else
                    {
                        HashValidationMessage = "ROM checksum is INVALID.";
                    }
                }
            }
            else
            {
                IsRomValid = false;
                HashValidationMessage = "File does not exist.";
            }
        }

        //
        // Properties
        //

        [Reactive]
        public AppConfigurationSettings AppConfigurationSettings { get; private set; }

        [Reactive]
        public RandomizationSettings Settings { get; private set; }

        public SettingsPresets SettingsPresets { get; }

        public string Version { get; }

        [Reactive]
        public bool IsRomSourcePathValid { get; private set; } = false;

        private readonly ObservableAsPropertyHelper<bool> _isSeedValid;
        public bool IsSeedValid => _isSeedValid.Value;

        [Reactive]
        public bool IsRomValid { get; private set; } = false;

        [Reactive]
        public string IsRomValidText { get; private set; } = "";

        [Reactive]
        public string RomStatusTooltip { get; private set; } = "";

        [Reactive]
        public string HashValidationMessage { get; private set; } = "";

        [Reactive]
        public bool IsShowingHint { get; private set; } = true;

        [Reactive]
        public bool CanOpenContainingFolder { get; private set; } = false;

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
        public SettingsPreset? SettingsPreset { get; private set; } = null;

        [Reactive]
        public bool IsTournament { get; private set; } = false;

        // Add this property to bind to your slider
        [Reactive]
        public int RandomSeedCount { get; set; } = 1;

        //
        // Commands
        //

        [RelayCommand]
        protected async Task OpenRomFile(Window in_Window)
        {
            string? exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var storage = in_Window.StorageProvider;
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

            //// TODO: Handle web cases
            AppConfigurationSettings.RomSourcePath = stgFiles[0].TryGetLocalPath()!;
        }

        [RelayCommand]
        protected async Task CreateFromGivenSeed(Window in_Window)
        {
            if (true == String.IsNullOrEmpty(this.AppConfigurationSettings?.SeedString))
            {
                await this.CreateFromRandomSeedMultiple(in_Window);
            }
            else
            {
                try
                {
                    this.PerformRandomization(in_Window, in_DefaultSeed: false);
                    this.AppConfigurationSettings.SeedString = this.mCurrentRandomizationContext!.Seed.SeedString;
                }
                catch (Exception e)
                {
                    await MessageBox.Show(in_Window, e.ToString(), "Error", MessageBox.MessageBoxButtons.Ok);
                }
            }
        }


        [RelayCommand]
        protected async Task CreateFromRandomSeedMultiple(Window in_Window)
        {
            for (int i = 1; i <= this.RandomSeedCount; i++)
            {
                try
                {
                    this.PerformRandomization(in_Window, in_DefaultSeed: true);
                    this.AppConfigurationSettings!.SeedString = this.mCurrentRandomizationContext!.Seed.SeedString;
                    HashValidationMessage = $"Successfully copied and patched {i} of {this.RandomSeedCount} ROMs!";
                }
                catch (Exception e)
                {
                    string s = e.ToString();
                    await MessageBox.Show(in_Window, e.ToString(), "Error", MessageBox.MessageBoxButtons.Ok);
                }
            }
        }


        [RelayCommand]
        protected async Task OpenContainingFolder(Window in_Window)
        {
            var launcher = TopLevel.GetTopLevel(in_Window)?.Launcher;
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

        void PerformRandomization(Window in_Window, Boolean in_DefaultSeed)
        {
            // Perform randomization based on settings, then generate the ROM.
            var settings = AppConfigurationSettings;
            var rndOpts = AppConfigurationSettings.RandomizationSettings;

            rndOpts.SeedString = (true == in_DefaultSeed) ? null : settings.SeedString;
            rndOpts.RomSourcePath = settings.RomSourcePath;
            rndOpts.CreateLogFile = settings.CreateLogFile && !rndOpts.IsTournament;

            //Settings.SettingsPreset = AppConfigurationSettings.SettingsPresetIndex != 0 ? SettingsPreset : null;

            RandomMM2.RandomizerCreate(Settings, out RandomizationContext context);
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

            // Flag UI as having created a ROM, enabling the "open folder" button
            CanOpenContainingFolder = TopLevel.GetTopLevel(in_Window)?.Launcher != null;
        }

        [RelayCommand]
        protected async Task ImportSettings(Window in_Window)
        {
            string? exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var storage = in_Window.StorageProvider;
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
        protected async Task ExportSettings(Window in_Window)
        {
            string? exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var storage = in_Window.StorageProvider;
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

        public bool CanAcceptDrop(IDataTransfer transfer)
        {
            var path = GetDragDropPath(transfer);
            return path != null;
        }

        public bool TryDrop(IStorageProvider storage, IDataTransfer transfer)
        {
            var path = GetDragDropPath(transfer);
            if (path == null)
                return false;

            AppConfigurationSettings.RomSourcePath = path;

            return true;
        }


        //
        // Constants
        //

        private const Double BYTES_PER_MEGABYTE = 1024d * 1024d;
        private const Int64 ONE_MEGABYTE = 1024 * 1024;

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

        private Action<byte[]> SaveSettings;

        private RandomizationContext? mCurrentRandomizationContext = null;

        private static string? GetDragDropPath(IDataTransfer transfer)
        {
            // Only allow if the dragged data contains text or filenames
            string? path = null;
            if (transfer.Contains(DataFormat.Text))
                path = transfer.TryGetText();
            else if (transfer.Formats.Contains(DataFormat.File)
                && (transfer.TryGetFiles() ?? Array.Empty<IStorageItem>()).Length == 1)
                path = transfer.TryGetFile()!.TryGetLocalPath()!;

            if (path != null
                && path.ToLowerInvariant().EndsWith(".nes")
                && File.Exists(path))
                return path;

            return null;
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
