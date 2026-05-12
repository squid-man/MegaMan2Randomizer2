using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MM2RandoLib.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;

internal class BrowserRomSaver : IRomSaver
{
    public bool IsDisposed { get; private set; } = false;

    public BrowserRomSaver(string? basePath, IStorageProvider storageProvider)
    {
        _stg = storageProvider;
    }

    public void Dispose()
    {
        if (_archive != null)
        {
            _archive.Dispose();
            _archiveStream!.Dispose();
            _fileData = null;
        }

        IsDisposed = true;
    }

    public void AddFile(string filename, byte[] data)
    {
        if (IsDisposed)
            throw new ObjectDisposedException(nameof(BrowserRomSaver));

        if (_numFiles == 0)
        {
            _fileName = Path.GetFileName(filename);
            _fileData = data;
        }
        else
        {
            if (_archive == null)
            {
                _archiveStream = new();
                _archive = new(_archiveStream, ZipArchiveMode.Create, true);

                InternalAddFile(_fileName!, _fileData!);

                _fileData = null;
            }

            InternalAddFile(Path.GetFileName(filename), data);
        }

        _numFiles += 1;
    }

    public async Task Commit()
    {
        if (IsDisposed)
            throw new ObjectDisposedException(nameof(BrowserRomSaver));

        IsDisposed = true;

        if (_archive != null)
        {
            _archive.Dispose();

            var stgFile = await _stg.SaveFilePickerAsync(new()
            {
                Title = "Save ROM Archive",
                FileTypeChoices = _zipFileTypes,
                SuggestedFileName = Path.GetFileNameWithoutExtension(_fileName) + ".zip",
                //SuggestedStartLocation = initDir,
                SuggestedFileType = _zipFileTypes[0],
                ShowOverwritePrompt = true,
            });

            // Process input if the user clicked OK.
            if (stgFile == null)
                return;

            using (Stream stream = await stgFile.OpenWriteAsync())
            {
                _archiveStream!.Seek(0, SeekOrigin.Begin);
                await _archiveStream.CopyToAsync(stream);
            }
        }
        else if (_fileData != null)
        {
            var stgFile = await _stg.SaveFilePickerAsync(new()
            {
                Title = "Save ROM",
                FileTypeChoices = _nesRomFileTypes,
                SuggestedFileName = _fileName,
                //SuggestedStartLocation = initDir,
                SuggestedFileType = _nesRomFileTypes[0],
                ShowOverwritePrompt = true,
            });

            // Process input if the user clicked OK.
            if (stgFile == null)
                return;

            using (Stream stream = await stgFile.OpenWriteAsync())
                await stream.WriteAsync(_fileData);
        }
    }

    static readonly FilePickerFileType[] _nesRomFileTypes = [
        new("NES ROMs") { Patterns = ["*.nes"] }
    ];

    static readonly FilePickerFileType[] _zipFileTypes = [
        new("Zip Files") { Patterns = ["*.zip"] }
    ];

    IStorageProvider _stg;

    int _numFiles = 0;

    string? _fileName = null;
    byte[]? _fileData = null;

    MemoryStream? _archiveStream = null;
    ZipArchive? _archive = null;

    void InternalAddFile(string filename, byte[] data)
    {
        Debug.Assert(_archive != null);

        var entry = _archive.CreateEntry(filename, CompressionLevel.Optimal);
        using (var stream = entry.Open())
            stream.Write(data);
    }
}
