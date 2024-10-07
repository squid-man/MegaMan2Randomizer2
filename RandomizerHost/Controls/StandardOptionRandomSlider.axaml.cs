using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace RandomizerHost.Controls;

public partial class StandardOptionRandomSlider : UserControl
{
    public StandardOptionRandomSlider()
    {
        InitializeComponent();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}