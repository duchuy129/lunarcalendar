using LunarCalendar.Api.Models;
using LunarCalendar.Core.Services;

namespace LunarCalendar.Api.Services;

/// <summary>
/// API Lunar Calendar Service - extends Core's LunarCalculationService with additional features
/// Adds HeavenlyStem, EarthlyBranch, Festivals, and SolarTerms on top of Core calculations
/// </summary>
public class LunarCalendarService : ILunarCalendarService
{
    private readonly ILunarCalculationService _coreLunarService;

    private static readonly string[] HeavenlyStems = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
    private static readonly string[] EarthlyBranches = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

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

    public LunarCalendarService(ILunarCalculationService coreLunarService)
    {
        _coreLunarService = coreLunarService;
    }

    public LunarDateInfo ConvertToLunar(DateTime gregorianDate)
    {
        // Use Core service for base calculation
        var coreLunarDate = _coreLunarService.ConvertToLunar(gregorianDate);

        // Add API-specific fields
        var heavenlyStem = HeavenlyStems[(coreLunarDate.LunarYear - 4) % 10];
        var earthlyBranch = EarthlyBranches[(coreLunarDate.LunarYear - 4) % 12];

        return new LunarDateInfo
        {
            GregorianDate = coreLunarDate.GregorianDate,
            LunarYear = coreLunarDate.LunarYear,
            LunarMonth = coreLunarDate.LunarMonth,
            LunarDay = coreLunarDate.LunarDay,
            IsLeapMonth = coreLunarDate.IsLeapMonth,
            LunarYearName = coreLunarDate.LunarYearName,
            LunarMonthName = coreLunarDate.LunarMonthName,
            LunarDayName = coreLunarDate.LunarDayName,
            AnimalSign = coreLunarDate.AnimalSign,
            HeavenlyStem = heavenlyStem,
            EarthlyBranch = earthlyBranch
        };
    }

    public DateTime ConvertToGregorian(int year, int month, int day, bool isLeapMonth = false)
    {
        return _coreLunarService.ConvertToGregorian(year, month, day, isLeapMonth);
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
        // Use Core service for base calculation
        var coreLunarDates = _coreLunarService.GetMonthInfo(year, month);

        // Convert to API's LunarDateInfo with extra fields
        return coreLunarDates.Select(coreLunarDate =>
        {
            var heavenlyStem = HeavenlyStems[(coreLunarDate.LunarYear - 4) % 10];
            var earthlyBranch = EarthlyBranches[(coreLunarDate.LunarYear - 4) % 12];

            return new LunarDateInfo
            {
                GregorianDate = coreLunarDate.GregorianDate,
                LunarYear = coreLunarDate.LunarYear,
                LunarMonth = coreLunarDate.LunarMonth,
                LunarDay = coreLunarDate.LunarDay,
                IsLeapMonth = coreLunarDate.IsLeapMonth,
                LunarYearName = coreLunarDate.LunarYearName,
                LunarMonthName = coreLunarDate.LunarMonthName,
                LunarDayName = coreLunarDate.LunarDayName,
                AnimalSign = coreLunarDate.AnimalSign,
                HeavenlyStem = heavenlyStem,
                EarthlyBranch = earthlyBranch
            };
        });
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
