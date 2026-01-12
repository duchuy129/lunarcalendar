using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;
using LunarCalendar.MobileApp.Data;
using LunarCalendar.MobileApp.Data.Models;
using System.Diagnostics;

namespace LunarCalendar.MobileApp.Services;

/// <summary>
/// Calendar service with local calculation - no API required
/// Calculations happen instantly on device
/// </summary>
public class CalendarService : ICalendarService
{
    private readonly ILunarCalculationService _lunarCalculationService;
    private readonly LunarCalendarDatabase _database;
    private readonly ILogService _logService;

    public CalendarService(
        ILunarCalculationService lunarCalculationService,
        LunarCalendarDatabase database,
        ILogService logService)
    {
        _lunarCalculationService = lunarCalculationService;
        _database = database;
        _logService = logService;
    }

    public async Task<LunarDate?> GetLunarDateAsync(DateTime date)
    {
        try
        {
            // Calculate locally - instant, no network needed
            var lunarDate = _lunarCalculationService.ConvertToLunar(date);

            // FIX: Removed fire-and-forget Task.Run to prevent unobserved exceptions
            // Database caching is not critical - calculations are instant
            // If caching is needed later, it should be awaited or use proper background service

            return lunarDate;
        }
        catch (Exception ex)
        {
            _logService.LogError($"Failed to get lunar date for {date}", ex, "CalendarService.GetLunarDateAsync");
            return null;
        }
    }

    public async Task<List<LunarDate>> GetMonthLunarDatesAsync(int year, int month)
    {
        try
        {
            // Calculate locally - instant, no network needed
            var lunarDates = _lunarCalculationService.GetMonthInfo(year, month);

            // FIX: Removed fire-and-forget Task.Run to prevent unobserved exceptions
            // Database caching is not critical - calculations are instant
            // If caching is needed later, it should be awaited or use proper background service

            return lunarDates;
        }
        catch (Exception ex)
        {
            _logService.LogError($"Failed to get lunar dates for month {year}-{month}", ex, "CalendarService.GetMonthLunarDatesAsync");
            return new List<LunarDate>();
        }
    }
}
