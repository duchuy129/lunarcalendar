using LunarCalendar.Core.Models;
using LunarCalendar.MobileApp.Resources.Strings;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.Helpers;
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

    // Localized properties - override to use resource keys
    public string HolidayName =>
        LocalizationHelper.GetLocalizedHolidayName(
            Holiday.NameResourceKey,
            Holiday.Name);

    public string HolidayDescription =>
        LocalizationHelper.GetLocalizedHolidayDescription(
            Holiday.DescriptionResourceKey,
            Holiday.Description);

    public string ColorHex => string.IsNullOrWhiteSpace(HolidayOccurrence.ColorHex) 
        ? "#FF0000" 
        : HolidayOccurrence.ColorHex;
    
    // AOT-safe color binding - returns actual Color object, not string
    public Color BackgroundColor => Color.FromArgb(ColorHex);
    
    // AOT-safe brush binding for shadows
    public Brush BackgroundBrush => new SolidColorBrush(BackgroundColor);
    
    public bool IsPublicHoliday => HolidayOccurrence.IsPublicHoliday;
    
    // Culture-aware Gregorian date formatting
    public string GregorianDateFormatted => 
        DateFormatterHelper.FormatGregorianDateShort(HolidayOccurrence.GregorianDate);

    // Localized lunar date display
    public string LunarDateDisplay
    {
        get
        {
            if (!HasLunarDate) return string.Empty;

            var lunarText = DateFormatterHelper.FormatLunarDateWithLabel(
                Holiday.LunarDay, 
                Holiday.LunarMonth);

            // Add animal sign for all lunar holidays
            if (!string.IsNullOrEmpty(AnimalSign))
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
        OnPropertyChanged(nameof(HolidayName));
        OnPropertyChanged(nameof(HolidayDescription));
    }
}
