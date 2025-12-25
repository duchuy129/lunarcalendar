namespace LunarCalendar.Api.DTOs;

public class LunarCalendarInfoResponse
{
    public LunarDateResponse DateInfo { get; set; } = new();
    public string Festival { get; set; } = string.Empty;
    public string SolarTerm { get; set; } = string.Empty;
    public bool IsHoliday { get; set; }
}
