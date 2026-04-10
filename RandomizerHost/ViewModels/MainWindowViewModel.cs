using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;
using MM2Randomizer;
using MM2Randomizer.Settings;
using RandomizerHost.Settings;
using RandomizerHost.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace RandomizerHost.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Version { get; }

        //
        // Constructor
        //

        public MainWindowViewModel()
        {

            Version = Assembly
                .GetExecutingAssembly()
                .GetName()
                .Version?
                .ToString() ?? "Unknown";

            this.AppConfigurationSettings.PropertyChanged += this.AppConfigurationSettings_PropertyChanged;
            this.AppConfigurationSettings.RandomizationSettingsAdapter.PropertyChanged += this.AppConfigurationSettings_PropertyChanged;

            this.SettingsPresets = new(Settings);
            this.SettingsPreset = this.SettingsPresets.Presets[0];

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

                    this.AppConfigurationSettings.RomSourcePath = path;
                    this.IsShowingHint = false;
                }
            }
        }

        private void AppConfigurationSettings_PropertyChanged(Object? sender, System.ComponentModel.PropertyChangedEventArgs? e)
        {
            this.AppConfigurationSettings!.Save();
        }


        //
        // Properties
        //

        public RandomizationSettings Settings => AppConfigurationSettings!.RandomizationSettings;
        public SettingsPresets SettingsPresets { get; }

        public AppConfigurationSettings AppConfigurationSettings
        {
            get => this.mAppConfigurationSettings;
            set => this.RaiseAndSetIfChanged(ref this.mAppConfigurationSettings, value);
        }

        public Boolean IsShowingHint
        {
            get => this.mIsShowingHint;
            set => this.RaiseAndSetIfChanged(ref this.mIsShowingHint, value);
        }

        public Boolean CanOpenContainingFolder
        {
            get => this.mCanOpenContainngFolder;
            set => this.RaiseAndSetIfChanged(ref this.mCanOpenContainngFolder, value);
        }

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

        public SettingsPreset? SettingsPreset
        {
            get => mSettingsPreset;
            set
            {
                this.RaiseAndSetIfChanged(ref mSettingsPreset, value);

                IsTournament = !string.IsNullOrEmpty(value?.TournamentTitleScreenString);
                Settings.SettingsPreset = value; 
            }
        }

        public bool IsTournament
        {
            get => mIsTournament;
            private set => this.RaiseAndSetIfChanged(ref mIsTournament, value);
        }

        // Add this property to bind to your slider (default value 1)
        private int mRandomSeedCount = 1;
        public int RandomSeedCount
        {
            get => mRandomSeedCount;
            set => this.RaiseAndSetIfChanged(ref mRandomSeedCount, value);
        }

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
            string? fileName = stgFiles[0].TryGetLocalPath()!;

            this.IsShowingHint = false;
            this.mAppConfigurationSettings.RomSourcePath = fileName;

            TextBox? romFile = in_Window.FindControl<TextBox>("TextBox_RomFile");
            Debug.Assert(romFile != null);

            romFile.Text = fileName;
            /*romFile.Focus();

            if (null != romFile.Text)
            {
                romFile.SelectionStart = romFile.Text.Length;
            }*/
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
                    this.AppConfigurationSettings.HashValidationMessage = $"Successfully copied and patched {i} of {this.RandomSeedCount} ROMs!";
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
            this.AppConfigurationSettings!.UpdateRandomizerSettings(in_DefaultSeed);
            Settings.SettingsPreset = !object.ReferenceEquals(mSettingsPreset, SettingsPresets.Presets[0]) ? mSettingsPreset : null;

            RandomMM2.RandomizerCreate(Settings, out RandomizationContext context);
            this.AppConfigurationSettings.HashValidationMessage = "Successfully copied and patched! File: " + context.FileName;

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
                FileTypeFilter = mXmlSettingsFileTypes,
                SuggestedStartLocation = initDir,
                SuggestedFileType = mXmlSettingsFileTypes[0],
                AllowMultiple = false,
            });

            // Process input if the user clicked OK.
            if (stgFiles.Count != 1)
                return;

            using (var stream = await stgFiles[0].OpenReadAsync())
            {
                using (XmlReader xmlReader = XmlReader.Create(stream, new XmlReaderSettings() { IgnoreComments = true, IgnoreWhitespace = true }))
                {
                    this.AppConfigurationSettings!.ReadXml(xmlReader);
                    xmlReader.Close();
                }
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
                FileTypeChoices = mXmlSettingsFileTypes,
                SuggestedStartLocation = initDir,
                SuggestedFileType = mXmlSettingsFileTypes[0],
                ShowOverwritePrompt = true,
            });

            // Process input if the user clicked OK.
            if (stgFile == null)
                return;

            using (var stream = await stgFile.OpenWriteAsync())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stream))
                {
                    this.AppConfigurationSettings!.WriteXml(xmlWriter);
                }
            }
        }


        //
        // Public Methods
        //

        public void SetTheme()
        {
            Application.Current!.RequestedThemeVariant = mAppConfigurationSettings.EnableAppUiDarkTheme
                ? ThemeVariant.Dark
                : ThemeVariant.Light;
        }


        //
        // Private Data Members
        //

        private static readonly FilePickerFileType[] mNesRomFileTypes = [
            new("NES ROMs") { Patterns = ["*.nes"] }
        ];

        private static readonly FilePickerFileType[] mXmlSettingsFileTypes = [
            new("XML Settings") { Patterns = ["*.xml"] }
        ];

        private AppConfigurationSettings mAppConfigurationSettings = new AppConfigurationSettings();
        private RandomizationContext? mCurrentRandomizationContext = null;
        private SettingsPreset? mSettingsPreset = null;
        private Boolean mIsShowingHint = true;
        private Boolean mCanOpenContainngFolder = false;

        // NOTE This isn't actually necessary as it's computed from the value of mSettingsPreset, but having a field to hold the previous value makes it easier to wire to the property change notification system.
        private bool mIsTournament = false;
    }
}
