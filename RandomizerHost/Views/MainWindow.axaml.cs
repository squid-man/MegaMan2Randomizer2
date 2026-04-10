using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using RandomizerHost.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace RandomizerHost.Views
{
    public partial class MainWindow : Window
    {
        //
        // Constructor
        //

        public MainWindow()
        {
            InitializeComponent();

            // I don't know why but I can't get it to find the handlers when these are specified in the AXAML
            DragDrop.AddDropHandler(this, OnDrop);
            DragDrop.AddDragOverHandler(this, OnDragOver);
        }


        private void OnDragOver(object? sender, DragEventArgs e)
        {
            var res = GetDragDropPath(e);

            e.DragEffects = res.Effects;
        }

        private void OnDrop(object? sender, DragEventArgs e)
        {
            var res = GetDragDropPath(e);
            e.DragEffects = res.Effects;

            if (res.Path == null)
                return;

            TextBox_RomFile.Text = res.Path;
        }

        private static (string? Path, DragDropEffects Effects) GetDragDropPath(DragEventArgs e)
        {
            // Only allow if the dragged data contains text or filenames
            string? path = null;
            if (e.DataTransfer.Contains(DataFormat.Text))
                path = e.DataTransfer.TryGetText();
            else if (e.DataTransfer.Formats.Contains(DataFormat.File)
                && (e.DataTransfer.TryGetFiles() ?? Array.Empty<IStorageItem>()).Length == 1)
                path = e.DataTransfer.TryGetFile()!.TryGetLocalPath()!;

            if (path == null
                || !path.ToLowerInvariant().EndsWith(".nes")
                || !File.Exists(path))
                return (null, DragDropEffects.None);

            return (path, e.DragEffects & (DragDropEffects.Copy | DragDropEffects.Link));
        }
    }
}
