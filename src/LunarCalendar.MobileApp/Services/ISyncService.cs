namespace LunarCalendar.MobileApp.Services;

public interface ISyncService
{
    event EventHandler<SyncStatusChangedEventArgs>? SyncStatusChanged;

    bool IsSyncing { get; }
    DateTime? LastSyncTime { get; }
    string? LastSyncError { get; }

    Task<bool> SyncHolidaysForYearAsync(int year, CancellationToken cancellationToken = default);
    Task<bool> SyncLunarDatesForMonthAsync(int year, int month, CancellationToken cancellationToken = default);
    Task<bool> SyncAllAsync(int year, int month, CancellationToken cancellationToken = default);
}

public class SyncStatusChangedEventArgs : EventArgs
{
    public bool IsSyncing { get; set; }
    public string? Status { get; set; }
    public bool Success { get; set; }
    public string? Error { get; set; }
}
