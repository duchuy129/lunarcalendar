using System.Globalization;
using LunarCalendar.MobileApp.Resources.Strings;

namespace LunarCalendar.MobileApp.Helpers;

/// <summary>
/// Helper class for formatting dates according to cultural preferences
/// Vietnamese: "Ngày X Tháng Y Năm Z" format
/// English: "Month Day, Year" format
/// </summary>
public static class DateFormatterHelper
{
    /// <summary>
    /// Formats a Gregorian date according to the current culture
    /// Vietnamese: "Ngày 1 Tháng 1 Năm 2026 (Thứ Hai)"
    /// English: "January 1, 2026 (Monday)"
    /// </summary>
    public static string FormatGregorianDate(DateTime date, bool includeDayOfWeek = false)
    {
        var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        if (currentCulture == "vi")
        {
            // Vietnamese format: Ngày X Tháng Y Năm Z
            var dayOfWeek = includeDayOfWeek ? $" ({GetVietnameseDayOfWeek(date.DayOfWeek)})" : "";
            return $"Ngày {date.Day} Tháng {date.Month} Năm {date.Year}{dayOfWeek}";
        }
        else
        {
            // English format: Month Day, Year
            var format = includeDayOfWeek ? "MMMM dd, yyyy (dddd)" : "MMMM dd, yyyy";
            return date.ToString(format, CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Formats a Gregorian date for short display (without day of week)
    /// Vietnamese: "Ngày 1 Tháng 1 Năm 2026"
    /// English: "January 1, 2026"
    /// </summary>
    public static string FormatGregorianDateShort(DateTime date)
    {
        return FormatGregorianDate(date, includeDayOfWeek: false);
    }

    /// <summary>
    /// Formats a Gregorian date with day of week
    /// Vietnamese: "Ngày 1 Tháng 1 Năm 2026 (Thứ Hai)"
    /// English: "January 1, 2026 (Monday)"
    /// </summary>
    public static string FormatGregorianDateLong(DateTime date)
    {
        return FormatGregorianDate(date, includeDayOfWeek: true);
    }

    /// <summary>
    /// Formats a lunar date according to the current culture
    /// Vietnamese: "Ngày 3 Tháng 10"
    /// English: "3/10" (simple numeric format)
    /// </summary>
    public static string FormatLunarDate(int lunarDay, int lunarMonth)
    {
        var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        if (currentCulture == "vi")
        {
            // Vietnamese format: Ngày X Tháng Y
            return $"Ngày {lunarDay} Tháng {lunarMonth}";
        }
        else
        {
            // English format: Simple numeric (3/10)
            return $"{lunarDay}/{lunarMonth}";
        }
    }

    /// <summary>
    /// Formats a lunar date with year and animal sign
    /// Vietnamese: "Ngày 3 Tháng 10 Năm Tỵ"
    /// English: "3/10, Year of the Snake" (simple format)
    /// </summary>
    public static string FormatLunarDateWithYear(int lunarDay, int lunarMonth, string animalSign)
    {
        var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        
        System.Diagnostics.Debug.WriteLine($"=== FormatLunarDateWithYear called ===");
        System.Diagnostics.Debug.WriteLine($"=== CurrentUICulture: {CultureInfo.CurrentUICulture.Name} ===");
        System.Diagnostics.Debug.WriteLine($"=== TwoLetterISO: {currentCulture} ===");
        System.Diagnostics.Debug.WriteLine($"=== Input: {lunarDay}/{lunarMonth}, {animalSign} ===");

        string result;
        if (currentCulture == "vi")
        {
            // Vietnamese format: Ngày X Tháng Y, Năm [Con Giáp]
            result = $"Ngày {lunarDay} Tháng {lunarMonth}, Năm {animalSign}";
            System.Diagnostics.Debug.WriteLine($"=== Vietnamese format: {result} ===");
        }
        else
        {
            // English format: X/Y Year of the [Animal]
            result = $"{lunarDay}/{lunarMonth} Year of the {animalSign}";
            System.Diagnostics.Debug.WriteLine($"=== English format: {result} ===");
        }
        
        return result;
    }

    /// <summary>
    /// Formats a simple lunar date display (used in Today section)
    /// Vietnamese: "3/10"
    /// English: "3/10"
    /// </summary>
    public static string FormatLunarDateSimple(int lunarDay, int lunarMonth)
    {
        return $"{lunarDay}/{lunarMonth}";
    }

    /// <summary>
    /// Formats the lunar date label with prefix
    /// Vietnamese: "Âm lịch: Ngày 3 Tháng 10"
    /// English: "Lunar: 3/10" (simple format)
    /// </summary>
    public static string FormatLunarDateWithLabel(int lunarDay, int lunarMonth)
    {
        var lunarLabel = AppResources.LunarLabel;
        var formattedDate = FormatLunarDate(lunarDay, lunarMonth);
        return $"{lunarLabel} {formattedDate}";
    }

    /// <summary>
    /// Gets Vietnamese day of week name
    /// </summary>
    private static string GetVietnameseDayOfWeek(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => "Thứ Hai",
            DayOfWeek.Tuesday => "Thứ Ba",
            DayOfWeek.Wednesday => "Thứ Tư",
            DayOfWeek.Thursday => "Thứ Năm",
            DayOfWeek.Friday => "Thứ Sáu",
            DayOfWeek.Saturday => "Thứ Bảy",
            DayOfWeek.Sunday => "Chủ Nhật",
            _ => ""
        };
    }

    /// <summary>
    /// Formats lunar date for calendar display (simple format)
    /// Both cultures: "1/15" or "10/1"
    /// </summary>
    public static string FormatLunarDateForCalendar(int lunarDay, int lunarMonth)
    {
        return $"{lunarDay}/{lunarMonth}";
    }
}
