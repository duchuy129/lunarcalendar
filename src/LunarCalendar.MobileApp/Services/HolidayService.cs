using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;
using LunarCalendar.MobileApp.Data;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace LunarCalendar.MobileApp.Services;

/// <summary>
/// Holiday service with local calculation - no API required
/// Computes holiday occurrences on device
/// </summary>
public class HolidayService : IHolidayService
{
    private readonly IHolidayCalculationService _holidayCalculationService;
    private readonly LunarCalendarDatabase _database;
    private readonly ILogService _logService;
    private List<HolidayOccurrence>? _cachedMonthHolidays;
    private int _cachedYear;
    private int _cachedMonth;

    // FIX: Use ConcurrentDictionary for thread-safe year cache without manual locking
    private readonly ConcurrentDictionary<int, List<HolidayOccurrence>> _yearCache = new();

    public HolidayService(
        IHolidayCalculationService holidayCalculationService,
        LunarCalendarDatabase database,
        ILogService logService)
    {
        _holidayCalculationService = holidayCalculationService;
        _database = database;
        _logService = logService;
    }

    public async Task<List<Holiday>> GetAllHolidaysAsync()
    {
        return await Task.FromResult(_holidayCalculationService.GetAllHolidays());
    }

    public async Task<List<HolidayOccurrence>> GetHolidaysForYearAsync(int year)
    {
        // FIX: Use ConcurrentDictionary.GetOrAdd for thread-safe caching
        // This is atomic and eliminates the need for manual locking
        var holidays = _yearCache.GetOrAdd(year, y =>
        {
            // Calculate locally - instant, no network needed
            return _holidayCalculationService.GetHolidaysForYear(y);
        });

        return await Task.FromResult(holidays);
    }

    public async Task<List<HolidayOccurrence>> GetHolidaysForMonthAsync(int year, int month)
    {
        // Check memory cache first
        if (_cachedMonthHolidays != null && _cachedYear == year && _cachedMonth == month)
        {
            return _cachedMonthHolidays;
        }

        try
        {

            // Calculate locally - instant, no network needed
            var holidays = _holidayCalculationService.GetHolidaysForMonth(year, month);

            // Cache the result in memory
            _cachedMonthHolidays = holidays;
            _cachedYear = year;
            _cachedMonth = month;

            // FIX: Removed fire-and-forget Task.Run to prevent unobserved exceptions
            // Database caching is not critical - calculations are instant
            // If caching is needed later, it should be awaited or use proper background service

            return await Task.FromResult(_cachedMonthHolidays);
        }
        catch (Exception ex)
        {
            _logService.LogError("Failed to get year holidays", ex, "HolidayService.GetYearHolidaysAsync");
            return new List<HolidayOccurrence>();
        }
    }

    public async Task<Holiday?> GetHolidayForDateAsync(DateTime date)
    {
        try
        {
            var holiday = _holidayCalculationService.GetHolidayForDate(date);
            return await Task.FromResult(holiday);
        }
        catch (Exception ex)
        {
            _logService.LogError($"Failed to get holiday for date {date}", ex, "HolidayService.GetHolidayForDateAsync");
            return null;
        }
    }
}
