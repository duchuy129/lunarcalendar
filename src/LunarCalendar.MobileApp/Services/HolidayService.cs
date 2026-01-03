using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;
using LunarCalendar.MobileApp.Data;
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
    private List<HolidayOccurrence>? _cachedMonthHolidays;
    private int _cachedYear;
    private int _cachedMonth;

    // Year-level cache to prevent duplicate calculations
    private readonly Dictionary<int, List<HolidayOccurrence>> _yearCache = new();
    private readonly object _yearCacheLock = new();

    public HolidayService(
        IHolidayCalculationService holidayCalculationService,
        LunarCalendarDatabase database)
    {
        _holidayCalculationService = holidayCalculationService;
        _database = database;
    }

    public async Task<List<Holiday>> GetAllHolidaysAsync()
    {
        return await Task.FromResult(_holidayCalculationService.GetAllHolidays());
    }

    public async Task<List<HolidayOccurrence>> GetHolidaysForYearAsync(int year)
    {
        // Check cache first (thread-safe)
        lock (_yearCacheLock)
        {
            if (_yearCache.TryGetValue(year, out var cached))
            {
                Debug.WriteLine($"HolidayService: Using cached holidays for year {year} ({cached.Count} items)");
                return cached;
            }
        }

        try
        {
            Debug.WriteLine($"HolidayService: Calculating holidays for year {year} locally");

            // Calculate locally - instant, no network needed
            var holidays = _holidayCalculationService.GetHolidaysForYear(year);

            // Store in cache (thread-safe)
            lock (_yearCacheLock)
            {
                _yearCache[year] = holidays;
                Debug.WriteLine($"HolidayService: Cached {holidays.Count} holidays for year {year}");
            }

            // Save to database in background for historical tracking
            _ = Task.Run(async () =>
            {
                try
                {
                    var entities = holidays.Select(h => new Data.Models.HolidayOccurrenceEntity
                    {
                        HolidayId = 0,
                        GregorianDate = h.GregorianDate,
                        Year = year,
                        Month = h.GregorianDate.Month,
                        Name = h.Holiday.Name,
                        Description = h.Holiday.Description,
                        Type = h.Holiday.Type.ToString(),
                        ColorHex = h.Holiday.ColorHex,
                        IsPublicHoliday = h.Holiday.IsPublicHoliday
                    }).ToList();

                    await _database.SaveHolidayOccurrencesAsync(entities);
                    Debug.WriteLine($"HolidayService: Saved {entities.Count} holidays to database");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error saving holidays to database: {ex.Message}");
                }
            });

            return await Task.FromResult(holidays);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error calculating holidays for year {year}: {ex.Message}");
            return new List<HolidayOccurrence>();
        }
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
            Debug.WriteLine($"HolidayService: Calculating holidays for {year}/{month} locally");

            // Calculate locally - instant, no network needed
            var holidays = _holidayCalculationService.GetHolidaysForMonth(year, month);

            // Cache the result in memory
            _cachedMonthHolidays = holidays;
            _cachedYear = year;
            _cachedMonth = month;

            // Save to database in background for historical tracking
            if (_cachedMonthHolidays.Any())
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var entities = _cachedMonthHolidays.Select(h => new Data.Models.HolidayOccurrenceEntity
                        {
                            HolidayId = 0,
                            GregorianDate = h.GregorianDate,
                            Year = year,
                            Month = month,
                            Name = h.Holiday.Name,
                            Description = h.Holiday.Description,
                            Type = h.Holiday.Type.ToString(),
                            ColorHex = h.Holiday.ColorHex,
                            IsPublicHoliday = h.Holiday.IsPublicHoliday
                        }).ToList();

                        await _database.SaveHolidayOccurrencesAsync(entities);
                        Debug.WriteLine($"HolidayService: Saved {entities.Count} holidays to database");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error saving holidays to database: {ex.Message}");
                    }
                });
            }

            return await Task.FromResult(_cachedMonthHolidays);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error calculating holidays for {year}/{month}: {ex.Message}");
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
            Debug.WriteLine($"Error getting holiday for date {date}: {ex.Message}");
            return null;
        }
    }
}
