using LunarCalendar.Api.Data;
using LunarCalendar.Api.Models;

namespace LunarCalendar.Api.Services;

public class HolidayService : IHolidayService
{
    private readonly ILunarCalendarService _lunarCalendarService;
    private readonly List<Holiday> _holidays;

    public HolidayService(ILunarCalendarService lunarCalendarService)
    {
        _lunarCalendarService = lunarCalendarService;
        _holidays = HolidaySeeder.GetVietnameseLunarHolidays();
    }

    public Task<List<Holiday>> GetAllHolidaysAsync()
    {
        return Task.FromResult(_holidays);
    }

    public Task<List<HolidayOccurrence>> GetHolidaysForYearAsync(int year)
    {
        var occurrences = new List<HolidayOccurrence>();

        foreach (var holiday in _holidays)
        {
            // Handle Gregorian-based holidays
            if (holiday.GregorianMonth.HasValue && holiday.GregorianDay.HasValue)
            {
                var gregorianDate = new DateTime(year, holiday.GregorianMonth.Value, holiday.GregorianDay.Value);
                var lunarInfo = _lunarCalendarService.ConvertToLunar(gregorianDate);

                occurrences.Add(new HolidayOccurrence
                {
                    Holiday = holiday,
                    GregorianDate = gregorianDate,
                    AnimalSign = lunarInfo.AnimalSign
                });
            }
            // Handle Lunar-based holidays
            else if (holiday.LunarMonth > 0 && holiday.LunarDay > 0)
            {
                // Lunar holidays can span across Gregorian years
                // For example: Tết 1/1 lunar year 2026 falls in Gregorian Feb 2026
                // But Tết 1/1 lunar year 2025 falls in Gregorian Jan 2025
                // We need to check both the current lunar year and previous lunar year

                // Try current lunar year (may fall late in Gregorian year)
                TryAddLunarHoliday(occurrences, holiday, year, year);

                // Try previous lunar year (may fall early in Gregorian year)
                TryAddLunarHoliday(occurrences, holiday, year - 1, year);
            }
        }

        return Task.FromResult(occurrences.OrderBy(h => h.GregorianDate).ToList());
    }

    private void TryAddLunarHoliday(List<HolidayOccurrence> occurrences, Holiday holiday, int lunarYear, int targetGregorianYear)
    {
        try
        {
            // Special handling for Giao Thừa (New Year's Eve) - lunar 12/30
            // Some lunar years have only 29 days in month 12
            // Try day 30 first, if it fails try day 29
            int dayToTry = holiday.LunarDay;

            if (holiday.Id == 19 && holiday.LunarMonth == 12 && holiday.LunarDay == 30)
            {
                // Try day 30 first
                try
                {
                    var gregorianDate30 = _lunarCalendarService.ConvertToGregorian(
                        lunarYear,
                        holiday.LunarMonth,
                        30,
                        holiday.IsLeapMonth
                    );

                    if (gregorianDate30.Year == targetGregorianYear)
                    {
                        if (!occurrences.Any(o => o.Holiday.Id == holiday.Id && o.GregorianDate.Date == gregorianDate30.Date))
                        {
                            var lunarInfo30 = _lunarCalendarService.ConvertToLunar(gregorianDate30);
                            occurrences.Add(new HolidayOccurrence
                            {
                                Holiday = holiday,
                                GregorianDate = gregorianDate30,
                                AnimalSign = lunarInfo30.AnimalSign
                            });
                        }
                    }
                    return; // Successfully added day 30
                }
                catch
                {
                    // Day 30 doesn't exist, try day 29
                    dayToTry = 29;
                }
            }

            var gregorianDate = _lunarCalendarService.ConvertToGregorian(
                lunarYear,
                holiday.LunarMonth,
                dayToTry,
                holiday.IsLeapMonth
            );

            // Only add if it falls in the target Gregorian year
            if (gregorianDate.Year == targetGregorianYear)
            {
                // Check if we already have this holiday occurrence
                if (!occurrences.Any(o => o.Holiday.Id == holiday.Id && o.GregorianDate.Date == gregorianDate.Date))
                {
                    var lunarInfo = _lunarCalendarService.ConvertToLunar(gregorianDate);
                    occurrences.Add(new HolidayOccurrence
                    {
                        Holiday = holiday,
                        GregorianDate = gregorianDate,
                        AnimalSign = lunarInfo.AnimalSign
                    });
                }
            }
        }
        catch (Exception)
        {
            // Skip holidays that can't be converted for this lunar year
            // (e.g., leap month doesn't exist in this year)
        }
    }

    public async Task<List<HolidayOccurrence>> GetHolidaysForMonthAsync(int year, int month)
    {
        var yearHolidays = await GetHolidaysForYearAsync(year);
        return yearHolidays
            .Where(h => h.GregorianDate.Year == year && h.GregorianDate.Month == month)
            .ToList();
    }

    public async Task<Holiday?> GetHolidayForDateAsync(DateTime date)
    {
        var monthHolidays = await GetHolidaysForMonthAsync(date.Year, date.Month);
        var occurrence = monthHolidays.FirstOrDefault(h => h.GregorianDate.Date == date.Date);
        return occurrence?.Holiday;
    }
}
