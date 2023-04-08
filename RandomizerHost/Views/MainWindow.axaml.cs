using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using MM2Randomizer;
using MM2Randomizer.Settings.Options;

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

            // Set up the player sprite ComboBox
            ComboBox comboBoxPlayerSprite = this.Find<ComboBox>("ComboBox_PlayerSprite");
            comboBoxPlayerSprite.Items = Enum.GetValues(typeof(PlayerSpriteOption));

            // Set up the HUD element ComboBox
            ComboBox comboBoxHudElement = this.Find<ComboBox>("ComboBox_HudElement");
            comboBoxHudElement.Items = Enum.GetValues(typeof(HudElementOption));

            // Set up the font ComboBox
            ComboBox comboBoxFont = this.Find<ComboBox>("ComboBox_Font");
            comboBoxFont.Items = Enum.GetValues(typeof(FontOption));

            // Set up the hit point charging speed ComboBox
            ComboBox comboBoxHitPointChargingSpeed = this.Find<ComboBox>("ComboBox_HitPointChargingSpeed");
            comboBoxHitPointChargingSpeed.Items = Enum.GetValues(typeof(ChargingSpeedOption));

            // Set up the weapon energy charging speed ComboBox
            ComboBox comboBoxWeaponEnergyChargingSpeed = this.Find<ComboBox>("ComboBox_WeaponEnergyChargingSpeed");
            comboBoxWeaponEnergyChargingSpeed.Items = Enum.GetValues(typeof(ChargingSpeedOption));

            // Set up the energy tank charging speed ComboBox
            ComboBox comboBoxEnergyTankChargingSpeed = this.Find<ComboBox>("ComboBox_EnergyTankChargingSpeed");
            comboBoxEnergyTankChargingSpeed.Items = Enum.GetValues(typeof(ChargingSpeedOption));

            // Set up the robot master energy charging speed ComboBox
            ComboBox comboBoxRobotMasterEnergyChargingSpeed = this.Find<ComboBox>("ComboBox_RobotMasterEnergyChargingSpeed");
            comboBoxRobotMasterEnergyChargingSpeed.Items = Enum.GetValues(typeof(ChargingSpeedOption));

            // Set up the castle boss energy charging speed ComboBox
            ComboBox comboBoxCastleBossEnergyChargingSpeed = this.Find<ComboBox>("ComboBox_CastleBossEnergyChargingSpeed");
            comboBoxCastleBossEnergyChargingSpeed.Items = Enum.GetValues(typeof(ChargingSpeedOption));

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
