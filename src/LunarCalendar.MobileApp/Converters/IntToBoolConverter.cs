using System.Globalization;

namespace LunarCalendar.MobileApp.Converters;

public class IntToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            var result = intValue > 0;
            // Check if parameter is "inverse" to invert the result
            if (parameter is string paramString && paramString.Equals("inverse", StringComparison.OrdinalIgnoreCase))
            {
                return !result;
            }
            return result;
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
