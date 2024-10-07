using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System.Reactive.Linq;

namespace RandomizerHost.Views
{
    public class MainWindow : Window
    {
        //
        // Constructor
        //

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }


        //
        // Initialization
        //

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            ///
            // Set up drag and drop for the rom file path text box
            TextBox textBoxRomFile = this.Find<TextBox>("TextBox_RomFile");
            DragDrop.SetAllowDrop(textBoxRomFile, true);
            textBoxRomFile.AddHandler(DragDrop.DragOverEvent, this.DragOver);
            textBoxRomFile.AddHandler(DragDrop.DropEvent, this.Drop);
            textBoxRomFile.PropertyChanged += this.TextBoxRomFile_PropertyChanged;
        }

        private void TextBoxRomFile_PropertyChanged(Object sender, AvaloniaPropertyChangedEventArgs e)
        {
        }

        private void DragOver(Object sender, DragEventArgs in_DragEventArgs)
        {
            // Only allow if the dragged data contains text or filenames
            if (true == in_DragEventArgs.Data.Contains(DataFormats.Text) ||
                true == in_DragEventArgs.Data.Contains(DataFormats.FileNames))
            {
                // Only allow copy or link as drop operations
                in_DragEventArgs.DragEffects = in_DragEventArgs.DragEffects & (DragDropEffects.Copy | DragDropEffects.Link);
            }
            else
            {
                in_DragEventArgs.DragEffects = DragDropEffects.None;
            }
        }


        private void Drop(Object in_Sender, DragEventArgs in_DragEventArgs)
        {
            TextBox romFile = in_Sender as TextBox;

            if (true == in_DragEventArgs.Data.Contains(DataFormats.Text))
            {
                romFile.Text = in_DragEventArgs.Data.GetText();
            }
            else if (true == in_DragEventArgs.Data.Contains(DataFormats.FileNames))
            {
                romFile.Text = in_DragEventArgs.Data.GetFileNames().First();
            }
        }
    }
}
