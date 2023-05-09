using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
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
        // Flag Properties
        //
        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean SetTheme
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.SETTHEME_SETTING_NAME,
                    AppConfigurationSettings.SETTHEME_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.SETTHEME_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("True")]
        public Boolean CheckboxOn
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.CHECKBOXON_SETTING_NAME,
                    AppConfigurationSettings.CHECKBOXON_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.CHECKBOXON_SETTING_NAME] = value;
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


        [UserScopedSetting]
        [DefaultSettingValue("False")]
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


        [UserScopedSetting]
        [DefaultSettingValue("False")]
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


        [UserScopedSetting]
        [DefaultSettingValue("False")]
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


        [UserScopedSetting]
        [DefaultSettingValue("False")]
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


        [UserScopedSetting]
        [DefaultSettingValue("False")]
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


        // This property has a constant value; it does not access the app configuration
        public Boolean EnableRandomizationOfRefightTeleporters
        {
            get
            {
                return AppConfigurationSettings.mEnableRandomizationOfRefightTeleporters;
            }
        }


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


        // This property has a constant value; it does not access the app configuration
        public Boolean EnableRandomizationOfRobotMasterStageSelection
        {
            get
            {
                return AppConfigurationSettings.mEnableRandomizationOfRobotMasterStageSelection;
            }
        }


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


        [UserScopedSetting]
        [DefaultSettingValue("False")]
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


        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean EnableSpoilerFreeMode
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_SPOILER_FREE_MODE_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_SPOILER_FREE_MODE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_SPOILER_FREE_MODE_SETTING_NAME] = value;
            }
        }


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
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("IsRomSourcePathValid"));
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
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("IsSeedValid"));
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
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("IsRomValid"));
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
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("HashStringMD5"));
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
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("HashStringSHA256"));
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
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("HashValidationMessage"));
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

            settings.CreateLogFile = this.CreateLogFile;
            settings.EnableSpoilerFreeMode = this.EnableSpoilerFreeMode;
            settings.SetTheme = this.SetTheme;

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
            settings.QualityOfLifeOption.DisableFlashingEffects.Randomize = this.RandomlyChooseSetting_DisableFlashingEffects;
            settings.QualityOfLifeOption.DisableFlashingEffects.Value = (BooleanOption)Convert.ToInt32(this.DisableFlashingEffects);
            settings.QualityOfLifeOption.EnableUnderwaterLagReduction.Randomize = this.RandomlyChooseSetting_EnableUnderwaterLagReduction;
            settings.QualityOfLifeOption.EnableUnderwaterLagReduction.Value = (BooleanOption)Convert.ToInt32(this.EnableUnderwaterLagReduction);

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

        // Flag Property Constants
        private const String SETTHEME_SETTING_NAME = @"SetTheme";
        private const Boolean SETTHEME_DEFAULT_VALUE = true;

        private const String CHECKBOXON_SETTING_NAME = @"CheckboxOn";
        private const Boolean CHECKBOXON_DEFAULT_VALUE = true;

        // Flag Property Constants
        private const String CREATE_LOG_FILE_SETTING_NAME = @"CreateLogFile";
        private const Boolean CREATE_LOG_FILE_DEFAULT_VALUE = false;

        // Disable Waterfall
        private const String DISABLE_WATERFALL_SETTING_NAME = @"DisableWaterfall";
        private const Boolean DISABLE_WATERFALL_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_DISABLE_WATERFALL_SETTING_NAME = @"RandomlyChooseSetting_DisableWaterfall";
        private const Boolean RANDOMLY_CHOOSE_SETTING_DISABLE_WATERFALL_DEFAULT_VALUE = false;

        // Disable Flashing Effects
        private const String DISABLE_FLASHING_EFFECTS_SETTING_NAME = @"DisableFlashingEffects";
        private const Boolean DISABLE_FLASHING_EFFECTS_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_DISABLE_FLASHING_EFFECTS_SETTING_NAME = @"RandomlyChooseSetting_DisableFlashingEffects";
        private const Boolean RANDOMLY_CHOOSE_SETTING_DISABLE_FLASHING_EFFECTS_DEFAULT_VALUE = false;

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
        private const Boolean ENABLE_RANDOMIZATION_OF_BOSS_SPRITES_DEFAULT_VALUE = false;

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
        private const Boolean ENABLE_RANDOMIZATION_OF_ENEMY_SPRITES_DEFAULT_VALUE = false;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_SPRITES_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfEnemySprites";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_SPRITES_DEFAULT_VALUE = false;

        // Enable Randomization of Enemy Weaknesses
        private const String ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_SETTING_NAME = @"EnableRandomizationOfEnemyWeaknesses";
        private const Boolean ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfEnemyWeaknesses";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_DEFAULT_VALUE = false;

        // Enable Randomization of Environment Sprites
        private const String ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_SETTING_NAME = @"EnableRandomizationOfEnvironmentSprites";
        private const Boolean ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_DEFAULT_VALUE = false;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfEnvironmentSprites";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_DEFAULT_VALUE = false;

        // Enable Randomization of False Floors
        private const String ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_SETTING_NAME = @"EnableRandomizationOfFalseFloors";
        private const Boolean ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfFalseFloors";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_DEFAULT_VALUE = false;

        // Enable Randomization of Item Pickup Sprites
        private const String ENABLE_RANDOMIZATION_OF_ITEM_PICKUP_SPRITES_SETTING_NAME = @"EnableRandomizationOfItemPickupSprites";
        private const Boolean ENABLE_RANDOMIZATION_OF_ITEM_PICKUP_SPRITES_DEFAULT_VALUE = false;

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
        private const Boolean ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_SPRITES_DEFAULT_VALUE = false;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_SPRITES_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfSpecialWeaponSprites";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_SPRITES_DEFAULT_VALUE = false;

        // Enable Randomization of In-Game Text
        private const String ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_SETTING_NAME = @"EnableRandomizationOfInGameText";
        private const Boolean ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_SETTING_NAME = @"RandomlyChooseSetting_EnableRandomizationOfInGameText";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_DEFAULT_VALUE = false;

        // Enable Spoiler-Free Mode
        private const String ENABLE_SPOILER_FREE_MODE_SETTING_NAME = @"EnableSpoilerFreeMode";
        private const Boolean ENABLE_SPOILER_FREE_MODE_DEFAULT_VALUE = false;

        // Enable Underwater Lag Reduction
        private const String ENABLE_UNDERWATER_LAG_REDUCTION_SETTING_NAME = @"EnableUnderwaterLagReduction";
        private const Boolean ENABLE_UNDERWATER_LAG_REDUCTION_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_ENABLE_UNDERWATER_LAG_REDUCTION_SETTING_NAME = @"RandomlyChooseSetting_EnableUnderwaterLagReduction";
        private const Boolean RANDOMLY_CHOOSE_SETTING_ENABLE_UNDERWATER_LAG_REDUCTION_DEFAULT_VALUE = false;

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
