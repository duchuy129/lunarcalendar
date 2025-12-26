using SQLite;

namespace LunarCalendar.MobileApp.Data.Models;

[Table("Holidays")]
public class HolidayEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int? LunarMonth { get; set; }
    public int? LunarDay { get; set; }
    public bool? IsLeapMonth { get; set; }

    public int? GregorianMonth { get; set; }
    public int? GregorianDay { get; set; }

    public string Type { get; set; } = string.Empty;
    public string? ColorHex { get; set; }
    public bool IsPublicHoliday { get; set; }
    public string Culture { get; set; } = string.Empty;

    public DateTime LastSyncedAt { get; set; }
}
