using System.Globalization;

namespace LunarCalendar.MobileApp.Converters;

/// <summary>
/// Converter that returns true if a string is not null or empty, false otherwise.
/// Useful for controlling visibility of UI elements based on string content.
/// </summary>
public class StringNotEmptyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return !string.IsNullOrEmpty(value as string);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
