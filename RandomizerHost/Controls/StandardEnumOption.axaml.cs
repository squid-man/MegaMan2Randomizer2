using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MM2RandoLib.Settings.Options;

#nullable enable

namespace RandomizerHost.Controls;

public partial class StandardEnumOption : UserControl
{
    public IEnumOption? Option
    {
        get => GetValue(OptionProperty);
        set => SetValue(OptionProperty, value);
    }
    public static readonly StyledProperty<IEnumOption?> OptionProperty = AvaloniaProperty.Register<StandardEnumOption, IEnumOption?>(
        nameof(Option),
        null,
        false,
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

    /*public string? FormatString
    {
        get => GetValue(FormatStringProperty);
        set => SetValue(FormatStringProperty, value);
    }
    public static readonly StyledProperty<string?> FormatStringProperty = AvaloniaProperty.Register<StandardEnumOption, string?>(
        nameof(FormatString),
        null,
        false,
        Avalonia.Data.BindingMode.OneWay);

    public int? ComboBoxWidth
     {
         get => GetValue(ComboBoxWidthProperty);
         set => SetValue(ComboBoxWidthProperty, value);
     }
     public static readonly StyledProperty<int?> ComboBoxWidthProperty = AvaloniaProperty.Register<StandardEnumOption, int?>(
         nameof(ComboBoxWidth),
         null,
         false,
         Avalonia.Data.BindingMode.TwoWay);*/

    public StandardEnumOption()
    {
        InitializeComponent();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}