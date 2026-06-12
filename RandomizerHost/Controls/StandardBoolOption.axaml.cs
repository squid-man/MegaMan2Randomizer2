using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MM2RandoLib.Settings.Options;

namespace RandomizerHost.Controls;

public partial class StandardBoolOption : UserControl
{
    private BoolOption? _option;

    public BoolOption? Option
    {
        get => _option;
        set => SetAndRaise(OptionProperty, ref _option, value);
    }
    public static readonly DirectProperty<StandardBoolOption, BoolOption?> OptionProperty = AvaloniaProperty.RegisterDirect<StandardBoolOption, BoolOption?>(
        nameof(Option), 
        c => c.Option,
        (c, v) => c.Option = v,
        null, 
        Avalonia.Data.BindingMode.TwoWay);

    public StandardBoolOption()
    {
        InitializeComponent();
    }
}