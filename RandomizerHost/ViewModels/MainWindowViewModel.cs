using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Xml;
using Avalonia.Controls;
using Avalonia.Themes.Fluent;
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
            this.ToggleTournamentModeCommand = ReactiveCommand.Create<Window>(this.ToggleTournamentMode);

            this.ImportSettingsCommand = ReactiveCommand.Create<Window>(this.ImportSettings);
            this.ExportSettingsCommand = ReactiveCommand.Create<Window>(this.ExportSettings);
            this.SetThemeCommand = ReactiveCommand.Create(this.SetTheme);
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
        public ICommand SetThemeCommand { get; }
        public ICommand ToggleTournamentModeCommand { get; }


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


        public void ToggleTournamentMode(Window in_Window)
        {
            if (true == this.mAppConfigurationSettings.TournamentMode)
            {
                // Disable the toggle switches
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_CastleBossEnergyRefillSpeed = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_DisableFlashingEffects = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_DisablePauseLock = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_DisableWaterfall = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableBurstChaserMode = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableFasterCutsceneText = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableHiddenStageNames = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableLeftwardWallEjection = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfBossSprites = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfBossWeaknesses = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfColorPalettes = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfEnemySpawns = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfEnemySprites = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfEnemyWeaknesses = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfEnvironmentSprites = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfFalseFloors = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfInGameText = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfItemPickupSprites = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfMenusAndTransitionScreens = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfMusicTracks = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfRobotMasterBehavior = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfRobotMasterLocations = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfSpecialItemLocations = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfSpecialWeaponBehavior = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfSpecialWeaponSprites = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableUnderwaterLagReduction = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnergyTankRefillSpeed = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_Font = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_HitPointRefillSpeed = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_HudElement = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_MercilessMode = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_PlayerSprite = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_RobotMasterEnergyRefillSpeed = false;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_WeaponEnergyRefillSpeed = false;

                // Disable the check boxes
                this.mAppConfigurationSettings.EnableSetting_CastleBossEnergyRefillSpeed = false;
                this.mAppConfigurationSettings.EnableSetting_DisableFlashingEffects = false;
                this.mAppConfigurationSettings.EnableSetting_DisablePauseLock = false;
                this.mAppConfigurationSettings.EnableSetting_DisableWaterfall = false;
                this.mAppConfigurationSettings.EnableSetting_EnableBurstChaserMode = false;
                this.mAppConfigurationSettings.EnableSetting_EnableFasterCutsceneText = false;
                this.mAppConfigurationSettings.EnableSetting_EnableHiddenStageNames = false;
                this.mAppConfigurationSettings.EnableSetting_EnableLeftwardWallEjection = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfBossSprites = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfBossWeaknesses = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfColorPalettes = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfEnemySpawns = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfEnemySprites = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfEnemyWeaknesses = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfEnvironmentSprites = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfFalseFloors = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfInGameText = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfItemPickupSprites = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfMenusAndTransitionScreens = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfMusicTracks = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfRobotMasterBehavior = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfRobotMasterLocations = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfSpecialItemLocations = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfSpecialWeaponBehavior = false;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfSpecialWeaponSprites = false;
                this.mAppConfigurationSettings.EnableSetting_EnableUnderwaterLagReduction = false;
                this.mAppConfigurationSettings.EnableSetting_EnergyTankRefillSpeed = false;
                //this.mAppConfigurationSettings.EnableSetting_Font = false;
                this.mAppConfigurationSettings.EnableSetting_HitPointRefillSpeed = false;
                //this.mAppConfigurationSettings.EnableSetting_HudElement = false;
                this.mAppConfigurationSettings.EnableSetting_MercilessMode = false;
                //this.mAppConfigurationSettings.EnableSetting_PlayerSprite = false;
                this.mAppConfigurationSettings.EnableSetting_RobotMasterEnergyRefillSpeed = false;
                this.mAppConfigurationSettings.EnableSetting_WeaponEnergyRefillSpeed = false;

                // Set all of the toggle buttons to false
                this.mAppConfigurationSettings.RandomlyChooseSetting_CastleBossEnergyRefillSpeed = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_DisableFlashingEffects = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_DisablePauseLock = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_DisableWaterfall = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableBurstChaserMode = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableFasterCutsceneText = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableHiddenStageNames = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableLeftwardWallEjection = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfBossSprites = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfBossWeaknesses = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfColorPalettes = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfEnemySpawns = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfEnemySprites = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfEnemyWeaknesses = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfEnvironmentSprites = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfFalseFloors = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfInGameText = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfItemPickupSprites = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfMenusAndTransitionScreens = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfMusicTracks = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfRobotMasterBehavior = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfRobotMasterLocations = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfSpecialItemLocations = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfSpecialWeaponBehavior = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableRandomizationOfSpecialWeaponSprites = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnableUnderwaterLagReduction = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_EnergyTankRefillSpeed = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_Font = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_HitPointRefillSpeed = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_HudElement = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_MercilessMode = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_PlayerSprite = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_RobotMasterEnergyRefillSpeed = false;
                this.mAppConfigurationSettings.RandomlyChooseSetting_WeaponEnergyRefillSpeed = false;

                // Set the chosen settings for the 2023 tournament
                this.mAppConfigurationSettings.EnableRandomizationOfEnemyWeaknesses = true;
                this.mAppConfigurationSettings.EnableRandomizationOfRobotMasterBehavior = true;
                this.mAppConfigurationSettings.CreateLogFile = false;
                this.mAppConfigurationSettings.EnableRandomizationOfSpecialWeaponSprites = true;
                this.mAppConfigurationSettings.EnableRandomizationOfMenusAndTransitionScreens = true;
                this.mAppConfigurationSettings.CastleBossEnergyRefillSpeed = MM2Randomizer.Settings.Options.ChargingSpeedOption.Fastest;
                this.mAppConfigurationSettings.EnableHiddenStageNames = false;
                this.mAppConfigurationSettings.EnableRandomizationOfSpecialItemLocations = true;
                this.mAppConfigurationSettings.EnableRandomizationOfEnemySpawns = true;
                this.mAppConfigurationSettings.EnableRandomizationOfMusicTracks = true;
                this.mAppConfigurationSettings.EnableUnderwaterLagReduction = true;
                this.mAppConfigurationSettings.MercilessMode = false;
                this.mAppConfigurationSettings.DisablePauseLock = true;
                this.mAppConfigurationSettings.EnableRandomizationOfEnemySprites = true;
                this.mAppConfigurationSettings.EnableRandomizationOfBossWeaknesses = true;
                this.mAppConfigurationSettings.EnableRandomizationOfSpecialWeaponBehavior = true;
                this.mAppConfigurationSettings.EnableRandomizationOfColorPalettes = true;
                this.mAppConfigurationSettings.EnableRandomizationOfFalseFloors = true;
                this.mAppConfigurationSettings.EnableFasterCutsceneText = true;
                this.mAppConfigurationSettings.EnableBurstChaserMode = false;
                this.mAppConfigurationSettings.EnableRandomizationOfItemPickupSprites = true;
                this.mAppConfigurationSettings.HitPointRefillSpeed = MM2Randomizer.Settings.Options.ChargingSpeedOption.Fastest;
                this.mAppConfigurationSettings.EnableRandomizationOfRobotMasterLocations = true;
                this.mAppConfigurationSettings.RobotMasterEnergyRefillSpeed = MM2Randomizer.Settings.Options.ChargingSpeedOption.Fastest;
                this.mAppConfigurationSettings.DisableWaterfall = true;
                this.mAppConfigurationSettings.EnableLeftwardWallEjection = true;
                this.mAppConfigurationSettings.EnergyTankRefillSpeed = MM2Randomizer.Settings.Options.ChargingSpeedOption.Fastest;
                this.mAppConfigurationSettings.EnableRandomizationOfInGameText = true;
                this.mAppConfigurationSettings.DisableFlashingEffects = true;
                this.mAppConfigurationSettings.WeaponEnergyRefillSpeed = MM2Randomizer.Settings.Options.ChargingSpeedOption.Fastest;
                this.mAppConfigurationSettings.EnableRandomizationOfEnvironmentSprites = true;
                this.mAppConfigurationSettings.EnableRandomizationOfBossSprites = true;

                // Disable the buttons
                this.AppConfigurationSettings.Enable_ImportSettings = false;
                this.AppConfigurationSettings.Enable_ExportSettings = false;
                this.AppConfigurationSettings.Enable_CreateLogFile = false;

                this.mAppConfigurationSettings.CreateLogFile = false;
            }
            else
            {
                // Enable the toggle switches
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_CastleBossEnergyRefillSpeed = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_DisableFlashingEffects = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_DisablePauseLock = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_DisableWaterfall = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableBurstChaserMode = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableFasterCutsceneText = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableHiddenStageNames = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableLeftwardWallEjection = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfBossSprites = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfBossWeaknesses = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfColorPalettes = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfEnemySpawns = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfEnemySprites = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfEnemyWeaknesses = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfEnvironmentSprites = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfFalseFloors = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfInGameText = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfItemPickupSprites = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfMenusAndTransitionScreens = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfMusicTracks = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfRobotMasterBehavior = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfRobotMasterLocations = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfSpecialItemLocations = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfSpecialWeaponBehavior = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableRandomizationOfSpecialWeaponSprites = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnableUnderwaterLagReduction = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_EnergyTankRefillSpeed = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_Font = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_HitPointRefillSpeed = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_HudElement = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_MercilessMode = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_PlayerSprite = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_RobotMasterEnergyRefillSpeed = true;
                this.mAppConfigurationSettings.Enable_RandomChoiceSetting_WeaponEnergyRefillSpeed = true;

                // Enable the check boxes
                this.mAppConfigurationSettings.EnableSetting_CastleBossEnergyRefillSpeed = true;
                this.mAppConfigurationSettings.EnableSetting_DisableFlashingEffects = true;
                this.mAppConfigurationSettings.EnableSetting_DisablePauseLock = true;
                this.mAppConfigurationSettings.EnableSetting_DisableWaterfall = true;
                this.mAppConfigurationSettings.EnableSetting_EnableBurstChaserMode = true;
                this.mAppConfigurationSettings.EnableSetting_EnableFasterCutsceneText = true;
                this.mAppConfigurationSettings.EnableSetting_EnableHiddenStageNames = true;
                this.mAppConfigurationSettings.EnableSetting_EnableLeftwardWallEjection = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfBossSprites = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfBossWeaknesses = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfColorPalettes = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfEnemySpawns = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfEnemySprites = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfEnemyWeaknesses = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfEnvironmentSprites = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfFalseFloors = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfInGameText = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfItemPickupSprites = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfMenusAndTransitionScreens = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfMusicTracks = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfRobotMasterBehavior = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfRobotMasterLocations = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfSpecialItemLocations = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfSpecialWeaponBehavior = true;
                this.mAppConfigurationSettings.EnableSetting_EnableRandomizationOfSpecialWeaponSprites = true;
                this.mAppConfigurationSettings.EnableSetting_EnableUnderwaterLagReduction = true;
                this.mAppConfigurationSettings.EnableSetting_EnergyTankRefillSpeed = true;
                //this.mAppConfigurationSettings.EnableSetting_Font = true;
                this.mAppConfigurationSettings.EnableSetting_HitPointRefillSpeed = true;
                //this.mAppConfigurationSettings.EnableSetting_HudElement = true;
                this.mAppConfigurationSettings.EnableSetting_MercilessMode = true;
                //this.mAppConfigurationSettings.EnableSetting_PlayerSprite = true;
                this.mAppConfigurationSettings.EnableSetting_RobotMasterEnergyRefillSpeed = true;
                this.mAppConfigurationSettings.EnableSetting_WeaponEnergyRefillSpeed = true;

                // Enable the buttons
                this.AppConfigurationSettings.Enable_ImportSettings = true;
                this.AppConfigurationSettings.Enable_ExportSettings = true;
                this.AppConfigurationSettings.Enable_CreateLogFile = true;
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
                false == this.AppConfigurationSettings.TournamentMode)
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
            if (dialogResult?.Length > 0)
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

        public void SetTheme()
        {
            if (true == this.mAppConfigurationSettings.EnableAppUiDarkTheme)
            {
                Avalonia.Application.Current.Styles[0] =
                    new FluentTheme(new Uri("avares://Dummy/App.xaml"))
                    {
                        Mode = FluentThemeMode.Dark
                    };
            }
            else
            {
                Avalonia.Application.Current.Styles[0] =
                    new FluentTheme(new Uri("avares://Dummy/App.xaml"))
                    {
                        Mode = FluentThemeMode.Light
                    };
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
