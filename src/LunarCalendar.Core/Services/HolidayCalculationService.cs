using LunarCalendar.Core.Models;
using LunarCalendar.Core.Data;

namespace LunarCalendar.Core.Services;

/// <summary>
/// Holiday calculation service - computes holiday occurrences
/// Can be used by both mobile app and API
/// </summary>
public class HolidayCalculationService : IHolidayCalculationService
{
    private readonly ILunarCalculationService _lunarCalculationService;
    private readonly List<Holiday> _holidays;

    public HolidayCalculationService(ILunarCalculationService lunarCalculationService)
    {
        _lunarCalculationService = lunarCalculationService;
        _holidays = HolidaySeeder.GetVietnameseLunarHolidays();
    }

    public List<Holiday> GetAllHolidays()
    {
        return _holidays;
    }

    public List<HolidayOccurrence> GetHolidaysForYear(int year)
    {
        var occurrences = new List<HolidayOccurrence>();

        foreach (var holiday in _holidays)
        {
            // Handle Gregorian-based holidays
            if (holiday.GregorianMonth.HasValue && holiday.GregorianDay.HasValue)
            {
                var gregorianDate = new DateTime(year, holiday.GregorianMonth.Value, holiday.GregorianDay.Value);
                var lunarInfo = _lunarCalculationService.ConvertToLunar(gregorianDate);

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
                // Try current lunar year (may fall late in Gregorian year)
                TryAddLunarHoliday(occurrences, holiday, year, year);

                // Try previous lunar year (may fall early in Gregorian year)
                TryAddLunarHoliday(occurrences, holiday, year - 1, year);
            }
        }

        return occurrences.OrderBy(h => h.GregorianDate).ToList();
    }

    private void TryAddLunarHoliday(List<HolidayOccurrence> occurrences, Holiday holiday, int lunarYear, int targetGregorianYear)
    {
        try
        {
            // Special handling for Giao Thừa (New Year's Eve) - lunar 12/30
            // Some lunar years have only 29 days in month 12
            int dayToTry = holiday.LunarDay;

            if (holiday.Id == 19 && holiday.LunarMonth == 12 && holiday.LunarDay == 30)
            {
                // Try day 30 first
                try
                {
                    var gregorianDate30 = _lunarCalculationService.ConvertToGregorian(
                        lunarYear,
                        holiday.LunarMonth,
                        30,
                        holiday.IsLeapMonth
                    );

                    if (gregorianDate30.Year == targetGregorianYear)
                    {
                        if (!occurrences.Any(o => o.Holiday.Id == holiday.Id && o.GregorianDate.Date == gregorianDate30.Date))
                        {
                            var lunarInfo30 = _lunarCalculationService.ConvertToLunar(gregorianDate30);
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

            var gregorianDate = _lunarCalculationService.ConvertToGregorian(
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
                    var lunarInfo = _lunarCalculationService.ConvertToLunar(gregorianDate);
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

    public List<HolidayOccurrence> GetHolidaysForMonth(int year, int month)
    {
        var yearHolidays = GetHolidaysForYear(year);
        return yearHolidays
            .Where(h => h.GregorianDate.Year == year && h.GregorianDate.Month == month)
            .ToList();
    }

    public Holiday? GetHolidayForDate(DateTime date)
    {
        var monthHolidays = GetHolidaysForMonth(date.Year, date.Month);
        var occurrence = monthHolidays.FirstOrDefault(h => h.GregorianDate.Date == date.Date);
        return occurrence?.Holiday;
    }
}
