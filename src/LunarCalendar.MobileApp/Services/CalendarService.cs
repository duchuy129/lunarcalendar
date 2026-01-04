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

            // Save to database in background for historical tracking
            _ = Task.Run(async () =>
            {
                try
                {
                    var entity = new LunarDateEntity
                    {
                        GregorianDate = date.Date,
                        LunarYear = lunarDate.LunarYear,
                        LunarMonth = lunarDate.LunarMonth,
                        LunarDay = lunarDate.LunarDay,
                        IsLeapMonth = lunarDate.IsLeapMonth,
                        LunarYearName = lunarDate.LunarYearName,
                        LunarMonthName = lunarDate.LunarMonthName,
                        LunarDayName = lunarDate.LunarDayName,
                        AnimalSign = lunarDate.AnimalSign
                    };
                    await _database.SaveLunarDateAsync(entity);
                }
                catch (Exception ex)
                {
                    _logService.LogError($"Failed to save lunar date to database for {date}", ex, "CalendarService.GetLunarDateAsync");
                }
            });

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


            // Save to database in background for historical tracking
            _ = Task.Run(async () =>
            {
                try
                {
                    var entities = lunarDates.Select(ld => new LunarDateEntity
                    {
                        GregorianDate = ld.GregorianDate,
                        LunarYear = ld.LunarYear,
                        LunarMonth = ld.LunarMonth,
                        LunarDay = ld.LunarDay,
                        IsLeapMonth = ld.IsLeapMonth,
                        LunarYearName = ld.LunarYearName,
                        LunarMonthName = ld.LunarMonthName,
                        LunarDayName = ld.LunarDayName,
                        AnimalSign = ld.AnimalSign
                    }).ToList();

                    await _database.SaveLunarDatesAsync(entities);
                }
                catch (Exception ex)
                {
                    _logService.LogError("Failed to save lunar dates to database", ex, "CalendarService.GetLunarDatesForMonthAsync");
                }
            });

            return lunarDates;
        }
        catch (Exception ex)
        {
            _logService.LogError($"Failed to get lunar dates for month {year}-{month}", ex, "CalendarService.GetMonthLunarDatesAsync");
            return new List<LunarDate>();
        }
    }
}
