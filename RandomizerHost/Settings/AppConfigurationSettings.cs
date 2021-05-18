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
        // Editable Properties
        //

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
                this.mIsRomSourcePathValid = File.Exists(value);
            }
        }

        [UserScopedSetting]
        public Boolean EnableWeaponBehaviorModule
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_WEAPON_BEHAVIOR_MODULE_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_WEAPON_BEHAVIOR_MODULE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_WEAPON_BEHAVIOR_MODULE_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public Boolean EnableBossWeaknessModule
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_BOSS_WEAKNESS_MODULE_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_BOSS_WEAKNESS_MODULE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_BOSS_WEAKNESS_MODULE_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public Boolean EnableBossRoomModule
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_BOSS_ROOM_MODULE_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_BOSS_ROOM_MODULE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_BOSS_ROOM_MODULE_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public Boolean EnableBossBehaviorModule
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_BOSS_BEHAVIOR_MODULE_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_BOSS_BEHAVIOR_MODULE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_BOSS_BEHAVIOR_MODULE_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public Boolean EnableSpecialItemsModule
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_SPECIAL_ITEMS_MODULE_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_SPECIAL_ITEMS_MODULE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_SPECIAL_ITEMS_MODULE_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public Boolean EnableEnemySpawnsModule
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_ENEMY_SPAWNS_MODULE_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_ENEMY_SPAWNS_MODULE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_ENEMY_SPAWNS_MODULE_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public Boolean EnableEnemyWeaknessModule
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_ENEMY_WEAKNESS_MODULE_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_ENEMY_WEAKNESS_MODULE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_ENEMY_WEAKNESS_MODULE_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public Boolean EnableStageLayoutPatches
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_STAGE_LAYOUT_PATCHES_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_STAGE_LAYOUT_PATCHES_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_STAGE_LAYOUT_PATCHES_SETTING_NAME] = value;
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
        public Boolean EnableBurstChaser
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_BURST_CHASER_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_BURST_CHASER_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_BURST_CHASER_SETTING_NAME] = value;
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
        public Boolean EnableRandomColorPalette
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOM_COLOR_PALETTE_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOM_COLOR_PALETTE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOM_COLOR_PALETTE_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public Boolean EnableRandomStageMusic
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOM_STAGE_MUSIC_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOM_STAGE_MUSIC_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOM_STAGE_MUSIC_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public Boolean EnableRandomText
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_RANDOM_TEXT_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_RANDOM_TEXT_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_RANDOM_TEXT_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public Boolean DisableScreenFlashing
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.DISABLE_SCREEN_FLASHING_SETTING_NAME,
                    AppConfigurationSettings.DISABLE_SCREEN_FLASHING_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.DISABLE_SCREEN_FLASHING_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public CharacterSprite CharacterSprite
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.CHARACTER_SPRITE_SETTING_NAME,
                    AppConfigurationSettings.CHARACTER_SPRITE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.CHARACTER_SPRITE_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public ChargingSpeed RefillSpeedHitPoints
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.REFILL_SPEED_HIT_POINTS_SETTING_NAME,
                    AppConfigurationSettings.REFILL_SPEED_HIT_POINTS_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.REFILL_SPEED_HIT_POINTS_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public ChargingSpeed RefillSpeedWeaponEnergy
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.REFILL_SPEED_WEAPON_ENERGY_SETTING_NAME,
                    AppConfigurationSettings.REFILL_SPEED_WEAPON_ENERGY_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.REFILL_SPEED_WEAPON_ENERGY_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public ChargingSpeed RefillSpeedEnergyTank
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.REFILL_SPEED_ENERGY_TANK_SETTING_NAME,
                    AppConfigurationSettings.REFILL_SPEED_ENERGY_TANK_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.REFILL_SPEED_ENERGY_TANK_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public ChargingSpeed RefillSpeedRobotMasterEnergy
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.REFILL_SPEED_ROBOT_MASTER_ENERGY_SETTING_NAME,
                    AppConfigurationSettings.REFILL_SPEED_ROBOT_MASTER_ENERGY_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.REFILL_SPEED_ROBOT_MASTER_ENERGY_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public ChargingSpeed RefillSpeedCastleBossEnergy
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.REFILL_SPEED_CASTLE_BOSS_ENERGY_SETTING_NAME,
                    AppConfigurationSettings.REFILL_SPEED_CASTLE_BOSS_ENERGY_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.REFILL_SPEED_CASTLE_BOSS_ENERGY_SETTING_NAME] = value;
            }
        }

        [UserScopedSetting]
        public Boolean EnableReduceUnderwaterLag
        {
            get
            {
                return this.GetValueOrDefault(
                    AppConfigurationSettings.ENABLE_REDUCE_UNDERWATER_LAG_SETTING_NAME,
                    AppConfigurationSettings.ENABLE_REDUCE_UNDERWATER_LAG_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettings.ENABLE_REDUCE_UNDERWATER_LAG_SETTING_NAME] = value;
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


        //
        // Private Helper Methods
        //

        private T GetValueOrDefault<T>(String in_ValueName, T in_Default)
        {
            Object value = this[in_ValueName];
            return (value is T) ? (T)value : in_Default;
        }

        public Boolean ValidateFile(String path)
        {
            // Check if file even exists
            //SourcePath = path;
            this.IsSourcePathValid = System.IO.File.Exists(path);
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
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    Byte[] hashSha256 = sha.ComputeHash(fs);
                    hashStrSha256 = BitConverter.ToString(hashSha256).Replace("-", String.Empty).ToLowerInvariant();
                }
            }

            // MD5
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    Byte[] hashMd5 = md5.ComputeHash(fs);
                    hashStrMd5 = BitConverter.ToString(hashMd5).Replace("-", "").ToLowerInvariant();
                }
            }

            // Update hash strings
            this.HashStringSHA256 = hashStrSha256;
            this.HashStringMD5 = hashStrMd5;

            // Check that the hash matches a supported hash
            List<String> md5s = new List<String>(EXPECTED_MD5_HASH_LIST);
            List<String> sha256s = new List<String>(EXPECTED_SHA256_HASH_LIST);

            this.IsHashValid = (md5s.Contains(this.HashStringMD5) && sha256s.Contains(this.HashStringSHA256));


            if (this.IsHashValid)
            {
                this.HashValidationMessage = "ROM checksum is valid, good to go!";
            }
            else
            {
                this.HashValidationMessage = "Wrong file checksum. Please try another ROM, or it may not work.";
                return false;
            }

            // If we made it this far, the file looks good!
            return true;
        }


        //
        // Private Data Members
        //

        private Boolean mIsRomSourcePathValid;

        //
        // Constatnts
        //

        private const String ROM_SOURCE_PATH_SETTING_NAME = @"RomSourcePath";
        private const String ROM_SOURCE_PATH_DEFAULT_VALUE = @"";

        private const String ENABLE_WEAPON_BEHAVIOR_MODULE_SETTING_NAME = @"EnableWeaponBehaviorModule";
        private const Boolean ENABLE_WEAPON_BEHAVIOR_MODULE_DEFAULT_VALUE = true;

        private const String ENABLE_BOSS_WEAKNESS_MODULE_SETTING_NAME = @"EnableBossWeaknessModule";
        private const Boolean ENABLE_BOSS_WEAKNESS_MODULE_DEFAULT_VALUE = true;

        private const String ENABLE_BOSS_ROOM_MODULE_SETTING_NAME = @"EnableBossRoomModule";
        private const Boolean ENABLE_BOSS_ROOM_MODULE_DEFAULT_VALUE = true;

        private const String ENABLE_BOSS_BEHAVIOR_MODULE_SETTING_NAME = @"EnableBossBehaviorModule";
        private const Boolean ENABLE_BOSS_BEHAVIOR_MODULE_DEFAULT_VALUE = true;

        private const String ENABLE_SPECIAL_ITEMS_MODULE_SETTING_NAME = @"EnableSpecialItemsModule";
        private const Boolean ENABLE_SPECIAL_ITEMS_MODULE_DEFAULT_VALUE = true;

        private const String ENABLE_ENEMY_SPAWNS_MODULE_SETTING_NAME = @"EnableEnemySpawnsModule";
        private const Boolean ENABLE_ENEMY_SPAWNS_MODULE_DEFAULT_VALUE = true;

        private const String ENABLE_ENEMY_WEAKNESS_MODULE_SETTING_NAME = @"EnableEnemyWeaknessModule";
        private const Boolean ENABLE_ENEMY_WEAKNESS_MODULE_DEFAULT_VALUE = true;

        private const String ENABLE_STAGE_LAYOUT_PATCHES_SETTING_NAME = @"EnableStageLayoutPatches";
        private const Boolean ENABLE_STAGE_LAYOUT_PATCHES_DEFAULT_VALUE = true;

        private const String ENABLE_FASTER_CUTSCENE_TEXT_SETTING_NAME = @"EnableFasterCutsceneText";
        private const Boolean ENABLE_FASTER_CUTSCENE_TEXT_DEFAULT_VALUE = true;

        private const String ENABLE_BURST_CHASER_SETTING_NAME = @"EnableBurstChaser";
        private const Boolean ENABLE_BURST_CHASER_DEFAULT_VALUE = false;

        private const String ENABLE_HIDDEN_STAGE_NAMES_SETTING_NAME = @"EnableHiddenStageNames";
        private const Boolean ENABLE_HIDDEN_STAGE_NAMES_DEFAULT_VALUE = false;

        private const String ENABLE_RANDOM_COLOR_PALETTE_SETTING_NAME = @"EnableRandomColorPalette";
        private const Boolean ENABLE_RANDOM_COLOR_PALETTE_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOM_STAGE_MUSIC_SETTING_NAME = @"EnableRandomColorPalette";
        private const Boolean ENABLE_RANDOM_STAGE_MUSIC_DEFAULT_VALUE = true;

        private const String ENABLE_RANDOM_TEXT_SETTING_NAME = @"EnableRandomText";
        private const Boolean ENABLE_RANDOM_TEXT_DEFAULT_VALUE = true;

        private const String DISABLE_SCREEN_FLASHING_SETTING_NAME = @"DisableScreenFlashing";
        private const Boolean DISABLE_SCREEN_FLASHING_DEFAULT_VALUE = true;

        private const String CHARACTER_SPRITE_SETTING_NAME = @"CharacterSprite";
        private const CharacterSprite CHARACTER_SPRITE_DEFAULT_VALUE = CharacterSprite.Rockman;

        private const String REFILL_SPEED_HIT_POINTS_SETTING_NAME = @"RefillSpeedHitPoints";
        private const ChargingSpeed REFILL_SPEED_HIT_POINTS_DEFAULT_VALUE = ChargingSpeed.Fastest;

        private const String REFILL_SPEED_WEAPON_ENERGY_SETTING_NAME = @"RefillSpeedWeaponEnergy";
        private const ChargingSpeed REFILL_SPEED_WEAPON_ENERGY_DEFAULT_VALUE = ChargingSpeed.Fastest;

        private const String REFILL_SPEED_ENERGY_TANK_SETTING_NAME = @"RefillSpeedEnergyTank";
        private const ChargingSpeed REFILL_SPEED_ENERGY_TANK_DEFAULT_VALUE = ChargingSpeed.Fastest;

        private const String REFILL_SPEED_ROBOT_MASTER_ENERGY_SETTING_NAME = @"RefillSpeedRobotMasterEnergy";
        private const ChargingSpeed REFILL_SPEED_ROBOT_MASTER_ENERGY_DEFAULT_VALUE = ChargingSpeed.Fastest;

        private const String REFILL_SPEED_CASTLE_BOSS_ENERGY_SETTING_NAME = @"RefillSpeedCastleBossEnergy";
        private const ChargingSpeed REFILL_SPEED_CASTLE_BOSS_ENERGY_DEFAULT_VALUE = ChargingSpeed.Fastest;

        private const String ENABLE_REDUCE_UNDERWATER_LAG_SETTING_NAME = @"EnableReduceUnderwaterLag";
        private const Boolean ENABLE_REDUCE_UNDERWATER_LAG_DEFAULT_VALUE = false;

        private const String DISABLE_DELAY_SCROLLING_SETTING_NAME = @"DisableDelayScrolling";
        private const Boolean DISABLE_DELAY_SCROLLING_DEFAULT_VALUE = false;
    }
}
