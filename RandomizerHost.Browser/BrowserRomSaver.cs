using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MM2RandoLib.Utilities;
using RandomizerHost;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;

internal class BrowserRomSaver : FolderRomSaver
{
    public BrowserRomSaver(
        IStorageFolder? folder, 
        int numFiles, 
        IStorageProvider storageProvider)
        : base(folder)
    {
        _stg = storageProvider;
        _numExpectedFiles = numFiles;
    }

    internal async Task Initialize()
    {
        if (IsDisposed)
            throw new ObjectDisposedException(nameof(BrowserRomSaver));
    }

    public override void Dispose()
    {
        if (IsDisposed)
            return;

        base.Dispose();

        if (_archive != null)
            _archive.Dispose();
        if (_archiveStream != null)
            _archiveStream.Close();
        if (_archiveFileStream != null)
            _archiveFileStream.Dispose();
    }

    public override async Task AddFile(string filename, byte[] data)
    {
        if (IsDisposed)
            throw new ObjectDisposedException(nameof(BrowserRomSaver));

        if (Folder != null)
        {
            await base.AddFile(filename, data);
            return;
        }

        if (_numExpectedFiles == 1)
        {
            Debug.Assert(_numFiles < 1);

            var stgFile = await _stg.SaveFilePickerAsync(new()
            {
                Title = "Save ROM",
                FileTypeChoices = _nesRomFileTypes,
                SuggestedFileName = filename,
                //SuggestedStartLocation = initDir,
                SuggestedFileType = _nesRomFileTypes[0],
                ShowOverwritePrompt = true,
            });

            // Process input if the user clicked OK.
            if (stgFile == null)
                throw new OperationCanceledException();

            using (Stream stream = await stgFile.OpenWriteAsync())
                await stream.WriteAsync(data);
        }
        else
        {
            if (_archive == null)
            {
                var stgFile = await _stg.SaveFilePickerAsync(new()
                {
                    Title = "Save ROM Archive",
                    FileTypeChoices = _zipFileTypes,
                    SuggestedFileName = Path.GetFileNameWithoutExtension(filename) + ".zip",
                    //SuggestedStartLocation = initDir,
                    SuggestedFileType = _zipFileTypes[0],
                    ShowOverwritePrompt = true,
                });

                // Process input if the user clicked OK.
                if (stgFile == null)
                    throw new OperationCanceledException();

                _archiveFileStream = await stgFile.OpenWriteAsync();
                _archiveStream = new MemoryStream();
                _archive = await ZipArchive.CreateAsync(_archiveStream, ZipArchiveMode.Create, true, null);
            }

            var entry = _archive.CreateEntry(
                Path.GetFileName(filename), CompressionLevel.Optimal);
            using (var stream = entry.Open())
                stream.Write(data);
        }

        _numFiles += 1;
    }

    public override async Task Commit()
    {
        if (IsDisposed)
            throw new ObjectDisposedException(nameof(BrowserRomSaver));

        base.Dispose();

        if (_archive != null)
        {
            _archive.Dispose();

            _archiveStream!.Seek(0, SeekOrigin.Begin);
            await _archiveStream.CopyToAsync(_archiveFileStream!);

            _archiveStream!.Close();
            _archiveFileStream!.Dispose();
        }
    }

    static readonly FilePickerFileType[] _nesRomFileTypes = [
        new("NES ROMs") {
            Patterns = ["*.nes"],
            MimeTypes = new[] { "application/octet-stream" },
        }
    ];

    static readonly FilePickerFileType[] _zipFileTypes = [
        new("Zip Files") 
        { 
            Patterns = ["*.zip"],
            MimeTypes = new[] { "application/zip", "application/x-zip-compressed" },
        }
    ];

    IStorageProvider _stg;
    int _numExpectedFiles;
    
    int _numFiles = 0;

    Stream? _archiveStream = null;
    ZipArchive? _archive = null;
    Stream? _archiveFileStream = null;
}
