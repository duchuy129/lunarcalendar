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
            Console.WriteLine($"========== CALLING API: GetMonthInfoAsync({year}, {month}) ==========");
            var result = await _apiClient.GetMonthInfoAsync(year, month);
            Console.WriteLine($"========== API RETURNED: {result?.Count ?? 0} lunar dates ==========");
            return result;
        }
        catch (Exception ex)
        {
            // Log error and return empty list for now
            // In a production app, you would use local fallback calculation
            Console.WriteLine($"========== ERROR: {ex.Message} ==========");
            Console.WriteLine($"========== STACK: {ex.StackTrace} ==========");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"========== INNER: {ex.InnerException.Message} ==========");
            }
            return new List<LunarDate>();
        }
    }
}
