using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MM2Randomizer
{
    public class Settings
    {
        //
        // Constructor
        //

        public Settings()
        {
        }


        //
        // Variable Properties
        //

        public String SeedString { get; set; }

        public String RomSourcePath { get; set; }


        //
        // Flag Options
        //

        public Boolean CreateLogFile { get; set; }

        public Boolean DisableDelayScrolling { get; set; }

        public Boolean DisableFlashingEffects { get; set; }

        public Boolean EnableBurstChaserMode { get; set; }

        public Boolean EnableFasterCutsceneText { get; set; }

        public Boolean EnableHiddenStageNames { get; set; }

        public Boolean EnableRandomizationOfBossWeaknesses { get; set; }

        public Boolean EnableRandomizationOfColorPalettes { get; set; }

        public Boolean EnableRandomizationOfEnemySpawns { get; set; }

        public Boolean EnableRandomizationOfEnemyWeaknesses { get; set; }

        public Boolean EnableRandomizationOfFalseFloors { get; set; }

        public Boolean EnableRandomizationOfMusicTracks { get; set; }

        public Boolean EnableRandomizationOfRefightTeleporters { get; set; }

        public Boolean EnableRandomizationOfRobotMasterBehavior { get; set; }

        public Boolean EnableRandomizationOfRobotMasterLocations { get; set; }

        public Boolean EnableRandomizationOfRobotMasterStageSelection { get; set; }

        public Boolean EnableRandomizationOfSpecialItemLocations { get; set; }

        public Boolean EnableRandomizationOfSpecialWeaponBehavior { get; set; }

        public Boolean EnableRandomizationOfSpecialWeaponNames { get; set; }

        public Boolean EnableRandomizationOfSpecialWeaponReward { get; set; }

        public Boolean EnableSpoilerFreeMode { get; set; }

        public Boolean EnableUnderwaterLagReduction { get; set; }


        //
        // Scalar Options
        //

        public ChargingSpeed CastleBossEnergyRefillSpeed { get; set; }

        public ChargingSpeed EnergyTankRefillSpeed { get; set; }

        public ChargingSpeed HitPointRefillSpeed { get; set; }

        public PlayerSprite PlayerSprite { get; set; }

        public ChargingSpeed RobotMasterEnergyRefillSpeed { get; set; }

        public ChargingSpeed WeaponEnergyRefillSpeed { get; set; }


        //
        // Public Methods
        //

        public String GetFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append((
                true == this.EnableRandomizationOfRobotMasterStageSelection &&
                true == this.EnableRandomizationOfSpecialWeaponReward &&
                true == this.EnableRandomizationOfRefightTeleporters) ? '!' : ' ');

            sb.Append(true == this.EnableRandomizationOfSpecialWeaponBehavior ? 'A' : ' ');
            sb.Append(true == this.EnableRandomizationOfBossWeaknesses ? 'B' : ' ');
            sb.Append(true == this.EnableRandomizationOfRobotMasterLocations ? 'C' : ' ');
            sb.Append(true == this.EnableRandomizationOfRobotMasterBehavior ? 'D' : ' ');
            sb.Append(true == this.EnableRandomizationOfSpecialItemLocations ? 'E' : ' ');
            sb.Append(true == this.EnableRandomizationOfEnemySpawns ? 'F' : ' ');
            sb.Append(true == this.EnableRandomizationOfEnemyWeaknesses ? 'G' : ' ');
            sb.Append(true == this.EnableRandomizationOfFalseFloors ? 'H' : ' ');

            sb.Append(true == this.EnableRandomizationOfSpecialWeaponNames ? '1' : ' ');
            sb.Append(true == this.EnableRandomizationOfColorPalettes ? '2' : ' ');
            sb.Append(true == this.EnableRandomizationOfMusicTracks ? '3' : ' ');

            sb.Append(true == this.EnableFasterCutsceneText ? 't' : ' ');
            sb.Append(true == this.EnableBurstChaserMode ? '@' : ' ');
            sb.Append(true == this.EnableHiddenStageNames ? '?' : ' ');

            return sb.ToString();
        }
    }
}
