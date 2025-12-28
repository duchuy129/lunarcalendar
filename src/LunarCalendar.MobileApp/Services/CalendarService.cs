using LunarCalendar.MobileApp.Models;
using LunarCalendar.MobileApp.Data;
using LunarCalendar.MobileApp.Data.Models;
using System.Diagnostics;

namespace LunarCalendar.MobileApp.Services;

public class CalendarService : ICalendarService
{
    private readonly ICalendarApiClient _apiClient;
    private readonly IConnectivityService _connectivityService;
    private readonly LunarCalendarDatabase _database;

    public CalendarService(
        ICalendarApiClient apiClient,
        IConnectivityService connectivityService,
        LunarCalendarDatabase database)
    {
        _apiClient = apiClient;
        _connectivityService = connectivityService;
        _database = database;
    }

    public async Task<LunarDate?> GetLunarDateAsync(DateTime date)
    {
        // Try online first if connected
        if (_connectivityService.IsConnected)
        {
            try
            {
                var lunarDate = await _apiClient.ConvertToLunarAsync(date);

                if (lunarDate != null)
                {
                    // Save to database in background
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            var entity = new LunarDateEntity
                            {
                                GregorianDate = date.Date,
                                LunarYear = lunarDate.LunarYear,
                                LunarMonth = lunarDate.LunarMonth,
                                LunarDay = lunarDate.LunarDay,
                                IsLeapMonth = lunarDate.IsLeapMonth,
                                LunarYearName = lunarDate.LunarYearName,
                                LunarMonthName = lunarDate.LunarMonthName,
                                LunarDayName = lunarDate.LunarDayName,
                                AnimalSign = lunarDate.AnimalSign
                            };
                            await _database.SaveLunarDateAsync(entity);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error saving lunar date to database: {ex.Message}");
                        }
                    });
                }

                return lunarDate;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching lunar date online: {ex.Message}, falling back to offline data");
            }
        }

        // Fallback to offline data
        try
        {
            var entity = await _database.GetLunarDateAsync(date);
            if (entity != null)
            {
                return new LunarDate
                {
                    GregorianDate = entity.GregorianDate,
                    LunarYear = entity.LunarYear,
                    LunarMonth = entity.LunarMonth,
                    LunarDay = entity.LunarDay,
                    IsLeapMonth = entity.IsLeapMonth,
                    LunarYearName = entity.LunarYearName ?? string.Empty,
                    LunarMonthName = entity.LunarMonthName ?? string.Empty,
                    LunarDayName = entity.LunarDayName ?? string.Empty,
                    AnimalSign = entity.AnimalSign ?? string.Empty
                };
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching offline lunar date: {ex.Message}");
        }

        return null;
    }

    public async Task<List<LunarDate>> GetMonthLunarDatesAsync(int year, int month)
    {
        // Try online first if connected
        if (_connectivityService.IsConnected)
        {
            try
            {
                Debug.WriteLine($"CalendarService: Calling API GetMonthInfoAsync({year}, {month})");
                var lunarDates = await _apiClient.GetMonthInfoAsync(year, month);
                Debug.WriteLine($"CalendarService: API returned {lunarDates?.Count ?? 0} lunar dates");

                if (lunarDates != null && lunarDates.Any())
                {
                    // Save to database in background
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            var entities = lunarDates.Select(ld => new LunarDateEntity
                            {
                                GregorianDate = ld.GregorianDate,
                                LunarYear = ld.LunarYear,
                                LunarMonth = ld.LunarMonth,
                                LunarDay = ld.LunarDay,
                                IsLeapMonth = ld.IsLeapMonth,
                                LunarYearName = ld.LunarYearName,
                                LunarMonthName = ld.LunarMonthName,
                                LunarDayName = ld.LunarDayName,
                                AnimalSign = ld.AnimalSign
                            }).ToList();

                            await _database.SaveLunarDatesAsync(entities);
                            Debug.WriteLine($"CalendarService: Saved {entities.Count} lunar dates to database");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error saving lunar dates to database: {ex.Message}");
                        }
                    });
                }

                return lunarDates;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CalendarService ERROR: {ex.Message}, falling back to offline data");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"CalendarService INNER: {ex.InnerException.Message}");
                }
            }
        }
        else
        {
            Debug.WriteLine($"CalendarService: No internet connection, using offline data");
        }

        // Fallback to offline data
        try
        {
            var entities = await _database.GetLunarDatesForMonthAsync(year, month);
            var lunarDates = entities.Select(e => new LunarDate
            {
                GregorianDate = e.GregorianDate,
                LunarYear = e.LunarYear,
                LunarMonth = e.LunarMonth,
                LunarDay = e.LunarDay,
                IsLeapMonth = e.IsLeapMonth,
                LunarYearName = e.LunarYearName ?? string.Empty,
                LunarMonthName = e.LunarMonthName ?? string.Empty,
                LunarDayName = e.LunarDayName ?? string.Empty,
                AnimalSign = e.AnimalSign ?? string.Empty
            }).ToList();

            Debug.WriteLine($"CalendarService: Retrieved {lunarDates.Count} lunar dates from offline database");
            return lunarDates;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching offline lunar dates: {ex.Message}");
            return new List<LunarDate>();
        }
    }
}
