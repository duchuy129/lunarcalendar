using System.Globalization;
using LunarCalendar.Api.Models;

namespace LunarCalendar.Api.Services;

public class LunarCalendarService : ILunarCalendarService
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

    // Solar terms (24 solar terms)
    private static readonly Dictionary<int, string> SolarTerms = new()
    {
        { 1, "立春" }, { 2, "雨水" }, { 3, "惊蛰" }, { 4, "春分" },
        { 5, "清明" }, { 6, "谷雨" }, { 7, "立夏" }, { 8, "小满" },
        { 9, "芒种" }, { 10, "夏至" }, { 11, "小暑" }, { 12, "大暑" },
        { 13, "立秋" }, { 14, "处暑" }, { 15, "白露" }, { 16, "秋分" },
        { 17, "寒露" }, { 18, "霜降" }, { 19, "立冬" }, { 20, "小雪" },
        { 21, "大雪" }, { 22, "冬至" }, { 23, "小寒" }, { 24, "大寒" }
    };

    // Traditional festivals
    private static readonly Dictionary<string, string> Festivals = new()
    {
        { "1-1", "Spring Festival" },
        { "1-15", "Lantern Festival" },
        { "5-5", "Dragon Boat Festival" },
        { "7-7", "Qixi Festival" },
        { "8-15", "Mid-Autumn Festival" },
        { "9-9", "Double Ninth Festival" },
        { "12-8", "Laba Festival" }
    };

    public LunarDateInfo ConvertToLunar(DateTime gregorianDate)
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

            return new LunarDateInfo
            {
                GregorianDate = gregorianDate,
                LunarYear = lunarYear,
                LunarMonth = displayMonth,
                LunarDay = lunarDay,
                IsLeapMonth = isLeapMonth,
                LunarYearName = $"{heavenlyStem}{earthlyBranch}年",
                LunarMonthName = lunarMonthName,
                LunarDayName = lunarDayName,
                AnimalSign = animalSign,
                HeavenlyStem = heavenlyStem,
                EarthlyBranch = earthlyBranch
            };
        }
        catch (Exception)
        {
            // Return default values if conversion fails (date out of range)
            return new LunarDateInfo
            {
                GregorianDate = gregorianDate,
                LunarYear = gregorianDate.Year,
                LunarMonth = gregorianDate.Month,
                LunarDay = gregorianDate.Day,
                IsLeapMonth = false,
                LunarYearName = string.Empty,
                LunarMonthName = string.Empty,
                LunarDayName = string.Empty,
                AnimalSign = string.Empty,
                HeavenlyStem = string.Empty,
                EarthlyBranch = string.Empty
            };
        }
    }

    public DateTime ConvertToGregorian(int year, int month, int day, bool isLeapMonth = false)
    {
        try
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
        catch (Exception)
        {
            // Return current date if conversion fails
            return DateTime.Today;
        }
    }

    public LunarCalendarInfo GetLunarInfo(DateTime gregorianDate)
    {
        var dateInfo = ConvertToLunar(gregorianDate);
        var festival = GetFestival(dateInfo.LunarMonth, dateInfo.LunarDay);
        var solarTerm = GetSolarTerm(gregorianDate);

        return new LunarCalendarInfo
        {
            DateInfo = dateInfo,
            Festival = festival,
            SolarTerm = solarTerm,
            IsHoliday = !string.IsNullOrEmpty(festival)
        };
    }

    public IEnumerable<LunarDateInfo> GetMonthInfo(int year, int month)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var results = new List<LunarDateInfo>();

        for (int day = 1; day <= daysInMonth; day++)
        {
            var gregorianDate = new DateTime(year, month, day);
            results.Add(ConvertToLunar(gregorianDate));
        }

        return results;
    }

    private string GetFestival(int lunarMonth, int lunarDay)
    {
        var key = $"{lunarMonth}-{lunarDay}";
        return Festivals.TryGetValue(key, out var festival) ? festival : string.Empty;
    }

    private string GetSolarTerm(DateTime date)
    {
        // Simplified solar term calculation
        // In a production app, you would use more accurate astronomical calculations
        // For now, returning empty string
        return string.Empty;
    }
}
