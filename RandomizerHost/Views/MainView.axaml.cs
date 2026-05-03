using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using RandomizerHost.ViewModels;
using System;
using System.Diagnostics;

namespace RandomizerHost.Views
{
    public partial class MainView : UserControl
    {
        //
        // Constructor
        //

        public MainView()
        {
            InitializeComponent();
        }


        private void OnDragOver(object? sender, DragEventArgs e)
        {
            bool canAccept = ((MainViewModel)DataContext!)
                .CanAcceptDrop(e.DataTransfer);

            SetDragDropEffects(e, canAccept);
        }

        private void OnDrop(object? sender, DragEventArgs e)
        {
            bool success = ((MainViewModel)DataContext!).TryDrop(
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
