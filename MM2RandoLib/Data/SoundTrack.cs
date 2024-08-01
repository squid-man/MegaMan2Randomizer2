using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.IO.Compression;
using System.Xml.Serialization;
using MM2Randomizer.Enums;

namespace MM2Randomizer.Data
{
    public class JsonHexStringConverter : JsonConverter
    {
        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            Debug.Assert(reader.Value is not null);

            if (reader.TokenType == JsonToken.Integer)
                // It's actually a boxed long
                return (int)(long)reader.Value;
            else if (reader.TokenType == JsonToken.String)
                return Convert.ToInt32((string)reader.Value, 16);

            throw new JsonReaderException("invalid hex value", reader.Path, -1, -1, null);
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class MusicInfoBase
    {
        [JsonIgnore]
        public const bool EnabledDefault = true;
        [JsonIgnore]
        public const bool StreamingSafeDefault = true;
        [JsonIgnore]
        public const bool SwapSquareChansDefault = false;
        [JsonIgnore]
        public static HashSet<string> UsesDefault { get { return new(StringComparer.InvariantCultureIgnoreCase) { "Stage", "Credits" }; } }

        [JsonProperty("enabled")]
        public bool? Enabled { get; set; } = null;

        /*[JsonProperty("number")]
        public int Number { get; set; } = 0;*/

        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; } = "";

        [JsonProperty("author")]
        public string? Author { get; set; } = null;

        [JsonProperty("streaming_safe")]
        public bool? StreamingSafe { get; set; } = null;

        [JsonProperty("swap_square_chans")]
        public bool? SwapSquareChans { get; set; } = null;

        [JsonIgnore]
        private HashSet<string> uses = new(StringComparer.InvariantCultureIgnoreCase);

        [JsonProperty("uses")]
        public HashSet<string> Uses
        {
            get { return uses; }
            private set
            {
                uses = value;
                Usage = SoundTrackUsage.FromStrings(Uses);
            }
        }

        [JsonIgnore]
        public ESoundTrackUsage Usage { get; private set; }

        /*public static T FlattenSetting<T>(T def, params T?[] values) where T : struct
        {
            for (int i = values.Length - 1; i >= 0; i--)
            {
                var cnd_val = values[i];
                if (cnd_val is not null)
                    return (T)cnd_val;
            }

            return def;
        }

        public static T? FlattenSetting<T>(params T?[] values) where T : class
        {
            for (int i = values.Length - 1; i >= 0; i--)
            {
                var cnd_val = values[i];
                if (cnd_val is not null)
                    return cnd_val;
            }

            return null;
        }*/
    }

    public abstract class MusicFileInfoBase : MusicInfoBase
    {
        [JsonIgnore]
        public abstract int StartAddrDefault { get; }

        [JsonProperty("start_addr")]
        [JsonConverter(typeof(JsonHexStringConverter))]
        public int? StartAddr { get; set; } = null;

        [JsonProperty("data", Required = Required.Always)]
        public string Data
        {
            get { return data; }
            set
            {
                data = value;
                UncompressData();
            }
        }

        [JsonIgnore]
        public byte[] UncompressedData { get; private set; } = new byte[0];

        [JsonIgnore]
        public int Size { get { return UncompressedData.Length; } }

        private string data = "";

        protected void UncompressData()
        {
            const string deflateHdr = "deflate:";
            if (Data.StartsWith(deflateHdr))
            {
                var data = Convert.FromBase64String(Data.Substring(deflateHdr.Length));
                using (var outStream = new MemoryStream())
                {
                    using (var memStream = new MemoryStream(data))
                    {
                        using (var cmpStream = new DeflateStream(memStream, CompressionMode.Decompress))
                            cmpStream.CopyTo(outStream);
                    }

                    UncompressedData = outStream.ToArray();
                }
            }
            else
                UncompressedData = Convert.FromBase64String(Data);
        }
    }

    [JsonObject]
    public class C2SongInfo : MusicFileInfoBase
    {
        public override int StartAddrDefault { get { return 0x8000; } }
    }

    [JsonObject]
    public class FtSongInfo : MusicInfoBase
    {
        [JsonProperty("number", Required = Required.Always)]
        public int Number { get; set; } = 0;
    }

    [JsonObject]
    public class C2SongGroupInfo : MusicInfoBase
    {
        [JsonProperty("songs")]
        public List<C2SongInfo> Songs { get; set; } = new();
    }

    [JsonObject]
    public class C2LibraryInfo
    {
        [JsonProperty("single")]
        public List<C2SongInfo> Single { get; set; } = new();

        [JsonProperty("groups")]
        public List<C2SongGroupInfo> Groups { get; set; } = new();
    }

    [JsonObject]
    public class FtModuleInfo : MusicFileInfoBase
    {
        [JsonIgnore]
        public override int StartAddrDefault { get { return 0; } }

        [JsonProperty("songs")]
        public List<FtSongInfo> Songs { get; set; } = new();
    }

    [JsonObject]
    public class FtModuleGroupInfo : MusicInfoBase
    {
        [JsonProperty("modules")]
        public List<FtModuleInfo> Modules { get; set; } = new();
    }

    [JsonObject]
    public class FtLibraryInfo
    {
        [JsonProperty("single")]
        public List<FtModuleInfo> Single { get; set; } = new();

        [JsonProperty("groups")]
        public List<FtModuleGroupInfo> Groups { get; set; } = new();
    }
}
