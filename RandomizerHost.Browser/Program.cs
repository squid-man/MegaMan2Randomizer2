using Avalonia;
using Avalonia.Browser;
using ReactiveUI.Avalonia;
using RandomizerHost;
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
            .UseReactiveUI(b => { });
}