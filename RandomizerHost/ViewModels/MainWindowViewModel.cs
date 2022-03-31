using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Xml;
using Avalonia.Controls;
using MM2Randomizer;
using RandomizerHost.Settings;
using RandomizerHost.Views;
using ReactiveUI;

namespace RandomizerHost.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
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
            this.CreateFromRandomSeedCommand = ReactiveCommand.Create<Window>(this.CreateFromRandomSeed, this.WhenAnyValue(x => x.AppConfigurationSettings.IsRomValid));
            this.OpenRomFileCommand = ReactiveCommand.Create<Window>(this.OpenRomFile);

            this.ImportSettingsCommand = ReactiveCommand.Create<Window>(this.ImportSettings);
            this.ExportSettingsCommand = ReactiveCommand.Create<Window>(this.ExportSettings);
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
        public ICommand ImportSettingsCommand { get; }
        public ICommand ExportSettingsCommand { get; }


        //
        // Properties
        //

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
            String exePath = Assembly.GetExecutingAssembly().Location;
            String exeDir = Path.GetDirectoryName(exePath);
            openFileDialog.Directory = exeDir;

            String[] dialogResult = await openFileDialog.ShowAsync(in_Window);

            // Process input if the user clicked OK.
            if (dialogResult?.Length > 0)
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
                try
                {
                    this.PerformRandomization(in_DefaultSeed: false);
                    this.AppConfigurationSettings.SeedString = this.mCurrentRandomizationContext.Seed.SeedString;
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
                this.PerformRandomization(in_DefaultSeed: true);
                this.AppConfigurationSettings.SeedString = this.mCurrentRandomizationContext.Seed.SeedString;
            }
            catch (Exception e)
            {
                await MessageBox.Show(in_Window, e.ToString(), "Error", MessageBox.MessageBoxButtons.Ok);
            }
        }


        public void OpenContainngFolder()
        {
            if (false == String.IsNullOrEmpty(this.mCurrentRandomizationContext?.FileName))
            {
                try
                {
                    Process.Start("explorer.exe", String.Format("/select,\"{0}\"", this.mCurrentRandomizationContext.FileName));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    Process.Start("explorer.exe", String.Format("/select,\"{0}\"", Assembly.GetExecutingAssembly().Location));
                }
            }
            else
            {
                Process.Start("explorer.exe", String.Format("/select,\"{0}\"", Assembly.GetExecutingAssembly().Location));
            }
        }


        public void PerformRandomization(Boolean in_DefaultSeed)
        {
            // Perform randomization based on settings, then generate the ROM.
            RandomMM2.RandomizerCreate(this.AppConfigurationSettings.AsRandomizerSettings(in_DefaultSeed), out RandomizationContext context);
            this.AppConfigurationSettings.HashValidationMessage = "Successfully copied and patched! File: " + context.FileName;

            // Get A-Z representation of seed
            String seedBase26 = context.Seed.Identifier;

            this.mCurrentRandomizationContext = context;

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
                    sw.WriteLine(context.RandomStages.ToString());
                    sw.WriteLine(context.RandomWeaponBehavior.ToString());
                    sw.WriteLine(context.RandomEnemyWeakness.ToString());
                    sw.WriteLine(context.RandomWeaknesses.ToString());
                    sw.Write(context.Patch.GetStringSortedByAddress());
                }
            }

            // Flag UI as having created a ROM, enabling the "open folder" button
            this.CanOpenContainngFolder = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public async void ImportSettings(Window in_Window)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.AllowMultiple = false;

            openFileDialog.Filters.Add(
                new FileDialogFilter()
                {
                    Name = @"XML Settings",
                    Extensions = new List<String>()
                    {
                        @"xml"
                    }
                });

            openFileDialog.Title = @"Import Settings";

            // Call the ShowDialog method to show the dialog box.
            String exePath = Assembly.GetExecutingAssembly().Location;
            String exeDir = Path.GetDirectoryName(exePath);
            openFileDialog.Directory = exeDir;

            String[] dialogResult = await openFileDialog.ShowAsync(in_Window);

            // Process input if the user clicked OK.
            if (dialogResult.Length > 0)
            {
                String fileName = dialogResult[0];
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (XmlReader xmlReader = XmlReader.Create(fileStream, new XmlReaderSettings() { IgnoreComments = true, IgnoreWhitespace = true }))
                    {
                        this.AppConfigurationSettings.ReadXml(xmlReader);
                        xmlReader.Close();
                    }
                }
            }
        }

        public async void ExportSettings(Window in_Window)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filters.Add(
                new FileDialogFilter()
                {
                    Name = @"XML Settings",
                    Extensions = new List<String>()
                    {
                        @"xml"
                    }
                });

            saveFileDialog.Title = @"Export Settings";

            // Call the ShowDialog method to show the dialog box.
            String exePath = Assembly.GetExecutingAssembly().Location;
            String exeDir = Path.GetDirectoryName(exePath);
            saveFileDialog.Directory = exeDir;

            String dialogResult = await saveFileDialog.ShowAsync(in_Window);

            // Process input if the user clicked OK.
            if (dialogResult?.Length > 0)
            {
                String fileName = dialogResult;
                using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(fileStream))
                    {
                        this.AppConfigurationSettings.WriteXml(xmlWriter);
                    }
                }
            }
        }


        //
        // Private Data Members
        //

        private AppConfigurationSettings mAppConfigurationSettings;
        private RandomizationContext mCurrentRandomizationContext = null;
        private Boolean mIsShowingHint = true;
        private Boolean mCanOpenContainngFolder = false;
    }
}
