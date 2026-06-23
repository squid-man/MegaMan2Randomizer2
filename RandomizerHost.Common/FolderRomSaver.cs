using Avalonia.Platform.Storage;
using MM2RandoLib.Utilities;
using MM2Randomizer;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RandomizerHost;

public class FolderRomSaver : IRomSaver
{
    public record struct RomEntry(string Name, byte[] Data);

    public bool IsDisposed { get; private set; } = false;

    // null is allowed so derived classes can implement alternate behavior
    public IStorageFolder? Folder { get; } = null;
    public string? FolderPath { get; } = null;

    public FolderRomSaver(IStorageFolder? folder)
    {
        Folder = folder;
        FolderPath = folder?.TryGetLocalPath();
    }

    public virtual void Dispose()
    {
        IsDisposed = true;
    }

    public virtual async Task AddFile(string filename, byte[] data)
    {
        if (IsDisposed)
            throw new ObjectDisposedException(nameof(FolderRomSaver));

        Debug.Assert(Folder != null);

        if (FolderPath != null)
        {
            // More robust version when direct path access is available
            string path = Path.Combine(FolderPath, filename),
            tempPath = filename + ".tmp";

            await File.WriteAllBytesAsync(tempPath, data);

            try
            {
                File.Move(tempPath, path, true);
            }
            catch
            {
                try
                {
                    File.Delete(tempPath);
                }
                catch
                { }

                throw;
            }

        }
        else
        {
            // Portable no-frills version
            using (var file = await Folder.CreateFileAsync(filename))
            {
                if (file == null)
                    throw new IOException($"unable to create file '{filename}' in '{Folder.TryGetLocalPath() ?? Folder.Name}'");

                try
                {
                    using (var stream = await file.OpenWriteAsync())
                        await stream.WriteAsync(data);
                }
                catch
                {
                    try
                    {
                        await file.DeleteAsync();
                    }
                    catch
                    { }

                    throw;
                }
            }
        }
    }

    public virtual async Task Commit()
    {
        if (IsDisposed)
            throw new ObjectDisposedException(nameof(FolderRomSaver));
    }
}
