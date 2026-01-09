using SQLite;

namespace LunarCalendar.MobileApp.Data.Models;

[Table("LunarDates")]
public class LunarDateEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Unique]
    public DateTime GregorianDate { get; set; }

    public int LunarYear { get; set; }
    public int LunarMonth { get; set; }
    public int LunarDay { get; set; }
    public bool IsLeapMonth { get; set; }

    public string? LunarYearName { get; set; }
    public string? LunarMonthName { get; set; }
    public string? LunarDayName { get; set; }
    public string? AnimalSign { get; set; }

    public DateTime LastSyncedAt { get; set; }
}
