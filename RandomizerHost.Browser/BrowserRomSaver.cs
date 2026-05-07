using MM2RandoLib.Utilities;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;

internal class BrowserRomSaver : IRomSaver
{
    public bool IsDisposed { get; private set; } = false;

    public BrowserRomSaver(string? basePath)
    {
    }

    public void Dispose()
    {
        IsDisposed = true;
    }

    public void AddFile(string filename, byte[] data)
    {
    }

    public async Task Commit()
    { }

    int _numFiles = 0;

    string? _lastFileName = null;
    byte[]? _lastFileData = null;

    ZipArchive? _archive = null;
}
