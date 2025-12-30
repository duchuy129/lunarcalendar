using LunarCalendar.Core.Models;
using LunarCalendar.MobileApp.Services;

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

    // Lunar special day indicator (1st and 15th of lunar month)
    public bool IsLunarSpecialDay => LunarInfo != null && 
        (LunarInfo.LunarDay == 1 || LunarInfo.LunarDay == 15);

    // Priority: Show holiday color if exists, otherwise show lunar special day color
    public Color HolidayColor => Holiday != null
        ? Color.FromArgb(Holiday.ColorHex)
        : (IsLunarSpecialDay ? Color.FromArgb("#4169E1") : Colors.Transparent);

    // Localized holiday name
    public string HolidayName => Holiday != null
        ? LocalizationHelper.GetLocalizedHolidayName(
            Holiday.NameResourceKey,
            Holiday.Name)
        : string.Empty;
}
