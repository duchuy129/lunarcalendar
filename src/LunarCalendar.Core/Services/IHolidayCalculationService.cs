using LunarCalendar.Core.Models;

namespace LunarCalendar.Core.Services;

public interface IHolidayCalculationService
{
    List<Holiday> GetAllHolidays();
    List<HolidayOccurrence> GetHolidaysForYear(int year);
    List<HolidayOccurrence> GetHolidaysForMonth(int year, int month);
    Holiday? GetHolidayForDate(DateTime date);
}
