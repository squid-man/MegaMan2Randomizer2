using MM2Randomizer.Settings;
using MM2Randomizer.Settings.Options;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RandomizerHost.Settings;

public partial class AppConfigurationSettings : ReactiveObject
{
    [Reactive]
    public partial RandomizationSettings RandomizationSettings { get; set; } = new();

    [Reactive]
    public partial string SeedString { get; set; } = "";

    [Reactive]
    public partial string RomSourceBookmark { get; set; } = "";

    [Reactive]
    public partial string OutputFolderBookmark { get; set; } = "";

    [Reactive]
    public partial bool EnableAppUiDarkTheme { get; set; } = true;

    [Reactive]
    public partial bool CreateLogFile { get; set; } = false;

    [Reactive]
    public partial int SettingsPresetIndex { get; set; } = 0;

    public AppConfigurationSettings()
    {
        // Required for Reactive properties to work properly
    }

    public static AppConfigurationSettings Deserialize(byte[] data)
    {
        JsonSerializerOptions opts = new()
        {
            TypeInfoResolver = JsonContext.Default,
        };
        opts.Converters.Add(new JsonStringEnumConverter());
        opts.Converters.Add(new RandomizationSettingsJsonConverter());

        return JsonSerializer.Deserialize<AppConfigurationSettings>(data, opts)!;
    }

    public byte[] Serialize()
    {
        JsonSerializerOptions opts = new()
        {
            TypeInfoResolver = JsonContext.Default,
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
        settings.RomSourcePath = RomSourceBookmark;
        settings.CreateLogFile = CreateLogFile && !settings.IsTournament;
    }

    [JsonSerializable(typeof(AppConfigurationSettings))]
    internal partial class JsonContext : JsonSerializerContext
    { }
}
