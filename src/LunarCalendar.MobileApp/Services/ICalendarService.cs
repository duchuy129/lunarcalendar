using LunarCalendar.Core.Models;

namespace LunarCalendar.MobileApp.Services;

public interface ICalendarService
{
    Task<LunarDate?> GetLunarDateAsync(DateTime date);
    Task<List<LunarDate>> GetMonthLunarDatesAsync(int year, int month);
}
