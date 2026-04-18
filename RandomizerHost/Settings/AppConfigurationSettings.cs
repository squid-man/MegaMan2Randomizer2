using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Hashing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MM2RandoLib.Settings.Options;
using MM2Randomizer;
using MM2Randomizer.Extensions;
using MM2Randomizer.Settings;
using MM2Randomizer.Settings.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace RandomizerHost.Settings
{
    public partial class AppConfigurationSettings : ReactiveObject
    {
        [JsonIgnore]
        public readonly RandomizationSettings RandomizationSettings = new();

        [Reactive]
        public string SeedString { get; set; } = "";

        [Reactive]
        public string RomSourcePath { get; set; } = "";

        [Reactive]
        public bool EnableAppUiDarkTheme { get; set; } = true;

        [Reactive]
        public bool CreateLogFile { get; set; } = false;

        /*bool IsRomSourcePathValid;
        bool IsSeedValid;
        bool IsRomValid;
        bool IsRomValidText;
        bool RomStatusTooltip;
        bool HashValidationMessage;*/

        public void ReadXml(XmlReader in_Reader)
        { }

        public void WriteXml(XmlWriter in_Writer)
        { }

        /*public void UpdateRandomizerSettings(bool defaultSeed)
        {
            var settings = RandomizationSettings;

            settings.SeedString = (true == defaultSeed) ? null : SeedString;
            settings.RomSourcePath = RomSourcePath;
            settings.CreateLogFile = CreateLogFile && !settings.IsTournament;
        }*/
    }

    /*public sealed class AppConfigurationSettingsOld : ApplicationSettingsBase, IXmlSerializable
    {
        //
        // Constructor
        //
        public AppConfigurationSettingsOld()
        {
            RandomizationSettingsAdapter = new(
                this.RandomizationSettings, 
                Providers[nameof(LocalFileSettingsProvider)]);
        }


        //
        // Variable Properties
        //

        public readonly RandomizationSettings RandomizationSettings = new();

        public RandomizationSettingsAdapter RandomizationSettingsAdapter { get; }

        [UserScopedSetting]
        [DefaultSettingValue("")]
        public String SeedString
        {
            get
            {
                String value = this.GetValueOrDefault(
                    AppConfigurationSettingsOld.SEED_STRING_SETTING_NAME,
                    AppConfigurationSettingsOld.SEED_STRING_DEFAULT_VALUE);

                this.ValidateSeed(ref value);

                return value;
            }

            set
            {
                this[AppConfigurationSettingsOld.SEED_STRING_SETTING_NAME] = value;
            }
        }


        [UserScopedSetting]
        [DefaultSettingValue("")]
        public String RomSourcePath
        {
            get
            {
                String value = this.GetValueOrDefault(
                    AppConfigurationSettingsOld.ROM_SOURCE_PATH_SETTING_NAME,
                    AppConfigurationSettingsOld.ROM_SOURCE_PATH_DEFAULT_VALUE);

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
                this[AppConfigurationSettingsOld.ROM_SOURCE_PATH_SETTING_NAME] = value;
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
                    AppConfigurationSettingsOld.ENABLE_APP_UI_DARK_THEME_SETTING_NAME,
                    AppConfigurationSettingsOld.ENABLE_APP_UI_DARK_THEME_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettingsOld.ENABLE_APP_UI_DARK_THEME_SETTING_NAME] = value;
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
                    AppConfigurationSettingsOld.CREATE_LOG_FILE_SETTING_NAME,
                    AppConfigurationSettingsOld.CREATE_LOG_FILE_DEFAULT_VALUE);
            }

            set
            {
                this[AppConfigurationSettingsOld.CREATE_LOG_FILE_SETTING_NAME] = value;
            }
        }


        //
        // Scalar Properties
        //

        //
        // Buttons
        //

        private Boolean mEnable_ImportSettings = true;

        public Boolean Enable_ImportSettings
        {
            get => mEnable_ImportSettings;
            set => RaiseAndSetIfChanged(ref mEnable_ImportSettings, value);
        }

        private Boolean mEnable_ExportSettings = true;

        public Boolean Enable_ExportSettings
        {
            get => mEnable_ExportSettings;
            set => RaiseAndSetIfChanged(ref mEnable_ExportSettings, value);
        }


        //
        // Read-only Properties
        //

        // This property has a constant value; it does not access the app configuration
        public Boolean IsRomSourcePathValid
        {
            get => mIsRomSourcePathValid;
            private set => RaiseAndSetIfChanged(ref mIsRomSourcePathValid, value);
        }


        // This property has a constant value; it does not access the app configuration
        public Boolean IsSeedValid
        {
            get => mIsSeedValid;
            private set => RaiseAndSetIfChanged(ref mIsSeedValid, value);
        }


        // This property has a constant value; it does not access the app configuration
        public Boolean IsRomValid
        {
            get => mIsRomValid;
            private set => RaiseAndSetIfChanged(ref mIsRomValid, value);
        }


        // This property has a constant value; it does not access the app configuration
        public String IsRomValidText
        {
            get => mIsRomValidText;
            private set => RaiseAndSetIfChanged(ref mIsRomValidText, value);
        }


        // This property has a constant value; it does not access the app configuration
        public String RomStatusTooltip
        {
            get => mRomStatusTooltip;
            private set => RaiseAndSetIfChanged(ref mRomStatusTooltip, value);
        }


        // This property has a constant value; it does not access the app configuration
        public String HashValidationMessage
        {
            get => mHashValidationMessage;
            set => RaiseAndSetIfChanged(ref mHashValidationMessage, value);
        }


        //
        // IXmlSerializable Methods
        //

        public XmlSchema? GetSchema()
        {
            return null;
        }


        public void ReadXml(XmlReader in_Reader)
        {
            in_Reader.MoveToContent();

            while (in_Reader.Read())
            {
                if (!in_Reader.IsEmptyElement &&
                    in_Reader.NodeType == XmlNodeType.Element)
                {
                    using (XmlReader xmlReader = in_Reader.ReadSubtree())
                    {
                        if (in_Reader.Name == GLOBAL_SETTINGS_ELEMENT_NAME)
                            ReadGlobalSettingsFromXml(xmlReader);
                        else if (in_Reader.Name == RANDOMIZATION_SETTINGS_ELEMENT_NAME)
                            ReadRandomizationSettingsFromXml(xmlReader);

                        //// TODO: Should really log failures or something
                    }
                }
            }
        }


        public void WriteXml(XmlWriter in_Writer)
        {
            in_Writer.WriteStartElement(SETTINGS_ELEMENT_NAME);
            {
                in_Writer.WriteStartElement(GLOBAL_SETTINGS_ELEMENT_NAME);
                {
                    foreach (SettingsPropertyValue settingsPropertyValue in this.PropertyValues)
                    {
                        in_Writer.WriteStartElement(settingsPropertyValue.Name);
                        in_Writer.WriteString(settingsPropertyValue.SerializedValue.ToString());
                        in_Writer.WriteEndElement();
                    }
                }
                in_Writer.WriteEndElement();

                in_Writer.WriteStartElement(RANDOMIZATION_SETTINGS_ELEMENT_NAME);
                {
                    foreach (var (grpPath, grp) in RandomizationSettings.GroupsByPath.Where(kv => kv.Value.Options.Count != 0))
                    {
                        in_Writer.WriteStartElement(OPTION_GROUP_ELEMENT_NAME);
                        {
                            in_Writer.WriteAttributeString(
                                OPTION_GROUP_PATH_ATTRIBUTE_NAME, 
                                grpPath);
                            
                            foreach (var opt in grp.Options.Where(opt => opt.Info!.SaveLoad))
                            {
                                in_Writer.WriteStartElement(OPTION_ELEMENT_NAME);

                                in_Writer.WriteAttributeString(
                                    OPTION_NAME_ATTRIBUTE_NAME, 
                                    opt.Info!.Name);
                                in_Writer.WriteAttributeString(
                                    OPTION_RANDOMIZE_ATTRIBUTE_NAME, 
                                    opt.Randomize.ToString());
                                in_Writer.WriteAttributeString(
                                    OPTION_VALUE_ATTRIBUTE_NAME, 
                                    opt.BaseValue.ToString());

                                in_Writer.WriteEndElement();
                            }
                        }
                        in_Writer.WriteEndElement();
                    }
                }
                in_Writer.WriteEndElement();
            }
            in_Writer.WriteEndElement();
        }


        //
        // Public Methods
        //

        public override void Save()
        {
            base.Save();
            this.RandomizationSettingsAdapter.Save();
        }

        public void UpdateRandomizerSettings(Boolean in_DefaultSeed)
        {
            var settings = RandomizationSettings;

            settings.SeedString = (true == in_DefaultSeed) ? null : this.SeedString;
            settings.RomSourcePath = this.RomSourcePath;
            settings.CreateLogFile = this.CreateLogFile && !settings.IsTournament;
        }


        //
        // Private Helper Methods
        //

        private void ReadGlobalSettingsFromXml(XmlReader in_Reader)
        {
            in_Reader.MoveToContent();

            while (true == in_Reader.Read())
            {
                if (false == in_Reader.IsEmptyElement &&
                    XmlNodeType.Element == in_Reader.NodeType)
                {
                    using (XmlReader xmlReader = in_Reader.ReadSubtree())
                        this.SetPropertyFromXml(xmlReader);
                }
            }
        }

        private void ReadRandomizationSettingsFromXml(XmlReader in_Reader)
        {
            if (!in_Reader.ReadToDescendant(OPTION_GROUP_ELEMENT_NAME))
                return;

            do
            {
                string? grpPath = in_Reader.GetAttribute(
                    OPTION_GROUP_PATH_ATTRIBUTE_NAME);
                if (grpPath is null
                    || !in_Reader.ReadToDescendant(OPTION_ELEMENT_NAME))
                    continue;

                do
                {
                    string? name = in_Reader.GetAttribute(
                        OPTION_NAME_ATTRIBUTE_NAME),
                        rndStr = in_Reader.GetAttribute(
                            OPTION_RANDOMIZE_ATTRIBUTE_NAME),
                        valueStr = in_Reader.GetAttribute(
                            OPTION_VALUE_ATTRIBUTE_NAME);
                    if (string.IsNullOrEmpty(name)
                        || string.IsNullOrEmpty(rndStr)
                        || string.IsNullOrEmpty(valueStr))
                        continue;

                    string path = string.Join(".", grpPath, name);
                    IOption? opt;
                    if (!RandomizationSettings.OptionsByPath.TryGetValue(
                        path, out opt))
                        continue;

                    try
                    {
                        bool rnd = bool.Parse(rndStr);
                        object value = opt.ParseType(valueStr);

                        // Ensure both parse correctly before modifying option
                        opt.Randomize = rnd;
                        opt.BaseValue = value;

                    }
                    catch (InvalidCastException)
                    {
                        //// TODO: Log something
                        continue;
                    }
                } while (in_Reader.ReadToNextSibling(OPTION_ELEMENT_NAME));
            } while (in_Reader.ReadToNextSibling(OPTION_GROUP_ELEMENT_NAME));

            return;
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
                    this[propertyName] = AppConfigurationSettingsOld.ConvertFromString(in_Reader.Value, settingsProperty.PropertyType);
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
            ref_Seed = ref_Seed.Trim().ToUpperInvariant().RemoveNonAlphanumericCharacters()!;

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
                this.IsRomValidText = "";
                this.RomStatusTooltip = "";
                this.HashValidationMessage = String.Empty;
                return;
            }

            this.IsRomValidText = "❌";

            this.IsRomSourcePathValid = File.Exists(in_FilePath);

            if (true == this.IsRomSourcePathValid)
            {
                // Ensure file size is small so that we can take the hash
                FileInfo info = new FileInfo(in_FilePath);
                Int64 fileSize = info.Length;

                if (fileSize > AppConfigurationSettingsOld.ONE_MEGABYTE)
                {
                    Double sizeInMegabytes = fileSize / AppConfigurationSettingsOld.BYTES_PER_MEGABYTE;

                    this.HashValidationMessage = $"File is too large! {sizeInMegabytes:0.00} MB";
                    this.IsRomValid = false;
                }
                else
                {
                    byte[]? file = File.ReadAllBytes(in_FilePath),
                        rom = file[0x10..(file.Length - 1)];

                    Dictionary<string, Func<byte[], byte[]>> romHashAlgs = new()
                    {
                        { "CRC", rom => Crc32.Hash(rom).Reverse().ToArray() },
                        { "MD5", MD5.HashData},
                    };
                    Dictionary<string, Func<byte[], byte[]>> fileHashAlgs = new()
                    {
                        { "CRC", rom => Crc32.Hash(rom).Reverse().ToArray() },
                        { "MD5", MD5.HashData },
                        { "SHA-1", SHA1.HashData },
                        { "SHA-256", SHA256.HashData },
                    };

                    var ByteToString = (byte[] b) => BitConverter.ToString(b)
                        .Replace("-", String.Empty).ToLowerInvariant();
                    var romHashes = romHashAlgs.ToDictionary(nf => nf.Key, 
                        nf => ByteToString(nf.Value(rom)));
                    var fileHashes = fileHashAlgs.ToDictionary(nf => nf.Key,
                        nf => ByteToString(nf.Value(file)));

                    StringBuilder tipSb = new();
                    tipSb.AppendLine("ROM");
                    foreach (var (name, fn) in romHashAlgs)
                        tipSb.AppendLine($"{name}: {ByteToString(fn(rom))}");

                    tipSb.AppendLine();
                    tipSb.AppendLine("File");
                    foreach (var (name, fn) in fileHashAlgs)
                        tipSb.AppendLine($"{name}: {ByteToString(fn(file))}");

                    // Check that the hash matches a supported hash
                    this.IsRomValid = EXPECTED_SHA256_HASH_LIST.Contains(
                        fileHashes["SHA-256"]);
                    this.RomStatusTooltip = tipSb.ToString();

                    if (this.IsRomValid)
                    {
                        this.HashValidationMessage = "ROM checksum is valid.";
                        this.IsRomValidText = "✅";
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

        private void RaiseAndSetIfChanged<T>(ref T target, T value, [CallerMemberName] String propertyName = "")
        {
            bool changed = ((target is null) != (value is null))
                || (target is not null && !target.Equals(value));

            target = value;

            if (changed)
                NotifyPropertyChanged(propertyName);
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
        private Boolean mIsRomSourcePathValid = false;
        private Boolean mIsSeedValid = false;
        private Boolean mIsRomValid = false;
        private String mIsRomValidText = "";
        private String mRomStatusTooltip = "";
        private String mHashValidationMessage = String.Empty;


        //
        // Constants
        //

        private const Double BYTES_PER_MEGABYTE = 1024d * 1024d;
        private const Int64 ONE_MEGABYTE = 1024 * 1024;

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

        private const String SETTINGS_ELEMENT_NAME = "Settings";
        private const String GLOBAL_SETTINGS_ELEMENT_NAME = "GlobalSettings";
        private const String RANDOMIZATION_SETTINGS_ELEMENT_NAME = "RandomizationSettings";

        private const String OPTION_GROUP_ELEMENT_NAME = "OptionGroup";
        private const String OPTION_GROUP_PATH_ATTRIBUTE_NAME = "path";

        private const String OPTION_ELEMENT_NAME = "Option";
        private const String OPTION_NAME_ATTRIBUTE_NAME = "name";
        private const String OPTION_RANDOMIZE_ATTRIBUTE_NAME = "randomize";
        private const String OPTION_VALUE_ATTRIBUTE_NAME = "value";

        // Theme Property Constants
        private const String ENABLE_APP_UI_DARK_THEME_SETTING_NAME = @"EnableAppUiDarkTheme";
        private const Boolean ENABLE_APP_UI_DARK_THEME_DEFAULT_VALUE = true;

        // Flag Property Constants
        private const String CREATE_LOG_FILE_SETTING_NAME = @"CreateLogFile";
        private const Boolean CREATE_LOG_FILE_DEFAULT_VALUE = false;

        //
        // Scalar Property Constants
        //
    }*/
}
