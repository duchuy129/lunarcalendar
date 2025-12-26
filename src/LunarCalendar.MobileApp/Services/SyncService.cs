using LunarCalendar.MobileApp.Data;
using LunarCalendar.MobileApp.Data.Models;
using System.Diagnostics;

namespace LunarCalendar.MobileApp.Services;

public class SyncService : ISyncService
{
    private readonly IConnectivityService _connectivityService;
    private readonly ICalendarApiClient _calendarApiClient;
    private readonly IHolidayService _holidayService;
    private readonly LunarCalendarDatabase _database;
    private readonly SemaphoreSlim _syncLock = new(1, 1);

    private bool _isSyncing;
    private DateTime? _lastSyncTime;
    private string? _lastSyncError;

    public event EventHandler<SyncStatusChangedEventArgs>? SyncStatusChanged;

    public bool IsSyncing => _isSyncing;
    public DateTime? LastSyncTime => _lastSyncTime;
    public string? LastSyncError => _lastSyncError;

    public SyncService(
        IConnectivityService connectivityService,
        ICalendarApiClient calendarApiClient,
        IHolidayService holidayService,
        LunarCalendarDatabase database)
    {
        _connectivityService = connectivityService;
        _calendarApiClient = calendarApiClient;
        _holidayService = holidayService;
        _database = database;
    }

    public async Task<bool> SyncHolidaysForYearAsync(int year, CancellationToken cancellationToken = default)
    {
        if (!_connectivityService.IsConnected)
        {
            Debug.WriteLine("SyncService: No internet connection, skipping holiday sync");
            return false;
        }

        await _syncLock.WaitAsync(cancellationToken);
        try
        {
            SetSyncStatus(true, $"Syncing holidays for {year}...");

            var retryCount = 0;
            var maxRetries = 3;
            var baseDelay = TimeSpan.FromSeconds(1);

            while (retryCount < maxRetries)
            {
                try
                {
                    var holidays = await _holidayService.GetHolidaysForYearAsync(year);

                    if (holidays != null && holidays.Any())
                    {
                        var entities = holidays.Select(h => new HolidayOccurrenceEntity
                        {
                            HolidayId = 0, // We don't have the base holiday ID in the API response
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
                        await _database.AddSyncLogAsync("Holidays", "Success", $"Synced {entities.Count} holidays for year {year}");

                        _lastSyncTime = DateTime.Now;
                        _lastSyncError = null;

                        SetSyncStatus(false, $"Synced {entities.Count} holidays", true);
                        return true;
                    }

                    SetSyncStatus(false, "No holidays found", true);
                    return true;
                }
                catch (Exception ex) when (retryCount < maxRetries - 1)
                {
                    retryCount++;
                    var delay = baseDelay * Math.Pow(2, retryCount - 1);
                    Debug.WriteLine($"SyncService: Retry {retryCount}/{maxRetries} after {delay.TotalSeconds}s due to: {ex.Message}");
                    await Task.Delay(delay, cancellationToken);
                }
            }

            throw new Exception("Max retry attempts reached");
        }
        catch (Exception ex)
        {
            _lastSyncError = ex.Message;
            await _database.AddSyncLogAsync("Holidays", "Failed", ex.Message);
            SetSyncStatus(false, "Sync failed", false, ex.Message);
            Debug.WriteLine($"SyncService: Failed to sync holidays: {ex.Message}");
            return false;
        }
        finally
        {
            _syncLock.Release();
        }
    }

    public async Task<bool> SyncLunarDatesForMonthAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        if (!_connectivityService.IsConnected)
        {
            Debug.WriteLine("SyncService: No internet connection, skipping lunar date sync");
            return false;
        }

        await _syncLock.WaitAsync(cancellationToken);
        try
        {
            SetSyncStatus(true, $"Syncing lunar dates for {year}/{month}...");

            var retryCount = 0;
            var maxRetries = 3;
            var baseDelay = TimeSpan.FromSeconds(1);

            while (retryCount < maxRetries)
            {
                try
                {
                    var lunarDates = await _calendarApiClient.GetMonthInfoAsync(year, month);

                    if (lunarDates != null && lunarDates.Any())
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
                        await _database.AddSyncLogAsync("LunarDates", "Success", $"Synced {entities.Count} lunar dates for {year}/{month}");

                        _lastSyncTime = DateTime.Now;
                        _lastSyncError = null;

                        SetSyncStatus(false, $"Synced {entities.Count} lunar dates", true);
                        return true;
                    }

                    SetSyncStatus(false, "No lunar dates found", true);
                    return true;
                }
                catch (Exception ex) when (retryCount < maxRetries - 1)
                {
                    retryCount++;
                    var delay = baseDelay * Math.Pow(2, retryCount - 1);
                    Debug.WriteLine($"SyncService: Retry {retryCount}/{maxRetries} after {delay.TotalSeconds}s due to: {ex.Message}");
                    await Task.Delay(delay, cancellationToken);
                }
            }

            throw new Exception("Max retry attempts reached");
        }
        catch (Exception ex)
        {
            _lastSyncError = ex.Message;
            await _database.AddSyncLogAsync("LunarDates", "Failed", ex.Message);
            SetSyncStatus(false, "Sync failed", false, ex.Message);
            Debug.WriteLine($"SyncService: Failed to sync lunar dates: {ex.Message}");
            return false;
        }
        finally
        {
            _syncLock.Release();
        }
    }

    public async Task<bool> SyncAllAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        if (!_connectivityService.IsConnected)
        {
            Debug.WriteLine("SyncService: No internet connection, skipping sync");
            return false;
        }

        await _syncLock.WaitAsync(cancellationToken);
        try
        {
            SetSyncStatus(true, "Syncing all data...");

            var lunarSuccess = await SyncLunarDatesForMonthAsync(year, month, cancellationToken);
            var holidaySuccess = await SyncHolidaysForYearAsync(year, cancellationToken);

            var success = lunarSuccess && holidaySuccess;
            SetSyncStatus(false, success ? "Sync completed" : "Sync partially completed", success);

            return success;
        }
        finally
        {
            _syncLock.Release();
        }
    }

    private void SetSyncStatus(bool isSyncing, string? status = null, bool success = false, string? error = null)
    {
        _isSyncing = isSyncing;

        SyncStatusChanged?.Invoke(this, new SyncStatusChangedEventArgs
        {
            IsSyncing = isSyncing,
            Status = status,
            Success = success,
            Error = error
        });
    }
}
