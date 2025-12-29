using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;

namespace LunarCalendar.Api.Services;

/// <summary>
/// API Holiday Service - wraps Core's HolidayCalculationService
/// Uses shared Core library for all holiday calculations
/// </summary>
public class HolidayService : IHolidayService
{
    private readonly IHolidayCalculationService _holidayCalculationService;

    public HolidayService(IHolidayCalculationService holidayCalculationService)
    {
        _holidayCalculationService = holidayCalculationService;
    }

    public Task<List<Holiday>> GetAllHolidaysAsync()
    {
        return Task.FromResult(_holidayCalculationService.GetAllHolidays());
    }

    public Task<List<HolidayOccurrence>> GetHolidaysForYearAsync(int year)
    {
        return Task.FromResult(_holidayCalculationService.GetHolidaysForYear(year));
    }

    public Task<List<HolidayOccurrence>> GetHolidaysForMonthAsync(int year, int month)
    {
        return Task.FromResult(_holidayCalculationService.GetHolidaysForMonth(year, month));
    }

    public Task<Holiday?> GetHolidayForDateAsync(DateTime date)
    {
        return Task.FromResult(_holidayCalculationService.GetHolidayForDate(date));
    }
}
