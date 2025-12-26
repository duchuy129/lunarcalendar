using CommunityToolkit.Mvvm.ComponentModel;
using LunarCalendar.MobileApp.Models;

namespace LunarCalendar.MobileApp.ViewModels;

[QueryProperty(nameof(Holiday), "Holiday")]
public partial class HolidayDetailViewModel : BaseViewModel
{
    [ObservableProperty]
    private HolidayOccurrence _holidayOccurrence = null!;

    [ObservableProperty]
    private string _gregorianDateFormatted = string.Empty;

    [ObservableProperty]
    private string _lunarDateFormatted = string.Empty;

    [ObservableProperty]
    private string _holidayTypeText = string.Empty;

    public HolidayOccurrence Holiday
    {
        set => Initialize(value);
    }

    public void Initialize(HolidayOccurrence holidayOccurrence)
    {
        HolidayOccurrence = holidayOccurrence;
        Title = holidayOccurrence.Holiday.Name;

        // Format Gregorian date
        GregorianDateFormatted = holidayOccurrence.GregorianDate.ToString("MMMM dd, yyyy (dddd)");

        // Format Lunar date if available
        if (holidayOccurrence.Holiday.HasLunarDate)
        {
            LunarDateFormatted = $"Lunar: {holidayOccurrence.Holiday.LunarDay}/{holidayOccurrence.Holiday.LunarMonth}";
            if (holidayOccurrence.Holiday.IsLeapMonth)
            {
                LunarDateFormatted += " (Leap Month)";
            }
        }
        else
        {
            LunarDateFormatted = "N/A";
        }

        // Format holiday type
        HolidayTypeText = holidayOccurrence.Holiday.Type switch
        {
            HolidayType.MajorHoliday => "Major Holiday",
            HolidayType.TraditionalFestival => "Traditional Festival",
            HolidayType.SeasonalCelebration => "Seasonal Celebration",
            _ => "Holiday"
        };
    }
}
