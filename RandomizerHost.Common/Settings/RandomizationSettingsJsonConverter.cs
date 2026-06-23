using MM2RandoLib.Settings.Options;
using MM2Randomizer.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RandomizerHost.Settings;

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
