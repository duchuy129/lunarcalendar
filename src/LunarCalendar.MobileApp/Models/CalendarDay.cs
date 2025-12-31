using LunarCalendar.Core.Models;
using LunarCalendar.MobileApp.Services;

namespace LunarCalendar.MobileApp.Models;

public class CalendarDay
{
    // PERFORMANCE FIX: Cache localized holiday name to avoid repeated resource lookups
    private string? _cachedHolidayName;
    private int? _lastHolidayId;
    
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

    // PERFORMANCE FIX: Cached localized holiday name with smart invalidation
    public string HolidayName
    {
        get
        {
            if (Holiday == null)
            {
                return string.Empty;
            }

            // Check if we need to recalculate (holiday changed or cache is empty)
            if (_cachedHolidayName == null || _lastHolidayId != Holiday.Id)
            {
                _cachedHolidayName = LocalizationHelper.GetLocalizedHolidayName(
                    Holiday.NameResourceKey,
                    Holiday.Name);
                _lastHolidayId = Holiday.Id;
            }

            return _cachedHolidayName;
        }
    }

    // Method to invalidate cache when language changes
    public void InvalidateLocalizedCache()
    {
        _cachedHolidayName = null;
        _lastHolidayId = null;
    }

    // PERFORMANCE FIX: Equals method for efficient comparison during collection updates
    public override bool Equals(object? obj)
    {
        if (obj is not CalendarDay other) return false;
        
        return Date.Date == other.Date.Date &&
               Day == other.Day &&
               IsCurrentMonth == other.IsCurrentMonth &&
               IsToday == other.IsToday &&
               HasEvents == other.HasEvents &&
               Holiday?.Id == other.Holiday?.Id &&
               LunarInfo?.LunarDay == other.LunarInfo?.LunarDay &&
               LunarInfo?.LunarMonth == other.LunarInfo?.LunarMonth;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Date.Date, Day, IsCurrentMonth, IsToday);
    }
}
