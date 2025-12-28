using LunarCalendar.MobileApp.Models;
using LunarCalendar.MobileApp.Data;
using System.Text.Json;
using System.Diagnostics;

namespace LunarCalendar.MobileApp.Services;

public class HolidayService : IHolidayService
{
    private readonly HttpClient _httpClient;
    private readonly IConnectivityService _connectivityService;
    private readonly LunarCalendarDatabase _database;
    private List<HolidayOccurrence>? _cachedMonthHolidays;
    private int _cachedYear;
    private int _cachedMonth;

    public HolidayService(
        HttpClient httpClient,
        IConnectivityService connectivityService,
        LunarCalendarDatabase database)
    {
        _httpClient = httpClient;
        _connectivityService = connectivityService;
        _database = database;
        // Note: HttpClient.BaseAddress is configured in MauiProgram.cs from appsettings.json
    }

    public async Task<List<Holiday>> GetAllHolidaysAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/v1/holiday");
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
        // Try online first if connected
        if (_connectivityService.IsConnected)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/v1/holiday/year/{year}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var holidays = JsonSerializer.Deserialize<List<HolidayOccurrence>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (holidays != null && holidays.Any())
                {
                    // Save to database in background
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            var entities = holidays.Select(h => new Data.Models.HolidayOccurrenceEntity
                            {
                                HolidayId = 0,
                                GregorianDate = h.GregorianDate,
                                Year = year,
                                Month = h.GregorianDate.Month,
                                Name = h.Holiday.Name,
                                Description = h.Holiday.Description,
                                Type = h.Holiday.Type.ToString(),
                                ColorHex = h.Holiday.ColorHex,
                                IsPublicHoliday = h.Holiday.IsPublicHoliday
                            }).ToList();

                            await _database.SaveHolidayOccurrencesAsync(entities);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error saving holidays to database: {ex.Message}");
                        }
                    });
                }

                return holidays ?? new List<HolidayOccurrence>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching holidays online for year {year}: {ex.Message}, falling back to offline data");
            }
        }

        // Fallback to offline data
        try
        {
            var cachedEntities = await _database.GetHolidaysForYearAsync(year);
            return cachedEntities.Select(e => new HolidayOccurrence
            {
                GregorianDate = e.GregorianDate,
                Holiday = new Holiday
                {
                    Name = e.Name,
                    Description = e.Description ?? string.Empty,
                    Type = Enum.TryParse<HolidayType>(e.Type, out var type) ? type : HolidayType.TraditionalFestival,
                    ColorHex = e.ColorHex ?? "#FF0000",
                    IsPublicHoliday = e.IsPublicHoliday
                }
            }).ToList();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching offline holidays for year {year}: {ex.Message}");
            return new List<HolidayOccurrence>();
        }
    }

    public async Task<List<HolidayOccurrence>> GetHolidaysForMonthAsync(int year, int month)
    {
        // Check memory cache first
        if (_cachedMonthHolidays != null && _cachedYear == year && _cachedMonth == month)
        {
            return _cachedMonthHolidays;
        }

        // Try online first if connected
        if (_connectivityService.IsConnected)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/v1/holiday/month/{year}/{month}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var holidays = JsonSerializer.Deserialize<List<HolidayOccurrence>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Cache the result in memory
                _cachedMonthHolidays = holidays ?? new List<HolidayOccurrence>();
                _cachedYear = year;
                _cachedMonth = month;

                // Save to database in background
                if (_cachedMonthHolidays.Any())
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            var entities = _cachedMonthHolidays.Select(h => new Data.Models.HolidayOccurrenceEntity
                            {
                                HolidayId = 0,
                                GregorianDate = h.GregorianDate,
                                Year = year,
                                Month = month,
                                Name = h.Holiday.Name,
                                Description = h.Holiday.Description,
                                Type = h.Holiday.Type.ToString(),
                                ColorHex = h.Holiday.ColorHex,
                                IsPublicHoliday = h.Holiday.IsPublicHoliday
                            }).ToList();

                            await _database.SaveHolidayOccurrencesAsync(entities);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error saving holidays to database: {ex.Message}");
                        }
                    });
                }

                return _cachedMonthHolidays;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching holidays online for {year}/{month}: {ex.Message}, falling back to offline data");
            }
        }

        // Fallback to offline data
        try
        {
            var cachedEntities = await _database.GetHolidaysForMonthAsync(year, month);
            _cachedMonthHolidays = cachedEntities.Select(e => new HolidayOccurrence
            {
                GregorianDate = e.GregorianDate,
                Holiday = new Holiday
                {
                    Name = e.Name,
                    Description = e.Description ?? string.Empty,
                    Type = Enum.TryParse<HolidayType>(e.Type, out var type) ? type : HolidayType.TraditionalFestival,
                    ColorHex = e.ColorHex ?? "#FF0000",
                    IsPublicHoliday = e.IsPublicHoliday
                }
            }).ToList();
            _cachedYear = year;
            _cachedMonth = month;

            return _cachedMonthHolidays;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching offline holidays for {year}/{month}: {ex.Message}");
            return new List<HolidayOccurrence>();
        }
    }

    public async Task<Holiday?> GetHolidayForDateAsync(DateTime date)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/v1/holiday/date/{date.Year}/{date.Month}/{date.Day}");

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
