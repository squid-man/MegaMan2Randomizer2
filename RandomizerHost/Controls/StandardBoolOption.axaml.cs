using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MM2RandoLib.Settings.Options;

namespace RandomizerHost.Controls;

#nullable enable

public partial class StandardBoolOption : UserControl
{
    public BoolOption? Option
    {
        get => GetValue(OptionProperty);
        set => SetValue(OptionProperty, value);
    }
    public static readonly StyledProperty<BoolOption?> OptionProperty = AvaloniaProperty.Register<StandardBoolOption, BoolOption?>(
        nameof(Option), 
        null, 
        false, 
        Avalonia.Data.BindingMode.TwoWay);

    public StandardBoolOption()
    {
        InitializeComponent();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}