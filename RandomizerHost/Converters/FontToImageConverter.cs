using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MM2Randomizer.Settings.Options;
using System;
using System.Diagnostics;
using System.Globalization;

namespace RandomizerHost.Converters;

public class FontToImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
            return null;

        if (value is not FontOption || !targetType.IsAssignableFrom(typeof(Bitmap)))
            throw new NotSupportedException();

        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        Debug.Assert(assets is not null);

        Uri uri = new($"avares://{nameof(RandomizerHost)}/Assets/FontSpritePreviews/Font_{value.ToString()}.png");
        var asset = assets.Open(uri);
        return new Bitmap(asset);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}