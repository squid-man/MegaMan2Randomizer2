using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MM2Randomizer;
using MM2Randomizer.Extensions;
using MM2Randomizer.Settings.Options;

namespace RandomizerHost.Settings
{
    public sealed class AppConfigurationSettings : ApplicationSettingsBase, IXmlSerializable
    {
        //
        // Constructor
        //
        public AppConfigurationSettings()
        {
        }


        //
        // Variable Properties
        //

        [UserScopedSetting]
        [DefaultSettingValue("")]
        public String SeedString
        {
            get
            {
                String value = this.GetValueOrDefault(
                    AppConfigurationSettings.SEED_STRING_SETTING_NAME,
                    AppConfigurationSettings.SEED_STRING_DEFAULT_VALUE);

                this.ValidateSeed(ref value);

                return value;
            }

            set
            {
                this[AppConfigurationSettings.SEED_STRING_SETTING_NAME] = value;
            }
        }


        [UserScopedSetting]
        [DefaultSettingValue("")]
        public String RomSourcePath
        {
            get
            {
                String value = this.GetValueOrDefault(
                    AppConfigurationSettings.ROM_SOURCE_PATH_SETTING_NAME,
                    AppConfigurationSettings.ROM_SOURCE_PATH_DEFAULT_VALUE);

                // Validate the file path, which sets read-only flags, here
                // because both getting and setting calls this method, and the
                // path also needs to be validated when reading, for example,
                // when the application starts, and the value is read from the
                // user settings.
                this.ValidateFile(value);

                return value;
            }

            set
            {
                this[AppConfigurationSettings.ROM_SOURCE_PATH_SETTING_NAME] = value;
            }
        }

        //
        // Theme Properties
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableAppUiDarkTheme
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_APP_UI_DARK_THEME_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_APP_UI_DARK_THEME_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_APP_UI_DARK_THEME_SETTING_NAME] = value;
            }
        }


        //
        // Flag Properties
        //

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean TournamentMode
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.TOURNAMENT_MODE_SETTING_NAME,
                    AppConfigurationSettings.TOURNAMENT_MODE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.TOURNAMENT_MODE_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean CreateLogFile
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.CREATE_LOG_FILE_SETTING_NAME,
                    AppConfigurationSettings.CREATE_LOG_FILE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.CREATE_LOG_FILE_SETTING_NAME] = value;
            }
        }



        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean DisableFlashingEffects
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.DISABLE_FLASHING_EFFECTS_SETTING_NAME,
                    AppConfigurationSettings.DISABLE_FLASHING_EFFECTS_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.DISABLE_FLASHING_EFFECTS_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_DisableFlashingEffects = true;

        public Boolean EnableSetting_DisableFlashingEffects
        {
            get
            {
                return this.mEnableSetting_DisableFlashingEffects;
            }

            set
            {
                if (value != this.mEnableSetting_DisableFlashingEffects)
                {
                    this.mEnableSetting_DisableFlashingEffects = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_DisableFlashingEffects
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_DISABLE_FLASHING_EFFECTS_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_DISABLE_FLASHING_EFFECTS_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_DISABLE_FLASHING_EFFECTS_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_DisableFlashingEffects = true;

        public Boolean Enable_RandomChoiceSetting_DisableFlashingEffects
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_DisableFlashingEffects;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_DisableFlashingEffects)
                {
                    this.mEnable_RandomChoiceSetting_DisableFlashingEffects = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean EnableBurstChaserMode
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_BURST_CHASER_MODE_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_BURST_CHASER_MODE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_BURST_CHASER_MODE_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableBurstChaserMode = true;

        public Boolean EnableSetting_EnableBurstChaserMode
        {
            get
            {
                return this.mEnableSetting_EnableBurstChaserMode;
            }

            set
            {
                if (value != this.mEnableSetting_EnableBurstChaserMode)
                {
                    this.mEnableSetting_EnableBurstChaserMode = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableBurstChaserMode
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_BURST_CHASER_MODE_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_BURST_CHASER_MODE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_BURST_CHASER_MODE_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableBurstChaserMode = true;

        public Boolean Enable_RandomChoiceSetting_EnableBurstChaserMode
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableBurstChaserMode;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableBurstChaserMode)
                {
                    this.mEnable_RandomChoiceSetting_EnableBurstChaserMode = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableFasterCutsceneText
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_FASTER_CUTSCENE_TEXT_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_FASTER_CUTSCENE_TEXT_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_FASTER_CUTSCENE_TEXT_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableFasterCutsceneText = true;

        public Boolean EnableSetting_EnableFasterCutsceneText
        {
            get
            {
                return this.mEnableSetting_EnableFasterCutsceneText;
            }

            set
            {
                if (value != this.mEnableSetting_EnableFasterCutsceneText)
                {
                    this.mEnableSetting_EnableFasterCutsceneText = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableFasterCutsceneText
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_FASTER_CUTSCENE_TEXT_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_FASTER_CUTSCENE_TEXT_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_FASTER_CUTSCENE_TEXT_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableFasterCutsceneText = true;

        public Boolean Enable_RandomChoiceSetting_EnableFasterCutsceneText
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableFasterCutsceneText;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableFasterCutsceneText)
                {
                    this.mEnable_RandomChoiceSetting_EnableFasterCutsceneText = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean EnableHiddenStageNames
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_HIDDEN_STAGE_NAMES_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_HIDDEN_STAGE_NAMES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_HIDDEN_STAGE_NAMES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableHiddenStageNames = true;

        public Boolean EnableSetting_EnableHiddenStageNames
        {
            get
            {
                return this.mEnableSetting_EnableHiddenStageNames;
            }

            set
            {
                if (value != this.mEnableSetting_EnableHiddenStageNames)
                {
                    this.mEnableSetting_EnableHiddenStageNames = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableHiddenStageNames
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_HIDDEN_STAGE_NAMES_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_HIDDEN_STAGE_NAMES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_HIDDEN_STAGE_NAMES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableHiddenStageNames = true;

        public Boolean Enable_RandomChoiceSetting_EnableHiddenStageNames
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableHiddenStageNames;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableHiddenStageNames)
                {
                    this.mEnable_RandomChoiceSetting_EnableHiddenStageNames = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfBossWeaknesses
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_BOSS_WEAKNESSES_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_BOSS_WEAKNESSES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_BOSS_WEAKNESSES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfBossWeaknesses = true;

        public Boolean EnableSetting_EnableRandomizationOfBossWeaknesses
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfBossWeaknesses;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfBossWeaknesses)
                {
                    this.mEnableSetting_EnableRandomizationOfBossWeaknesses = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfBossWeaknesses
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_BOSS_WEAKNESSES_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_BOSS_WEAKNESSES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_BOSS_WEAKNESSES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfBossWeaknesses = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfBossWeaknesses
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfBossWeaknesses;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfBossWeaknesses)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfBossWeaknesses = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfBossSprites
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_BOSS_SPRITES_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_BOSS_SPRITES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_BOSS_SPRITES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfBossSprites = true;

        public Boolean EnableSetting_EnableRandomizationOfBossSprites
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfBossSprites;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfBossSprites)
                {
                    this.mEnableSetting_EnableRandomizationOfBossSprites = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfBossSprites
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_BOSS_SPRITES_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_BOSS_SPRITES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_BOSS_SPRITES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfBossSprites = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfBossSprites
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfBossSprites;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfBossSprites)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfBossSprites = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfColorPalettes
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_COLOR_PALETTES_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_COLOR_PALETTES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_COLOR_PALETTES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfColorPalettes = true;

        public Boolean EnableSetting_EnableRandomizationOfColorPalettes
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfColorPalettes;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfColorPalettes)
                {
                    this.mEnableSetting_EnableRandomizationOfColorPalettes = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfColorPalettes
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_COLOR_PALETTES_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_COLOR_PALETTES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_COLOR_PALETTES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfColorPalettes = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfColorPalettes
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfColorPalettes;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfColorPalettes)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfColorPalettes = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfEnemySpawns
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ENEMY_SPAWNS_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ENEMY_SPAWNS_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ENEMY_SPAWNS_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfEnemySpawns = true;

        public Boolean EnableSetting_EnableRandomizationOfEnemySpawns
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfEnemySpawns;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfEnemySpawns)
                {
                    this.mEnableSetting_EnableRandomizationOfEnemySpawns = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfEnemySpawns
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_SPAWNS_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_SPAWNS_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_SPAWNS_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfEnemySpawns = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfEnemySpawns
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfEnemySpawns;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfEnemySpawns)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfEnemySpawns = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfEnemySprites
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ENEMY_SPRITES_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ENEMY_SPRITES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ENEMY_SPRITES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfEnemySprites = true;

        public Boolean EnableSetting_EnableRandomizationOfEnemySprites
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfEnemySprites;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfEnemySprites)
                {
                    this.mEnableSetting_EnableRandomizationOfEnemySprites = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfEnemySprites
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_SPRITES_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_SPRITES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_SPRITES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfEnemySprites = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfEnemySprites
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfEnemySprites;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfEnemySprites)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfEnemySprites = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfEnemyWeaknesses
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfEnemyWeaknesses = true;

        public Boolean EnableSetting_EnableRandomizationOfEnemyWeaknesses
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfEnemyWeaknesses;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfEnemyWeaknesses)
                {
                    this.mEnableSetting_EnableRandomizationOfEnemyWeaknesses = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfEnemyWeaknesses
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfEnemyWeaknesses = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfEnemyWeaknesses
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfEnemyWeaknesses;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfEnemyWeaknesses)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfEnemyWeaknesses = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfEnvironmentSprites
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfEnvironmentSprites = true;

        public Boolean EnableSetting_EnableRandomizationOfEnvironmentSprites
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfEnvironmentSprites;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfEnvironmentSprites)
                {
                    this.mEnableSetting_EnableRandomizationOfEnvironmentSprites = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfEnvironmentSprites
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfEnvironmentSprites = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfEnvironmentSprites
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfEnvironmentSprites;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfEnvironmentSprites)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfEnvironmentSprites = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfFalseFloors
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfFalseFloors = true;

        public Boolean EnableSetting_EnableRandomizationOfFalseFloors
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfFalseFloors;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfFalseFloors)
                {
                    this.mEnableSetting_EnableRandomizationOfFalseFloors = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfFalseFloors
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfFalseFloors = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfFalseFloors
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfFalseFloors;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfFalseFloors)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfFalseFloors = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean MercilessMode
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.MERCILESS_MODE_SETTING_NAME,
                    AppConfigurationSettings.MERCILESS_MODE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.MERCILESS_MODE_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_MercilessMode = true;

        public Boolean EnableSetting_MercilessMode
        {
            get
            {
                return this.mEnableSetting_MercilessMode;
            }

            set
            {
                if (value != this.mEnableSetting_MercilessMode)
                {
                    this.mEnableSetting_MercilessMode = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_MercilessMode
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_MERCILESS_MODE_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_MERCILESS_MODE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_MERCILESS_MODE_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_MercilessMode = true;

        public Boolean Enable_RandomChoiceSetting_MercilessMode
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_MercilessMode;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_MercilessMode)
                {
                    this.mEnable_RandomChoiceSetting_MercilessMode = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfItemPickupSprites
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ITEM_PICKUP_SPRITES_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ITEM_PICKUP_SPRITES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ITEM_PICKUP_SPRITES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfItemPickupSprites = true;

        public Boolean EnableSetting_EnableRandomizationOfItemPickupSprites
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfItemPickupSprites;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfItemPickupSprites)
                {
                    this.mEnableSetting_EnableRandomizationOfItemPickupSprites = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfItemPickupSprites
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ITEM_PICKUP_SPRITES_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ITEM_PICKUP_SPRITES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ITEM_PICKUP_SPRITES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfItemPickupSprites = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfItemPickupSprites
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfItemPickupSprites;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfItemPickupSprites)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfItemPickupSprites = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        // This property has a constant value; it does not access the app configuration
        public Boolean EnableRandomizationOfSpecialWeaponReward
        {
            get
            {
                return AppConfigurationSettings.mEnableRandomizationOfSpecialWeaponReward;
            }
        }


        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfMusicTracks
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_MUSIC_TRACKS_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_MUSIC_TRACKS_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_MUSIC_TRACKS_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfMusicTracks = true;

        public Boolean EnableSetting_EnableRandomizationOfMusicTracks
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfMusicTracks;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfMusicTracks)
                {
                    this.mEnableSetting_EnableRandomizationOfMusicTracks = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfMusicTracks
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_MUSIC_TRACKS_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_MUSIC_TRACKS_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_MUSIC_TRACKS_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfMusicTracks = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfMusicTracks
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfMusicTracks;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfMusicTracks)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfMusicTracks = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfMenusAndTransitionScreens
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_MENUS_AND_TRANSITION_SCREENS_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_MENUS_AND_TRANSITION_SCREENS_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_MENUS_AND_TRANSITION_SCREENS_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfMenusAndTransitionScreens = true;

        public Boolean EnableSetting_EnableRandomizationOfMenusAndTransitionScreens
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfMenusAndTransitionScreens;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfMenusAndTransitionScreens)
                {
                    this.mEnableSetting_EnableRandomizationOfMenusAndTransitionScreens = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfMenusAndTransitionScreens
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_MENUS_AND_TRANSITION_SCREENS_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_MENUS_AND_TRANSITION_SCREENS_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_MENUS_AND_TRANSITION_SCREENS_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfMenusAndTransitionScreens = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfMenusAndTransitionScreens
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfMenusAndTransitionScreens;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfMenusAndTransitionScreens)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfMenusAndTransitionScreens = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        // This property has a constant value; it does not access the app configuration
        public Boolean EnableRandomizationOfRefightTeleporters
        {
            get
            {
                return AppConfigurationSettings.mEnableRandomizationOfRefightTeleporters;
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfRobotMasterBehavior
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_BEHAVIOR_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_BEHAVIOR_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_BEHAVIOR_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfRobotMasterBehavior = true;

        public Boolean EnableSetting_EnableRandomizationOfRobotMasterBehavior
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfRobotMasterBehavior;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfRobotMasterBehavior)
                {
                    this.mEnableSetting_EnableRandomizationOfRobotMasterBehavior = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfRobotMasterBehavior
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_BEHAVIOR_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_BEHAVIOR_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_BEHAVIOR_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfRobotMasterBehavior = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfRobotMasterBehavior
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfRobotMasterBehavior;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfRobotMasterBehavior)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfRobotMasterBehavior = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfRobotMasterLocations
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_LOCATIONS_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_LOCATIONS_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_LOCATIONS_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfRobotMasterLocations = true;

        public Boolean EnableSetting_EnableRandomizationOfRobotMasterLocations
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfRobotMasterLocations;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfRobotMasterLocations)
                {
                    this.mEnableSetting_EnableRandomizationOfRobotMasterLocations = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfRobotMasterLocations
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_LOCATIONS_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_LOCATIONS_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_LOCATIONS_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfRobotMasterLocations = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfRobotMasterLocations
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfRobotMasterLocations;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfRobotMasterLocations)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfRobotMasterLocations = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        // This property has a constant value; it does not access the app configuration
        public Boolean EnableRandomizationOfRobotMasterStageSelection
        {
            get
            {
                return AppConfigurationSettings.mEnableRandomizationOfRobotMasterStageSelection;
            }
        }


        //
        // EnableRandomizationOfSpecialItemLocations
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfSpecialItemLocations
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_SPECIAL_ITEM_LOCATIONS_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_SPECIAL_ITEM_LOCATIONS_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_SPECIAL_ITEM_LOCATIONS_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfSpecialItemLocations = true;

        public Boolean EnableSetting_EnableRandomizationOfSpecialItemLocations
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfSpecialItemLocations;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfSpecialItemLocations)
                {
                    this.mEnableSetting_EnableRandomizationOfSpecialItemLocations = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfSpecialItemLocations
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_ITEM_LOCATIONS_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_ITEM_LOCATIONS_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_ITEM_LOCATIONS_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfSpecialItemLocations = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfSpecialItemLocations
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfSpecialItemLocations;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfSpecialItemLocations)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfSpecialItemLocations = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        // EnableRandomizationOfSpecialWeaponBehavior
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfSpecialWeaponBehavior
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_BEHAVIOR_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_BEHAVIOR_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_BEHAVIOR_SETTING_NAME] = value;
            }
        }


        private Boolean mEnableSetting_EnableRandomizationOfSpecialWeaponBehavior = true;

        public Boolean EnableSetting_EnableRandomizationOfSpecialWeaponBehavior
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfSpecialWeaponBehavior;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfSpecialWeaponBehavior)
                {
                    this.mEnableSetting_EnableRandomizationOfSpecialWeaponBehavior = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfSpecialWeaponBehavior
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_BEHAVIOR_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_BEHAVIOR_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_BEHAVIOR_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfSpecialWeaponBehavior = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfSpecialWeaponBehavior
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfSpecialWeaponBehavior;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfSpecialWeaponBehavior)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfSpecialWeaponBehavior = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        // EnableRandomizationOfSpecialWeaponSprites Setting
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfSpecialWeaponSprites
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_SPRITES_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_SPRITES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_SPRITES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfSpecialWeaponSprites = true;

        public Boolean EnableSetting_EnableRandomizationOfSpecialWeaponSprites
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfSpecialWeaponSprites;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfSpecialWeaponSprites)
                {
                    this.mEnableSetting_EnableRandomizationOfSpecialWeaponSprites = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfSpecialWeaponSprites
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_SPRITES_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_SPRITES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_SPRITES_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfSpecialWeaponSprites = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfSpecialWeaponSprites
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfSpecialWeaponSprites;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfSpecialWeaponSprites)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfSpecialWeaponSprites = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableRandomizationOfInGameText
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableRandomizationOfInGameText = true;

        public Boolean EnableSetting_EnableRandomizationOfInGameText
        {
            get
            {
                return this.mEnableSetting_EnableRandomizationOfInGameText;
            }

            set
            {
                if (value != this.mEnableSetting_EnableRandomizationOfInGameText)
                {
                    this.mEnableSetting_EnableRandomizationOfInGameText = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableRandomizationOfInGameText
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableRandomizationOfInGameText = true;

        public Boolean Enable_RandomChoiceSetting_EnableRandomizationOfInGameText
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableRandomizationOfInGameText;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableRandomizationOfInGameText)
                {
                    this.mEnable_RandomChoiceSetting_EnableRandomizationOfInGameText = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean EnableUnderwaterLagReduction
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_UNDERWATER_LAG_REDUCTION_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_UNDERWATER_LAG_REDUCTION_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_UNDERWATER_LAG_REDUCTION_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableUnderwaterLagReduction = true;

        public Boolean EnableSetting_EnableUnderwaterLagReduction
        {
            get
            {
                return this.mEnableSetting_EnableUnderwaterLagReduction;
            }

            set
            {
                if (value != this.mEnableSetting_EnableUnderwaterLagReduction)
                {
                    this.mEnableSetting_EnableUnderwaterLagReduction = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableUnderwaterLagReduction
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_UNDERWATER_LAG_REDUCTION_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_UNDERWATER_LAG_REDUCTION_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_UNDERWATER_LAG_REDUCTION_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableUnderwaterLagReduction = true;

        public Boolean Enable_RandomChoiceSetting_EnableUnderwaterLagReduction
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableUnderwaterLagReduction;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableUnderwaterLagReduction)
                {
                    this.mEnable_RandomChoiceSetting_EnableUnderwaterLagReduction = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean DisableWaterfall
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.DISABLE_WATERFALL_SETTING_NAME,
                    AppConfigurationSettings.DISABLE_WATERFALL_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.DISABLE_WATERFALL_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_DisableWaterfall = true;

        public Boolean EnableSetting_DisableWaterfall
        {
            get
            {
                return this.mEnableSetting_DisableWaterfall;
            }

            set
            {
                if (value != this.mEnableSetting_DisableWaterfall)
                {
                    this.mEnableSetting_DisableWaterfall = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_DisableWaterfall
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_DISABLE_WATERFALL_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_DISABLE_WATERFALL_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_DISABLE_WATERFALL_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_DisableWaterfall = true;

        public Boolean Enable_RandomChoiceSetting_DisableWaterfall
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_DisableWaterfall;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_DisableWaterfall)
                {
                    this.mEnable_RandomChoiceSetting_DisableWaterfall = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean EnableLeftwardWallEjection
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_LEFTWARD_WALL_EJECTION_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_LEFTWARD_WALL_EJECTION_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_LEFTWARD_WALL_EJECTION_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnableLeftwardWallEjection = true;

        public Boolean EnableSetting_EnableLeftwardWallEjection
        {
            get
            {
                return this.mEnableSetting_EnableLeftwardWallEjection;
            }

            set
            {
                if (value != this.mEnableSetting_EnableLeftwardWallEjection)
                {
                    this.mEnableSetting_EnableLeftwardWallEjection = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnableLeftwardWallEjection
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_LEFTWARD_WALL_EJECTION_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_LEFTWARD_WALL_EJECTION_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENABLE_LEFTWARD_WALL_EJECTION_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnableLeftwardWallEjection = true;

        public Boolean Enable_RandomChoiceSetting_EnableLeftwardWallEjection
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnableLeftwardWallEjection;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnableLeftwardWallEjection)
                {
                    this.mEnable_RandomChoiceSetting_EnableLeftwardWallEjection = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean DisablePauseLock
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.DISABLE_PAUSE_LOCK_SETTING_NAME,
                    AppConfigurationSettings.DISABLE_PAUSE_LOCK_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.DISABLE_PAUSE_LOCK_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_DisablePauseLock = true;

        public Boolean EnableSetting_DisablePauseLock
        {
            get
            {
                return this.mEnableSetting_DisablePauseLock;
            }

            set
            {
                if (value != this.mEnableSetting_DisablePauseLock)
                {
                    this.mEnableSetting_DisablePauseLock = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_DisablePauseLock
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_DISABLE_PAUSE_LOCK_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_DISABLE_PAUSE_LOCK_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_DISABLE_PAUSE_LOCK_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_DisablePauseLock = true;

        public Boolean Enable_RandomChoiceSetting_DisablePauseLock
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_DisablePauseLock;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_DisablePauseLock)
                {
                    this.mEnable_RandomChoiceSetting_DisablePauseLock = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        // Scalar Properties
        //

        [UserScopedSetting]
        [DefaultSettingValue("Fastest")]
        public ChargingSpeedOption CastleBossEnergyRefillSpeed
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.CASTLE_BOSS_ENERGY_REFILL_SPEED_SETTING_NAME,
                    AppConfigurationSettings.CASTLE_BOSS_ENERGY_REFILL_SPEED_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.CASTLE_BOSS_ENERGY_REFILL_SPEED_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_CastleBossEnergyRefillSpeed = true;

        public Boolean EnableSetting_CastleBossEnergyRefillSpeed
        {
            get
            {
                return this.mEnableSetting_CastleBossEnergyRefillSpeed;
            }

            set
            {
                if (value != this.mEnableSetting_CastleBossEnergyRefillSpeed)
                {
                    this.mEnableSetting_CastleBossEnergyRefillSpeed = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_CastleBossEnergyRefillSpeed
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_CASTLE_BOSS_ENERGY_REFILL_SPEED_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_CASTLE_BOSS_ENERGY_REFILL_SPEED_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_CASTLE_BOSS_ENERGY_REFILL_SPEED_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_CastleBossEnergyRefillSpeed = true;

        public Boolean Enable_RandomChoiceSetting_CastleBossEnergyRefillSpeed
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_CastleBossEnergyRefillSpeed;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_CastleBossEnergyRefillSpeed)
                {
                    this.mEnable_RandomChoiceSetting_CastleBossEnergyRefillSpeed = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("Fastest")]
        public ChargingSpeedOption EnergyTankRefillSpeed
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENERGY_TANK_REFILL_SPEED_SETTING_NAME,
                    AppConfigurationSettings.ENERGY_TANK_REFILL_SPEED_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENERGY_TANK_REFILL_SPEED_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_EnergyTankRefillSpeed = true;

        public Boolean EnableSetting_EnergyTankRefillSpeed
        {
            get
            {
                return this.mEnableSetting_EnergyTankRefillSpeed;
            }

            set
            {
                if (value != this.mEnableSetting_EnergyTankRefillSpeed)
                {
                    this.mEnableSetting_EnergyTankRefillSpeed = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_EnergyTankRefillSpeed
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENERGY_TANK_REFILL_SPEED_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENERGY_TANK_REFILL_SPEED_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ENERGY_TANK_REFILL_SPEED_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_EnergyTankRefillSpeed = true;

        public Boolean Enable_RandomChoiceSetting_EnergyTankRefillSpeed
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_EnergyTankRefillSpeed;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_EnergyTankRefillSpeed)
                {
                    this.mEnable_RandomChoiceSetting_EnergyTankRefillSpeed = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("Fastest")]
        public ChargingSpeedOption HitPointRefillSpeed
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.HIT_POINT_REFILL_SPEED_SETTING_NAME,
                    AppConfigurationSettings.HIT_POINT_REFILL_SPEED_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.HIT_POINT_REFILL_SPEED_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_HitPointRefillSpeed = true;

        public Boolean EnableSetting_HitPointRefillSpeed
        {
            get
            {
                return this.mEnableSetting_HitPointRefillSpeed;
            }

            set
            {
                if (value != this.mEnableSetting_HitPointRefillSpeed)
                {
                    this.mEnableSetting_HitPointRefillSpeed = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_HitPointRefillSpeed
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_HIT_POINT_REFILL_SPEED_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_HIT_POINT_REFILL_SPEED_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_HIT_POINT_REFILL_SPEED_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_HitPointRefillSpeed = true;

        public Boolean Enable_RandomChoiceSetting_HitPointRefillSpeed
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_HitPointRefillSpeed;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_HitPointRefillSpeed)
                {
                    this.mEnable_RandomChoiceSetting_HitPointRefillSpeed = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        //
        //

        [UserScopedSetting]
        [DefaultSettingValue("MegaMan")]
        public PlayerSpriteOption PlayerSprite
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.PLAYER_SPRITE_SETTING_NAME,
                    AppConfigurationSettings.PLAYER_SPRITE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.PLAYER_SPRITE_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_PlayerSprite = true;

        public Boolean EnableSetting_PlayerSprite
        {
            get
            {
                return this.mEnableSetting_PlayerSprite;
            }

            set
            {
                if (value != this.mEnableSetting_PlayerSprite)
                {
                    this.mEnableSetting_PlayerSprite = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_PlayerSprite
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_PLAYER_SPRITE_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_PLAYER_SPRITE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_PLAYER_SPRITE_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_PlayerSprite = true;

        public Boolean Enable_RandomChoiceSetting_PlayerSprite
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_PlayerSprite;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_PlayerSprite)
                {
                    this.mEnable_RandomChoiceSetting_PlayerSprite = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        // HudElement Setting
        //

        [UserScopedSetting]
        [DefaultSettingValue("Default")]
        public HudElementOption HudElement
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.HUD_ELEMENT_SETTING_NAME,
                    AppConfigurationSettings.HUD_ELEMENT_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.HUD_ELEMENT_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_HudElement = true;

        public Boolean EnableSetting_HudElement
        {
            get
            {
                return this.mEnableSetting_HudElement;
            }

            set
            {
                if (value != this.mEnableSetting_HudElement)
                {
                    this.mEnableSetting_HudElement = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_HudElement
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_HUD_ELEMENT_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_HUD_ELEMENT_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_HUD_ELEMENT_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_HudElement = true;

        public Boolean Enable_RandomChoiceSetting_HudElement
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_HudElement;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_HudElement)
                {
                    this.mEnable_RandomChoiceSetting_HudElement = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        // Font Setting
        //

        [UserScopedSetting]
        [DefaultSettingValue("Default")]
        public FontOption Font
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.FONT_SETTING_NAME,
                    AppConfigurationSettings.FONT_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.FONT_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_Font = true;

        public Boolean EnableSetting_Font
        {
            get
            {
                return this.mEnableSetting_Font;
            }

            set
            {
                if (value != this.mEnableSetting_Font)
                {
                    this.mEnableSetting_Font = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_Font
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_FONT_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_FONT_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_FONT_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_Font = true;

        public Boolean Enable_RandomChoiceSetting_Font
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_Font;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_Font)
                {
                    this.mEnable_RandomChoiceSetting_Font = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        // RobotMasterEnergyRefillSpeed Setting
        //

        [UserScopedSetting]
        [DefaultSettingValue("Fastest")]
        public ChargingSpeedOption RobotMasterEnergyRefillSpeed
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ROBOT_MASTER_ENERGY_REFILL_SPEED_SETTING_NAME,
                    AppConfigurationSettings.ROBOT_MASTER_ENERGY_REFILL_SPEED_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ROBOT_MASTER_ENERGY_REFILL_SPEED_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_RobotMasterEnergyRefillSpeed = true;

        public Boolean EnableSetting_RobotMasterEnergyRefillSpeed
        {
            get
            {
                return this.mEnableSetting_RobotMasterEnergyRefillSpeed;
            }

            set
            {
                if (value != this.mEnableSetting_RobotMasterEnergyRefillSpeed)
                {
                    this.mEnableSetting_RobotMasterEnergyRefillSpeed = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_RobotMasterEnergyRefillSpeed
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ROBOT_MASTER_ENERGY_REFILL_SPEED_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ROBOT_MASTER_ENERGY_REFILL_SPEED_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_ROBOT_MASTER_ENERGY_REFILL_SPEED_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_RobotMasterEnergyRefillSpeed = true;

        public Boolean Enable_RandomChoiceSetting_RobotMasterEnergyRefillSpeed
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_RobotMasterEnergyRefillSpeed;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_RobotMasterEnergyRefillSpeed)
                {
                    this.mEnable_RandomChoiceSetting_RobotMasterEnergyRefillSpeed = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        // WeaponEnergyRefillSpeed Setting
        //

        [UserScopedSetting]
        [DefaultSettingValue("Fastest")]
        public ChargingSpeedOption WeaponEnergyRefillSpeed
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.WEAPON_ENERGY_REFILL_SPEED_SETTING_NAME,
                    AppConfigurationSettings.WEAPON_ENERGY_REFILL_SPEED_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.WEAPON_ENERGY_REFILL_SPEED_SETTING_NAME] = value;
            }
        }

        private Boolean mEnableSetting_WeaponEnergyRefillSpeed = true;

        public Boolean EnableSetting_WeaponEnergyRefillSpeed
        {
            get
            {
                return this.mEnableSetting_WeaponEnergyRefillSpeed;
            }

            set
            {
                if (value != this.mEnableSetting_WeaponEnergyRefillSpeed)
                {
                    this.mEnableSetting_WeaponEnergyRefillSpeed = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_WeaponEnergyRefillSpeed
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_WEAPON_ENERGY_REFILL_SPEED_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_WEAPON_ENERGY_REFILL_SPEED_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_WEAPON_ENERGY_REFILL_SPEED_SETTING_NAME] = value;
            }
        }

        private Boolean mEnable_RandomChoiceSetting_WeaponEnergyRefillSpeed = true;

        public Boolean Enable_RandomChoiceSetting_WeaponEnergyRefillSpeed
        {
            get
            {
                return this.mEnable_RandomChoiceSetting_WeaponEnergyRefillSpeed;
            }

            set
            {
                if (value != this.mEnable_RandomChoiceSetting_WeaponEnergyRefillSpeed)
                {
                    this.mEnable_RandomChoiceSetting_WeaponEnergyRefillSpeed = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        // Buttons
        //

        private Boolean mEnable_ImportSettings = true;

        public Boolean Enable_ImportSettings
        {
            get
            {
                return this.mEnable_ImportSettings;
            }

            set
            {
                if (value != this.mEnable_ImportSettings)
                {
                    this.mEnable_ImportSettings = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        private Boolean mEnable_ExportSettings = true;

        public Boolean Enable_ExportSettings
        {
            get
            {
                return this.mEnable_ExportSettings;
            }

            set
            {
                if (value != this.mEnable_ExportSettings)
                {
                    this.mEnable_ExportSettings = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        private Boolean mEnable_CreateLogFile = true;

        public Boolean Enable_CreateLogFile
        {
            get
            {
                return this.mEnable_CreateLogFile;
            }

            set
            {
                if (value != this.mEnable_CreateLogFile)
                {
                    this.mEnable_CreateLogFile = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        //
        // Read-only Properties
        //

        // This property has a constant value; it does not access the app configuration
        public Boolean IsRomSourcePathValid
        {
            get
            {
                return this.mIsRomSourcePathValid;
            }

            private set
            {
                this.mIsRomSourcePathValid = value;
                this.NotifyPropertyChanged();
            }
        }


        // This property has a constant value; it does not access the app configuration
        public Boolean IsSeedValid
        {
            get
            {
                return this.mIsSeedValid;
            }

            private set
            {
                this.mIsSeedValid = value;
                this.NotifyPropertyChanged();
            }
        }


        // This property has a constant value; it does not access the app configuration
        public Boolean IsRomValid
        {
            get
            {
                return this.mIsRomValid;
            }

            private set
            {
                this.mIsRomValid = value;
                this.NotifyPropertyChanged();
            }
        }


        // This property has a constant value; it does not access the app configuration
        public String HashStringMD5
        {
            get
            {
                return this.mHashStringMD5;
            }

            private set
            {
                this.mHashStringMD5 = value;
                this.NotifyPropertyChanged();
            }
        }


        // This property has a constant value; it does not access the app configuration
        public String HashStringSHA256
        {
            get
            {
                return this.mHashStringSHA256;
            }

            private set
            {
                this.mHashStringSHA256 = value;
                this.NotifyPropertyChanged();
            }
        }


        // This property has a constant value; it does not access the app configuration
        public String HashValidationMessage
        {
            get
            {
                return this.mHashValidationMessage;
            }

            set
            {
                this.mHashValidationMessage = value;
                this.NotifyPropertyChanged();
            }
        }


        //
        // IXmlSerializable Methods
        //

        public XmlSchema GetSchema()
        {
            return null;
        }


        public void ReadXml(XmlReader in_Reader)
        {
            in_Reader.MoveToContent();

            if ("Settings" == in_Reader.Name &&
                false == in_Reader.IsEmptyElement &&
                XmlNodeType.Element == in_Reader.NodeType)
            {
                using (XmlReader xmlReader = in_Reader.ReadSubtree())
                {
                    this.ReadSettingsFromXml(xmlReader);
                    xmlReader.Close();
                }
            }
        }


        public void WriteXml(XmlWriter in_Writer)
        {
            in_Writer.WriteStartElement("Settings");
            {
                foreach (SettingsPropertyValue settingsPropertyValue in this.PropertyValues)
                {
                    in_Writer.WriteStartElement(settingsPropertyValue.Name);
                    in_Writer.WriteString(settingsPropertyValue.SerializedValue.ToString());
                    in_Writer.WriteEndElement();
                }
            }
            in_Writer.WriteEndElement();
        }


        //
        // Public Methods
        //

        public MM2Randomizer.Settings.RandomizationSettings AsRandomizerSettings(Boolean in_DefaultSeed)
        {
            MM2Randomizer.Settings.RandomizationSettings settings = new MM2Randomizer.Settings.RandomizationSettings();

            settings.SeedString = (true == in_DefaultSeed) ? null : this.SeedString;
            settings.RomSourcePath = this.RomSourcePath;

            settings.TournamentMode = this.TournamentMode;
            settings.CreateLogFile = this.CreateLogFile;

            // Gameplay options
            settings.GameplayOption.BurstChaserMode.Randomize = this.RandomlyChooseSetting_EnableBurstChaserMode;
            settings.GameplayOption.BurstChaserMode.Value = (BooleanOption)Convert.ToInt32(this.EnableBurstChaserMode);
            settings.GameplayOption.FasterCutsceneText.Randomize = this.RandomlyChooseSetting_EnableFasterCutsceneText;
            settings.GameplayOption.FasterCutsceneText.Value = (BooleanOption)Convert.ToInt32(this.EnableFasterCutsceneText);
            settings.GameplayOption.HideStageNames.Randomize = this.RandomlyChooseSetting_EnableHiddenStageNames;
            settings.GameplayOption.HideStageNames.Value = (BooleanOption)Convert.ToInt32(this.EnableHiddenStageNames);
            settings.GameplayOption.RandomizeBossWeaknesses.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfBossWeaknesses;
            settings.GameplayOption.RandomizeBossWeaknesses.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfBossWeaknesses);
            settings.GameplayOption.RandomizeEnemySpawns.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfEnemySpawns;
            settings.GameplayOption.RandomizeEnemySpawns.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfEnemySpawns);
            settings.GameplayOption.RandomizeEnemyWeaknesses.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfEnemyWeaknesses;
            settings.GameplayOption.RandomizeEnemyWeaknesses.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfEnemyWeaknesses);
            settings.GameplayOption.RandomizeFalseFloors.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfFalseFloors;
            settings.GameplayOption.RandomizeFalseFloors.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfFalseFloors);
            settings.GameplayOption.RandomizeRefightTeleporters.Randomize = false; // Not an exposed option yet
            settings.GameplayOption.RandomizeRefightTeleporters.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfRefightTeleporters);
            settings.GameplayOption.RandomizeRobotMasterBehavior.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfRobotMasterBehavior;
            settings.GameplayOption.RandomizeRobotMasterBehavior.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfRobotMasterBehavior);
            settings.GameplayOption.RandomizeRobotMasterLocations.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfRobotMasterLocations;
            settings.GameplayOption.RandomizeRobotMasterLocations.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfRobotMasterLocations);
            settings.GameplayOption.RandomizeRobotMasterStageSelection.Randomize = false; // Not an exposed option yet
            settings.GameplayOption.RandomizeRobotMasterStageSelection.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfRobotMasterStageSelection);
            settings.GameplayOption.RandomizeSpecialItemLocations.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfSpecialItemLocations;
            settings.GameplayOption.RandomizeSpecialItemLocations.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfSpecialItemLocations);
            settings.GameplayOption.RandomizeSpecialWeaponBehavior.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfSpecialWeaponBehavior;
            settings.GameplayOption.RandomizeSpecialWeaponBehavior.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfSpecialWeaponBehavior);
            settings.GameplayOption.RandomizeSpecialWeaponReward.Randomize = false; // Not an exposed option yet
            settings.GameplayOption.RandomizeSpecialWeaponReward.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfSpecialWeaponReward);
            settings.GameplayOption.MercilessMode.Randomize = this.RandomlyChooseSetting_MercilessMode;
            settings.GameplayOption.MercilessMode.Value = (BooleanOption)Convert.ToInt32(this.MercilessMode);

            // Charging speed options
            settings.ChargingSpeedOption.CastleBossEnergy.Randomize = this.RandomlyChooseSetting_CastleBossEnergyRefillSpeed;
            settings.ChargingSpeedOption.CastleBossEnergy.Value = this.CastleBossEnergyRefillSpeed;
            settings.ChargingSpeedOption.EnergyTank.Randomize = this.RandomlyChooseSetting_EnergyTankRefillSpeed;
            settings.ChargingSpeedOption.EnergyTank.Value = this.EnergyTankRefillSpeed;
            settings.ChargingSpeedOption.HitPoints.Randomize = this.RandomlyChooseSetting_HitPointRefillSpeed;
            settings.ChargingSpeedOption.HitPoints.Value = this.HitPointRefillSpeed;
            settings.ChargingSpeedOption.RobotMasterEnergy.Randomize = this.RandomlyChooseSetting_RobotMasterEnergyRefillSpeed;
            settings.ChargingSpeedOption.RobotMasterEnergy.Value = this.RobotMasterEnergyRefillSpeed;
            settings.ChargingSpeedOption.WeaponEnergy.Randomize = this.RandomlyChooseSetting_WeaponEnergyRefillSpeed;
            settings.ChargingSpeedOption.WeaponEnergy.Value = this.WeaponEnergyRefillSpeed;

            // Sprite options
            settings.SpriteOption.RandomizeBossSprites.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfBossSprites;
            settings.SpriteOption.RandomizeBossSprites.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfBossSprites);
            settings.SpriteOption.RandomizeEnemySprites.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfEnemySprites;
            settings.SpriteOption.RandomizeEnemySprites.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfEnemySprites);
            settings.SpriteOption.RandomizeEnvironmentSprites.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfEnvironmentSprites;
            settings.SpriteOption.RandomizeEnvironmentSprites.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfEnvironmentSprites);
            settings.SpriteOption.RandomizeItemPickupSprites.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfItemPickupSprites;
            settings.SpriteOption.RandomizeItemPickupSprites.Value = (BooleanOption)Convert.ToInt32(this.RandomlyChooseSetting_EnableRandomizationOfItemPickupSprites);
            settings.SpriteOption.RandomizeSpecialWeaponSprites.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfSpecialWeaponSprites;
            settings.SpriteOption.RandomizeSpecialWeaponSprites.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfSpecialWeaponSprites);

            // Quality of life options
            settings.QualityOfLifeOption.DisableWaterfall.Randomize = this.RandomlyChooseSetting_DisableWaterfall;
            settings.QualityOfLifeOption.DisableWaterfall.Value = (BooleanOption)Convert.ToInt32(this.DisableWaterfall);
            settings.QualityOfLifeOption.EnableLeftwardWallEjection.Randomize = this.RandomlyChooseSetting_EnableLeftwardWallEjection;
            settings.QualityOfLifeOption.EnableLeftwardWallEjection.Value = (BooleanOption)Convert.ToInt32(this.EnableLeftwardWallEjection);
            settings.QualityOfLifeOption.DisableFlashingEffects.Randomize = this.RandomlyChooseSetting_DisableFlashingEffects;
            settings.QualityOfLifeOption.DisableFlashingEffects.Value = (BooleanOption)Convert.ToInt32(this.DisableFlashingEffects);
            settings.QualityOfLifeOption.EnableUnderwaterLagReduction.Randomize = this.RandomlyChooseSetting_EnableUnderwaterLagReduction;
            settings.QualityOfLifeOption.EnableUnderwaterLagReduction.Value = (BooleanOption)Convert.ToInt32(this.EnableUnderwaterLagReduction);
            settings.QualityOfLifeOption.DisablePauseLock.Randomize = this.RandomlyChooseSetting_DisablePauseLock;
            settings.QualityOfLifeOption.DisablePauseLock.Value = (BooleanOption)Convert.ToInt32(this.DisablePauseLock);

            // Cosmetic options
            settings.CosmeticOption.Font.Randomize = this.RandomlyChooseSetting_Font;
            settings.CosmeticOption.Font.Value = this.Font;
            settings.CosmeticOption.HudElement.Randomize = this.RandomlyChooseSetting_HudElement;
            settings.CosmeticOption.HudElement.Value = this.HudElement;
            settings.CosmeticOption.PlayerSprite.Randomize = this.RandomlyChooseSetting_PlayerSprite;
            settings.CosmeticOption.PlayerSprite.Value = this.PlayerSprite;
            settings.CosmeticOption.RandomizeColorPalettes.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfColorPalettes;
            settings.CosmeticOption.RandomizeColorPalettes.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfColorPalettes);
            settings.CosmeticOption.RandomizeInGameText.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfInGameText;
            settings.CosmeticOption.RandomizeInGameText.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfInGameText);
            settings.CosmeticOption.RandomizeMusicTracks.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfMusicTracks;
            settings.CosmeticOption.RandomizeMusicTracks.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfMusicTracks);
            settings.CosmeticOption.RandomizeMenusAndTransitionScreens.Randomize = this.RandomlyChooseSetting_EnableRandomizationOfMenusAndTransitionScreens;
            settings.CosmeticOption.RandomizeMenusAndTransitionScreens.Value = (BooleanOption)Convert.ToInt32(this.EnableRandomizationOfMenusAndTransitionScreens);

            return settings;
        }


        //
        // Private Helper Methods
        //

        private void ReadSettingsFromXml(XmlReader in_Reader)
        {
            in_Reader.MoveToContent();

            while (true == in_Reader.Read())
            {
                if (false == in_Reader.IsEmptyElement &&
                    XmlNodeType.Element == in_Reader.NodeType)
                {
                    using (XmlReader xmlReader = in_Reader.ReadSubtree())
                    {
                        this.SetPropertyFromXml(xmlReader);
                        xmlReader.Close();
                    }
                }
            }
        }


        private void SetPropertyFromXml(XmlReader in_Reader)
        {
            in_Reader.MoveToContent();

            String propertyName = in_Reader.Name;
            SettingsPropertyValue settingsPropertyValue = this.PropertyValues[propertyName];

            if (null != settingsPropertyValue)
            {
                in_Reader.Read();
                in_Reader.MoveToContent();

                if (XmlNodeType.Text == in_Reader.NodeType)
                {
                    SettingsProperty settingsProperty = this.Properties[propertyName];
                    this[propertyName] = AppConfigurationSettings.ConvertFromString(in_Reader.Value, settingsProperty.PropertyType);
                }
            }
        }


        private T GetValueOrDefault<T>(String in_ValueName, T in_Default)
        {
            Object value = this[in_ValueName];
            return (value is T) ? (T)value : in_Default;
        }


        private void ValidateSeed(ref String ref_Seed)
        {
            // First, clean the seed of non-alphanumerics.  This isn't for the
            // seed generation code, but to maintain safe file names
            ref_Seed = ref_Seed.Trim().ToUpperInvariant().RemoveNonAlphanumericCharacters();

            if (true == String.IsNullOrWhiteSpace(ref_Seed))
            {
                this.IsSeedValid = false;
            }
            else
            {
                this.IsSeedValid = true;
            }
        }


        private void ValidateFile(String in_FilePath)
        {
            if (true == String.IsNullOrWhiteSpace(in_FilePath))
            {
                this.IsRomSourcePathValid = false;
                this.IsRomValid = false;
                this.HashStringSHA256 = String.Empty;
                this.HashStringMD5 = String.Empty;
                this.HashValidationMessage = String.Empty;
                return;
            }

            this.IsRomSourcePathValid = File.Exists(in_FilePath);

            if (true == this.IsRomSourcePathValid)
            {
                // Ensure file size is small so that we can take the hash
                FileInfo info = new FileInfo(in_FilePath);
                Int64 fileSize = info.Length;

                if (fileSize > AppConfigurationSettings.ONE_MEGABYTE)
                {
                    Double sizeInMegabytes = fileSize / AppConfigurationSettings.BYTES_PER_MEGABYTE;

                    this.HashValidationMessage = $"File is too large! {sizeInMegabytes:0.00} MB";
                    this.IsRomValid = false;
                }
                else
                {
                    using (FileStream fs = new FileStream(in_FilePath, FileMode.Open, FileAccess.Read))
                    {
                        using (System.Security.Cryptography.SHA256 sha = System.Security.Cryptography.SHA256.Create())
                        {
                            Byte[] hashSha256 = sha.ComputeHash(fs);
                            this.HashStringSHA256 = BitConverter.ToString(hashSha256).Replace("-", String.Empty).ToLowerInvariant();
                        }

                        fs.Seek(0, SeekOrigin.Begin);

                        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                        {
                            Byte[] hashMd5 = md5.ComputeHash(fs);
                            this.HashStringMD5 = BitConverter.ToString(hashMd5).Replace("-", "").ToLowerInvariant();
                        }
                    }

                    // Check that the hash matches a supported hash
                    this.IsRomValid =
                        EXPECTED_MD5_HASH_LIST.Contains(this.HashStringMD5) &&
                        EXPECTED_SHA256_HASH_LIST.Contains(this.HashStringSHA256);

                    if (this.IsRomValid)
                    {
                        this.HashValidationMessage = "ROM checksum is valid.";
                    }
                    else
                    {
                        this.HashValidationMessage = "ROM checksum is INVALID.";
                    }
                }
            }
            else
            {
                this.IsRomValid = false;
                this.HashValidationMessage = "File does not exist.";
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            this.OnPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        //
        // Private Static Methods
        //

        private static Object ConvertFromString(String in_Value, Type in_Type)
        {
            if (true == in_Type.IsEnum)
            {
                return Enum.Parse(in_Type, in_Value);
            }
            else
            {
                return Convert.ChangeType(in_Value, in_Type);
            }
        }


        //
        // Private Data Members
        //

        private const Boolean mEnableRandomizationOfRobotMasterStageSelection = true;
        private const Boolean mEnableRandomizationOfSpecialWeaponReward = true;
        private const Boolean mEnableRandomizationOfRefightTeleporters = true;

        private Boolean mIsRomSourcePathValid = false;
        private Boolean mIsSeedValid = false;
        private Boolean mIsRomValid = false;

        private String mHashStringMD5 = String.Empty;
        private String mHashStringSHA256 = String.Empty;
        private String mHashValidationMessage = String.Empty;


        //
        // Constants
        //

        private const Double BYTES_PER_MEGABYTE = 1024d * 1024d;
        private const Int64 ONE_MEGABYTE = 1024 * 1024;

        private readonly List<String> EXPECTED_MD5_HASH_LIST = new List<String>()
        {
            "caaeb9ee3b52839de261fd16f93103e6", // Mega Man 2 (U)
            "8e4bc5b03ffbd4ef91400e92e50dd294", // Mega Man 2 (USA)
        };

        private readonly List<String> EXPECTED_SHA256_HASH_LIST = new List<String>()
        {
            "27b5a635df33ed57ed339dfc7fd62fc603b39c1d1603adb5cdc3562a0b0d555b", // Mega Man 2 (U)
            "49136b412ff61beac6e40d0bbcd8691a39a50cd2744fdcdde3401eed53d71edf", // Mega Man 2 (USA)
        };

        // Variable Property Constants
        private const String SEED_STRING_SETTING_NAME = @"SeedString";
        private const String SEED_STRING_DEFAULT_VALUE = @"";

        private const String ROM_SOURCE_PATH_SETTING_NAME = @"RomSourcePath";
        private const String ROM_SOURCE_PATH_DEFAULT_VALUE = @"";

        // Theme Property Constants
        private const String ENABLE_APP_UI_DARK_THEME_SETTING_NAME = @"EnableAppUiDarkTheme";
        private const Boolean ENABLE_APP_UI_DARK_THEME_DEFAULT_VALUE = true;

        // Flag Property Constants
        private const String TOURNAMENT_MODE_SETTING_NAME = @"TournamentMode";
        private const Boolean TOURNAMENT_MODE_DEFAULT_VALUE = false;

        private const String CREATE_LOG_FILE_SETTING_NAME = @"CreateLogFile";
        private const Boolean CREATE_LOG_FILE_DEFAULT_VALUE = false;

        // Disable Waterfall
        private const String DISABLE_WATERFALL_SETTING_NAME = @"DisableWaterfall";
        private const Boolean DISABLE_WATERFALL_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_DISABLE_WATERFALL_SETTING_NAME = @"RandomlyChooseSetting_DisableWaterfall";
        private const Boolean RANDOMLY_CHOOSE_SETTING_DISABLE_WATERFALL_DEFAULT_VALUE = false;

        // Enable Leftward Wall Ejection
        private const String ENABLE_LEFTWARD_WALL_EJECTION_SETTING_NAME = @"EnableLeftwardWallEjection";
        private const Boolean ENABLE_LEFTWARD_WALL_EJECTION_DEFAULT_VALUE = false;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_LEFTWARD_WALL_EJECTION_SETTING_NAME = @"RandomlyChooseSetting_EnableLeftwardWallEjection";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_LEFTWARD_WALL_EJECTION_DEFAULT_VALUE = false;

        // Disable Flashing Effects
        private const String DISABLE_FLASHING_EFFECTS_SETTING_NAME = @"DisableFlashingEffects";
        private const Boolean DISABLE_FLASHING_EFFECTS_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_DISABLE_FLASHING_EFFECTS_SETTING_NAME = @"RandomlyChooseSetting_DisableFlashingEffects";
        private const Boolean RANDOMLY_CHOOSE_SETTING_DISABLE_FLASHING_EFFECTS_DEFAULT_VALUE = false;

        // Disable Pause Lock
        private const String DISABLE_PAUSE_LOCK_SETTING_NAME = @"DisablePauseLock";
        private const Boolean DISABLE_PAUSE_LOCK_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_DISABLE_PAUSE_LOCK_SETTING_NAME = @"RandomlyChooseSetting_DisablePauseLock";
        private const Boolean RANDOMLY_CHOOSE_SETTING_DISABLE_PAUSE_LOCK_DEFAULT_VALUE = false;

        // Enable Burst Chaser Mode
        private const String ENABLE_BURST_CHASER_MODE_SETTING_NAME = @"EnableBurstChaserMode";
        private const Boolean ENABLE_BURST_CHASER_MODE_DEFAULT_VALUE = false;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_BURST_CHASER_MODE_SETTING_NAME = @"RandomlyChooseSetting_EnableBurstChaserMode";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_BURST_CHASER_MODE_DEFAULT_VALUE = false;

        // Enable Faster Cutscene Text
        private const String ENABLE_FASTER_CUTSCENE_TEXT_SETTING_NAME = @"EnableFasterCutsceneText";
        private const Boolean ENABLE_FASTER_CUTSCENE_TEXT_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_FASTER_CUTSCENE_TEXT_SETTING_NAME = @"RandomlyChooseSetting_EnableFasterCutsceneText";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_FASTER_CUTSCENE_TEXT_DEFAULT_VALUE = false;

        // Enable Hidden Stage Names
        private const String ENABLE_HIDDEN_STAGE_NAMES_SETTING_NAME = @"EnableHiddenStageNames";
        private const Boolean ENABLE_HIDDEN_STAGE_NAMES_DEFAULT_VALUE = false;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_HIDDEN_STAGE_NAMES_SETTING_NAME = @"RandomlyChooseSetting_EnableHiddenStageNames";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_HIDDEN_STAGE_NAMES_DEFAULT_VALUE = false;

        // Enable Randomization of Boss Weaknesses
        private const String ENABLE_RANDOMIZATION_OF_BOSS_WEAKNESSES_SETTING_NAME = @"EnableRandomizationOfBossWeaknesses";
        private const Boolean ENABLE_RANDOMIZATION_OF_BOSS_WEAKNESSES_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_BOSS_WEAKNESSES_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfBossWeaknesses";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_BOSS_WEAKNESSES_DEFAULT_VALUE = false;

        // Enable Randomization of Boss Sprites
        private const String ENABLE_RANDOMIZATION_OF_BOSS_SPRITES_SETTING_NAME = @"EnableRandomizationOfBossSprites";
        private const Boolean ENABLE_RANDOMIZATION_OF_BOSS_SPRITES_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_BOSS_SPRITES_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfBossSprites";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_BOSS_SPRITES_DEFAULT_VALUE = false;

        // Enable Randomization of Color Palettes
        private const String ENABLE_RANDOMIZATION_OF_COLOR_PALETTES_SETTING_NAME = @"EnableRandomizationOfColorPalettes";
        private const Boolean ENABLE_RANDOMIZATION_OF_COLOR_PALETTES_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_COLOR_PALETTES_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfColorPalettes";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_COLOR_PALETTES_DEFAULT_VALUE = false;

        // Enable Randomization of Enemy Spawns
        private const String ENABLE_RANDOMIZATION_OF_ENEMY_SPAWNS_SETTING_NAME = @"EnableRandomizationOfEnemySpawns";
        private const Boolean ENABLE_RANDOMIZATION_OF_ENEMY_SPAWNS_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_SPAWNS_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfEnemySpawns";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_SPAWNS_DEFAULT_VALUE = false;

        // Enable Randomization of Enemy Sprites
        private const String ENABLE_RANDOMIZATION_OF_ENEMY_SPRITES_SETTING_NAME = @"EnableRandomizationOfEnemySprites";
        private const Boolean ENABLE_RANDOMIZATION_OF_ENEMY_SPRITES_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_SPRITES_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfEnemySprites";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_SPRITES_DEFAULT_VALUE = false;

        // Enable Randomization of Enemy Weaknesses
        private const String ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_SETTING_NAME = @"EnableRandomizationOfEnemyWeaknesses";
        private const Boolean ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfEnemyWeaknesses";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_DEFAULT_VALUE = false;

        // Enable Randomization of Environment Sprites
        private const String ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_SETTING_NAME = @"EnableRandomizationOfEnvironmentSprites";
        private const Boolean ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfEnvironmentSprites";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_DEFAULT_VALUE = false;

        // Enable Randomization of False Floors
        private const String ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_SETTING_NAME = @"EnableRandomizationOfFalseFloors";
        private const Boolean ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfFalseFloors";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_DEFAULT_VALUE = false;

        // Enable Randomization of Item Pickup Sprites
        private const String ENABLE_RANDOMIZATION_OF_ITEM_PICKUP_SPRITES_SETTING_NAME = @"EnableRandomizationOfItemPickupSprites";
        private const Boolean ENABLE_RANDOMIZATION_OF_ITEM_PICKUP_SPRITES_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ITEM_PICKUP_SPRITES_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfItemPickupSprites";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ITEM_PICKUP_SPRITES_DEFAULT_VALUE = false;

        // Enable Randomization of Music Tracks
        private const String ENABLE_RANDOMIZATION_OF_MUSIC_TRACKS_SETTING_NAME = @"EnableRandomizationOfMusicTracks";
        private const Boolean ENABLE_RANDOMIZATION_OF_MUSIC_TRACKS_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_MUSIC_TRACKS_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfMusicTracks";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_MUSIC_TRACKS_DEFAULT_VALUE = false;

        // Enable Randomization of Menus and Transition Screens
        private const String ENABLE_RANDOMIZATION_OF_MENUS_AND_TRANSITION_SCREENS_SETTING_NAME = @"EnableRandomizationOfMenusAndTransitionScreens";
        private const Boolean ENABLE_RANDOMIZATION_OF_MENUS_AND_TRANSITION_SCREENS_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_MENUS_AND_TRANSITION_SCREENS_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfMenusAndTransitionScreens";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_MENUS_AND_TRANSITION_SCREENS_DEFAULT_VALUE = false;

        // Enable Randomization of Robot Master Behavior
        private const String ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_BEHAVIOR_SETTING_NAME = @"EnableRandomizationOfRobotMasterBehavior";
        private const Boolean ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_BEHAVIOR_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_BEHAVIOR_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfRobotMasterBehavior";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_BEHAVIOR_DEFAULT_VALUE = false;

        // Enable Randomization of Robot Master Locations
        private const String ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_LOCATIONS_SETTING_NAME = @"EnableRandomizationOfRobotMasterLocations";
        private const Boolean ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_LOCATIONS_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_LOCATIONS_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfRobotMasterLocations";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_LOCATIONS_DEFAULT_VALUE = false;

        // Enable Randomization of Special Item Locations
        private const String ENABLE_RANDOMIZATION_OF_SPECIAL_ITEM_LOCATIONS_SETTING_NAME = @"EnableRandomizationOfSpecialItemLocations";
        private const Boolean ENABLE_RANDOMIZATION_OF_SPECIAL_ITEM_LOCATIONS_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_ITEM_LOCATIONS_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfSpecialItemLocations";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_ITEM_LOCATIONS_DEFAULT_VALUE = false;

        // Enable Randomization of Special Weapon Behavior
        private const String ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_BEHAVIOR_SETTING_NAME = @"EnableRandomizationOfSpecialWeaponBehavior";
        private const Boolean ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_BEHAVIOR_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_BEHAVIOR_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfSpecialWeaponBehavior";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_BEHAVIOR_DEFAULT_VALUE = false;

        // Enable Randomization of Special Weapon Sprites
        private const String ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_SPRITES_SETTING_NAME = @"EnableRandomizationOfSpecialWeaponSprites";
        private const Boolean ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_SPRITES_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_SPRITES_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfSpecialWeaponSprites";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_SPRITES_DEFAULT_VALUE = false;

        // Enable Randomization of In-Game Text
        private const String ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_SETTING_NAME = @"EnableRandomizationOfInGameText";
        private const Boolean ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfInGameText";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_DEFAULT_VALUE = false;

        // Enable Underwater Lag Reduction
        private const String ENABLE_UNDERWATER_LAG_REDUCTION_SETTING_NAME = @"EnableUnderwaterLagReduction";
        private const Boolean ENABLE_UNDERWATER_LAG_REDUCTION_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_UNDERWATER_LAG_REDUCTION_SETTING_NAME = @"RandomlyChooseSetting_EnableUnderwaterLagReduction";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_UNDERWATER_LAG_REDUCTION_DEFAULT_VALUE = false;

        // Enable Merciless Mode
        private const String MERCILESS_MODE_SETTING_NAME = @"MercilessMode";
        private const Boolean MERCILESS_MODE_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_MERCILESS_MODE_SETTING_NAME = @"RandomlyChooseSetting_MercilessMode";
        private const Boolean RANDOMLY_CHOOSE_SETTING_MERCILESS_MODE_DEFAULT_VALUE = false;

        //
        // Scalar Property Constants
        //

        // Castle Boss Energy Refill Speed
        private const String CASTLE_BOSS_ENERGY_REFILL_SPEED_SETTING_NAME = @"CastleBossEnergyRefillSpeed";
        private const ChargingSpeedOption CASTLE_BOSS_ENERGY_REFILL_SPEED_DEFAULT_VALUE = ChargingSpeedOption.Fastest;

        private const String RANDOMLY_CHOOSE_SETTING_CASTLE_BOSS_ENERGY_REFILL_SPEED_SETTING_NAME = @"RandomlyChooseSetting_CastleBossEnergyRefillSpeed";
        private const Boolean RANDOMLY_CHOOSE_SETTING_CASTLE_BOSS_ENERGY_REFILL_SPEED_DEFAULT_VALUE = false;

        // Energy Tank Refill Speed
        private const String ENERGY_TANK_REFILL_SPEED_SETTING_NAME = @"EnergyTankRefillSpeed";
        private const ChargingSpeedOption ENERGY_TANK_REFILL_SPEED_DEFAULT_VALUE = ChargingSpeedOption.Fastest;

        private const String RANDOMLY_CHOOSE_SETTING_ENERGY_TANK_REFILL_SPEED_SETTING_NAME = @"RandomlyChooseSetting_EnergyTankRefillSpeed";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENERGY_TANK_REFILL_SPEED_DEFAULT_VALUE = false;

        // Hit Point Refill Speed
        private const String HIT_POINT_REFILL_SPEED_SETTING_NAME = @"HitPointRefillSpeed";
        private const ChargingSpeedOption HIT_POINT_REFILL_SPEED_DEFAULT_VALUE = ChargingSpeedOption.Fastest;

        private const String RANDOMLY_CHOOSE_SETTING_HIT_POINT_REFILL_SPEED_SETTING_NAME = @"RandomlyChooseSetting_HitPointRefillSpeed";
        private const Boolean RANDOMLY_CHOOSE_SETTING_HIT_POINT_REFILL_SPEED_DEFAULT_VALUE = false;

        // Player Sprite
        private const String PLAYER_SPRITE_SETTING_NAME = @"PlayerSprite";
        private const PlayerSpriteOption PLAYER_SPRITE_DEFAULT_VALUE = PlayerSpriteOption.MegaMan;

        private const String RANDOMLY_CHOOSE_SETTING_PLAYER_SPRITE_SETTING_NAME = @"RandomlyChooseSetting_PlayerSprite";
        private const Boolean RANDOMLY_CHOOSE_SETTING_PLAYER_SPRITE_DEFAULT_VALUE = false;

        // HUD Element
        private const String HUD_ELEMENT_SETTING_NAME = @"HudElement";
        private const HudElementOption HUD_ELEMENT_DEFAULT_VALUE = HudElementOption.Default;

        private const String RANDOMLY_CHOOSE_SETTING_HUD_ELEMENT_SETTING_NAME = @"RandomlyChooseSetting_HudElement";
        private const Boolean RANDOMLY_CHOOSE_SETTING_HUD_ELEMENT_DEFAULT_VALUE = false;

        // Font
        private const String FONT_SETTING_NAME = @"Font";
        private const FontOption FONT_DEFAULT_VALUE = FontOption.Default;

        private const String RANDOMLY_CHOOSE_SETTING_FONT_SETTING_NAME = @"RandomlyChooseSetting_Font";
        private const Boolean RANDOMLY_CHOOSE_SETTING_FONT_DEFAULT_VALUE = false;

        // Robot Master Energy Refill Speed
        private const String ROBOT_MASTER_ENERGY_REFILL_SPEED_SETTING_NAME = @"RobotMasterEnergyRefillSpeed";
        private const ChargingSpeedOption ROBOT_MASTER_ENERGY_REFILL_SPEED_DEFAULT_VALUE = ChargingSpeedOption.Fastest;

        private const String RANDOMLY_CHOOSE_SETTING_ROBOT_MASTER_ENERGY_REFILL_SPEED_SETTING_NAME = @"RandomlyChooseSetting_RobotMasterEnergyRefillSpeed";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ROBOT_MASTER_ENERGY_REFILL_SPEED_DEFAULT_VALUE = false;

        // Weapon Dnergy Refill Speed
        private const String WEAPON_ENERGY_REFILL_SPEED_SETTING_NAME = @"WeaponEnergyRefillSpeed";
        private const ChargingSpeedOption WEAPON_ENERGY_REFILL_SPEED_DEFAULT_VALUE = ChargingSpeedOption.Fastest;

        private const String RANDOMLY_CHOOSE_SETTING_WEAPON_ENERGY_REFILL_SPEED_SETTING_NAME = @"RandomlyChooseSetting_WeaponEnergyRefillSpeed";
        private const Boolean RANDOMLY_CHOOSE_SETTING_WEAPON_ENERGY_REFILL_SPEED_DEFAULT_VALUE = false;
    }
}
