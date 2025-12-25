using LunarCalendar.MobileApp.Models;

namespace LunarCalendar.MobileApp.Services;

public interface IHolidayService
{
    Task<List<Holiday>> GetAllHolidaysAsync();
    Task<List<HolidayOccurrence>> GetHolidaysForYearAsync(int year);
    Task<List<HolidayOccurrence>> GetHolidaysForMonthAsync(int year, int month);
    Task<Holiday?> GetHolidayForDateAsync(DateTime date);
}
