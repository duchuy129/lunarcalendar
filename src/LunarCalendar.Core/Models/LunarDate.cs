namespace LunarCalendar.Core.Models;

public class LunarDate
{
    public DateTime GregorianDate { get; set; }
    public int LunarYear { get; set; }
    public int LunarMonth { get; set; }
    public int LunarDay { get; set; }
    public bool IsLeapMonth { get; set; }
    public string LunarYearName { get; set; } = string.Empty;
    public string LunarMonthName { get; set; } = string.Empty;
    public string LunarDayName { get; set; } = string.Empty;
    public string AnimalSign { get; set; } = string.Empty;
}
