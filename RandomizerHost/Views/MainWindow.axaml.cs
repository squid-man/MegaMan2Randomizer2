using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using MM2Randomizer.Randomizers;
using RandomizerHost;
using RandomizerHost.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

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

            /// TODO: Find a better place for this
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var versionString = version?.ToString(version.Revision > 0 ? 4 : 3);
            string title = $"Mega Man 2 Randomizer {versionString}";

            if (!GitInfo.IsOfficialBuild)
            {
                string branch = GitInfo.Branch, cmtSuff = "", dbgSuff = "";
                if (!GitInfo.IsDirty)
                    cmtSuff = $":{GitInfo.Commit}";

#if DEBUG
                dbgSuff = " (Debug)";
#endif

                title += $" EXPERIMENTAL [{branch}{cmtSuff}]{dbgSuff}";
            }

            Title = title;
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
