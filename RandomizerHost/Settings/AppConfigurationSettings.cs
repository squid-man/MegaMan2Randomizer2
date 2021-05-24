using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Configuration;
using System.IO;

namespace RandomizerHost.Settings
{
    [Serializable]
    public sealed class AppConfigurationSettings : ApplicationSettingsBase
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
        public String SeedString
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.SEED_STRING_SETTING_NAME,
                    AppConfigurationSettings.SEED_STRING_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.SEED_STRING_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public String RomSourcePath
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ROM_SOURCE_PATH_SETTING_NAME,
                    AppConfigurationSettings.ROM_SOURCE_PATH_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ROM_SOURCE_PATH_SETTING_NAME] = value;
                this.ValidateFile(value);
            }
        }


        //
        // Flag Properties
        //

        [UserScopedSetting]
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

        public Boolean EnableRandomizationOfSpecialWeaponReward
        {
            get
            {
                return AppConfigurationSettings.mEnableRandomizationOfSpecialWeaponReward;
            }
        }

        [UserScopedSetting]
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

        public Boolean EnableRandomizationOfRefightTeleporters
        {
            get
            {
                return AppConfigurationSettings.mEnableRandomizationOfRefightTeleporters;
            }
        }

        [UserScopedSetting]
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

        public Boolean EnableRandomizationOfRobotMasterStageSelection
        {
            get
            {
                return AppConfigurationSettings.mEnableRandomizationOfRobotMasterStageSelection;
            }
        }

        [UserScopedSetting]
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
        public Boolean EnableRandomizationOfSpecialWeaponNames
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_NAMES_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_NAMES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_NAMES_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
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

        public Boolean IsRomSourcePathValid
        {
            get
            {
                return this.mIsRomSourcePathValid;
            }

            private set
            {
                this.mIsRomSourcePathValid = value;
            }
        }

        public Boolean IsSeedValid
        {
            get
            {
                return this.mIsSeedValid;
            }

            private set
            {
                this.mIsSeedValid = value;
            }
        }

        public Boolean IsHashValid
        {
            get
            {
                return this.mIsHashValid;
            }

            private set
            {
                this.mIsHashValid = value;
            }
        }

        public String HashStringMD5
        {
            get
            {
                return this.mHashStringMD5;
            }

            private set
            {
                this.mHashStringMD5 = value;
            }
        }

        public String HashStringSHA256
        {
            get
            {
                return this.mHashStringSHA256;
            }

            private set
            {
                this.mHashStringSHA256 = value;
            }
        }

        public String HashValidationMessage
        {
            get
            {
                return this.mHashValidationMessage;
            }

            set
            {
                this.mHashValidationMessage = value;
            }
        }


        //
        // Public Methods
        //

        public MM2Randomizer.Settings AsRandomizerSettings()
        {
            MM2Randomizer.Settings settings = new MM2Randomizer.Settings();

            //settings.IsSourcePathValid;
            //settings.IsSeedValid;
            //settings.IsSourcePathAndSeedValid;
            //settings.HashStringMD5;
            //settings.HashStringSHA256;

            settings.SeedString = this.SeedString;
            settings.RomSourcePath = this.RomSourcePath;

            settings.CreateLogFile = this.CreateLogFile;
            settings.DisableDelayScrolling = this.DisableDelayScrolling;
            settings.DisableFlashingEffects = this.DisableFlashingEffects;
            settings.EnableBurstChaserMode = this.EnableBurstChaserMode;
            settings.EnableFasterCutsceneText = this.EnableFasterCutsceneText;
            settings.EnableHiddenStageNames = this.EnableHiddenStageNames;
            settings.EnableRandomizationOfBossWeaknesses = this.EnableRandomizationOfBossWeaknesses;
            settings.EnableRandomizationOfColorPalettes = this.EnableRandomizationOfColorPalettes;
            settings.EnableRandomizationOfEnemySpawns = this.EnableRandomizationOfEnemySpawns;
            settings.EnableRandomizationOfEnemyWeaknesses = this.EnableRandomizationOfEnemyWeaknesses;
            settings.EnableRandomizationOfFalseFloors = this.EnableRandomizationOfFalseFloors;
            settings.EnableRandomizationOfMusicTracks = this.EnableRandomizationOfMusicTracks;
            settings.EnableRandomizationOfRefightTeleporters = this.EnableRandomizationOfRefightTeleporters;
            settings.EnableRandomizationOfRobotMasterBehavior = this.EnableRandomizationOfRobotMasterBehavior;
            settings.EnableRandomizationOfRobotMasterLocations = this.EnableRandomizationOfRobotMasterLocations;
            settings.EnableRandomizationOfRobotMasterStageSelection = this.EnableRandomizationOfRobotMasterStageSelection;
            settings.EnableRandomizationOfSpecialItemLocations = this.EnableRandomizationOfSpecialItemLocations;
            settings.EnableRandomizationOfSpecialWeaponBehavior = this.EnableRandomizationOfSpecialWeaponBehavior;
            settings.EnableRandomizationOfSpecialWeaponNames = this.EnableRandomizationOfSpecialWeaponNames;
            settings.EnableRandomizationOfSpecialWeaponReward = this.EnableRandomizationOfSpecialWeaponReward;
            settings.EnableSpoilerFreeMode = this.EnableSpoilerFreeMode;
            settings.EnableUnderwaterLagReduction = this.EnableUnderwaterLagReduction;

            settings.CastleBossEnergyRefillSpeed = (MM2Randomizer.ChargingSpeed)this.CastleBossEnergyRefillSpeed;
            settings.EnergyTankRefillSpeed = (MM2Randomizer.ChargingSpeed)this.EnergyTankRefillSpeed;
            settings.HitPointRefillSpeed = (MM2Randomizer.ChargingSpeed)this.HitPointRefillSpeed;
            settings.PlayerSprite = (MM2Randomizer.PlayerSprite)this.PlayerSprite;
            settings.RobotMasterEnergyRefillSpeed = (MM2Randomizer.ChargingSpeed)this.RobotMasterEnergyRefillSpeed;
            settings.WeaponEnergyRefillSpeed = (MM2Randomizer.ChargingSpeed)this.WeaponEnergyRefillSpeed;

            return settings;
        }


        //
        // Private Helper Methods
        //

        private T GetValueOrDefault<T>(String in_ValueName, T in_Default)
        {
            Object value = this[in_ValueName];
            return (value is T) ? (T)value : in_Default;
        }


        public Boolean ValidateFile(String in_FilePath)
        {
            this.mIsRomSourcePathValid = File.Exists(in_FilePath);

            if (true == this.mIsRomSourcePathValid)
            {
            }
            else
            {
            }

            this.IsSourcePathAndSeedValid = this.IsSourcePathValid && this.IsSeedValid;

            if (false == this.IsSourcePathValid)
            {
                this.IsHashValid = false;
                this.HashValidationMessage = "File does not exist.";
                return false;
            }

            // Ensure file size is small so that we can take the hash
            FileInfo info = new System.IO.FileInfo(path);
            Int64 size = info.Length;

            if (size > 2000000)
            {
                Decimal MB = (size / (decimal)(1024d * 1024d));

                this.HashValidationMessage = $"File is {MB:0.00} MB, clearly not a NES ROM. WTF are you doing?";
                this.IsSourcePathValid = false;
                this.IsHashValid = false;
                return false;
            }

            // Calculate the file's hash
            String hashStrMd5 = "";
            String hashStrSha256 = "";

            // SHA256
            using (System.Security.Cryptography.SHA256Managed sha = new System.Security.Cryptography.SHA256Managed())
            {
                using (FileStream fs = new FileStream(in_FilePath, FileMode.Open, FileAccess.Read))
                {
                    Byte[] hashSha256 = sha.ComputeHash(fs);
                    hashStrSha256 = BitConverter.ToString(hashSha256).Replace("-", String.Empty).ToLowerInvariant();
                }
            }

            // MD5
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                using (FileStream fs = new FileStream(in_FilePath, FileMode.Open, FileAccess.Read))
                {
                    Byte[] hashMd5 = md5.ComputeHash(fs);
                    hashStrMd5 = BitConverter.ToString(hashMd5).Replace("-", "").ToLowerInvariant();
                }
            }

            // Update hash strings
            this.HashStringSHA256 = hashStrSha256;
            this.HashStringMD5 = hashStrMd5;

            // Check that the hash matches a supported hash
            this.IsHashValid =
                EXPECTED_MD5_HASH_LIST.Contains(this.HashStringMD5) &&
                EXPECTED_SHA256_HASH_LIST.Contains(this.HashStringSHA256);

            if (this.IsHashValid)
            {
                this.HashValidationMessage = "ROM checksum is valid.";
            }
            else
            {
                this.HashValidationMessage = "ROM checksum is INVALID.";
                return false;
            }

            return true;
        }


        //
        // Private Data Members
        //

        private const Boolean mEnableRandomizationOfRobotMasterStageSelection = true;
        private const Boolean mEnableRandomizationOfSpecialWeaponReward = true;
        private const Boolean mEnableRandomizationOfRefightTeleporters = true;

        private Boolean mIsRomSourcePathValid = false;
        private Boolean mIsSeedValid = false;
        private Boolean mIsHashValid = false;
        private String mHashStringMD5 = String.Empty;
        private String mHashStringSHA256 = String.Empty;
        private String mHashValidationMessage = String.Empty;


        //
        // Constatnts
        //

        public readonly List<String> EXPECTED_MD5_HASH_LIST = new List<String>()
        {
            "caaeb9ee3b52839de261fd16f93103e6", // Mega Man 2 (U)
            "8e4bc5b03ffbd4ef91400e92e50dd294", // Mega Man 2 (USA)
        };

        public readonly List<String> EXPECTED_SHA256_HASH_LIST = new List<String>()
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
        private const Boolean DISABLE_DELAY_SCROLLING_DEFAULT_VALUE = false;

        private const String DISABLE_FLASHING_EFFECTS_SETTING_NAME = @"DisableFlashingEffects";
        private const Boolean DISABLE_FLASHING_EFFECTS_DEFAULT_VALUE = true;

        private const String ENABLE_BURST_CHASER_MODE_SETTING_NAME = @"EnableBurstChaserMode";
        private const Boolean ENABLE_BURST_CHASER_MODE_DEFAULT_VALUE = false;

        private const String ENABLE_FASTER_CUTSCENE_TEXT_SETTING_NAME = @"EnableFasterCutsceneText";
        private const Boolean ENABLE_FASTER_CUTSCENE_TEXT_DEFAULT_VALUE = true;

        private const String ENABLE_HIDDEN_STAGE_NAMES_SETTING_NAME = @"EnableHiddenStageNames";
        private const Boolean ENABLE_HIDDEN_STAGE_NAMES_DEFAULT_VALUE = false;

        private const String ENABLE_RANDOMIZATION_OF_BOSS_WEAKNESSES_SETTING_NAME = @"EnableRandomizationOfBossWeaknesses";
        private const Boolean ENABLE_RANDOMIZATION_OF_BOSS_WEAKNESSES_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOMIZATION_OF_COLOR_PALETTES_SETTING_NAME = @"EnableRandomizationOfColorPalettes";
        private const Boolean ENABLE_RANDOMIZATION_OF_COLOR_PALETTES_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOMIZATION_OF_ENEMY_SPAWNS_SETTING_NAME = @"EnableRandomizationOfEnemySpawns";
        private const Boolean ENABLE_RANDOMIZATION_OF_ENEMY_SPAWNS_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_SETTING_NAME = @"EnableRandomizationOfEnemyWeaknesses";
        private const Boolean ENABLE_RANDOMIZATION_OF_ENEMY_WEAKNESSES_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_SETTING_NAME = @"EnableRandomizationOfFalseFloors";
        private const Boolean ENABLE_RANDOMIZATION_OF_FALSE_FLOORS_DEFAULT_VALUE = true;

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

        private const String ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_NAMES_SETTING_NAME = @"EnableRandomizationOfSpecialWeaponNames";
        private const Boolean ENABLE_RANDOMIZATION_OF_SPECIAL_WEAPON_NAMES_DEFAULT_VALUE = true;

        private const String ENABLE_SPOILER_FREE_MODE_SETTING_NAME = @"EnableSpoilerFreeMode";
        private const Boolean ENABLE_SPOILER_FREE_MODE_DEFAULT_VALUE = false;

        private const String ENABLE_UNDERWATER_LAG_REDUCTION_SETTING_NAME = @"EnableUnderwaterLagReduction";
        private const Boolean ENABLE_UNDERWATER_LAG_REDUCTION_DEFAULT_VALUE = false;

        // Scalar Property Constants
        private const String CASTLE_BOSS_ENERGY_REFILL_SPEED_SETTING_NAME = @"CastleBossEnergyRefillSpeed";
        private const ChargingSpeed CASTLE_BOSS_ENERGY_REFILL_SPEED_DEFAULT_VALUE = ChargingSpeed.Fastest;

        private const String ENERGY_TANK_REFILL_SPEED_SETTING_NAME = @"EnergyTankRefillSpeed";
        private const ChargingSpeed ENERGY_TANK_REFILL_SPEED_DEFAULT_VALUE = ChargingSpeed.Fastest;

        private const String HIT_POINT_REFILL_SPEED_SETTING_NAME = @"HitPointRefillSpeed";
        private const ChargingSpeed HIT_POINT_REFILL_SPEED_DEFAULT_VALUE = ChargingSpeed.Fastest;

        private const String PLAYER_SPRITE_SETTING_NAME = @"PlayerSprite";
        private const PlayerSprite PLAYER_SPRITE_DEFAULT_VALUE = PlayerSprite.Rockman;

        private const String ROBOT_MASTER_ENERGY_REFILL_SPEED_SETTING_NAME = @"RobotMasterEnergyRefillSpeed";
        private const ChargingSpeed ROBOT_MASTER_ENERGY_REFILL_SPEED_DEFAULT_VALUE = ChargingSpeed.Fastest;

        private const String WEAPON_ENERGY_REFILL_SPEED_SETTING_NAME = @"WeaponEnergyRefillSpeed";
        private const ChargingSpeed WEAPON_ENERGY_REFILL_SPEED_DEFAULT_VALUE = ChargingSpeed.Fastest;
    }
}
