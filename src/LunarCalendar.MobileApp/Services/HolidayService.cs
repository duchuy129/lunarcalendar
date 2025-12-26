using LunarCalendar.MobileApp.Models;
using System.Text.Json;

namespace LunarCalendar.MobileApp.Services;

public class HolidayService : IHolidayService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private List<HolidayOccurrence>? _cachedMonthHolidays;
    private int _cachedYear;
    private int _cachedMonth;

    public HolidayService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        // Configure base URL based on platform and device type
        if (DeviceInfo.Platform == DevicePlatform.Android && DeviceInfo.DeviceType == DeviceType.Virtual)
        {
            // Android emulator uses special IP to access host machine
            _baseUrl = "http://10.0.2.2:5090";
        }
        else if (DeviceInfo.DeviceType == DeviceType.Physical)
        {
            // For physical devices, use your computer's actual IP address on the local network
            _baseUrl = "http://10.0.0.72:5090"; // Your computer's IP
        }
        else
        {
            // iOS simulator and other virtual devices use localhost
            _baseUrl = "http://localhost:5090";
        }
    }

    public async Task<List<Holiday>> GetAllHolidaysAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/holiday");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var holidays = JsonSerializer.Deserialize<List<Holiday>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return holidays ?? new List<Holiday>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching holidays: {ex.Message}");
            return new List<Holiday>();
        }
    }

    public async Task<List<HolidayOccurrence>> GetHolidaysForYearAsync(int year)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/holiday/year/{year}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var holidays = JsonSerializer.Deserialize<List<HolidayOccurrence>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return holidays ?? new List<HolidayOccurrence>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching holidays for year {year}: {ex.Message}");
            return new List<HolidayOccurrence>();
        }
    }

    public async Task<List<HolidayOccurrence>> GetHolidaysForMonthAsync(int year, int month)
    {
        // Check cache first
        if (_cachedMonthHolidays != null && _cachedYear == year && _cachedMonth == month)
        {
            return _cachedMonthHolidays;
        }

        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/holiday/month/{year}/{month}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var holidays = JsonSerializer.Deserialize<List<HolidayOccurrence>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Cache the result
            _cachedMonthHolidays = holidays ?? new List<HolidayOccurrence>();
            _cachedYear = year;
            _cachedMonth = month;

            return _cachedMonthHolidays;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching holidays for {year}/{month}: {ex.Message}");
            return new List<HolidayOccurrence>();
        }
    }

    public async Task<Holiday?> GetHolidayForDateAsync(DateTime date)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/holiday/date/{date.Year}/{date.Month}/{date.Day}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var holiday = JsonSerializer.Deserialize<Holiday>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return holiday;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching holiday for date {date}: {ex.Message}");
            return null;
        }
    }
}
