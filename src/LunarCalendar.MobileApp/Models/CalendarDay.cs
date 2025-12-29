using LunarCalendar.Core.Models;

namespace LunarCalendar.MobileApp.Models;

public class CalendarDay
{
    public DateTime Date { get; set; }
    public int Day { get; set; }
    public bool IsCurrentMonth { get; set; }
    public bool IsToday { get; set; }
    public bool HasEvents { get; set; }
    public LunarDate? LunarInfo { get; set; }
    public Holiday? Holiday { get; set; }

    // Lunar date in dd/mm format
    public string LunarDateDisplay => LunarInfo != null
        ? $"{LunarInfo.LunarDay}/{LunarInfo.LunarMonth}"
        : string.Empty;

    // Holiday indicator
    public bool HasHoliday => Holiday != null;

    // Holiday color for background
    public Color HolidayColor => Holiday != null
        ? Color.FromArgb(Holiday.ColorHex)
        : Colors.Transparent;
}
