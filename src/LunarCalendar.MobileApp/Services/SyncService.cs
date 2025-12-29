using LunarCalendar.MobileApp.Data;
using System.Diagnostics;

namespace LunarCalendar.MobileApp.Services;

/// <summary>
/// Sync service for bundled architecture - no API sync needed
/// All calculations are local, so sync always succeeds instantly
/// </summary>
public class SyncService : ISyncService
{
    private readonly LunarCalendarDatabase _database;

    private bool _isSyncing;
    private DateTime? _lastSyncTime;
    private string? _lastSyncError;

    public event EventHandler<SyncStatusChangedEventArgs>? SyncStatusChanged;

    public bool IsSyncing => _isSyncing;
    public DateTime? LastSyncTime => _lastSyncTime;
    public string? LastSyncError => _lastSyncError;

    public SyncService(LunarCalendarDatabase database)
    {
        _database = database;
        // Mark as always synced since we calculate locally
        _lastSyncTime = DateTime.Now;
    }

    public async Task<bool> SyncHolidaysForYearAsync(int year, CancellationToken cancellationToken = default)
    {
        // Bundled architecture: No API sync needed - calculations are instant and local
        Debug.WriteLine($"SyncService: No sync needed for bundled architecture (local calculation)");
        _lastSyncTime = DateTime.Now;
        _lastSyncError = null;
        SetSyncStatus(false, "All data is local - no sync required", true);
        return await Task.FromResult(true);
    }

    public async Task<bool> SyncLunarDatesForMonthAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        // Bundled architecture: No API sync needed - calculations are instant and local
        Debug.WriteLine($"SyncService: No sync needed for bundled architecture (local calculation)");
        _lastSyncTime = DateTime.Now;
        _lastSyncError = null;
        SetSyncStatus(false, "All data is local - no sync required", true);
        return await Task.FromResult(true);
    }

    public async Task<bool> SyncAllAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        // Bundled architecture: No API sync needed - calculations are instant and local
        Debug.WriteLine($"SyncService: No sync needed for bundled architecture (local calculation)");
        _lastSyncTime = DateTime.Now;
        _lastSyncError = null;
        SetSyncStatus(false, "All data is local - no sync required", true);
        return await Task.FromResult(true);
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
