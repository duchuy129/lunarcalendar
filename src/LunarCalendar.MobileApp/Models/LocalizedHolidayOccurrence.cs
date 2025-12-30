using LunarCalendar.Core.Models;
using LunarCalendar.MobileApp.Resources.Strings;
using LunarCalendar.MobileApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LunarCalendar.MobileApp.Models;

/// <summary>
/// Wrapper for HolidayOccurrence that provides localized display properties
/// </summary>
public partial class LocalizedHolidayOccurrence : ObservableObject
{
    public HolidayOccurrence HolidayOccurrence { get; }

    public LocalizedHolidayOccurrence(HolidayOccurrence holidayOccurrence)
    {
        HolidayOccurrence = holidayOccurrence;
    }

    // Pass-through properties from HolidayOccurrence
    public Holiday Holiday => HolidayOccurrence.Holiday;
    public DateTime GregorianDate => HolidayOccurrence.GregorianDate;
    public string AnimalSign => HolidayOccurrence.AnimalSign;
    public bool HasLunarDate => HolidayOccurrence.HasLunarDate;
    public string HolidayName => HolidayOccurrence.HolidayName;
    public string HolidayDescription => HolidayOccurrence.HolidayDescription;
    public string ColorHex => HolidayOccurrence.ColorHex;
    public bool IsPublicHoliday => HolidayOccurrence.IsPublicHoliday;
    public string GregorianDateFormatted => HolidayOccurrence.GregorianDateFormatted;

    // Localized lunar date display
    public string LunarDateDisplay
    {
        get
        {
            if (!HasLunarDate) return string.Empty;

            var lunarText = $"{AppResources.LunarLabel} {Holiday.LunarDay}/{Holiday.LunarMonth}";

            // Add animal sign for Tet holidays (1/1, 1/2, 1/3)
            if (!string.IsNullOrEmpty(AnimalSign) &&
                Holiday.LunarMonth == 1 &&
                Holiday.LunarDay >= 1 &&
                Holiday.LunarDay <= 3)
            {
                var localizedAnimalSign = LocalizationHelper.GetLocalizedAnimalSign(AnimalSign);
                lunarText += $" - {AppResources.YearOfThe} {localizedAnimalSign}";
            }

            return lunarText;
        }
    }

    // Method to refresh localized properties
    public void RefreshLocalizedProperties()
    {
        OnPropertyChanged(nameof(LunarDateDisplay));
        OnPropertyChanged(nameof(GregorianDateFormatted));
    }
}
