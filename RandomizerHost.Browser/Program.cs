using Avalonia;
using Avalonia.Browser;
using Microsoft.Extensions.DependencyInjection;
using RandomizerHost;
using ReactiveUI.Avalonia;
using System.Threading.Tasks;

internal sealed partial class Program
{
    private static Task Main(string[] args) => BuildAvaloniaApp()
            .StartBrowserAppAsync("out");

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI(b => { })
            .AfterSetup(builder =>
            {
                var services = new ServiceCollection();
                services.AddSingleton<IHostPlatformServices, BrowserPlatformServices>();

                ((App)builder.Instance!).InitializeServices(
                    services.BuildServiceProvider());
            });
}