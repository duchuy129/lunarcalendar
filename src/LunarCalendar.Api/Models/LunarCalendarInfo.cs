namespace LunarCalendar.Api.Models;

public class LunarCalendarInfo
{
    public LunarDateInfo DateInfo { get; set; } = new();
    public string Festival { get; set; } = string.Empty;
    public string SolarTerm { get; set; } = string.Empty;
    public bool IsHoliday { get; set; }
}
