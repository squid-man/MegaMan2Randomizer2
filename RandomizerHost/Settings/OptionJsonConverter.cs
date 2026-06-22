using MM2RandoLib.Settings.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RandomizerHost.Settings;

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
