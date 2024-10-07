using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MM2RandoLib.Settings.Options;
using MM2Randomizer;
using MM2Randomizer.Extensions;
using MM2Randomizer.Settings;
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


        //
        // Scalar Properties
        //

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
                            
                            foreach (var opt in grp.Options.Where(opt => opt.Info.SaveLoad))
                            {
                                in_Writer.WriteStartElement(OPTION_ELEMENT_NAME);

                                in_Writer.WriteAttributeString(
                                    OPTION_NAME_ATTRIBUTE_NAME, 
                                    opt.Info.Name);
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
                string grpPath = in_Reader.GetAttribute(
                    OPTION_GROUP_PATH_ATTRIBUTE_NAME);
                if (grpPath is null
                    || !in_Reader.ReadToDescendant(OPTION_ELEMENT_NAME))
                    continue;

                do
                {
                    string name = in_Reader.GetAttribute(
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
                    IOption opt;
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
    }
}
