using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LunarCalendar.Core.Models;
using LunarCalendar.MobileApp.Models;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.Resources.Strings;

namespace LunarCalendar.MobileApp.ViewModels;

[QueryProperty(nameof(Holiday), "Holiday")]
public partial class HolidayDetailViewModel : BaseViewModel
{
    private readonly ICalendarService _calendarService;

    public HolidayDetailViewModel(ICalendarService calendarService)
    {
        _calendarService = calendarService;

        // Subscribe to language changes
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (r, m) =>
        {
            if (HolidayOccurrence != null)
            {
                UpdateLocalizedStrings();
            }
        });
    }
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
        set => _ = InitializeAsync(value); // Fire and forget for property setter
    }

    public async Task InitializeAsync(HolidayOccurrence holidayOccurrence)
    {
        HolidayOccurrence = holidayOccurrence;
        Title = LocalizationHelper.GetLocalizedHolidayName(
            holidayOccurrence.Holiday.NameResourceKey,
            holidayOccurrence.Holiday.Name);

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

        // Always get lunar date for the Gregorian date
        string lunarDateText;
        if (holidayOccurrence.Holiday.HasLunarDate)
        {
            // This is a lunar-based holiday, use the holiday's lunar date
            lunarDateText = $"{AppResources.LunarLabel} {holidayOccurrence.Holiday.LunarDay}/{holidayOccurrence.Holiday.LunarMonth}";
            if (holidayOccurrence.Holiday.IsLeapMonth)
            {
                lunarDateText += " (Leap Month)";
            }
        }
        else
        {
            // This is a Gregorian holiday, convert the Gregorian date to lunar
            try
            {
                var lunarDates = await _calendarService.GetMonthLunarDatesAsync(
                    holidayOccurrence.GregorianDate.Year,
                    holidayOccurrence.GregorianDate.Month);

                var lunarDate = lunarDates.FirstOrDefault(ld =>
                    ld.GregorianDate.Date == holidayOccurrence.GregorianDate.Date);

                if (lunarDate != null)
                {
                    lunarDateText = $"{AppResources.LunarLabel} {lunarDate.LunarDay}/{lunarDate.LunarMonth}";
                }
                else
                {
                    lunarDateText = "Lunar date unavailable";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting lunar date: {ex.Message}");
                lunarDateText = "Lunar date unavailable";
            }
        }

        LunarDateFormatted = lunarDateText;

        // Format holiday type using resources
        HolidayTypeText = holidayOccurrence.Holiday.Type switch
        {
            HolidayType.MajorHoliday => AppResources.MajorDay,
            HolidayType.TraditionalFestival => AppResources.TraditionalFestival,
            HolidayType.SeasonalCelebration => AppResources.SeasonalCelebration,
            HolidayType.LunarSpecialDay => AppResources.LunarSpecialDay,
            _ => string.Empty
        };

        // Set animal sign display for all lunar holidays - localized version
        if (!string.IsNullOrEmpty(holidayOccurrence.AnimalSign) &&
            holidayOccurrence.Holiday.HasLunarDate)
        {
            var localizedAnimalSign = LocalizationHelper.GetLocalizedAnimalSign(holidayOccurrence.AnimalSign);
            AnimalSignDisplay = $" - {AppResources.YearOfThe} {localizedAnimalSign}";
        }
        else
        {
            AnimalSignDisplay = string.Empty;
        }

        // Combine lunar date with animal sign for iOS compatibility (avoid FormattedString)
        LunarDateWithAnimalSign = LunarDateFormatted + AnimalSignDisplay;

        // Set visibility flags and description for iOS compatibility
        IsPublicHoliday = holidayOccurrence.Holiday.IsPublicHoliday;
        HolidayDescription = LocalizationHelper.GetLocalizedHolidayDescription(
            holidayOccurrence.Holiday.DescriptionResourceKey,
            holidayOccurrence.Holiday.Description ?? string.Empty);
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
    }

    private void UpdateLocalizedStrings()
    {
        // Update holiday title with current language
        Title = LocalizationHelper.GetLocalizedHolidayName(
            HolidayOccurrence.Holiday.NameResourceKey,
            HolidayOccurrence.Holiday.Name);

        // Update holiday description with current language
        HolidayDescription = LocalizationHelper.GetLocalizedHolidayDescription(
            HolidayOccurrence.Holiday.DescriptionResourceKey,
            HolidayOccurrence.Holiday.Description ?? string.Empty);
        HasDescription = !string.IsNullOrWhiteSpace(HolidayDescription);

        // Update holiday type text
        HolidayTypeText = HolidayOccurrence.Holiday.Type switch
        {
            HolidayType.MajorHoliday => AppResources.MajorDay,
            HolidayType.TraditionalFestival => AppResources.TraditionalFestival,
            HolidayType.SeasonalCelebration => AppResources.SeasonalCelebration,
            HolidayType.LunarSpecialDay => AppResources.LunarSpecialDay,
            _ => string.Empty
        };

        // Update dates with current culture
        GregorianDateFormatted = HolidayOccurrence.GregorianDate.ToString("MMMM dd, yyyy (dddd)");
        GregorianDateMonth = HolidayOccurrence.GregorianDate.ToString("MMM");

        // Update lunar date text and animal sign
        if (HolidayOccurrence.Holiday.HasLunarDate)
        {
            var lunarText = $"{AppResources.LunarLabel} {HolidayOccurrence.Holiday.LunarDay}/{HolidayOccurrence.Holiday.LunarMonth}";
            if (HolidayOccurrence.Holiday.IsLeapMonth)
            {
                lunarText += " (Leap Month)";
            }
            LunarDateFormatted = lunarText;

            // Update animal sign with localized version for all lunar holidays
            if (!string.IsNullOrEmpty(HolidayOccurrence.AnimalSign))
            {
                var localizedAnimalSign = LocalizationHelper.GetLocalizedAnimalSign(HolidayOccurrence.AnimalSign);
                AnimalSignDisplay = $" - {AppResources.YearOfThe} {localizedAnimalSign}";
            }
            else
            {
                AnimalSignDisplay = string.Empty;
            }

            LunarDateWithAnimalSign = lunarText + AnimalSignDisplay;
        }
    }
}
