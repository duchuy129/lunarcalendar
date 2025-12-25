using LunarCalendar.Api.Models;

namespace LunarCalendar.Api.Services;

public interface ILunarCalendarService
{
    /// <summary>
    /// Converts a Gregorian date to lunar date information
    /// </summary>
    LunarDateInfo ConvertToLunar(DateTime gregorianDate);

    /// <summary>
    /// Converts a lunar date to Gregorian date
    /// </summary>
    DateTime ConvertToGregorian(int year, int month, int day, bool isLeapMonth = false);

    /// <summary>
    /// Gets lunar calendar information for a specific Gregorian date
    /// </summary>
    LunarCalendarInfo GetLunarInfo(DateTime gregorianDate);

    /// <summary>
    /// Gets lunar calendar information for a month range
    /// </summary>
    IEnumerable<LunarDateInfo> GetMonthInfo(int year, int month);
}
