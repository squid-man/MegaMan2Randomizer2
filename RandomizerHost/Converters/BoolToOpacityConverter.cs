using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace RandomizerHost.Converters;

public class BoolToOpacityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
            return b ? 1.0 : 0.0;
        else
            return value != null ? 1.0 : 0.0;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
