using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MM2RandoLib.Utilities;
using RandomizerHost.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RandomizerHost;

public interface IHostPlatformServices : IPlatformServices
{
    string? SettingsPath { get; }

    Task<byte[]?> LoadSettingsData();
    Task SaveSettingsData(byte[] data);

    bool TryLoadCachedRom(out string path, out byte[] data);
    void SaveRomCache(string path, byte[] data);

    Task<string?> GetInitialRomPath();
    Task<IStorageFolder?> GetDefaultOutputFolder(IStorageProvider storage);

    Task<IRomSaver> CreateRomSaver(IStorageFolder? folder, int numFiles, IStorageProvider storageProvider);

    Task<bool> CanPlatformWriteFiles();
    bool CanPlatformLaunch(TopLevel topLevel);
}
