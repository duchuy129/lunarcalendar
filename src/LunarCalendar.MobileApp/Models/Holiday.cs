namespace LunarCalendar.MobileApp.Models;

public class Holiday
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int LunarMonth { get; set; }
    public int LunarDay { get; set; }
    public bool IsLeapMonth { get; set; }
    public int? GregorianMonth { get; set; }
    public int? GregorianDay { get; set; }
    public HolidayType Type { get; set; }
    public string ColorHex { get; set; } = string.Empty;
    public bool IsPublicHoliday { get; set; }
    public string Culture { get; set; } = "Vietnamese";
}

public class HolidayOccurrence
{
    public Holiday Holiday { get; set; } = null!;
    public DateTime GregorianDate { get; set; }
}

public enum HolidayType
{
    MajorHoliday = 1,
    TraditionalFestival = 2,
    SeasonalCelebration = 3
}
