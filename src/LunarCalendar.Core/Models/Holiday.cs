namespace LunarCalendar.Core.Models;

public class Holiday
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string NameResourceKey { get; set; } = string.Empty;
    public string DescriptionResourceKey { get; set; } = string.Empty;
    public int LunarMonth { get; set; }
    public int LunarDay { get; set; }
    public bool IsLeapMonth { get; set; }
    public int? GregorianMonth { get; set; }
    public int? GregorianDay { get; set; }
    public HolidayType Type { get; set; }
    public string ColorHex { get; set; } = "#FF0000";
    public bool IsPublicHoliday { get; set; }
    public string Culture { get; set; } = "Vietnamese";

    // Computed property to check if holiday has lunar date
    public bool HasLunarDate => LunarMonth > 0 && LunarDay > 0;
}

public class HolidayOccurrence
{
    public Holiday Holiday { get; set; } = null!;
    public DateTime GregorianDate { get; set; }
    public string AnimalSign { get; set; } = string.Empty;
    public int LunarYear { get; set; } // Added: Stores the correct lunar year for stem-branch calculation
    
    // Actual lunar date values for this occurrence (may differ from Holiday.LunarDay/LunarMonth)
    // Example: Giao Thừa is defined as 12/30, but in some years month 12 only has 29 days
    public int ActualLunarMonth { get; set; }
    public int ActualLunarDay { get; set; }

    // Computed properties for iOS compatibility - avoids nested bindings
    public bool HasLunarDate => Holiday?.HasLunarDate ?? false;
    public string HolidayName => Holiday?.Name ?? string.Empty;
    public string HolidayDescription => Holiday?.Description ?? string.Empty;
    public string ColorHex => !string.IsNullOrWhiteSpace(Holiday?.ColorHex) 
        ? Holiday.ColorHex 
        : "#FF0000";
    public bool IsPublicHoliday => Holiday?.IsPublicHoliday ?? false;
    public string GregorianDateFormatted => GregorianDate.ToString("MMMM dd, yyyy");

    // Lunar date display with animal sign for Tet holidays
    public string LunarDateDisplay
    {
        get
        {
            if (!HasLunarDate) return string.Empty;

            var lunarText = $"Lunar: {Holiday.LunarDay}/{Holiday.LunarMonth}";

            // Add animal sign for Tet holidays (1/1, 1/2, 1/3)
            if (!string.IsNullOrEmpty(AnimalSign) &&
                Holiday.LunarMonth == 1 &&
                Holiday.LunarDay >= 1 &&
                Holiday.LunarDay <= 3)
            {
                lunarText += $" - Year of the {AnimalSign}";
            }

            return lunarText;
        }
    }

    // Show animal sign only for Tet holidays (1/1, 1/2, 1/3)
    public string AnimalSignDisplay
    {
        get
        {
            if (!string.IsNullOrEmpty(AnimalSign) &&
                Holiday.HasLunarDate &&
                Holiday.LunarMonth == 1 &&
                Holiday.LunarDay >= 1 &&
                Holiday.LunarDay <= 3)
            {
                return $" - Year of the {AnimalSign}";
            }
            return string.Empty;
        }
    }
}

public enum HolidayType
{
    MajorHoliday = 1,
    TraditionalFestival = 2,
    SeasonalCelebration = 3,
    LunarSpecialDay = 4  // For 1st and 15th of lunar months (Mùng 1 and Rằm)
}
