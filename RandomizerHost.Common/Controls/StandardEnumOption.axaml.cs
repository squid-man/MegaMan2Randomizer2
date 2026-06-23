using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MM2RandoLib.Settings.Options;

namespace RandomizerHost.Controls;

public partial class StandardEnumOption : UserControl
{
    private IEnumOption? _option;

    public IEnumOption? Option
    {
        get => _option;
        set => SetAndRaise(OptionProperty, ref _option, value);
    }
    public static readonly DirectProperty<StandardEnumOption, IEnumOption?> OptionProperty = AvaloniaProperty.RegisterDirect<StandardEnumOption, IEnumOption?>(
        nameof(Option),
        c => c.Option,
        (c, v) => c.Option = v,
        null,
        Avalonia.Data.BindingMode.TwoWay);

    public string? ItemPrefix
    {
        get => GetValue(ItemPrefixProperty) ?? "";
        set => SetValue(ItemPrefixProperty, value);
    }
    public static readonly StyledProperty<string?> ItemPrefixProperty = AvaloniaProperty.Register<StandardEnumOption, string?>(
        nameof(ItemPrefix),
        null,
        false,
        Avalonia.Data.BindingMode.TwoWay);

    public string? ItemSuffix
    {
        get => GetValue(ItemSuffixProperty) ?? "";
        set => SetValue(ItemSuffixProperty, value);
    }
    public static readonly StyledProperty<string?> ItemSuffixProperty = AvaloniaProperty.Register<StandardEnumOption, string?>(
        nameof(ItemSuffix),
        null,
        false,
        Avalonia.Data.BindingMode.TwoWay);

    public StandardEnumOption()
    {
        InitializeComponent();
    }
}