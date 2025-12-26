using Refit;
using LunarCalendar.MobileApp.Models;

namespace LunarCalendar.MobileApp.Services;

public interface ICalendarApiClient
{
    [Get("/api/v1/calendar/convert")]
    Task<LunarDate> ConvertToLunarAsync([Query] DateTime? date = null);

    [Get("/api/v1/calendar/month")]
    Task<List<LunarDate>> GetMonthInfoAsync([Query] int year, [Query] int month);
}
