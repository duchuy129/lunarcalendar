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
    private readonly Core.Services.ISexagenaryService _sexagenaryService;

    [ObservableProperty]
    private bool _showCulturalBackground = true;

    public HolidayDetailViewModel(
        ICalendarService calendarService, 
        ILogService logService,
        Core.Services.ISexagenaryService sexagenaryService)
    {
        _calendarService = calendarService;
        _logService = logService;
        _sexagenaryService = sexagenaryService;

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
    private string _dayStemBranchDisplay = string.Empty;

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
            // This is a lunar-based holiday, use actual lunar date if available (fixes Giao Thừa 12/29 vs 12/30)
            var lunarDay = holidayOccurrence.ActualLunarDay > 0 ? holidayOccurrence.ActualLunarDay : holidayOccurrence.Holiday.LunarDay;
            var lunarMonth = holidayOccurrence.ActualLunarMonth > 0 ? holidayOccurrence.ActualLunarMonth : holidayOccurrence.Holiday.LunarMonth;
            
            lunarDateText = DateFormatterHelper.FormatLunarDate(lunarDay, lunarMonth);
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
                    lunarDateText = DateFormatterHelper.FormatLunarDate(
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

        // T060: Display full stem-branch (e.g., "Năm Ất Tỵ") instead of just animal sign
        // Calculate year and day stem-branch from the Gregorian date
        if (holidayOccurrence.Holiday.HasLunarDate)
        {
            try
            {
                // CRITICAL: Get the correct lunar year by converting the Gregorian date
                // This handles cases where Gregorian January/February falls in the previous lunar year
                var lunarDates = await _calendarService.GetMonthLunarDatesAsync(
                    holidayOccurrence.GregorianDate.Year,
                    holidayOccurrence.GregorianDate.Month);

                var lunarDate = lunarDates.FirstOrDefault(ld =>
                    ld.GregorianDate.Date == holidayOccurrence.GregorianDate.Date);

                if (lunarDate != null)
                {
                    // Use the actual lunar year from the converted date
                    var lunarYear = lunarDate.LunarYear;
                    
                    // Get year stem-branch from sexagenary service
                    var (yearStem, yearBranch, _) = _sexagenaryService.GetYearInfo(lunarYear);
                    
                    // Format using the shared helper for consistency
                    var yearStemBranchText = SexagenaryFormatterHelper.FormatYearStemBranch(yearStem, yearBranch);
                    
                    // Vietnamese: "Năm Ất Tỵ", English: "Year Yi Si (Snake)", Chinese: "年乙巳"
                    var currentCulture = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                    var yearPrefix = currentCulture switch
                    {
                        "vi" => "Năm",
                        "zh" => "年",
                        _ => "Year"
                    };
                    
                    AnimalSignDisplay = $" - {yearPrefix} {yearStemBranchText}";
                    
                    // Calculate and set day stem-branch (only for lunar dates)
                    var sexagenaryInfo = _sexagenaryService.GetSexagenaryInfo(holidayOccurrence.GregorianDate);
                    DayStemBranchDisplay = SexagenaryFormatterHelper.FormatDayStemBranch(
                        sexagenaryInfo.DayStem, 
                        sexagenaryInfo.DayBranch);
                }
                else
                {
                    // Fallback if lunar date not found
                    AnimalSignDisplay = string.Empty;
                    DayStemBranchDisplay = string.Empty;
                }
            }
            catch (Exception ex)
            {
                _logService.LogWarning($"Failed to calculate stem-branch: {ex.Message}", "HolidayDetailViewModel.InitializeAsync");
                
                // Fallback to original animal sign display
                if (!string.IsNullOrEmpty(holidayOccurrence.AnimalSign))
                {
                    var localizedAnimalSign = LocalizationHelper.GetLocalizedAnimalSign(holidayOccurrence.AnimalSign);
                    AnimalSignDisplay = $" - {AppResources.YearOfThe} {localizedAnimalSign}";
                }
                else
                {
                    AnimalSignDisplay = string.Empty;
                }
                
                DayStemBranchDisplay = string.Empty;
            }
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

            // T060: Update year and day stem-branch display (use stored lunar year)
            try
            {
                var lunarYear = HolidayOccurrence.LunarYear; // Use the stored lunar year from holiday calculation
                var (yearStem, yearBranch, _) = _sexagenaryService.GetYearInfo(lunarYear);
                var yearStemBranchText = SexagenaryFormatterHelper.FormatYearStemBranch(yearStem, yearBranch);
                
                var currentCulture = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                var yearPrefix = currentCulture switch
                {
                    "vi" => "Năm",
                    "zh" => "年",
                    _ => "Year"
                };
                
                AnimalSignDisplay = $" - {yearPrefix} {yearStemBranchText}";
                
                // Update day stem-branch with new language
                var sexagenaryInfo = _sexagenaryService.GetSexagenaryInfo(HolidayOccurrence.GregorianDate);
                DayStemBranchDisplay = SexagenaryFormatterHelper.FormatDayStemBranch(
                    sexagenaryInfo.DayStem, 
                    sexagenaryInfo.DayBranch);
            }
            catch
            {
                // Fallback to animal sign only
                if (!string.IsNullOrEmpty(HolidayOccurrence.AnimalSign))
                {
                    var localizedAnimalSign = LocalizationHelper.GetLocalizedAnimalSign(HolidayOccurrence.AnimalSign);
                    AnimalSignDisplay = $" - {AppResources.YearOfThe} {localizedAnimalSign}";
                }
                else
                {
                    AnimalSignDisplay = string.Empty;
                }
            }

            LunarDateWithAnimalSign = lunarText + AnimalSignDisplay;
        }
    }
}
