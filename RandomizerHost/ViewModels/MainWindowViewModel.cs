using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Avalonia.Controls;
using MM2Randomizer;
using MM2Randomizer.Extensions;
using RandomizerHost.Views;
using ReactiveUI;
using RandomizerHost.Settings;

namespace RandomizerHost.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        //
        // Constructor
        //

        public MainWindowViewModel()
        {
            this.AppConfigurationSettings = new AppConfigurationSettings();
            this.AppConfigurationSettings.PropertyChanged += this.AppConfigurationSettings_PropertyChanged;

            // If the application configuration settings does not have a saved value,
            // try to load the Mega Man 2 rom from the executable path
            if (true == String.IsNullOrEmpty(this.AppConfigurationSettings.RomSourcePath))
            {
                String tryLocalpath = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "MM2.nes");

                if (File.Exists(tryLocalpath))
                {
                    this.AppConfigurationSettings.RomSourcePath = tryLocalpath;
                    this.IsShowingHint = false;
                }
            }

            this.OpenContainingFolderCommand = ReactiveCommand.Create(this.OpenContainngFolder, this.WhenAnyValue(x => x.CanOpenContainngFolder));
            this.CreateFromGivenSeedCommand = ReactiveCommand.Create<Window>(this.CreateFromGivenSeed, this.WhenAnyValue(x => x.AppConfigurationSettings.IsSeedValid));
            this.CreateFromRandomSeedCommand = ReactiveCommand.Create<Window>(this.CreateFromRandomSeed, this.WhenAnyValue(x => x.AppConfigurationSettings.IsHashValid));
            this.OpenRomFileCommand = ReactiveCommand.Create<Window>(this.OpenRomFile);
        }

        private void AppConfigurationSettings_PropertyChanged(Object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.AppConfigurationSettings.Save();
        }


        //
        // Commands
        //

        public ICommand OpenRomFileCommand { get; }
        public ICommand CreateFromGivenSeedCommand { get; }
        public ICommand CreateFromRandomSeedCommand { get; }
        public ICommand OpenContainingFolderCommand { get; }


        //
        // Properties
        //

        public AppConfigurationSettings AppConfigurationSettings
        {
            get => this.mAppConfigurationSettings;
            set => this.RaiseAndSetIfChanged(ref this.mAppConfigurationSettings, value);
        }

        /*???
        public RandoSettings RandoSettings
        {
            get => this.mRandoSettings;
            set => this.RaiseAndSetIfChanged(ref this.mRandoSettings, value);
        }
        */

        public Boolean IsShowingHint
        {
            get => this.mIsShowingHint;
            set => this.RaiseAndSetIfChanged(ref this.mIsShowingHint, value);
        }

        public Boolean CanOpenContainngFolder
        {
            get => this.mCanOpenContainngFolder;
            set => this.RaiseAndSetIfChanged(ref this.mCanOpenContainngFolder, value);
        }

        public Boolean IsCoreModulesChecked
        {
            get => this.AppConfigurationSettings.EnableRandomizationOfRobotMasterStageSelection &&
                   this.AppConfigurationSettings.EnableRandomizationOfSpecialWeaponReward &&
                   this.AppConfigurationSettings.EnableRandomizationOfRefightTeleporters;
        }


        //
        // Public Methods
        //

        public async void OpenRomFile(Window in_Window)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.AllowMultiple = false;

            openFileDialog.Filters.Add(
                new FileDialogFilter()
                {
                    Name = @"ROM Image",
                    Extensions = new List<String>()
                    {
                        @"nes"
                    }
                });

            openFileDialog.Title = @"Open Mega Man 2 (U) NES ROM File";

            // Call the ShowDialog method to show the dialog box.
            String exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            String exeDir = Path.GetDirectoryName(exePath);
            openFileDialog.Directory = exeDir;

            String[] dialogResult = await openFileDialog.ShowAsync(in_Window);

            // Process input if the user clicked OK.
            if (dialogResult.Length > 0)
            {
                String fileName = dialogResult[0];

                this.IsShowingHint = false;
                this.mAppConfigurationSettings.RomSourcePath = fileName;

                TextBox romFile = in_Window.FindControl<TextBox>("TextBox_RomFile");
                romFile.Focus();

                if (null != romFile.Text)
                {
                    romFile.SelectionStart = romFile.Text.Length;
                }
            }
        }


        public async void CreateFromGivenSeed(Window in_Window)
        {
            if (true == String.IsNullOrEmpty(this.AppConfigurationSettings.SeedString))
            {
                this.CreateFromRandomSeed(in_Window);
            }
            else
            {
                // First, clean the seed of non-alphanumerics.  This isn't for the
                // seed generation code, but to maintain safe file names
                this.AppConfigurationSettings.SeedString = this.AppConfigurationSettings.SeedString.Trim().ToUpperInvariant().RemoveNonAlphanumericCharacters();

                try
                {
                    // Perform randomization based on settings, then generate the ROM.
                    this.PerformRandomization();
                    //this.AppConfigurationSettings.SeedString = RandomMM2.Seed.SeedString;
                }
                catch (Exception e)
                {
                    await MessageBox.Show(in_Window, e.ToString(), "Error", MessageBox.MessageBoxButtons.Ok);
                }
            }
        }


        public async void CreateFromRandomSeed(Window in_Window)
        {
            try
            {
                this.PerformRandomization();
                this.AppConfigurationSettings.SeedString = this.mCurrentRandomizedRom.Seed.SeedString;
            }
            catch (Exception e)
            {
                await MessageBox.Show(in_Window, e.ToString(), "Error", MessageBox.MessageBoxButtons.Ok);
            }
        }


        public void OpenContainngFolder()
        {
            if (false == String.IsNullOrEmpty(this.mCurrentRandomizedRom?.FileName))
            {
                try
                {
                    Process.Start("explorer.exe", String.Format("/select,\"{0}\"", this.mCurrentRandomizedRom.FileName));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    Process.Start("explorer.exe", String.Format("/select,\"{0}\"", System.Reflection.Assembly.GetExecutingAssembly().Location));
                }
            }
            else
            {
                Process.Start("explorer.exe", String.Format("/select,\"{0}\"", System.Reflection.Assembly.GetExecutingAssembly().Location));
            }
        }


        public void PerformRandomization()
        {
            // Perform randomization based on settings, then generate the ROM.
            RandomMM2.RandomizerCreate(this.AppConfigurationSettings.AsRandomizerSettings(), out RomInfo romInfo);
            this.AppConfigurationSettings.HashValidationMessage = "Successfully copied and patched! File: " + romInfo.FileName;

            // Get A-Z representation of seed
            String seedBase26 = romInfo.Seed.Identifier;

            this.mCurrentRandomizedRom = romInfo;

            Debug.WriteLine("\nSeed: " + seedBase26 + "\n");

            // Create log file if left shift is pressed while clicking
            if (true == this.AppConfigurationSettings.CreateLogFile &&
                false == this.AppConfigurationSettings.EnableSpoilerFreeMode)
            {
                String logFileName = $"MM2RNG-{seedBase26}.log";

                using (StreamWriter sw = new StreamWriter(logFileName, false))
                {
                    sw.WriteLine("Mega Man 2 Randomizer");
                    sw.WriteLine($"Version {RandomMM2.AssemblyVersion}");
                    sw.WriteLine($"Seed {seedBase26}\n");
                    //???sw.WriteLine(RandomMM2.randomStages.ToString());
                    //???sw.WriteLine(RandomMM2.randomWeaponBehavior.ToString());
                    //???sw.WriteLine(RandomMM2.randomEnemyWeakness.ToString());
                    //???sw.WriteLine(RandomMM2.randomWeaknesses.ToString());
                    //???sw.Write(RandomMM2.Patch.GetStringSortedByAddress());
                }
            }

            // Flag UI as having created a ROM, enabling the "open folder" button
            this.CanOpenContainngFolder = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }


        //
        // Private Data Members
        //

        private AppConfigurationSettings mAppConfigurationSettings;
        private RomInfo mCurrentRandomizedRom = null;
        private Boolean mIsShowingHint = true;
        private Boolean mCanOpenContainngFolder = false;
    }
}
