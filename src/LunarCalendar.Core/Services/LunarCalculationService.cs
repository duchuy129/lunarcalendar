using System.Globalization;
using LunarCalendar.Core.Models;

namespace LunarCalendar.Core.Services;

/// <summary>
/// Lunar calendar calculation service - uses .NET's ChineseLunisolarCalendar
/// Can be used by both mobile app and API
/// </summary>
public class LunarCalculationService : ILunarCalculationService
{
    private readonly ChineseLunisolarCalendar _chineseCalendar = new();

    private static readonly string[] HeavenlyStems = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
    private static readonly string[] EarthlyBranches = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };
    private static readonly string[] AnimalSigns = { "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig" };
    private static readonly string[] LunarMonthNames = { "正月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "冬月", "腊月" };
    private static readonly string[] LunarDayNames = {
        "初一", "初二", "初三", "初四", "初五", "初六", "初七", "初八", "初九", "初十",
        "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十",
        "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十"
    };

    public LunarDate ConvertToLunar(DateTime gregorianDate)
    {
        try
        {
            var lunarYear = _chineseCalendar.GetYear(gregorianDate);
            var lunarMonth = _chineseCalendar.GetMonth(gregorianDate);
            var lunarDay = _chineseCalendar.GetDayOfMonth(gregorianDate);
            var isLeapMonth = _chineseCalendar.IsLeapMonth(lunarYear, lunarMonth);

            // Adjust month number if it's after a leap month
            var displayMonth = lunarMonth;
            if (isLeapMonth)
            {
                displayMonth = lunarMonth - 1;
            }
            else
            {
                var leapMonth = _chineseCalendar.GetLeapMonth(lunarYear);
                if (leapMonth > 0 && lunarMonth > leapMonth)
                {
                    displayMonth = lunarMonth - 1;
                }
            }

            // Ensure display month is within valid range
            if (displayMonth < 1) displayMonth = 1;
            if (displayMonth > 12) displayMonth = 12;

            var heavenlyStem = HeavenlyStems[(lunarYear - 4) % 10];
            var earthlyBranch = EarthlyBranches[(lunarYear - 4) % 12];
            var animalSign = AnimalSigns[(lunarYear - 4) % 12];

            var lunarMonthName = isLeapMonth ? $"闰{LunarMonthNames[displayMonth - 1]}" : LunarMonthNames[displayMonth - 1];
            var lunarDayName = lunarDay <= LunarDayNames.Length ? LunarDayNames[lunarDay - 1] : $"{lunarDay}";

            return new LunarDate
            {
                GregorianDate = gregorianDate,
                LunarYear = lunarYear,
                LunarMonth = displayMonth,
                LunarDay = lunarDay,
                IsLeapMonth = isLeapMonth,
                LunarYearName = $"{heavenlyStem}{earthlyBranch}年",
                LunarMonthName = lunarMonthName,
                LunarDayName = lunarDayName,
                AnimalSign = animalSign
            };
        }
        catch (Exception)
        {
            // Return default values if conversion fails (date out of range)
            return new LunarDate
            {
                GregorianDate = gregorianDate,
                LunarYear = gregorianDate.Year,
                LunarMonth = gregorianDate.Month,
                LunarDay = gregorianDate.Day,
                IsLeapMonth = false,
                LunarYearName = string.Empty,
                LunarMonthName = string.Empty,
                LunarDayName = string.Empty,
                AnimalSign = string.Empty
            };
        }
    }

    public DateTime ConvertToGregorian(int year, int month, int day, bool isLeapMonth = false)
    {
        var leapMonth = _chineseCalendar.GetLeapMonth(year);

        var adjustedMonth = month;
        if (isLeapMonth && leapMonth == month)
        {
            adjustedMonth = month;
        }
        else if (leapMonth > 0 && month >= leapMonth && !isLeapMonth)
        {
            adjustedMonth = month + 1;
        }

        return _chineseCalendar.ToDateTime(year, adjustedMonth, day, 0, 0, 0, 0);
    }

    public List<LunarDate> GetMonthInfo(int year, int month)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var results = new List<LunarDate>();

        for (int day = 1; day <= daysInMonth; day++)
        {
            var gregorianDate = new DateTime(year, month, day);
            results.Add(ConvertToLunar(gregorianDate));
        }

        return results;
    }
}
