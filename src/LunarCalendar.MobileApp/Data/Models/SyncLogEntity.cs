using SQLite;

namespace LunarCalendar.MobileApp.Data.Models;

[Table("SyncLog")]
public class SyncLogEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string EntityType { get; set; } = string.Empty;
    public string SyncResult { get; set; } = string.Empty;
    public DateTime SyncedAt { get; set; }
    public string? ErrorMessage { get; set; }
}
