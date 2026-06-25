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
using System.Threading;
using System.Threading.Tasks;

namespace RandomizerHost.Views
{
    public partial class MainView : UserControl
    {
        public TopLevel TopLevel => TopLevel.GetTopLevel(this)!;
        public MainViewModel ViewModel => (MainViewModel)DataContext!;

        public static readonly StyledProperty<Control?> ModalDialogProperty =
            AvaloniaProperty.Register<Control, Control?>(nameof(ModalDialog));

        public Control? ModalDialog
        {
            get => GetValue(ModalDialogProperty);
            set => SetValue(ModalDialogProperty, value);
        }

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
            var storage = TopLevel.StorageProvider;
            var initDir = await storage.TryGetFolderFromPathAsync(RandomMM2.BasePath);

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

            await RunAndDisplayError(
                async () => await ViewModel.OpenRomFile(stgFiles[0]));
        }

        [RelayCommand]
        async Task SelectOutputFolder()
        {
            var stg = TopLevel.StorageProvider;
            if (!ViewModel.CanSelectFolder)
                return;

            var vm = ViewModel;
            var folders = await DisplayDialog(stg.OpenFolderPickerAsync(new()
            {
                Title = "Select Output Folder",
                SuggestedStartLocation = vm.OutputFolder,
                AllowMultiple = false,
            }));

            if (folders != null && folders.Count == 1)
                await ViewModel.SetOutputFolder(folders[0]);
        }

        [RelayCommand]
        async Task CreateFromGivenSeed()
        {
            await RunWithProgressDialog(async (vm, token) =>
            {
                using (var romSaver = await ViewModel.PlatformServices.CreateRomSaver(
                    ViewModel.OutputFolder,
                    1,
                    TopLevel.StorageProvider))
                {
                    var msg = await ViewModel.CreateFromGivenSeed(romSaver, vm, token);

                    await romSaver.Commit();

                    return msg;
                }
            });
        }


        [RelayCommand]
        async Task CreateFromRandomSeedMultiple()
        {
            await RunWithProgressDialog(async (vm, token) =>
            {
                using (var romSaver = await ViewModel.PlatformServices.CreateRomSaver(
                    ViewModel.OutputFolder,
                    ViewModel.RandomSeedCount,
                    TopLevel.StorageProvider))
                {
                    var msg = await ViewModel.CreateFromRandomSeedMultiple(romSaver, vm, token);

                    await romSaver.Commit();

                    return msg;
                }
            });
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

            await RunAndDisplayError(async () =>
            {
                await launcher.LaunchDirectoryInfoAsync(new(RandomMM2.BasePath));
            });
        }

        [RelayCommand]
        async Task ImportSettings()
        {
            var storage = TopLevel.GetTopLevel(this)!.StorageProvider;
            var initDir = await storage.TryGetFolderFromPathAsync(RandomMM2.BasePath);
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

            await RunAndDisplayError(
                async () => await ViewModel.ImportSettings(stgFiles[0]));
        }

        [RelayCommand]
        async Task ExportSettings()
        {
            var storage = TopLevel.GetTopLevel(this)!.StorageProvider;
            var initDir = await storage.TryGetFolderFromPathAsync(RandomMM2.BasePath);
            var stgFile = await DisplayDialogNullable(storage.SaveFilePickerAsync(new()
            {
                Title = "Export Settings",
                FileTypeChoices = _jsonSettingsFileTypes,
                SuggestedStartLocation = initDir,
                SuggestedFileName = "settings.json",
                SuggestedFileType = _jsonSettingsFileTypes[0],
                ShowOverwritePrompt = true,
            }));

            // Process input if the user clicked OK.
            if (stgFile == null)
                return;

            await RunAndDisplayError(
                async () => await ViewModel.ExportSettings(stgFile));
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
            new("NES ROMs") {
                Patterns = ["*.nes"],
                MimeTypes = new[] { "application/octet-stream" },
            }
        ];

        static readonly FilePickerFileType[] _jsonSettingsFileTypes = [
            new("JSON Settings")
            {
                Patterns = ["*.json", "*.jsn"],
                MimeTypes = new[] { "application/json" },
            }
        ];

        async Task<bool> RunAndDisplayError(Func<Task> action)
        {
            try
            {
                await action();

                return true;
            }
            catch (OperationCanceledException)
            { }
            catch (Exception e)
            {
                await MessageBox.ShowAsync(
                    this, e.ToString(), "Error", ButtonEnum.Ok);
            }

            return false;
        }

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

        async Task RunWithProgressDialog(
            Func<ProgressDialogViewModel, CancellationToken, Task<string?>> action)
        {
            await RunAndDisplayError(async () =>
            {
                var token = new CancellationTokenSource();
                var vm = new ProgressDialogViewModel();
                var progDlg = new ProgressDialog() { DataContext = vm };

                progDlg.CancelButton.Click += (sender, e) => token.Cancel();

                ModalDialog = progDlg;

                var result = await action(vm, token.Token);

                ModalDialog = null;

                if (result != null)
                    await MessageBox.ShowAsync(this, result, "");
            });

            ModalDialog = null;
        }

        private void SpriteCreditsLink_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var url = "https://docs.google.com/spreadsheets/d/147CBXSSu1AuzM6UPDsY2GAtL_e9iQFibZnuOs_Ef8tk/edit?gid=2022826131#gid=2022826131";
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception)
            {
                // optionally log or show a user-visible error
            }
        }
    }
}
