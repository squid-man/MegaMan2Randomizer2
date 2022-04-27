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
        public Boolean DisableDelayScrolling
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.DISABLE_DELAY_SCROLLING_SETTING_NAME,
                    AppConfigurationSettings.DISABLE_DELAY_SCROLLING_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.DISABLE_DELAY_SCROLLING_SETTING_NAME] = value;
            }
        }


        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public Boolean RandomlyChooseSetting_DisableDelayScrolling
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_DISABLE_DELAY_SCROLLING_SETTING_NAME,
                    AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_DISABLE_DELAY_SCROLLING_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.RANDOMLY_CHOOSE_SETTING_DISABLE_DELAY_SCROLLING_SETTING_NAME] = value;
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


        //
        // Scalar Properties
        //

        [UserScopedSetting]
        [DefaultSettingValue("Fastest")]
        public ChargingSpeed CastleBossEnergyRefillSpeed
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
        [DefaultSettingValue("Fastest")]
        public ChargingSpeed EnergyTankRefillSpeed
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
        [DefaultSettingValue("Fastest")]
        public ChargingSpeed HitPointRefillSpeed
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
        [DefaultSettingValue("MegaMan")]
        public PlayerSprite PlayerSprite
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
        [DefaultSettingValue("Default")]
        public HudElement HudElement
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
        [DefaultSettingValue("Fastest")]
        public ChargingSpeed RobotMasterEnergyRefillSpeed
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
        [DefaultSettingValue("Fastest")]
        public ChargingSpeed WeaponEnergyRefillSpeed
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

        public MM2Randomizer.Settings AsRandomizerSettings(Boolean in_DefaultSeed)
        {
            MM2Randomizer.Settings settings = new MM2Randomizer.Settings();

            settings.SeedString = (true == in_DefaultSeed) ? null : this.SeedString;
            settings.RomSourcePath = this.RomSourcePath;

            settings.CreateLogFile = this.CreateLogFile;
            settings.DisableDelayScrolling = this.DisableDelayScrolling;
            settings.DisableFlashingEffects = this.DisableFlashingEffects;
            settings.EnableBurstChaserMode = this.EnableBurstChaserMode;
            settings.EnableFasterCutsceneText = this.EnableFasterCutsceneText;
            settings.EnableHiddenStageNames = this.EnableHiddenStageNames;
            settings.EnableRandomizationOfBossWeaknesses = this.EnableRandomizationOfBossWeaknesses;
            settings.EnableRandomizationOfBossSprites = this.EnableRandomizationOfBossSprites;
            settings.EnableRandomizationOfColorPalettes = this.EnableRandomizationOfColorPalettes;
            settings.EnableRandomizationOfEnemySpawns = this.EnableRandomizationOfEnemySpawns;
            settings.EnableRandomizationOfEnemySprites = this.EnableRandomizationOfEnemySprites;
            settings.EnableRandomizationOfEnemyWeaknesses = this.EnableRandomizationOfEnemyWeaknesses;
            settings.EnableRandomizationOfFalseFloors = this.EnableRandomizationOfFalseFloors;
            settings.EnableRandomizationOfInGameText = this.EnableRandomizationOfInGameText;
            settings.EnableRandomizationOfMusicTracks = this.EnableRandomizationOfMusicTracks;
            settings.EnableRandomizationOfRefightTeleporters = this.EnableRandomizationOfRefightTeleporters;
            settings.EnableRandomizationOfRobotMasterBehavior = this.EnableRandomizationOfRobotMasterBehavior;
            settings.EnableRandomizationOfRobotMasterLocations = this.EnableRandomizationOfRobotMasterLocations;
            settings.EnableRandomizationOfRobotMasterStageSelection = this.EnableRandomizationOfRobotMasterStageSelection;
            settings.EnableRandomizationOfSpecialItemLocations = this.EnableRandomizationOfSpecialItemLocations;
            settings.EnableRandomizationOfSpecialWeaponBehavior = this.EnableRandomizationOfSpecialWeaponBehavior;
            settings.EnableRandomizationOfSpecialWeaponReward = this.EnableRandomizationOfSpecialWeaponReward;
            settings.EnableRandomizationOfSpecialWeaponSprites = this.EnableRandomizationOfSpecialWeaponSprites;
            settings.EnableRandomizationOfItemPickupSprites = this.EnableRandomizationOfItemPickupSprites;
            settings.EnableRandomizationOfEnvironmentSprites = this.EnableRandomizationOfEnvironmentSprites;
            settings.EnableSpoilerFreeMode = this.EnableSpoilerFreeMode;
            settings.EnableUnderwaterLagReduction = this.EnableUnderwaterLagReduction;

            settings.CastleBossEnergyRefillSpeed = this.CastleBossEnergyRefillSpeed;
            settings.EnergyTankRefillSpeed = this.EnergyTankRefillSpeed;
            settings.HitPointRefillSpeed = this.HitPointRefillSpeed;
            settings.PlayerSprite = this.PlayerSprite;
            settings.HudElement = this.HudElement;
            settings.RobotMasterEnergyRefillSpeed = this.RobotMasterEnergyRefillSpeed;
            settings.WeaponEnergyRefillSpeed = this.WeaponEnergyRefillSpeed;

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
        private const String CREATE_LOG_FILE_SETTING_NAME = @"CreateLogFile";
        private const Boolean CREATE_LOG_FILE_DEFAULT_VALUE = false;

        private const String DISABLE_DELAY_SCROLLING_SETTING_NAME = @"DisableDelayScrolling";
        private const Boolean DISABLE_DELAY_SCROLLING_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_DISABLE_DELAY_SCROLLING_SETTING_NAME = @"RandomlyChooseSetting_DisableDelayScrolling";
        private const Boolean RANDOMLY_CHOOSE_SETTING_DISABLE_DELAY_SCROLLING_DEFAULT_VALUE = false;

        private const String DISABLE_FLASHING_EFFECTS_SETTING_NAME = @"DisableFlashingEffects";
        private const Boolean DISABLE_FLASHING_EFFECTS_DEFAULT_VALUE = true;

        private const String RANDOMLY_CHOOSE_SETTING_DISABLE_FLASHING_EFFECTS_SETTING_NAME = @"RandomlyChooseSetting_DisableFlashingEffects";
        private const Boolean RANDOMLY_CHOOSE_SETTING_DISABLE_FLASHING_EFFECTS_DEFAULT_VALUE = false;

        private const String ENABLE_BURST_CHASER_MODE_SETTING_NAME = @"EnableBurstChaserMode";
        private const Boolean ENABLE_BURST_CHASER_MODE_DEFAULT_VALUE = false;

        private const String ENABLE_FASTER_CUTSCENE_TEXT_SETTING_NAME = @"EnableFasterCutsceneText";
        private const Boolean ENABLE_FASTER_CUTSCENE_TEXT_DEFAULT_VALUE = true;

        private const String ENABLE_HIDDEN_STAGE_NAMES_SETTING_NAME = @"EnableHiddenStageNames";
        private const Boolean ENABLE_HIDDEN_STAGE_NAMES_DEFAULT_VALUE = false;

        private const String ENABLE_RANDOMIZATION_OF_BOSS_WEAKNESSES_SETTING_NAME = @"EnableRandomizationOfBossWeaknesses";
        private const Boolean ENABLE_RANDOMIZATION_OF_BOSS_WEAKNESSES_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOMIZATION_OF_BOSS_SPRITES_SETTING_NAME = @"EnableRandomizationOfBossSprites";
        private const Boolean ENABLE_RANDOMIZATION_OF_BOSS_SPRITES_DEFAULT_VALUE = false;

        private const String ENABLE_RANDOMIZATION_OF_COLOR_PALETTES_SETTING_NAME = @"EnableRandomizationOfColorPalettes";
        private const Boolean ENABLE_RANDOMIZATION_OF_COLOR_PALETTES_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOMIZATION_OF_ENEMY_SPAWNS_SETTING_NAME = @"EnableRandomizationOfEnemySpawns";
        private const Boolean ENABLE_RANDOMIZATION_OF_ENEMY_SPAWNS_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOMIZATION_OF_ENEMY_SPRITES_SETTING_NAME = @"EnableRandomizationOfEnemySprites";
        private const Boolean ENABLE_RANDOMIZATION_OF_ENEMY_SPRITES_DEFAULT_VALUE = false;

        private const String ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_SETTING_NAME = @"EnableRandomizationOfEnemyWeaknesses";
        private const Boolean ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_SETTING_NAME = @"EnableRandomizationOfEnvironmentSprites";
        private const Boolean ENABLE_RANDOMIZATION_OF_ENVIRONMENT_SPRITES_DEFAULT_VALUE = false;

        private const String ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_SETTING_NAME = @"EnableRandomizationOfFalseFloors";
        private const Boolean ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOMIZATION_OF_ITEM_PICKUP_SPRITES_SETTING_NAME = @"EnableRandomizationOfItemPickupSprites";
        private const Boolean ENABLE_RANDOMIZATION_OF_ITEM_PICKUP_SPRITES_DEFAULT_VALUE = false;

        private const String ENABLE_RANDOMIZATION_OF_MUSIC_TRACKS_SETTING_NAME = @"EnableRandomizationOfMusicTracks";
        private const Boolean ENABLE_RANDOMIZATION_OF_MUSIC_TRACKS_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_BEHAVIOR_SETTING_NAME = @"EnableRandomizationOfRobotMasterBehavior";
        private const Boolean ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_BEHAVIOR_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_LOCATIONS_SETTING_NAME = @"EnableRandomizationOfRobotMasterLocations";
        private const Boolean ENABLE_RANDOMIZATION_OF_ROBOT_MASTER_LOCATIONS_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOMIZATION_OF_SPECIAL_ITEM_LOCATIONS_SETTING_NAME = @"EnableRandomizationOfSpecialItemLocations";
        private const Boolean ENABLE_RANDOMIZATION_OF_SPECIAL_ITEM_LOCATIONS_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_BEHAVIOR_SETTING_NAME = @"EnableRandomizationOfSpecialWeaponBehavior";
        private const Boolean ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_BEHAVIOR_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_SPRITES_SETTING_NAME = @"EnableRandomizationOfSpecialWeaponSprites";
        private const Boolean ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_SPRITES_DEFAULT_VALUE = false;

        private const String ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_SETTING_NAME = @"EnableRandomizationOfInGameText";
        private const Boolean ENABLE_RANDOMIZATION_OF_IN_GAME_TEXT_DEFAULT_VALUE = true;

        private const String ENABLE_SPOILER_FREE_MODE_SETTING_NAME = @"EnableSpoilerFreeMode";
        private const Boolean ENABLE_SPOILER_FREE_MODE_DEFAULT_VALUE = false;

        private const String ENABLE_UNDERWATER_LAG_REDUCTION_SETTING_NAME = @"EnableUnderwaterLagReduction";
        private const Boolean ENABLE_UNDERWATER_LAG_REDUCTION_DEFAULT_VALUE = true;

        // Scalar Property Constants
        private const String CASTLE_BOSS_ENERGY_REFILL_SPEED_SETTING_NAME = @"CastleBossEnergyRefillSpeed";
        private const ChargingSpeed CASTLE_BOSS_ENERGY_REFILL_SPEED_DEFAULT_VALUE = ChargingSpeed.Fastest;

        private const String ENERGY_TANK_REFILL_SPEED_SETTING_NAME = @"EnergyTankRefillSpeed";
        private const ChargingSpeed ENERGY_TANK_REFILL_SPEED_DEFAULT_VALUE = ChargingSpeed.Fastest;

        private const String HIT_POINT_REFILL_SPEED_SETTING_NAME = @"HitPointRefillSpeed";
        private const ChargingSpeed HIT_POINT_REFILL_SPEED_DEFAULT_VALUE = ChargingSpeed.Fastest;

        private const String PLAYER_SPRITE_SETTING_NAME = @"PlayerSprite";
        private const PlayerSprite PLAYER_SPRITE_DEFAULT_VALUE = PlayerSprite.MegaMan;

        private const String HUD_ELEMENT_SETTING_NAME = @"HudElement";
        private const HudElement HUD_ELEMENT_DEFAULT_VALUE = HudElement.Default;

        private const String ROBOT_MASTER_ENERGY_REFILL_SPEED_SETTING_NAME = @"RobotMasterEnergyRefillSpeed";
        private const ChargingSpeed ROBOT_MASTER_ENERGY_REFILL_SPEED_DEFAULT_VALUE = ChargingSpeed.Fastest;

        private const String WEAPON_ENERGY_REFILL_SPEED_SETTING_NAME = @"WeaponEnergyRefillSpeed";
        private const ChargingSpeed WEAPON_ENERGY_REFILL_SPEED_DEFAULT_VALUE = ChargingSpeed.Fastest;
    }
}
