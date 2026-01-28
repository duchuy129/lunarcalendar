using CommunityToolkit.Mvvm.ComponentModel;
using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;
using LunarCalendar.MobileApp.Helpers;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace LunarCalendar.MobileApp.ViewModels;

/// <summary>
/// ViewModel for DateDetailPage - shows comprehensive date information
/// including Gregorian, Lunar, Stem-Branch, and Holiday details
/// </summary>
public partial class DateDetailViewModel : BaseViewModel
{
    private readonly ISexagenaryService _sexagenaryService;
    private readonly ICalendarService _calendarService;
    private readonly IHolidayService _holidayService;
    private readonly ILogger<DateDetailViewModel> _logger;

    // Date Information
    [ObservableProperty]
    private DateTime _selectedDate;

    [ObservableProperty]
    private string _gregorianDateDisplay = string.Empty;

    [ObservableProperty]
    private string _dayOfWeekDisplay = string.Empty;

    // Lunar Date Information
    [ObservableProperty]
    private string _lunarDateDisplay = string.Empty;

    [ObservableProperty]
    private string _lunarMonthYearDisplay = string.Empty;

    [ObservableProperty]
    private bool _isLeapMonth;

    // Stem-Branch Information
    [ObservableProperty]
    private string _dayStemBranchDisplay = string.Empty;

    [ObservableProperty]
    private string _monthStemBranchDisplay = string.Empty;

    [ObservableProperty]
    private string _yearStemBranchDisplay = string.Empty;

    [ObservableProperty]
    private string _dayElementDisplay = string.Empty;

    [ObservableProperty]
    private Color _dayElementColor = Colors.Gray;

    // Holiday Information
    [ObservableProperty]
    private bool _hasHoliday;

    [ObservableProperty]
    private string _holidayName = string.Empty;

    [ObservableProperty]
    private string _holidayDescription = string.Empty;

    [ObservableProperty]
    private Color _holidayColor = Colors.Transparent;

    public DateDetailViewModel(
        ISexagenaryService sexagenaryService,
        ICalendarService calendarService,
        IHolidayService holidayService,
        ILogger<DateDetailViewModel> logger)
    {
        _sexagenaryService = sexagenaryService;
        _calendarService = calendarService;
        _holidayService = holidayService;
        _logger = logger;
    }

    /// <summary>
    /// Initialize the ViewModel with a selected date
    /// Loads all date information: Gregorian, Lunar, Stem-Branch, and Holidays
    /// </summary>
    public async Task InitializeAsync(DateTime selectedDate)
    {
        try
        {
            IsBusy = true;
            SelectedDate = selectedDate;

            // Load all information in parallel for better performance
            await Task.WhenAll(
                LoadGregorianDateInfoAsync(),
                LoadLunarDateInfoAsync(),
                LoadSexagenaryInfoAsync(),
                LoadHolidayInfoAsync()
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize DateDetailViewModel for date {Date}", selectedDate);
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Load Gregorian date information
    /// </summary>
    private Task LoadGregorianDateInfoAsync()
    {
        return Task.Run(() =>
        {
            // Format date based on current culture
            GregorianDateDisplay = SelectedDate.ToString("MMMM dd, yyyy", CultureInfo.CurrentUICulture);
            DayOfWeekDisplay = SelectedDate.ToString("dddd", CultureInfo.CurrentUICulture);
        });
    }

    /// <summary>
    /// Load Lunar date information
    /// </summary>
    private async Task LoadLunarDateInfoAsync()
    {
        try
        {
            var lunarDates = await _calendarService.GetMonthLunarDatesAsync(
                SelectedDate.Year, SelectedDate.Month);

            var lunarDate = lunarDates.FirstOrDefault(ld =>
                ld.GregorianDate.Date == SelectedDate.Date);

            if (lunarDate != null)
            {
                var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

                if (currentCulture == "vi")
                {
                    LunarDateDisplay = $"Ngày {lunarDate.LunarDay}";
                    LunarMonthYearDisplay = lunarDate.IsLeapMonth
                        ? $"Tháng {lunarDate.LunarMonth} nhuận, Năm {lunarDate.LunarYear}"
                        : $"Tháng {lunarDate.LunarMonth}, Năm {lunarDate.LunarYear}";
                }
                else
                {
                    LunarDateDisplay = $"Day {lunarDate.LunarDay}";
                    LunarMonthYearDisplay = lunarDate.IsLeapMonth
                        ? $"Leap Month {lunarDate.LunarMonth}, Year {lunarDate.LunarYear}"
                        : $"Month {lunarDate.LunarMonth}, Year {lunarDate.LunarYear}";
                }

                IsLeapMonth = lunarDate.IsLeapMonth;
            }
            else
            {
                LunarDateDisplay = "N/A";
                LunarMonthYearDisplay = string.Empty;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load lunar date info for {Date}", SelectedDate);
            LunarDateDisplay = "Error loading";
        }
    }

    /// <summary>
    /// Load Sexagenary (Stem-Branch) information
    /// </summary>
    private Task LoadSexagenaryInfoAsync()
    {
        return Task.Run(() =>
        {
            try
            {
                var sexagenaryInfo = _sexagenaryService.GetSexagenaryInfo(SelectedDate);

                // Format stem-branch strings using the helper
                DayStemBranchDisplay = SexagenaryFormatterHelper.FormatDayStemBranch(
                    sexagenaryInfo.DayStem, sexagenaryInfo.DayBranch);

                MonthStemBranchDisplay = FormatMonthStemBranch(
                    sexagenaryInfo.MonthStem, sexagenaryInfo.MonthBranch);

                YearStemBranchDisplay = SexagenaryFormatterHelper.FormatYearStemBranch(
                    sexagenaryInfo.YearStem, sexagenaryInfo.YearBranch);

                // Element information
                var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                var elementName = GetElementName(sexagenaryInfo.DayElement, currentCulture);
                DayElementDisplay = currentCulture == "vi"
                    ? $"Hành: {elementName}"
                    : $"Element: {elementName}";

                DayElementColor = GetElementColor(sexagenaryInfo.DayElement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load sexagenary info for {Date}", SelectedDate);
                DayStemBranchDisplay = "Error loading";
            }
        });
    }

    /// <summary>
    /// Load Holiday information
    /// </summary>
    private async Task LoadHolidayInfoAsync()
    {
        try
        {
            var holidays = await _holidayService.GetHolidaysForDateAsync(SelectedDate);

            if (holidays.Any())
            {
                HasHoliday = true;
                var holiday = holidays.First();
                HolidayName = holiday.Name;
                HolidayDescription = holiday.Description ?? string.Empty;
                HolidayColor = GetHolidayColor(holiday.Type);
            }
            else
            {
                HasHoliday = false;
                HolidayName = string.Empty;
                HolidayDescription = string.Empty;
                HolidayColor = Colors.Transparent;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load holiday info for {Date}", SelectedDate);
            HasHoliday = false;
        }
    }

    /// <summary>
    /// Format month stem-branch based on current language
    /// </summary>
    private string FormatMonthStemBranch(HeavenlyStem stem, EarthlyBranch branch)
    {
        var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        if (currentCulture == "vi")
        {
            var stemName = GetVietnameseStemName(stem);
            var branchName = GetVietnameseBranchName(branch);
            return $"Tháng {stemName} {branchName}";
        }
        else if (currentCulture == "zh")
        {
            return $"月{GetChineseStemName(stem)}{GetChineseBranchName(branch)}";
        }
        else
        {
            var animalName = GetAnimalNameFromBranch(branch);
            return $"Month {stem} {branch} ({animalName})";
        }
    }

    /// <summary>
    /// Map FiveElement to Color
    /// </summary>
    private Color GetElementColor(FiveElement element)
    {
        return element switch
        {
            FiveElement.Wood => Color.FromArgb("#4CAF50"),   // Green
            FiveElement.Fire => Color.FromArgb("#F44336"),   // Red
            FiveElement.Earth => Color.FromArgb("#FFC107"),  // Amber/Yellow
            FiveElement.Metal => Color.FromArgb("#E0E0E0"),  // Gray/Silver
            FiveElement.Water => Color.FromArgb("#2196F3"),  // Blue
            _ => Colors.Gray
        };
    }

    /// <summary>
    /// Get element name based on language
    /// </summary>
    private string GetElementName(FiveElement element, string language)
    {
        if (language == "vi")
        {
            return element switch
            {
                FiveElement.Wood => "Mộc",
                FiveElement.Fire => "Hỏa",
                FiveElement.Earth => "Thổ",
                FiveElement.Metal => "Kim",
                FiveElement.Water => "Thủy",
                _ => element.ToString()
            };
        }
        else if (language == "zh")
        {
            return element switch
            {
                FiveElement.Wood => "木",
                FiveElement.Fire => "火",
                FiveElement.Earth => "土",
                FiveElement.Metal => "金",
                FiveElement.Water => "水",
                _ => element.ToString()
            };
        }
        else
        {
            return element.ToString();
        }
    }

    /// <summary>
    /// Get holiday color based on type
    /// </summary>
    private Color GetHolidayColor(HolidayType type)
    {
        return type switch
        {
            HolidayType.LunarNewYear => Color.FromArgb("#D32F2F"),    // Red
            HolidayType.NationalHoliday => Color.FromArgb("#1976D2"), // Blue
            HolidayType.TraditionalFestival => Color.FromArgb("#F57C00"), // Orange
            HolidayType.Memorial => Color.FromArgb("#7B1FA2"),        // Purple
            HolidayType.SolarTerm => Color.FromArgb("#388E3C"),       // Green
            _ => Color.FromArgb("#757575")  // Gray
        };
    }

    // Helper methods for Vietnamese names
    private string GetVietnameseStemName(HeavenlyStem stem) => stem switch
    {
        HeavenlyStem.Jia => "Giáp",
        HeavenlyStem.Yi => "Ất",
        HeavenlyStem.Bing => "Bính",
        HeavenlyStem.Ding => "Đinh",
        HeavenlyStem.Wu => "Mậu",
        HeavenlyStem.Ji => "Kỷ",
        HeavenlyStem.Geng => "Canh",
        HeavenlyStem.Xin => "Tân",
        HeavenlyStem.Ren => "Nhâm",
        HeavenlyStem.Gui => "Quý",
        _ => stem.ToString()
    };

    private string GetVietnameseBranchName(EarthlyBranch branch) => branch switch
    {
        EarthlyBranch.Zi => "Tý",
        EarthlyBranch.Chou => "Sửu",
        EarthlyBranch.Yin => "Dần",
        EarthlyBranch.Mao => "Mão",
        EarthlyBranch.Chen => "Thìn",
        EarthlyBranch.Si => "Tỵ",
        EarthlyBranch.Wu => "Ngọ",
        EarthlyBranch.Wei => "Mùi",
        EarthlyBranch.Shen => "Thân",
        EarthlyBranch.You => "Dậu",
        EarthlyBranch.Xu => "Tuất",
        EarthlyBranch.Hai => "Hợi",
        _ => branch.ToString()
    };

    // Helper methods for Chinese characters
    private string GetChineseStemName(HeavenlyStem stem) => stem switch
    {
        HeavenlyStem.Jia => "甲",
        HeavenlyStem.Yi => "乙",
        HeavenlyStem.Bing => "丙",
        HeavenlyStem.Ding => "丁",
        HeavenlyStem.Wu => "戊",
        HeavenlyStem.Ji => "己",
        HeavenlyStem.Geng => "庚",
        HeavenlyStem.Xin => "辛",
        HeavenlyStem.Ren => "壬",
        HeavenlyStem.Gui => "癸",
        _ => stem.ToString()
    };

    private string GetChineseBranchName(EarthlyBranch branch) => branch switch
    {
        EarthlyBranch.Zi => "子",
        EarthlyBranch.Chou => "丑",
        EarthlyBranch.Yin => "寅",
        EarthlyBranch.Mao => "卯",
        EarthlyBranch.Chen => "辰",
        EarthlyBranch.Si => "巳",
        EarthlyBranch.Wu => "午",
        EarthlyBranch.Wei => "未",
        EarthlyBranch.Shen => "申",
        EarthlyBranch.You => "酉",
        EarthlyBranch.Xu => "戌",
        EarthlyBranch.Hai => "亥",
        _ => branch.ToString()
    };

    private string GetAnimalNameFromBranch(EarthlyBranch branch) => branch switch
    {
        EarthlyBranch.Zi => "Rat",
        EarthlyBranch.Chou => "Ox",
        EarthlyBranch.Yin => "Tiger",
        EarthlyBranch.Mao => "Rabbit",
        EarthlyBranch.Chen => "Dragon",
        EarthlyBranch.Si => "Snake",
        EarthlyBranch.Wu => "Horse",
        EarthlyBranch.Wei => "Goat",
        EarthlyBranch.Shen => "Monkey",
        EarthlyBranch.You => "Rooster",
        EarthlyBranch.Xu => "Dog",
        EarthlyBranch.Hai => "Pig",
        _ => "Unknown"
    };
}
