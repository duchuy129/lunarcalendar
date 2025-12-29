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

    [ObservableProperty]
    private string _animalSignDisplay = string.Empty;

    [ObservableProperty]
    private bool _isPublicHoliday;

    [ObservableProperty]
    private bool _hasDescription;

    [ObservableProperty]
    private string _holidayDescription = string.Empty;

    [ObservableProperty]
    private string _holidayColorHex = "#FF0000";

    [ObservableProperty]
    private string _holidayCulture = "Vietnamese";

    [ObservableProperty]
    private string _gregorianDateMonth = string.Empty;

    [ObservableProperty]
    private string _gregorianDateDay = string.Empty;

    [ObservableProperty]
    private string _gregorianDateYear = string.Empty;

    [ObservableProperty]
    private string _lunarDateWithAnimalSign = string.Empty;

    public HolidayOccurrence Holiday
    {
        set => Initialize(value);
    }

    public async void Initialize(HolidayOccurrence holidayOccurrence)
    {
        // Ensure UI updates happen on main thread for iOS
        await Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync(() =>
        {
            HolidayOccurrence = holidayOccurrence;
            Title = holidayOccurrence.Holiday.Name;

            // DEBUG: Log holiday data to verify what we're receiving
            System.Diagnostics.Debug.WriteLine($"========== HOLIDAY DETAIL DEBUG ==========");
            System.Diagnostics.Debug.WriteLine($"Holiday Name: {holidayOccurrence.Holiday.Name}");
            System.Diagnostics.Debug.WriteLine($"Description: {holidayOccurrence.Holiday.Description}");
            System.Diagnostics.Debug.WriteLine($"Description Length: {holidayOccurrence.Holiday.Description?.Length ?? 0}");
            System.Diagnostics.Debug.WriteLine($"IsPublicHoliday: {holidayOccurrence.Holiday.IsPublicHoliday}");
            System.Diagnostics.Debug.WriteLine($"Type: {holidayOccurrence.Holiday.Type}");
            System.Diagnostics.Debug.WriteLine($"ColorHex: {holidayOccurrence.Holiday.ColorHex}");
            System.Diagnostics.Debug.WriteLine($"==========================================");

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

            // Set animal sign display for Tet holidays (1/1, 1/2, 1/3)
            AnimalSignDisplay = holidayOccurrence.AnimalSignDisplay;

            // Combine lunar date with animal sign for iOS compatibility (avoid FormattedString)
            LunarDateWithAnimalSign = LunarDateFormatted + AnimalSignDisplay;

            // Set visibility flags and description for iOS compatibility
            IsPublicHoliday = holidayOccurrence.Holiday.IsPublicHoliday;
            HolidayDescription = holidayOccurrence.Holiday.Description ?? string.Empty;
            HasDescription = !string.IsNullOrWhiteSpace(HolidayDescription);

            // Set additional properties for iOS compatibility (avoid nested bindings)
            HolidayColorHex = holidayOccurrence.Holiday.ColorHex;
            HolidayCulture = holidayOccurrence.Holiday.Culture;
            GregorianDateMonth = holidayOccurrence.GregorianDate.ToString("MMM");
            GregorianDateDay = holidayOccurrence.GregorianDate.ToString("dd");
            GregorianDateYear = holidayOccurrence.GregorianDate.ToString("yyyy");

            // DEBUG: Log the calculated properties
            System.Diagnostics.Debug.WriteLine($"AnimalSignDisplay: '{AnimalSignDisplay}'");
            System.Diagnostics.Debug.WriteLine($"IsPublicHoliday: {IsPublicHoliday}");
            System.Diagnostics.Debug.WriteLine($"HasDescription: {HasDescription}");
            System.Diagnostics.Debug.WriteLine($"HolidayDescription: '{HolidayDescription}'");
            System.Diagnostics.Debug.WriteLine($"==========================================");
        });
    }
}
