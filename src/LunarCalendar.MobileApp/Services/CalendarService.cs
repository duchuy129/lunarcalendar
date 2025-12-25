using LunarCalendar.MobileApp.Models;

namespace LunarCalendar.MobileApp.Services;

public class CalendarService : ICalendarService
{
    private readonly ICalendarApiClient _apiClient;

    public CalendarService(ICalendarApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<LunarDate?> GetLunarDateAsync(DateTime date)
    {
        try
        {
            return await _apiClient.ConvertToLunarAsync(date);
        }
        catch (Exception ex)
        {
            // Log error and return null for now
            // In a production app, you would use local fallback calculation
            System.Diagnostics.Debug.WriteLine($"Error fetching lunar date: {ex.Message}");
            return null;
        }
    }

    public async Task<List<LunarDate>> GetMonthLunarDatesAsync(int year, int month)
    {
        try
        {
            return await _apiClient.GetMonthInfoAsync(year, month);
        }
        catch (Exception ex)
        {
            // Log error and return empty list for now
            // In a production app, you would use local fallback calculation
            System.Diagnostics.Debug.WriteLine($"Error fetching month lunar dates: {ex.Message}");
            return new List<LunarDate>();
        }
    }
}
