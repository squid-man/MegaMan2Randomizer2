using js65;
using MM2RandoLib.Utilities;
using RandomizerHost.Settings;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace RandomizerHost.Desktop;

public class DesktopPlatformServices : IHostPlatformServices
{
    public string? SettingsPath { get; private set; } = null;

    public async Task<byte[]?> LoadSettingsData()
    {
        string appDataPath = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData);
        string cfgPath = Path.Join(appDataPath,
            RandomizerSettingsFolderName,
            RandomizerSettingsFilename);
        Directory.CreateDirectory(Path.GetDirectoryName(cfgPath)!);

        if (!File.Exists(cfgPath))
        {
            string cwdPath = Path.Join(
                Directory.GetCurrentDirectory(), RandomizerSettingsFilename);
            if (Path.Exists(cwdPath))
                cfgPath = cwdPath;

            // If neither exists default to app data directory
        }

        SettingsPath = cfgPath;

        return File.Exists(cfgPath)
            ? File.ReadAllBytes(cfgPath)
            : null;
    }

    public async Task SaveSettingsData(byte[] data)
    {
        Debug.Assert(SettingsPath != null);

        File.WriteAllBytes(SettingsPath, data);
    }

    public bool TryLoadCachedRom(out string path, out byte[] data)
    {
        path = "";
        data = Array.Empty<byte>();

        return false;
    }

    public void SaveRomCache(string path, byte[] data)
    { }

    public async Task<string?> GetInitialRomPath()
    {
        string? curDir = Path.GetDirectoryName(typeof(IPlatformServices).Assembly.Location);
        Debug.Assert(curDir is not null);

        while (!string.IsNullOrEmpty(curDir))
        {
            String[] tryNames = new String[]
            {
                        "MM2.nes",
                        "MegaMan2.nes",
                        "Mega Man 2.nes",
                        "Megaman II (U) [!].nes",
                        "Mega Man 2 (USA).nes",
            };
            foreach (var path in tryNames
                .Select(name => Path.Combine(curDir, name))
                .Where(path => File.Exists(path)))
                return path;

            curDir = Path.GetDirectoryName(curDir);
        }

        return null;
    }

    public Assembler CreateAssembler(Js65Options? options, bool debugJavascript)
#pragma warning disable CA1416 // Validate platform compatibility
        => new ClearScriptEngine(options, true, debugJavascript);
#pragma warning restore CA1416 // Validate platform compatibility

    public IRomSaver CreateRomSaver(string? basePath)
        => new DesktopRomSaver(basePath);

    const string RandomizerSettingsFolderName = "Mega Man 2 Randomizer";
    const string RandomizerSettingsFilename = "Settings.json";
}
