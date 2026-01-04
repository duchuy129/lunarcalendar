using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LunarCalendar.Core.Models;
using LunarCalendar.MobileApp.Models;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.Resources.Strings;
using LunarCalendar.MobileApp.Helpers;

namespace LunarCalendar.MobileApp.ViewModels;

[QueryProperty(nameof(Holiday), "Holiday")]
public partial class HolidayDetailViewModel : BaseViewModel
{
    private readonly ICalendarService _calendarService;
    private readonly ILogService _logService;

    [ObservableProperty]
    private bool _showCulturalBackground = true;

    public HolidayDetailViewModel(ICalendarService calendarService, ILogService logService)
    {
        _calendarService = calendarService;
        _logService = logService;

        // Initialize settings
        ShowCulturalBackground = SettingsViewModel.GetShowCulturalBackground();

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

        // Format Gregorian date using culture-aware formatter
        GregorianDateFormatted = DateFormatterHelper.FormatGregorianDateLong(holidayOccurrence.GregorianDate);

        // Always get lunar date for the Gregorian date
        string lunarDateText;
        if (holidayOccurrence.Holiday.HasLunarDate)
        {
            // This is a lunar-based holiday, use the holiday's lunar date with formatter
            lunarDateText = DateFormatterHelper.FormatLunarDateWithLabel(
                holidayOccurrence.Holiday.LunarDay, 
                holidayOccurrence.Holiday.LunarMonth);
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
                    lunarDateText = DateFormatterHelper.FormatLunarDateWithLabel(
                        lunarDate.LunarDay, 
                        lunarDate.LunarMonth);
                }
                else
                {
                    lunarDateText = "Lunar date unavailable";
                }
            }
            catch (Exception ex)
            {
                _logService.LogWarning("Failed to convert holiday date to lunar format", "HolidayDetailViewModel.OnHolidayChanged");
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
        GregorianDateFormatted = DateFormatterHelper.FormatGregorianDateLong(HolidayOccurrence.GregorianDate);
        GregorianDateMonth = HolidayOccurrence.GregorianDate.ToString("MMM");

        // Update lunar date text and animal sign
        if (HolidayOccurrence.Holiday.HasLunarDate)
        {
            var lunarText = DateFormatterHelper.FormatLunarDateWithLabel(
                HolidayOccurrence.Holiday.LunarDay,
                HolidayOccurrence.Holiday.LunarMonth);
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
