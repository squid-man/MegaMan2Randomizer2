using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using RandomizerHost.Extensions;

namespace RandomizerHost.Converters
{
    public class EnumValueToDescriptionConverter : IValueConverter
    {
        public Object Convert(Object in_Value, Type in_TargetType, Object in_Parameter, CultureInfo in_Culture)
        {
            if (typeof(String) != in_TargetType)
            {
                return new BindingNotification(
                    new NotSupportedException(@"Only String target type is allowed"),
                    BindingErrorType.Error);
            }

            if (in_Value is Enum value)
            {
                return value.GetDescription();
            }
            else
            {
                return new BindingNotification(
                    new ArgumentNullException(nameof(in_Value)),
                    BindingErrorType.Error);
            }
        }

        public Object ConvertBack(Object in_Value, Type in_TargetType, Object in_Parameter, CultureInfo in_Culture)
        {
            throw new NotSupportedException();
        }
    }
}
