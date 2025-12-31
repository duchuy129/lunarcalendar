using System.Globalization;

namespace LunarCalendar.MobileApp.Converters;

public class BoolToExpandIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isExpanded)
        {
            // Using clean chevron icons: down chevron when expanded, right chevron when collapsed
            return isExpanded ? "⌄" : "›";
        }
        return "›";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
