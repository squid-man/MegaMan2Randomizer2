using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using MM2RandoLib.Utilities;
using MM2Randomizer;
using MsBox.Avalonia.Enums;
using RandomizerHost.Settings;
using RandomizerHost.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RandomizerHost.Views
{
    public partial class MainView : UserControl
    {
        public TopLevel TopLevel => TopLevel.GetTopLevel(this)!;
        public MainViewModel ViewModel => (MainViewModel)DataContext!;

        public MainView()
        {
            InitializeComponent();
        }

        protected override async void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            await ViewModel.OnViewCreated(this);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            DragDrop.SetAllowDrop(TopLevel, true);

            TopLevel.AddHandler(DragDrop.DragEnterEvent, OnDragOver, RoutingStrategies.Tunnel, handledEventsToo: true);
            TopLevel.AddHandler(DragDrop.DragOverEvent, OnDragOver, RoutingStrategies.Tunnel, handledEventsToo: true);
            TopLevel.AddHandler(DragDrop.DropEvent, OnDrop, RoutingStrategies.Tunnel, handledEventsToo: true);
            /*top.AddHandler(DragDrop.DragEnterEvent, OnDragOver, RoutingStrategies.Bubble, handledEventsToo: true);
            top.AddHandler(DragDrop.DragOverEvent, OnDragOver, RoutingStrategies.Bubble, handledEventsToo: true);
            top.AddHandler(DragDrop.DropEvent, OnDrop, RoutingStrategies.Bubble, handledEventsToo: true);*/
        }

        async void OnDragOver(object? sender, DragEventArgs e)
        {
            e.Handled = true;
            Console.WriteLine("OnDragOver");

            // The browser will likely prevent examination of the file being dropped for security reasons, so only check that it IS a file
            SetDragDropEffects(e, 
                e.DataTransfer.Formats.Contains(DataFormat.File));
        }

        async void OnDrop(object? sender, DragEventArgs e)
        {
            Console.WriteLine("OnDrop");

            bool success = await ViewModel.TryDrop(
                TopLevel.StorageProvider, 
                e.DataTransfer);

            SetDragDropEffects(e, success);
        }

        [RelayCommand]
        async Task OpenRomFile()
        {
            string? exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var storage = TopLevel.StorageProvider;
            var initDir = exeDir != null
                ? await storage.TryGetFolderFromPathAsync(exeDir)
                : null;

            var stgFiles = await DisplayDialog(storage.OpenFilePickerAsync(new()
            {
                Title = "Open Mega Man 2 (US) NES ROM File",
                FileTypeFilter = _nesRomFileTypes,
                SuggestedStartLocation = initDir,
                SuggestedFileType = _nesRomFileTypes[0],
                AllowMultiple = false,
            }));

            // Process input if the user clicked OK.
            if (stgFiles == null || stgFiles.Count != 1)
                return;

            try
            {
                await ViewModel.OpenRomFile(stgFiles[0]);
            }
            catch (Exception e)
            {
                await MessageBox.ShowAsync(
                    this, e.ToString(), "Error", ButtonEnum.Ok);
            }
        }

        [RelayCommand]
        async Task CreateFromGivenSeed()
        {
            try
            {
                await ViewModel.CreateFromGivenSeed(TopLevel.Launcher != null);
            }
            catch (Exception e)
            {
                await MessageBox.ShowAsync(
                    this, e.ToString(), "Error", ButtonEnum.Ok);
            }
        }


        [RelayCommand]
        async Task CreateFromRandomSeedMultiple()
        {
            try
            {
                await ViewModel.CreateFromRandomSeedMultiple(TopLevel.Launcher != null);
            }
            catch (Exception e)
            {
                await MessageBox.ShowAsync(
                    this, e.ToString(), "Error", ButtonEnum.Ok);
            }
        }


        [RelayCommand]
        async Task OpenContainingFolder()
        {
            var launcher = TopLevel.Launcher;
            if (launcher == null)
                return;

            if (ViewModel.ContainingFolder != null)
            {
                try
                {
                    if (await launcher.LaunchDirectoryInfoAsync(new(Path.TrimEndingDirectorySeparator(ViewModel.ContainingFolder))))
                        return;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            await launcher.LaunchDirectoryInfoAsync(new(Path.TrimEndingDirectorySeparator(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!)));
        }

        [RelayCommand]
        async Task ImportSettings()
        {
            string? exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var storage = TopLevel.GetTopLevel(this)!.StorageProvider;
            var initDir = exeDir != null
                ? await storage.TryGetFolderFromPathAsync(exeDir)
                : null;

            var stgFiles = await DisplayDialog(storage.OpenFilePickerAsync(new()
            {
                Title = "Import Settings",
                FileTypeFilter = _jsonSettingsFileTypes,
                SuggestedStartLocation = initDir,
                SuggestedFileType = _jsonSettingsFileTypes[0],
                AllowMultiple = false,
            }));

            // Process input if the user clicked OK.
            if (stgFiles == null || stgFiles.Count != 1)
                return;

            try
            {
                await ViewModel.ImportSettings(stgFiles[0]);
            }
            catch (Exception e)
            {
                await MessageBox.ShowAsync(
                    this, e.ToString(), "Error", ButtonEnum.Ok);
            }
        }

        [RelayCommand]
        async Task ExportSettings()
        {
            string? exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var storage = TopLevel.GetTopLevel(this)!.StorageProvider;
            var initDir = exeDir != null
                ? await storage.TryGetFolderFromPathAsync(exeDir)
                : null;

            var stgFile = await DisplayDialogNullable(storage.SaveFilePickerAsync(new()
            {
                Title = "Export Settings",
                FileTypeChoices = _jsonSettingsFileTypes,
                SuggestedStartLocation = initDir,
                SuggestedFileType = _jsonSettingsFileTypes[0],
                ShowOverwritePrompt = true,
            }));

            // Process input if the user clicked OK.
            if (stgFile == null)
                return;

            try
            {
                await ViewModel.ExportSettings(stgFile);
            }
            catch (Exception e)
            {
                await MessageBox.ShowAsync(
                    this, e.ToString(), "Error", ButtonEnum.Ok);
            }
        }

        static void SetDragDropEffects(DragEventArgs e, bool success)
        {
            if (success)
            {
                if (e.DragEffects.HasFlag(DragDropEffects.Copy))
                    e.DragEffects = DragDropEffects.Copy;
                else if (e.DragEffects.HasFlag(DragDropEffects.Link))
                    e.DragEffects = DragDropEffects.Link;
                else
                    e.DragEffects = DragDropEffects.Move;
            }
            else
                e.DragEffects = DragDropEffects.None;

            e.Handled = true;
        }

        static readonly FilePickerFileType[] _nesRomFileTypes = [
            new("NES ROMs") { Patterns = ["*.nes"] }
        ];

        static readonly FilePickerFileType[] _jsonSettingsFileTypes = [
            new("JSON Settings") { Patterns = ["*.json", "*.jsn"] }
        ];

        async Task<T?> DisplayDialog<T>(Task<T> dialog)
            where T : class
        {
            try
            {
                using DialogMonitor mon = new(this);
                return await dialog.WaitAsync(mon.Token);
            }
            catch (OperationCanceledException)
            { return null; }
        }

        async Task<T?> DisplayDialogNullable<T>(Task<T?> dialog)
            where T : class
        {
            try
            {
                using DialogMonitor mon = new(this);
                return await dialog.WaitAsync(mon.Token);
            }
            catch (OperationCanceledException)
            { return null; }
        }

        async Task<T?> DisplayDialogStruct<T>(Task<T> dialog)
            where T : struct
        {
            try
            {
                using DialogMonitor mon = new(this);
                return await dialog.WaitAsync(mon.Token);
            }
            catch (OperationCanceledException)
            { return null; }
        }
    }
}
