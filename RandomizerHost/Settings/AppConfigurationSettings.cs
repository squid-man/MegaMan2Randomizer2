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
using System.Text.Json;
using System.Text.Json.Serialization;
using MM2RandoLib.Settings.Options;
using MM2Randomizer;
using MM2Randomizer.Extensions;
using MM2Randomizer.Settings;
using MM2Randomizer.Settings.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace RandomizerHost.Settings
{
    class OptionJsonConverter : JsonConverter<IOption>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableTo(typeof(IOption));
        }

        public override IOption? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IOption value, JsonSerializerOptions options)
        {
            var optInfo = value!.Info!;
            if (!optInfo.SaveLoad)
                return;

            writer.WriteStartObject(optInfo.Name);
            writer.WritePropertyName("Value");
            JsonSerializer.Serialize(writer, value.BaseValue, options);
            writer.WriteBoolean("Randomize", value.Randomize);
            writer.WriteEndObject();
        }
    }

    class OptionGroupJsonConverter : JsonConverter<OptionGroup>
    {

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableTo(typeof(OptionGroup));
        }

        public override OptionGroup? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, OptionGroup value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (var opt in value.Options)
                JsonSerializer.Serialize(writer, opt, options);
            
            foreach (var (mbr, mbrInfo) in value.MemberInfos)
            {
                object? mbrValue;
                if (mbrInfo is PropertyInfo propInfo)
                    mbrValue = propInfo.GetValue(value);
                else if (mbrInfo is FieldInfo fieldInfo)
                    mbrValue = fieldInfo.GetValue(value);
                else
                    continue;

                if (mbrValue is not OptionGroup grp)
                    continue;

                writer.WritePropertyName(mbrInfo.Name);
                JsonSerializer.Serialize(writer, grp, options);
            }

            writer.WriteEndObject();
        }
    }

    class RandomizationSettingsJsonConverter : JsonConverter<RandomizationSettings>
    {
        record ClassStackEntry(Dictionary<string, ClassStackEntry> Objects, Dictionary<string, object> Values);

        public override RandomizationSettings? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            RandomizationSettings settings = new();

            var jsonDoc = JsonDocument.ParseValue(ref reader);

            ReadGroup(settings, jsonDoc.RootElement, "", settings);

            return settings;

        }

        public override void Write(Utf8JsonWriter writer, RandomizationSettings value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        void ReadGroup(
            RandomizationSettings settings,
            JsonElement grpEl,
            string grpPath,
            OptionGroup grp)
        {
            Type type = grp.GetType();
            foreach (var en in grpEl.EnumerateObject())
            {
                string propName = en.Name;
                string propPath = grpPath.Length != 0
                    ? $"{grpPath}.{propName}"
                    : propName;
                if (settings.OptionsByPath.TryGetValue(propPath, out var iopt))
                {
                    var optEl = en.Value;
                    var optType = iopt.Type;
                    if (optEl.TryGetProperty("Value", out var valueEl))
                    {
                        if (optType == typeof(bool))
                            iopt.BaseValue = valueEl.GetBoolean();
                        else
                            iopt.BaseValue = iopt.ParseType(valueEl.GetString()!);
                    }

                    if (optEl.TryGetProperty("Randomize", out var rndEl))
                        iopt.Randomize = rndEl.GetBoolean();
                }
                else if (settings.GroupsByPath.TryGetValue(propPath, out var propGrp))
                    ReadGroup(settings, en.Value, propPath, propGrp);
            }
        }
    }

    public partial class AppConfigurationSettings : ReactiveObject
    {
        [Reactive]
        public RandomizationSettings RandomizationSettings { get; set; } = new();

        [Reactive]
        public string SeedString { get; set; } = "";

        [Reactive]
        public string RomSourcePath { get; set; } = "";

        [Reactive]
        public bool EnableAppUiDarkTheme { get; set; } = true;

        [Reactive]
        public bool CreateLogFile { get; set; } = false;

        [Reactive]
        public int SettingsPresetIndex { get; set; } = 0;

        public static AppConfigurationSettings Deserialize(byte[] data)
        {
            JsonSerializerOptions opts = new();
            opts.Converters.Add(new JsonStringEnumConverter());
            opts.Converters.Add(new RandomizationSettingsJsonConverter());

            return JsonSerializer.Deserialize<AppConfigurationSettings>(data, opts)!;
        }

        public byte[] Serialize()
        {
            JsonSerializerOptions opts = new()
            {
                WriteIndented = true,
            };
            opts.Converters.Add(new JsonStringEnumConverter());
            opts.Converters.Add(new OptionJsonConverter());
            opts.Converters.Add(new OptionGroupJsonConverter());

            return JsonSerializer.SerializeToUtf8Bytes(this, opts);
        }

        public void UpdateRandomizerSettings(bool defaultSeed)
        {
            var settings = RandomizationSettings;

            settings.SeedString = defaultSeed ? null : SeedString;
            settings.RomSourcePath = RomSourcePath;
            settings.CreateLogFile = CreateLogFile && !settings.IsTournament;
        }
    }
}
