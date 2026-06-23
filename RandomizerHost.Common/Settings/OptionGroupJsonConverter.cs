using MM2RandoLib.Settings.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RandomizerHost.Settings;

class OptionGroupJsonConverter : JsonConverter<OptionGroup>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableTo(typeof(OptionGroup));
    }

    public override OptionGroup? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Reading groups doesn't use OptionGroupJsonConverter
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
                throw new UnreachableException(
                    "option group member info must be property or field");

            if (mbrValue is not OptionGroup grp)
                continue; // Options are already handled

            writer.WritePropertyName(mbrInfo.Name);
            JsonSerializer.Serialize(writer, grp, options);
        }

        writer.WriteEndObject();
    }
}
