using SQLite;

namespace LunarCalendar.MobileApp.Data.Models;

[Table("HolidayOccurrences")]
public class HolidayOccurrenceEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int HolidayId { get; set; }

    [Indexed]
    public DateTime GregorianDate { get; set; }

    [Indexed]
    public int Year { get; set; }

    [Indexed]
    public int Month { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? ColorHex { get; set; }
    public bool IsPublicHoliday { get; set; }

    public DateTime LastSyncedAt { get; set; }
}
