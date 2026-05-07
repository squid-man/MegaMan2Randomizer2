using MM2RandoLib.Utilities;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace RandomizerHost.Desktop;

public class DesktopRomSaver : IRomSaver
{
    public record struct RomEntry(string Name, byte[] Data);

    public string BasePath { get; }

    public bool IsDisposed { get; private set; } = false;

    public DesktopRomSaver(string? basePath)
    {
        BasePath = !string.IsNullOrEmpty(basePath)
            ? basePath
            : Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
    }

    public void Dispose()
    {
        IsDisposed = true;
    }

    public void AddFile(string filename, byte[] data)
    {
        string path = Path.Combine(BasePath, filename),
            tempPath = Path.GetTempFileName();
        File.WriteAllBytes(tempPath, data);

        if (File.Exists(path))
            File.Replace(tempPath, path, null);
        else
            File.Move(tempPath, path, true);
    }

    public async Task Commit()
    { }
}
