using System.Globalization;

namespace LunarCalendar.MobileApp.Converters;

public class MonthIndexConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int month && month >= 1 && month <= 12)
        {
            return month - 1; // Convert 1-based month to 0-based index
        }
        return 0;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int index && index >= 0 && index <= 11)
        {
            return index + 1; // Convert 0-based index to 1-based month
        }
        return 1;
    }
}
