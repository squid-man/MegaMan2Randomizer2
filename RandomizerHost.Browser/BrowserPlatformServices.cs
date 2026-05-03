using Avalonia.Controls;
using Avalonia.Platform.Storage;
using js65;
using Microsoft.JSInterop;
using MM2RandoLib.Utilities;
using RandomizerHost;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;

internal partial class BrowserPlatformServices : IHostPlatformServices
{
    public string? SettingsPath => null;

    public BrowserPlatformServices()
    {
    }

    public async Task<byte[]?> LoadSettingsData()
    {
        string? encSettings = GetLocalSetting(RandomizerSettingsName);
        if (encSettings == null)
            return null;
            
        return Convert.FromBase64String(encSettings);
    }

    public async Task SaveSettingsData(byte[] data)
    {
        SetLocalSetting(RandomizerSettingsName, Convert.ToBase64String(data));
    }

    public bool TryLoadCachedRom(out string path, out byte[] data)
    {
        path = GetLocalSetting(RomPathSettingName) ?? "";
        data = Array.Empty<byte>();

        if (path != "")
        {
            string? encRom = GetLocalSetting(RomDataSettingName);
            if (encRom != null)
            {
                data = Convert.FromBase64String(encRom);

                return true;
            }
        }

        return false;
    }

    public void SaveRomCache(string path, byte[] data)
    {
        SetLocalSetting(RomPathSettingName, path);
        SetLocalSetting(RomDataSettingName, Convert.ToBase64String(data));
    }

    public async Task<string?> GetInitialRomPath()
        => null;

    public async Task<IStorageFolder?> GetDefaultOutputFolder(
        IStorageProvider storage)
        => null;

    public async Task<bool> CanPlatformWriteFiles()
    {
        await JSHost.ImportAsync("Interop", "../interop.js");

        return SupportsFileSystemWrites();
    }

    public bool CanPlatformLaunch(TopLevel topLevel)
        => false;

    public Assembler CreateAssembler(Js65Options? options, bool debugJavascript)
        => new BrowserJsEngine(options);

    public async Task<IRomSaver> CreateRomSaver(IStorageFolder? folder, int numFiles, IStorageProvider storageProvider)
    {
        var romSaver = new BrowserRomSaver(folder, numFiles, storageProvider);

        await romSaver.Initialize();

        return romSaver;
    }

    const string RandomizerSettingsName = "Settings.json";
    const string RomPathSettingName = "RomPath";
    const string RomDataSettingName = "Rom";

    [JSImport("globalThis.localStorage.setItem")]
    internal static partial void SetLocalSetting(string key, string value);

    [JSImport("globalThis.localStorage.getItem")]
    internal static partial string? GetLocalSetting(string key);

    [JSImport("supportsFileSystemWrites", "Interop")]
    internal static partial bool SupportsFileSystemWrites();
}
