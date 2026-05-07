using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using MM2RandoLib;
using MM2RandoLib.Utilities;
using RandomizerHost.Settings;
using RandomizerHost.ViewModels;
using RandomizerHost.Views;
using System;
using System.IO;
using System.Linq;

namespace RandomizerHost
{
    public class App : Application
    {
        public IServiceProvider? Services { get; private set; } = null;

        public override void Initialize()
        {
            RequestedThemeVariant = ThemeVariant.Dark;
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var platformServices = Services?
                .GetRequiredService<IHostPlatformServices>()!;

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel(platformServices),
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
            {
                string cfgPath = GetConfigFilePath();
                AppConfigurationSettings settings = File.Exists(cfgPath)
                    ? AppConfigurationSettings.Deserialize(
                        File.ReadAllBytes(cfgPath))
                    : new();

                singleView.MainView = new MainView
                {
                    DataContext = new MainViewModel(platformServices),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        public void InitializeServices(IServiceProvider provider)
        {
            Services = provider;
        }

        string GetConfigFilePath()
        {
            string cfgPath = GetAppDataSettingsPath();
            Directory.CreateDirectory(Path.GetDirectoryName(cfgPath)!);

            if (File.Exists(cfgPath))
                return cfgPath;

            string cwdPath = Path.Join(
                Directory.GetCurrentDirectory(), RandomizerSettingsFilename);
            if (Path.Exists(cwdPath))
                return cwdPath;

            // If neither exists default to app data directory
            return cfgPath;
        }

        string GetAppDataSettingsPath()
            => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                RandomizerSettingsFolderName,
                RandomizerSettingsFilename);

        void SaveSettings(string path, byte[] data)
        {
            File.WriteAllBytes(path, data);
        }

        const string RandomizerSettingsFolderName = "Mega Man 2 Randomizer";
        const string RandomizerSettingsFilename = "Settings.json";
    }
}
