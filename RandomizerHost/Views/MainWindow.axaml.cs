using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using RandomizerHost.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        }


        private void OnDragOver(object? sender, DragEventArgs e)
        {
            bool canAccept = ((MainWindowViewModel)DataContext!)
                .CanAcceptDrop(e.DataTransfer);

            SetDragDropEffects(e, canAccept);
        }

        private void OnDrop(object? sender, DragEventArgs e)
        {
            bool success = ((MainWindowViewModel)DataContext!).TryDrop(
                TopLevel.GetTopLevel(this)!.StorageProvider, 
                e.DataTransfer);

            SetDragDropEffects(e, success);
        }

        private static void SetDragDropEffects(DragEventArgs e, bool success)
        {
            e.DragEffects = success
                ? e.DragEffects & (DragDropEffects.Copy | DragDropEffects.Link)
                : DragDropEffects.None;
        }
    }
}
