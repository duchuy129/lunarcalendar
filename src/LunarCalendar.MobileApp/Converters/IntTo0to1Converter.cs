using System.Globalization;

namespace LunarCalendar.MobileApp.Converters;

/// <summary>
/// Converts an int (0-100) to a double (0.0-1.0) for use with ProgressBar.Progress.
/// </summary>
public class IntTo0to1Converter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intVal)
            return Math.Clamp(intVal / 100.0, 0.0, 1.0);
        return 0.0;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
