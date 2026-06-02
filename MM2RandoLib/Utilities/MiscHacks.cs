using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using MM2Randomizer.Enums;
using MM2Randomizer.Extensions;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;
using MM2Randomizer.Randomizers;
using MM2Randomizer.Randomizers.Stages;
using MM2Randomizer.Resources;
using MM2Randomizer.Settings;
using MM2Randomizer.Settings.Options;

namespace MM2Randomizer.Utilities
{
    public static class MiscHacks
    {
        public static Dictionary<ResourceNode, ResourceNode?> ApplyOneIpsPerDir(
            RandomizationContext context,
            string basePath,
            bool canBeNull = true,
            ISeed? seed = null,
            Patch? patch = null,
            byte[]? rom = null,
            ResourceTree? resTree = null,
            bool? rebasePatch = null)
        {
            if (seed is null)
                seed = context.Seed;
            if (patch is null)
                patch = context.Patch;
            if (rom is null)
                rom = context.Rom;
            if (resTree is null)
                resTree = context.ResourceTree;

            return ApplyOneIpsPerDir(
                resTree,
                seed,
                patch,
                basePath,
                canBeNull,
                rom,
                rebasePatch);
        }

        public static  Dictionary<ResourceNode, ResourceNode?> ApplyOneIpsPerDir(
            ResourceTree resTree,
            ISeed seed,
            Patch patch,
            string basePath,
            bool canBeNull,
            byte[] rom,
            bool? rebasePatch = null)
        {
            var relRoot = resTree.Find(basePath);
            var selDirNodes = relRoot.PickOneFilePerDirectory(
                seed, 
                canBeNull,
                null,
                n => n.Name.EndsWith(".ips", StringComparison.InvariantCultureIgnoreCase));

            foreach (var (dirNode, fileNode) in selDirNodes)
            {
                if (fileNode is null)
                    continue;

                var ips = resTree.LoadResource(fileNode);
                patch.ApplyIPSPatch(rom, ips, rebasePatch);
            }

            return selDirNodes;
        }

        public static void DrawTitleScreenChanges(Patch p, String in_SeedBase26, RandomizationSettings settings)
        {
            // Adjust cursor positions
            p.Add(0x0362D4, 0x90, "Title screen Cursor top position"); // default 0x98
            p.Add(0x0362D5, 0xA0, "Title screen Cursor bottom position"); // default 0xA8

            //
            // Draw version header and value onto the title screen
            //

            string versionString = $"VER. {RandomMM2.AssemblyVersion}";
            p.Add(0x037402, versionString.AsIntroString(), "Title Screen Version Header");


            //
            // Draw the hash header and value onto the title screen
            //

            Byte[] hashHeader = "HASH ".AsIntroString();
            p.Add(0x0373C2, hashHeader, "Title Screen Hash Header");

            String seedAlpha = in_SeedBase26;
            for (Int32 i = 0; i < seedAlpha.Length; i++)
            {
                Byte value = seedAlpha[i].AsIntroCharacter();
                p.Add(0x0373C7 + i, value, "Title Screen Hash Value");
            }

            //
            // Draw the flags string onto the game start screen
            //

            Byte[] behaviorFlagHeader = "FLAG ".AsIntroString();
            p.Add(0x0373A2, behaviorFlagHeader, "Title Screen flags");

            String behaviorFlags = settings.GetBehaviorFlagsString();
            for (Int32 i = 0; i < 14; i++)
            {
                Byte value = behaviorFlags[i].AsIntroCharacter();
                p.Add(0x0373A7 + i, value, $"Title Screen Flags: {behaviorFlags[i]}");
            }

            Byte[] behaviorFlagHeader2 = "     ".AsIntroString();
            p.Add(0x037382, behaviorFlagHeader2, "Title Screen flags 2");

            for (Int32 i = 0; i < 14; i++)
            {
                Byte value = behaviorFlags[14 + i].AsIntroCharacter();
                p.Add(0x037387 + i, value, $"Title Screen Flags: {behaviorFlags[i]}");
            }

            Byte[] cosmeticFlagHeader = "COSM ".AsIntroString();
            p.Add(0x037362, cosmeticFlagHeader, "Title Screen hash");

            String cosmeticFlags = settings.GetCosmeticFlagsString();
            for (Int32 i = 0; i < cosmeticFlags.Length; i++)
            {
                Byte value = cosmeticFlags[i].AsIntroCharacter();
                p.Add(0x037367 + i , value, $"Title Screen Flags: {cosmeticFlags[i]}");
            }


            // Draw tournament mode/spoiler free information
            if (settings.IsTournament)
            {
                p.Add(0x037564,
                    settings.SettingsPreset!.TournamentTitleScreenString!.AsIntroString(), 
                    "Title Screen Tournament Text");

                // Draw Hash symbols
                // Use $B8-$BF with custom gfx, previously unused tiles after converting from MM2U to RM2
                //p.Add(0x037367, (Byte)(0xB0), "Title Screen Flags");
                //p.Add(0x037368, (Byte)(0xB1), "Title Screen Flags");
                //p.Add(0x037369, (Byte)(0xB2), "Title Screen Flags");
                //p.Add(0x03736A, (Byte)(0xB3), "Title Screen Flags");
            }
        }

        /// <summary>
        /// </summary>
        public static void SetEnergyTankChargingSpeed(Patch p, ChargingSpeedOption chargingSpeed)
        {
            Int32 address = 0x0352B2;
            p.Add(address, (Byte)chargingSpeed, "Energy Tank Charging Speed");
        }

        /// <summary>
        /// Enabling Random Weapons or Random Stages will cause the wrong Robot Master portrait to
        /// be blacked out when a stage is completed. The game uses your acquired weapons to determine
        /// which portrait to black-out. This function changes the lookup table for x and y positions
        /// of portraits to black-out based on what was randomized.
        /// </summary>
        public static void FixPortraits(Patch Patch, Boolean is8StagesRandom, RStages randomStages, Boolean isWeaponGetRandom, RWeaponGet randomWeaponGet)
        {
            // Arrays of default values for X and Y of the black square that marks out each portrait
            // Index of arrays are stage order, e.g. Heat, Air, etc.
            // Note: It's terrible to be repeating the boss list here, but at least this way
            // the connection between each boss and their offset is explicit, which would not be the
            // case if we used the EBossIndex.RobotMasters property instead.
            Dictionary<EBossIndex, Byte> portraitBG_y = new()
            {
                { EBossIndex.Heat, 0x21 },
                { EBossIndex.Air, 0x20 },
                { EBossIndex.Wood, 0x21 },
                { EBossIndex.Bubble, 0x20 },
                { EBossIndex.Quick, 0x20 },
                { EBossIndex.Flash, 0x22 },
                { EBossIndex.Metal, 0x22 },
                { EBossIndex.Crash, 0x22 },
            };
            Dictionary<EBossIndex, Byte> portraitBG_x = new()
            {
                { EBossIndex.Heat, 0x86 },
                { EBossIndex.Air, 0x8E },
                { EBossIndex.Wood, 0x96 },
                { EBossIndex.Bubble, 0x86 },
                { EBossIndex.Quick, 0x96 },
                { EBossIndex.Flash, 0x8E },
                { EBossIndex.Metal, 0x86 },
                { EBossIndex.Crash, 0x96 },
            };

            // Adjusting the sprites is not necessary because the hacked portrait graphics ("?" images)
            // only use the background, and the sprites have been blacked out. Left in for reference.
            //Byte[] portraitSprite_x = new Byte[] { 0x3C, 0x0C, 0x4C, 0x00, 0x20, 0x84, 0x74, 0xA4 };
            //Byte[] portraitSprite_y = new Byte[] { 0x10, 0x14, 0x28, 0x0C, 0x1C, 0x20, 0x10, 0x18 };

            // Apply changes to portrait arrays based on shuffled stages
            if (is8StagesRandom)
            {
                randomStages.FixPortraits(ref portraitBG_x, ref portraitBG_y);
            }

            // Apply changes to portrait arrays based on shuffled weapons. Only need a standard "if" with no "else",
            // because the arrays must be permuted twice if both randomization settings are enabled.
            if (isWeaponGetRandom)
            {
                randomWeaponGet.FixPortraits(ref portraitBG_x, ref portraitBG_y);
            }

            foreach (EBossIndex i in EBossIndex.RobotMasters)
            {
                Byte y = portraitBG_y[i];
                Byte x = portraitBG_x[i];
                Patch.Add(0x034541 + i.Offset, y, $"Stage Select Portrait {i.Offset + 1} Y-Pos Fix");
                Patch.Add(0x034549 + i.Offset, x, $"Stage Select Portrait {i.Offset + 1} X-Pos Fix");
                // Changing this sprite table misplaces their positions by default.
                //stream.Position = 0x03460D + i;
                //stream.WriteByte(portraitSprite_y[i]);
                //stream.Position = 0x034615 + i;
                //stream.WriteByte(portraitSprite_x[i]);
            }
        }

        /// <summary>
        /// Load the IPS for the specified Mega Man player sprite, or null if the specified sprite is the default. Throws FileNotFoundException if the sprite cannot be found (should never happen).
        /// </summary>
        private static byte[]? LoadMegaManSpriteIps(
            ResourceTree resTree,
            PlayerSpriteOption sprite)
        {
            string spriteName = sprite.ToString();
            var enumInfo = sprite.GetType().GetMember(spriteName).First();
            var pathAttr = enumInfo.GetCustomAttribute<PlayerSpritePathAttribute>();
            var dirAttr = enumInfo.GetCustomAttribute<PlayerSpriteParentDirectoryAttribute>();

            var relRoot = resTree.Find("SpritePatches.Characters");
            ResourceNode? tgtNode = null;
            if (pathAttr is not null)
            {
                //// Does this code even work? It's not used and hasn't been tested.
                if (pathAttr.Path == "")
                    return null;

                string searchStr = $".{pathAttr.Path}.ips";
                foreach (var node in relRoot.Descendants.Where(n => n.IsFile))
                {
                    if (string.Compare(
                        node.Path.Substring(relRoot.Path.Length),
                        searchStr,
                        StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        tgtNode = node;
                        break;
                    }
                }
            }
            else
            {
                var dirNode = relRoot.Find(dirAttr?.Name ?? spriteName);
                tgtNode = dirNode.Files.FirstOrDefault(n => string.Compare(
                    n.Name, 
                    $"{spriteName}.ips", 
                    StringComparison.InvariantCultureIgnoreCase) == 0);
                if (tgtNode is null)
                    tgtNode = dirNode.Files.FirstOrDefault(n => string.Compare(
                        n.Name,
                        $"PlayerCharacter_{spriteName}.ips",
                        StringComparison.InvariantCultureIgnoreCase) == 0);
            }

            if (tgtNode is null)
                throw new FileNotFoundException();

            return resTree.LoadResource(tgtNode);
        }

        /// <summary>
        /// Replace the player's sprite graphics with a different sprite.
        /// This method applies the graphics patch directly to the ROM at
        /// tempFileName. If 'MegaMan' is the sprite, no patch is applied.
        /// </summary>
        public static void SetNewMegaManSprite(
            ResourceTree resTree,
            Patch p, 
            byte[] rom,
            PlayerSpriteOption sprite)
        {
#if DEBUG
            // Verify all sprites work
            foreach (var spriteValue in Enum.GetValues<PlayerSpriteOption>())
                LoadMegaManSpriteIps(resTree, spriteValue);
#endif

            var ips = LoadMegaManSpriteIps(resTree, sprite)!;
            if (ips is not null)
                p.ApplyIPSPatch(rom, ips);
        }

        /// <summary>
        /// Loads the IPS patch corresponding to the specified enum value. Throws FileNotFoundException if the specified patch cannot be found (should never happen).
        /// </summary>
        /// <param name="basePath">The base path of which all options are descendants.</param>
        /// <param name="fallbackPrefix">If a resource of the form $"{basePath}.{value.ToString()}.ips" cannot be found, try $"{basePath}.{fallbackPrefix}{value.ToString()}.ips".</param>
        private static byte[] LoadEnumBasedIps<TEnum>(
            ResourceTree resTree, 
            string basePath,
            string? fallbackPrefix,
            TEnum value)
            where TEnum : struct, Enum
        {
            var relRoot = resTree.Find(basePath);
            string valueName = value.ToString();
            var node = relRoot.Files.FirstOrDefault(n => string.Compare(
                n.Name,
                $"{valueName}.ips",
                StringComparison.InvariantCultureIgnoreCase) == 0);
            if (node is null && fallbackPrefix is not null)
                node = relRoot.Files.FirstOrDefault(n => string.Compare(
                    n.Name,
                    $"{fallbackPrefix}{valueName}.ips",
                    StringComparison.InvariantCultureIgnoreCase) == 0);

            if (node is null)
                throw new FileNotFoundException();

            return resTree.LoadResource(node);
        }

        /// <summary>
        /// Applies the IPS patch corresponding to the specified enum value. Throws FileNotFoundException if the specified patch cannot be found (should never happen).
        /// </summary>
        /// <param name="p">The patcher.</param>
        /// <param name="rom">The ROM to apply the patch to.</param>
        /// <param name="basePath">The base path of which all options are descendants.</param>
        /// <param name="fallbackPrefix">If a resource of the form $"{basePath}.{value.ToString()}.ips" cannot be found, try $"{basePath}.{fallbackPrefix}{value.ToString()}.ips".</param>
        /// <param name="defaultValue">The value of TEnum which corresponds to no patch being applied.</param>
        /// <param name="rebasePatch">The patch is not aware of the relocation of the common bank and should be rebased.</param>
        private static void ApplyEnumBasedIps<TEnum>(
            ResourceTree resTree,
            Patch p,
            byte[] rom,
            string basePath,
            string? fallbackPrefix,
            TEnum? defaultValue,
            TEnum value,
            bool rebasePatch = true)
            where TEnum : struct, Enum
        {
#if DEBUG
            foreach (var testValue in Enum.GetValues<TEnum>())
                if (defaultValue is null 
                    || !testValue.Equals(defaultValue))
                    LoadEnumBasedIps(
                        resTree, 
                        basePath,
                        fallbackPrefix,
                        testValue);
#endif

            if (defaultValue is not null && value.Equals(defaultValue))
                return;

            var ips = LoadEnumBasedIps(
                resTree, basePath, fallbackPrefix, value);
            p.ApplyIPSPatch(rom, ips, rebasePatch);
        }

        /// <summary>
        /// Replace the cannot shot sprite in the game with different sprites.
        /// This method applies the graphics patch directly to the ROM at
        /// tempFileName. If 'PlasmaCannon' is the HUD element, no patch is applied.
        /// </summary>
        public static void SetNewCannonShot(
            ResourceTree resTree, 
            Patch p, 
            byte[] rom, 
            CannonShotOption cannonShot)
        {
            ApplyEnumBasedIps(resTree,
                p,
                rom,
                "SpritePatches.CannonShot",
                "CannonShot_",
                CannonShotOption.PlasmaCannon,
                cannonShot);
        }


        /// <summary>
        /// Replace the HUD elements in the game with different sprites.
        /// This method applies the graphics patch directly to the ROM at
        /// tempFileName. If 'Default' is the HUD element, no patch is applied.
        /// </summary>
        public static void SetNewHudElement(
            ResourceTree resTree,
            Patch p,
            byte[] rom,
            HudElementOption hudElement)
        {
            ApplyEnumBasedIps(resTree,
                p,
                rom,
                "SpritePatches.HudElements",
                "HudElements_",
                HudElementOption.Default,
                hudElement);
        }


        /// <summary>
        /// Replace the font in the game with different sprites.
        /// This method applies the graphics patch directly to the ROM at
        /// tempFileName. If 'Default' is the font , no patch is applied.
        /// </summary>
        public static void SetNewFont(
            ResourceTree resTree,
            Patch p,
            byte[] rom,
            FontOption font)
        {
            ApplyEnumBasedIps(resTree,
                p,
                rom,
                "SpritePatches.Fonts",
                "Font_",
                FontOption.Default,
                font);
        }

        public static void AddLargeWeaponEnergyRefillPickupsToWily5TeleporterRoom(Patch p)
        {
            // The enemy and item spawn information will be made into a data
            // structure that can be written to the ROM in logical chunks, but,
            // in the interest of time, these values are being manually added
            // to the addresses required.

            // Each stage in the game gets a total of 256 respawnable
            // items/enemies and 64 spawn-once items/enemies.
            // That is (256 * 4) + (64 * 4) == 1,280 bytes per stage.
            //
            // NOTE: pairs of stages share the same data allocation.
            //
            // Stage data starts at address 0x3610 in the ROM.
            //
            // The sequence per stage is as follows:
            //  256 bytes for respawnable item (enemies) screen locations.
            //  256 bytes for respawnable item (enemies) x-coordinate positions.
            //  256 bytes for respawnable item (enemies) y-coordinate positions.
            //  256 bytes for respawnable item (enemies) IDs.
            //  64 bytes for spawn-once item screen locations.
            //  64 bytes for spawn-once item x-coordinate positions.
            //  64 bytes for spawn-once item y-coordinate positions.
            //  64 bytes for spawn-once item IDs.

            const Byte LargeWeaponEnergyRefillType = 0x78;

            // Weapon energy for the initial teleporter room
            const Int32 LargeWeaponEnergyRefill1_EnemyRoomNumberIndicatorAddress = 0x00013626;
            const Int32 LargeWeaponEnergyRefill2_EnemyRoomNumberIndicatorAddress = 0x00013627;

            const Byte LargeWeaponEnergyRefill1_RoomNumber = 0x18;
            const Byte LargeWeaponEnergyRefill2_RoomNumber = 0x18;

            const Int32 LargeWeaponEnergyRefill1_PositionXAddress = 0x00013726;
            const Int32 LargeWeaponEnergyRefill2_PositionXAddress = 0x00013727;

            const Byte LargeWeaponEnergyRefill1_PositionX = 0x58;
            const Byte LargeWeaponEnergyRefill2_PositionX = 0xA8;

            const Int32 LargeWeaponEnergyRefill1_PositionYAddress = 0x00013826;
            const Int32 LargeWeaponEnergyRefill2_PositionYAddress = 0x00013827;

            const Byte LargeWeaponEnergyRefill1_PositionY = 0x69;
            const Byte LargeWeaponEnergyRefill2_PositionY = 0x69;

            const Int32 LargeWeaponEnergyRefill1_TypeAddress = 0x00013926;
            const Int32 LargeWeaponEnergyRefill2_TypeAddress = 0x00013927;

            p.Add(LargeWeaponEnergyRefill1_EnemyRoomNumberIndicatorAddress, LargeWeaponEnergyRefill1_RoomNumber);
            p.Add(LargeWeaponEnergyRefill2_EnemyRoomNumberIndicatorAddress, LargeWeaponEnergyRefill2_RoomNumber);

            p.Add(LargeWeaponEnergyRefill1_PositionXAddress, LargeWeaponEnergyRefill1_PositionX);
            p.Add(LargeWeaponEnergyRefill2_PositionXAddress, LargeWeaponEnergyRefill2_PositionX);

            p.Add(LargeWeaponEnergyRefill1_PositionYAddress, LargeWeaponEnergyRefill1_PositionY);
            p.Add(LargeWeaponEnergyRefill2_PositionYAddress, LargeWeaponEnergyRefill2_PositionY);

            p.Add(LargeWeaponEnergyRefill1_TypeAddress, LargeWeaponEnergyRefillType);
            p.Add(LargeWeaponEnergyRefill2_TypeAddress, LargeWeaponEnergyRefillType);


            // Weapon energy for the teleporter room with Wily Machine teleporter
            const Int32 LargeWeaponEnergyRefill3_EnemyRoomNumberIndicatorAddress = 0x00013628;
            const Int32 LargeWeaponEnergyRefill4_EnemyRoomNumberIndicatorAddress = 0x00013629;

            const Byte LargeWeaponEnergyRefill3_RoomNumber = 0x28;
            const Byte LargeWeaponEnergyRefill4_RoomNumber = 0x28;

            const Int32 LargeWeaponEnergyRefill3_PositionXAddress = 0x00013728;
            const Int32 LargeWeaponEnergyRefill4_PositionXAddress = 0x00013729;

            const Byte LargeWeaponEnergyRefill3_PositionX = 0x58;
            const Byte LargeWeaponEnergyRefill4_PositionX = 0xA8;

            const Int32 LargeWeaponEnergyRefill3_PositionYAddress = 0x00013828;
            const Int32 LargeWeaponEnergyRefill4_PositionYAddress = 0x00013829;

            const Byte LargeWeaponEnergyRefill3_PositionY = 0x69;
            const Byte LargeWeaponEnergyRefill4_PositionY = 0x69;

            const Int32 LargeWeaponEnergyRefill3_TypeAddress = 0x00013928;
            const Int32 LargeWeaponEnergyRefill4_TypeAddress = 0x00013929;


            p.Add(LargeWeaponEnergyRefill3_EnemyRoomNumberIndicatorAddress, LargeWeaponEnergyRefill3_RoomNumber);
            p.Add(LargeWeaponEnergyRefill4_EnemyRoomNumberIndicatorAddress, LargeWeaponEnergyRefill4_RoomNumber);

            p.Add(LargeWeaponEnergyRefill3_PositionXAddress, LargeWeaponEnergyRefill3_PositionX);
            p.Add(LargeWeaponEnergyRefill4_PositionXAddress, LargeWeaponEnergyRefill4_PositionX);

            p.Add(LargeWeaponEnergyRefill3_PositionYAddress, LargeWeaponEnergyRefill3_PositionY);
            p.Add(LargeWeaponEnergyRefill4_PositionYAddress, LargeWeaponEnergyRefill4_PositionY);

            p.Add(LargeWeaponEnergyRefill3_TypeAddress, LargeWeaponEnergyRefillType);
            p.Add(LargeWeaponEnergyRefill4_TypeAddress, LargeWeaponEnergyRefillType);
        }

        /// <summary>
        /// Debugging function that checks all IPS patches for changes to the common bank.
        /// </summary>
        public static void FindAllPatchesWithCommonBankChanges(ResourceTree resTree)
        {
            List<ResourceNode> nodesWithCmnBankChanges = [];
            foreach (var node in resTree.AllFiles.Where(n => n.Name.EndsWith(
                ".ips", StringComparison.InvariantCultureIgnoreCase)))
            {
                var patch = resTree.LoadResource(node);
                if (Patch.EnumIpsSegments(patch).Any(s => s.TgtOffs >= 0x3c010))
                    nodesWithCmnBankChanges.Add(node);
            }

            return;
        }
    }
}
