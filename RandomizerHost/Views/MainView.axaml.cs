using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using RandomizerHost.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;

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

        protected override async void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            await ((MainViewModel)DataContext!).OnViewCreated(this);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            Console.WriteLine("OnAttachedToVisualTree");

            var top = TopLevel.GetTopLevel(this);
            if (top == null)
                return;

            Console.WriteLine("OnAttachedToVisualTree2");

            DragDrop.SetAllowDrop(top, true);

            top.AddHandler(DragDrop.DragEnterEvent, OnDragOver, RoutingStrategies.Tunnel, handledEventsToo: true);
            top.AddHandler(DragDrop.DragOverEvent, OnDragOver, RoutingStrategies.Tunnel, handledEventsToo: true);
            top.AddHandler(DragDrop.DropEvent, OnDrop, RoutingStrategies.Tunnel, handledEventsToo: true);
            /*top.AddHandler(DragDrop.DragEnterEvent, OnDragOver, RoutingStrategies.Bubble, handledEventsToo: true);
            top.AddHandler(DragDrop.DragOverEvent, OnDragOver, RoutingStrategies.Bubble, handledEventsToo: true);
            top.AddHandler(DragDrop.DropEvent, OnDrop, RoutingStrategies.Bubble, handledEventsToo: true);*/
        }

        private async void OnDragOver(object? sender, DragEventArgs e)
        {
            e.Handled = true;
            Console.WriteLine("OnDragOver");

            // The browser will likely prevent examination of the file being dropped for security reasons, so only check that it IS a file
            SetDragDropEffects(e, 
                e.DataTransfer.Formats.Contains(DataFormat.File));
        }

        private async void OnDrop(object? sender, DragEventArgs e)
        {
            Console.WriteLine("OnDrop");

            bool success = await ((MainViewModel)DataContext!).TryDrop(
                TopLevel.GetTopLevel(this)!.StorageProvider, 
                e.DataTransfer);

            SetDragDropEffects(e, success);
        }

        private static void SetDragDropEffects(DragEventArgs e, bool success)
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
    }
}
