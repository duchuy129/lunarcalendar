namespace LunarCalendar.Api.Models;

public class Holiday
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Lunar calendar date
    public int LunarMonth { get; set; }
    public int LunarDay { get; set; }
    public bool IsLeapMonth { get; set; }

    // If this holiday also has a fixed Gregorian date (like Vietnam National Day)
    public int? GregorianMonth { get; set; }
    public int? GregorianDay { get; set; }

    // Holiday type for color coding
    public HolidayType Type { get; set; }

    // Color hex code for display
    public string ColorHex { get; set; } = string.Empty;

    // Is this a public/national holiday?
    public bool IsPublicHoliday { get; set; }

    // Country/culture this holiday belongs to
    public string Culture { get; set; } = "Vietnamese";
}

public enum HolidayType
{
    MajorHoliday = 1,      // Red - Tết, National Day
    TraditionalFestival = 2, // Gold/Yellow - Mid-Autumn, Wandering Souls
    SeasonalCelebration = 3  // Green - Other traditional days
}
