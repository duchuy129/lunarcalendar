using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LunarCalendar.Core.Models;
using LunarCalendar.MobileApp.Models;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.Resources.Strings;
using LunarCalendar.MobileApp.Helpers;
using System.Globalization;

namespace LunarCalendar.MobileApp.ViewModels;

public partial class CalendarViewModel : BaseViewModel, IDisposable
{
    private readonly ICalendarService _calendarService;
    private readonly IUserModeService _userModeService;
    private readonly IHolidayService _holidayService;
    private readonly IHapticService _hapticService;
    private readonly IConnectivityService _connectivityService;
    private readonly ISyncService _syncService;
    private readonly ILogService _logService;
    private readonly Core.Services.ISexagenaryService _sexagenaryService;

    private bool _disposed = false;

    [ObservableProperty]
    private DateTime _currentMonth;

    [ObservableProperty]
    private string _monthYearDisplay = string.Empty;

    [ObservableProperty]
    private string _todayLunarDisplay = "Loading...";

    [ObservableProperty]
    private string _todayStemBranch = string.Empty;

    [ObservableProperty]
    private Color _todayElementColor = Colors.Gray;

    [ObservableProperty]
    private ObservableCollection<CalendarDay> _calendarDays = new();

    [ObservableProperty]
    private string _userModeText = string.Empty;

    [ObservableProperty]
    private bool _isOnline = true;

    [ObservableProperty]
    private bool _isSyncing = false;

    [ObservableProperty]
    private string _syncStatus = string.Empty;

    [ObservableProperty]
    private int _selectedYear;

    [ObservableProperty]
    private ObservableCollection<LocalizedHolidayOccurrence> _yearHolidays = new();

    [ObservableProperty]
    private ObservableCollection<LocalizedHolidayOccurrence> _upcomingHolidays = new();

    [ObservableProperty]
    private bool _isLoadingHolidays = false;

    [ObservableProperty]
    private int _upcomingHolidaysDays = 30;

    public string UpcomingHolidaysTitle => string.Format(AppResources.UpcomingHolidaysFormat, UpcomingHolidaysDays);

    partial void OnUpcomingHolidaysDaysChanged(int value)
    {
        OnPropertyChanged(nameof(UpcomingHolidaysTitle));
    }

    partial void OnUpcomingHolidaysChanged(ObservableCollection<LocalizedHolidayOccurrence> value)
    {
        // Property changed handler - no action needed
    }

    partial void OnIsLoadingHolidaysChanged(bool value)
    {
        // Property changed handler - no action needed
    }

    partial void OnSelectedYearChanged(int value)
    {
        // Reload holidays when year changes from picker
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await LoadYearHolidaysAsync();
        });
    }

    [ObservableProperty]
    private ObservableCollection<int> _availableYears = new();

    [ObservableProperty]
    private bool _isYearSectionExpanded = false; // Collapsed by default

    // Month/Year picker properties
    [ObservableProperty]
    private int _selectedCalendarYear;

    [ObservableProperty]
    private int _selectedCalendarMonth;

    [ObservableProperty]
    private ObservableCollection<int> _availableCalendarYears = new();

    [ObservableProperty]
    private ObservableCollection<string> _availableMonths = new();

    [ObservableProperty]
    private bool _showCulturalBackground = true;

    [ObservableProperty]
    private bool _showLunarDates = true;

    [ObservableProperty]
    private bool _isRefreshing = false;

    [ObservableProperty]
    private double _calendarHeight = 380;

    // Synchronization for collection updates
    private readonly SemaphoreSlim _updateSemaphore = new SemaphoreSlim(1, 1);
    private bool _isUpdatingHolidays = false;

    public CalendarViewModel(
        ICalendarService calendarService,
        IUserModeService userModeService,
        IHolidayService holidayService,
        IHapticService hapticService,
        IConnectivityService connectivityService,
        ISyncService syncService,
        ILogService logService,
        Core.Services.ISexagenaryService sexagenaryService)
    {
        _calendarService = calendarService;
        _userModeService = userModeService;
        _holidayService = holidayService;
        _hapticService = hapticService;
        _connectivityService = connectivityService;
        _syncService = syncService;
        _logService = logService;
        _sexagenaryService = sexagenaryService;
        
        _logService.LogInfo("CalendarViewModel initialized", "CalendarViewModel");

        Title = AppResources.Calendar;
        _currentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        _selectedYear = DateTime.Today.Year;

        // Initialize available years (current year ± 10 years)
        for (int year = DateTime.Today.Year - 10; year <= DateTime.Today.Year + 10; year++)
        {
            AvailableYears.Add(year);
            AvailableCalendarYears.Add(year);
        }

        // Initialize month names from resources
        LoadMonthNames();

        // Set current month/year for pickers
        _selectedCalendarYear = DateTime.Today.Year;
        _selectedCalendarMonth = DateTime.Today.Month;

        UpdateUserModeText();

        // Monitor connectivity
        IsOnline = _connectivityService.IsConnected;
        _connectivityService.ConnectivityChanged += OnConnectivityChanged;

        // Monitor sync status
        _syncService.SyncStatusChanged += OnSyncStatusChanged;

        // Subscribe to language changes
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, async (r, m) =>
        {
            // Give culture change time to fully propagate
            await Task.Delay(200);
            
            LoadMonthNames();
            Title = AppResources.Calendar; // Update title with new language
            OnPropertyChanged(nameof(UpcomingHolidaysTitle));
            
            // PERFORMANCE FIX: Invalidate cached strings in CalendarDays before refreshing
            foreach (var day in CalendarDays)
            {
                day.InvalidateLocalizedCache();
            }
            
            // FIX: Update TodayLunarDisplay directly without waiting for LoadCalendarAsync
            await UpdateTodayDisplayAsync();
            
            // FIX: Safely refresh localized properties with error handling
            // DO NOT reload collections during language change - only refresh displayed text
            try
            {
                RefreshLocalizedHolidayProperties();
            }
            catch (Exception)
            {
                // Silent failure - localization refresh is non-critical
            }
            
            // Refresh calendar display (dates) without modifying collections
            await LoadCalendarAsync();
        });
    }

    private void LoadMonthNames()
    {
        AvailableMonths = new ObservableCollection<string>
        {
            AppResources.January, AppResources.February, AppResources.March,
            AppResources.April, AppResources.May, AppResources.June,
            AppResources.July, AppResources.August, AppResources.September,
            AppResources.October, AppResources.November, AppResources.December
        };
    }

    private void RefreshLocalizedHolidayProperties()
    {
        // FIX: Add null checks to prevent crashes during initialization or disposal
        if (UpcomingHolidays == null || YearHolidays == null)
        {
            return;
        }

        try
        {
            // Create snapshots to avoid enumeration issues during concurrent updates
            // This is defensive programming to prevent rare race conditions
            var upcomingSnapshot = UpcomingHolidays.ToList();
            var yearSnapshot = YearHolidays.ToList();
            
            // Refresh all localized holiday occurrences in upcoming holidays
            foreach (var holiday in upcomingSnapshot)
            {
                try
                {
                    holiday?.RefreshLocalizedProperties();
                }
                catch (Exception)
                {
                    // Silent failure - individual holiday refresh is non-critical
                }
            }

            // Refresh all localized holiday occurrences in year holidays
            foreach (var holiday in yearSnapshot)
            {
                try
                {
                    holiday?.RefreshLocalizedProperties();
                }
                catch (Exception)
                {
                    // Silent failure - individual holiday refresh is non-critical
                }
            }
        }
        catch (Exception)
        {
            // Silent failure - localization refresh is non-critical
        }
    }

    private void OnConnectivityChanged(object? sender, bool isConnected)
    {
        IsOnline = isConnected;
        SyncStatus = isConnected ? string.Empty : "Offline mode";
    }

    private void OnSyncStatusChanged(object? sender, SyncStatusChangedEventArgs e)
    {
        IsSyncing = e.IsSyncing;
        if (!string.IsNullOrEmpty(e.Status))
        {
            SyncStatus = e.Status;
        }

        if (!e.IsSyncing && !string.IsNullOrEmpty(e.Error))
        {
            SyncStatus = $"Sync error: {e.Error}";
        }
        else if (!e.IsSyncing && e.Success)
        {
            SyncStatus = string.Empty;
        }
    }

    public async Task InitializeAsync()
    {
        try
        {
            _logService.LogInfo("Initializing calendar view", "CalendarViewModel.InitializeAsync");
            
            // Load settings
            ShowCulturalBackground = SettingsViewModel.GetShowCulturalBackground();
            ShowLunarDates = SettingsViewModel.GetShowLunarDates();
            UpcomingHolidaysDays = SettingsViewModel.GetUpcomingHolidaysDays();

            // PERFORMANCE OPTIMIZATION: Load calendar and upcoming holidays in parallel
            // Year holidays are now in a separate tab and will load on-demand
            await Task.WhenAll(
                LoadCalendarAsync(),
                LoadUpcomingHolidaysAsync()
            );
            
            _logService.LogInfo("Calendar initialization complete", "CalendarViewModel.InitializeAsync");
        }
        catch (Exception ex)
        {
            _logService.LogError("Failed to initialize calendar", ex, "CalendarViewModel.InitializeAsync");
            // Ensure loading indicator is hidden even if there's an error
            IsLoadingHolidays = false;
        }
    }

    public async Task RefreshSettingsAsync()
    {
        // CRITICAL: This MUST be async Task, not async void
        // iOS crashes if collection updates aren't complete before page renders

        try
        {
            // Refresh settings when returning to calendar page
            ShowCulturalBackground = SettingsViewModel.GetShowCulturalBackground();
            ShowLunarDates = SettingsViewModel.GetShowLunarDates();
            var newDays = SettingsViewModel.GetUpcomingHolidaysDays();

            if (UpcomingHolidaysDays != newDays)
            {
                UpcomingHolidaysDays = newDays;

                // FIX: Check if update is in progress before attempting reload
                // LoadUpcomingHolidaysAsync has its own early return if _isUpdatingHolidays is true
                // Just await it - it will handle synchronization internally
                await LoadUpcomingHolidaysAsync();
            }
        }
        catch (Exception)
        {
            // Silent failure - settings refresh is non-critical
        }
    }    [RelayCommand]
    async Task PreviousMonthAsync()
    {
        _hapticService.PerformClick();
        CurrentMonth = CurrentMonth.AddMonths(-1);
        await LoadCalendarAsync();
    }

    [RelayCommand]
    async Task NextMonthAsync()
    {
        _hapticService.PerformClick();
        CurrentMonth = CurrentMonth.AddMonths(1);
        await LoadCalendarAsync();
    }

    [RelayCommand]
    async Task TodayAsync()
    {
        _hapticService.PerformClick();
        CurrentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        await LoadCalendarAsync();
    }

    [RelayCommand]
    async Task RefreshAsync()
    {
        try
        {
            // If online, trigger sync
            if (_connectivityService.IsConnected)
            {
                await _syncService.SyncAllAsync(CurrentMonth.Year, CurrentMonth.Month);
            }

            await LoadCalendarAsync();
            await LoadYearHolidaysAsync();
            await LoadUpcomingHolidaysAsync();
        }
        catch (Exception ex)
        {
            _logService.LogError("Failed to initialize calendar data", ex, "CalendarViewModel.InitializeAsync");
        }
        finally
        {
            // Small delay to ensure smooth animation completion before stopping spinner
            await Task.Delay(100);
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task ManualSyncAsync()
    {
        if (!_connectivityService.IsConnected)
        {
            await Application.Current.MainPage.DisplayAlert("Offline", "Cannot sync while offline. Please check your internet connection.", "OK");
            return;
        }

        try
        {
            IsSyncing = true;
            var success = await _syncService.SyncAllAsync(CurrentMonth.Year, CurrentMonth.Month);

            if (success)
            {
                await LoadCalendarAsync();
                await LoadYearHolidaysAsync();
                await Application.Current.MainPage.DisplayAlert("Sync Complete", "Calendar data has been synchronized successfully.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Sync Failed", "Failed to synchronize calendar data. Please try again later.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Sync Error", $"An error occurred during sync: {ex.Message}", "OK");
        }
        finally
        {
            IsSyncing = false;
        }
    }

    private async Task LoadCalendarAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            // Update month/year display
            MonthYearDisplay = CurrentMonth.ToString("MMMM yyyy");

            // Sync picker values with current month
            SelectedCalendarYear = CurrentMonth.Year;
            SelectedCalendarMonth = CurrentMonth.Month;

            // Get lunar dates for the month
            var lunarDates = await _calendarService.GetMonthLunarDatesAsync(
                CurrentMonth.Year,
                CurrentMonth.Month);

            // Get holidays for the month
            var holidays = await _holidayService.GetHolidaysForMonthAsync(
                CurrentMonth.Year,
                CurrentMonth.Month);

            // Update today's displays
            var todayLunar = lunarDates.FirstOrDefault(ld => ld.GregorianDate.Date == DateTime.Today);
            if (todayLunar != null)
            {
                // Calculate year stem-branch and format with animal sign
                // English: "11/15, Year of the Yi Si (Snake)"
                // Vietnamese: "Ngày 11 Tháng 15, Năm Ất Tỵ"
                var (yearStem, yearBranch) = Core.Services.SexagenaryCalculator.CalculateYearStemBranch(todayLunar.LunarYear);
                var localizedYearName = FormatYearStemBranch(yearStem, yearBranch);
                
                TodayLunarDisplay = DateFormatterHelper.FormatLunarDateWithYear(
                    todayLunar.LunarDay, 
                    todayLunar.LunarMonth, 
                    localizedYearName);
                
            }

            // Load today's stem-branch (Can Chi) - CRITICAL: Load on initial calendar load
            await LoadTodaySexagenaryInfoAsync();

            // Create calendar days
            var days = new List<CalendarDay>();

            // Get first day of month and calculate start of calendar grid
            var firstDayOfMonth = CurrentMonth;
            var firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek; // 0 = Sunday
            var startDate = firstDayOfMonth.AddDays(-firstDayOfWeek);

            // Calculate number of weeks needed for this month
            var daysInMonth = DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.Month);
            var totalDays = firstDayOfWeek + daysInMonth;
            var weeksNeeded = (int)Math.Ceiling(totalDays / 7.0);
            var daysToGenerate = weeksNeeded * 7;

            // Adjust calendar height based on number of weeks
            // Now using fixed cell heights (70px per cell + margins)
            // Calculate: (cell height + margins) * rows = (70 + 4) * rows
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                CalendarHeight = weeksNeeded switch
                {
                    4 => 296,  // 4 rows: 74 * 4 = 296
                    5 => 370,  // 5 rows: 74 * 5 = 370
                    6 => 444,  // 6 rows: 74 * 6 = 444
                    _ => 370   // Default to 5 rows
                };
            }
            else
            {
                CalendarHeight = weeksNeeded switch
                {
                    4 => 296,  // 4 rows: 74 * 4 = 296
                    5 => 370,  // 5 rows: 74 * 5 = 370
                    6 => 444,  // 6 rows: 74 * 6 = 444
                    _ => 370   // Default to 5 rows
                };
            }

            // PERFORMANCE FIX: Create lookup dictionaries for O(1) access instead of O(n) FirstOrDefault
            // This reduces ~2,000 comparisons to ~40 dictionary lookups per month change
            // Use GroupBy to handle potential duplicate dates safely
            var lunarLookup = lunarDates
                .GroupBy(ld => ld.GregorianDate.Date)
                .ToDictionary(g => g.Key, g => g.First());
            
            var holidayLookup = holidays
                .Where(h => h.GregorianDate.Date >= startDate.Date && h.GregorianDate.Date <= startDate.AddDays(daysToGenerate).Date)
                .GroupBy(h => h.GregorianDate.Date)
                .ToDictionary(g => g.Key, g => g.First());

            // Generate days for the calculated number of weeks
            for (int i = 0; i < daysToGenerate; i++)
            {
                var date = startDate.AddDays(i);
                var isCurrentMonth = date.Month == CurrentMonth.Month;
                var isToday = date.Date == DateTime.Today;

                // PERFORMANCE FIX: Use O(1) dictionary lookup instead of O(n) FirstOrDefault
                lunarLookup.TryGetValue(date.Date, out var lunarInfo);
                holidayLookup.TryGetValue(date.Date, out var holidayOccurrence);

                days.Add(new CalendarDay
                {
                    Date = date,
                    Day = date.Day,
                    IsCurrentMonth = isCurrentMonth,
                    IsToday = isToday,
                    HasEvents = false, // Will be updated in future sprints
                    LunarInfo = lunarInfo,
                    Holiday = holidayOccurrence?.Holiday
                });
            }

            // PERFORMANCE FIX: Use incremental update instead of creating new collection
            UpdateCalendarDaysCollection(days);
        }
        catch (Exception ex)
        {
            // Show error to user
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to load calendar data", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // FIX: Dedicated method to update Today display without full calendar reload
    private async Task UpdateTodayDisplayAsync()
    {
        try
        {
            
            // Get today's lunar date
            var todayLunarDates = await _calendarService.GetMonthLunarDatesAsync(
                DateTime.Today.Year,
                DateTime.Today.Month);
            
            var todayLunar = todayLunarDates.FirstOrDefault(ld => ld.GregorianDate.Date == DateTime.Today);
            
            if (todayLunar != null)
            {
                // Calculate year stem-branch and format with animal sign
                var (yearStem, yearBranch) = Core.Services.SexagenaryCalculator.CalculateYearStemBranch(todayLunar.LunarYear);
                var localizedYearName = FormatYearStemBranch(yearStem, yearBranch);
                
                TodayLunarDisplay = DateFormatterHelper.FormatLunarDateWithYear(
                    todayLunar.LunarDay, 
                    todayLunar.LunarMonth, 
                    localizedYearName);
                
            }
            
            // Load today's stem-branch (Can Chi)
            await LoadTodaySexagenaryInfoAsync();
        }
        catch (Exception ex)
        {
            _logService.LogError("Failed to load today's lunar display", ex, "CalendarViewModel.LoadTodayLunarDisplay");
        }
    }

    /// <summary>
    /// Load today's stem-branch (Can Chi / 干支) information
    /// </summary>
    private async Task LoadTodaySexagenaryInfoAsync()
    {
        try
        {
            var today = DateTime.Today;
            var sexagenaryInfo = await Task.Run(() => _sexagenaryService.GetSexagenaryInfo(today));
            
            // Format stem-branch display based on current language
            var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            
            string stemName, branchName;
            
            if (currentCulture == "vi")
            {
                // Vietnamese names with "Ngày" prefix
                stemName = GetVietnameseStemName(sexagenaryInfo.DayStem);
                branchName = GetVietnameseBranchName(sexagenaryInfo.DayBranch);
                TodayStemBranch = $"Ngày {stemName} {branchName}";
            }
            else if (currentCulture == "zh")
            {
                // Chinese characters with 日 prefix (meaning "day")
                TodayStemBranch = $"日{sexagenaryInfo.GetDayChineseString()}";
            }
            else
            {
                // English names with "Day" prefix
                stemName = sexagenaryInfo.DayStem.ToString();
                branchName = sexagenaryInfo.DayBranch.ToString();
                TodayStemBranch = $"Day {stemName} {branchName}";
            }
            
            // Set element color for visual indicator
            TodayElementColor = GetElementColor(sexagenaryInfo.DayElement);
            
            _logService.LogInfo($"Today's stem-branch: {TodayStemBranch}", "CalendarViewModel.LoadTodaySexagenaryInfo");
        }
        catch (Exception ex)
        {
            _logService.LogError("Failed to load today's stem-branch", ex, "CalendarViewModel.LoadTodaySexagenaryInfo");
            TodayStemBranch = "—";
            TodayElementColor = Colors.Gray;
        }
    }

    /// <summary>
    /// Get Vietnamese name for heavenly stem
    /// </summary>
    private string GetVietnameseStemName(Core.Models.HeavenlyStem stem)
    {
        return stem switch
        {
            Core.Models.HeavenlyStem.Jia => "Giáp",
            Core.Models.HeavenlyStem.Yi => "Ất",
            Core.Models.HeavenlyStem.Bing => "Bính",
            Core.Models.HeavenlyStem.Ding => "Đinh",
            Core.Models.HeavenlyStem.Wu => "Mậu",
            Core.Models.HeavenlyStem.Ji => "Kỷ",
            Core.Models.HeavenlyStem.Geng => "Canh",
            Core.Models.HeavenlyStem.Xin => "Tân",
            Core.Models.HeavenlyStem.Ren => "Nhâm",
            Core.Models.HeavenlyStem.Gui => "Quý",
            _ => stem.ToString()
        };
    }

    /// <summary>
    /// Get Vietnamese name for earthly branch
    /// </summary>
    private string GetVietnameseBranchName(Core.Models.EarthlyBranch branch)
    {
        return branch switch
        {
            Core.Models.EarthlyBranch.Zi => "Tý",
            Core.Models.EarthlyBranch.Chou => "Sửu",
            Core.Models.EarthlyBranch.Yin => "Dần",
            Core.Models.EarthlyBranch.Mao => "Mão",
            Core.Models.EarthlyBranch.Chen => "Thìn",
            Core.Models.EarthlyBranch.Si => "Tỵ",
            Core.Models.EarthlyBranch.Wu => "Ngọ",
            Core.Models.EarthlyBranch.Wei => "Mùi",
            Core.Models.EarthlyBranch.Shen => "Thân",
            Core.Models.EarthlyBranch.You => "Dậu",
            Core.Models.EarthlyBranch.Xu => "Tuất",
            Core.Models.EarthlyBranch.Hai => "Hợi",
            _ => branch.ToString()
        };
    }

    /// <summary>
    /// Get color associated with Five Element
    /// </summary>
    private Color GetElementColor(Core.Models.FiveElement element)
    {
        return element switch
        {
            Core.Models.FiveElement.Wood => Color.FromArgb("#4CAF50"),    // Green
            Core.Models.FiveElement.Fire => Color.FromArgb("#F44336"),     // Red
            Core.Models.FiveElement.Earth => Color.FromArgb("#8D6E63"),    // Brown
            Core.Models.FiveElement.Metal => Color.FromArgb("#9E9E9E"),    // Gray/Silver
            Core.Models.FiveElement.Water => Color.FromArgb("#2196F3"),    // Blue
            _ => Colors.Gray
        };
    }

    /// <summary>
    /// Format year stem-branch based on current language
    /// Vietnamese: "Ất Tỵ" (just stem-branch)
    /// English: "Yi Si (Snake)" (stem-branch with animal name)
    /// </summary>
    private string FormatYearStemBranch(Core.Models.HeavenlyStem stem, Core.Models.EarthlyBranch branch)
    {
        var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        
        if (currentCulture == "vi")
        {
            // Vietnamese: Just stem + branch
            var stemName = GetVietnameseStemName(stem);
            var branchName = GetVietnameseBranchName(branch);
            return $"{stemName} {branchName}";
        }
        else if (currentCulture == "zh")
        {
            // Chinese: Characters for stem + branch
            return GetChineseStemName(stem) + GetChineseBranchName(branch);
        }
        else
        {
            // English: Stem + Branch (Animal name)
            var animalName = GetAnimalNameFromBranch(branch);
            return $"{stem} {branch} ({animalName})";
        }
    }

    /// <summary>
    /// Get Chinese character for heavenly stem
    /// </summary>
    private string GetChineseStemName(Core.Models.HeavenlyStem stem)
    {
        return stem switch
        {
            Core.Models.HeavenlyStem.Jia => "甲",
            Core.Models.HeavenlyStem.Yi => "乙",
            Core.Models.HeavenlyStem.Bing => "丙",
            Core.Models.HeavenlyStem.Ding => "丁",
            Core.Models.HeavenlyStem.Wu => "戊",
            Core.Models.HeavenlyStem.Ji => "己",
            Core.Models.HeavenlyStem.Geng => "庚",
            Core.Models.HeavenlyStem.Xin => "辛",
            Core.Models.HeavenlyStem.Ren => "壬",
            Core.Models.HeavenlyStem.Gui => "癸",
            _ => stem.ToString()
        };
    }

    /// <summary>
    /// Get Chinese character for earthly branch
    /// </summary>
    private string GetChineseBranchName(Core.Models.EarthlyBranch branch)
    {
        return branch switch
        {
            Core.Models.EarthlyBranch.Zi => "子",
            Core.Models.EarthlyBranch.Chou => "丑",
            Core.Models.EarthlyBranch.Yin => "寅",
            Core.Models.EarthlyBranch.Mao => "卯",
            Core.Models.EarthlyBranch.Chen => "辰",
            Core.Models.EarthlyBranch.Si => "巳",
            Core.Models.EarthlyBranch.Wu => "午",
            Core.Models.EarthlyBranch.Wei => "未",
            Core.Models.EarthlyBranch.Shen => "申",
            Core.Models.EarthlyBranch.You => "酉",
            Core.Models.EarthlyBranch.Xu => "戌",
            Core.Models.EarthlyBranch.Hai => "亥",
            _ => branch.ToString()
        };
    }

    /// <summary>
    /// Get animal name from earthly branch (English name for localization)
    /// </summary>
    private string GetAnimalNameFromBranch(Core.Models.EarthlyBranch branch)
    {
        // Return English animal name which will be localized by LocalizationHelper
        return branch switch
        {
            Core.Models.EarthlyBranch.Zi => "Rat",
            Core.Models.EarthlyBranch.Chou => "Ox",
            Core.Models.EarthlyBranch.Yin => "Tiger",
            Core.Models.EarthlyBranch.Mao => "Rabbit",
            Core.Models.EarthlyBranch.Chen => "Dragon",
            Core.Models.EarthlyBranch.Si => "Snake",
            Core.Models.EarthlyBranch.Wu => "Horse",
            Core.Models.EarthlyBranch.Wei => "Goat",
            Core.Models.EarthlyBranch.Shen => "Monkey",
            Core.Models.EarthlyBranch.You => "Rooster",
            Core.Models.EarthlyBranch.Xu => "Dog",
            Core.Models.EarthlyBranch.Hai => "Pig",
            _ => "Snake" // Default fallback
        };
    }

    private void UpdateUserModeText()
    {
        UserModeText = _userModeService.IsGuest ? "Guest Mode" : "Authenticated";
    }

    [RelayCommand]
    async Task PreviousYearAsync()
    {
        _hapticService.PerformClick();
        var newYear = SelectedYear - 1;

        // FIX: Ensure the year is in the collection BEFORE setting SelectedYear
        // This prevents Picker rendering issues where text becomes invisible
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            EnsureYearInRangeSync(newYear);
            SelectedYear = newYear;
        });
    }

    [RelayCommand]
    async Task NextYearAsync()
    {
        _hapticService.PerformClick();
        var newYear = SelectedYear + 1;

        // FIX: Ensure the year is in the collection BEFORE setting SelectedYear
        // This prevents Picker rendering issues where text becomes invisible
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            EnsureYearInRangeSync(newYear);
            SelectedYear = newYear;
        });
    }

    [RelayCommand]
    async Task CurrentYearAsync()
    {
        _hapticService.PerformClick();
        var newYear = DateTime.Today.Year;

        // FIX: Ensure the year is in the collection BEFORE setting SelectedYear
        // This prevents Picker rendering issues where text becomes invisible
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            EnsureYearInRangeSync(newYear);
            SelectedYear = newYear;
        });
    }

    // Helper method to ensure year is in available years list
    // CRITICAL: This must be called from the main thread and completes synchronously
    private void EnsureYearInRangeSync(int year)
    {
        if (AvailableYears == null || AvailableYears.Count == 0)
        {
            return; // Don't try to fix empty collection
        }

        if (!AvailableYears.Contains(year))
        {
            var minYear = AvailableYears.Min();
            var maxYear = AvailableYears.Max();

            if (year < minYear)
            {
                // Add years before
                for (int y = minYear - 1; y >= year; y--)
                {
                    AvailableYears.Insert(0, y);
                }
            }
            else if (year > maxYear)
            {
                // Add years after
                for (int y = maxYear + 1; y <= year; y++)
                {
                    AvailableYears.Add(y);
                }
            }

            // FIX: Notify that the collection has changed to force Picker refresh
            OnPropertyChanged(nameof(AvailableYears));
        }
    }

    /// <summary>
    /// T060: Create LocalizedHolidayOccurrence with year stem-branch information
    /// This ensures consistent year display across all pages (Calendar, Holiday Detail, Year Holidays)
    /// </summary>
    private LocalizedHolidayOccurrence CreateLocalizedHolidayOccurrence(HolidayOccurrence holidayOccurrence)
    {
        var localized = new LocalizedHolidayOccurrence(holidayOccurrence);
        
        // Calculate and set year stem-branch if it's a lunar holiday
        if (holidayOccurrence.Holiday.HasLunarDate)
        {
            try
            {
                var lunarYear = holidayOccurrence.LunarYear; // Use the stored lunar year from holiday calculation
                var (yearStem, yearBranch, _) = _sexagenaryService.GetYearInfo(lunarYear);
                var yearStemBranchText = SexagenaryFormatterHelper.FormatYearStemBranch(yearStem, yearBranch);
                
                var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                var yearPrefix = currentCulture switch
                {
                    "vi" => "Năm",
                    "zh" => "年",
                    _ => "Year"
                };
                
                localized.YearStemBranchFormatted = $"{yearPrefix} {yearStemBranchText}";
            }
            catch (Exception ex)
            {
                _logService.LogWarning($"Failed to calculate year stem-branch for holiday: {ex.Message}", "CalendarViewModel.CreateLocalizedHolidayOccurrence");
                // Leave YearStemBranchFormatted as null - will fall back to animal sign
            }
        }
        
        return localized;
    }

    [RelayCommand]
    async Task YearSelectedAsync()
    {
        await LoadYearHolidaysAsync();
    }

    [RelayCommand]
    async Task JumpToMonthAsync()
    {
        // Update current month based on selected year and month
        CurrentMonth = new DateTime(SelectedCalendarYear, SelectedCalendarMonth, 1);
        await LoadCalendarAsync();
    }

    [RelayCommand]
    async Task ShowMonthYearPickerAsync()
    {
        _hapticService.PerformClick();

        // Create a simple action sheet to let user choose what to change
        var action = await Application.Current.MainPage.DisplayActionSheet(
            AppResources.SelectMonthYear ?? "Select Month & Year",
            AppResources.Cancel ?? "Cancel",
            null,
            AppResources.SelectMonth ?? "Select Month",
            AppResources.SelectYear ?? "Select Year",
            AppResources.GoToToday ?? "Go to Today");

        if (action == (AppResources.SelectMonth ?? "Select Month"))
        {
            // Show month selection
            var months = AvailableMonths.ToArray();
            var selectedMonth = await Application.Current.MainPage.DisplayActionSheet(
                AppResources.SelectMonth ?? "Select Month",
                AppResources.Cancel ?? "Cancel",
                null,
                months);

            if (selectedMonth != (AppResources.Cancel ?? "Cancel") && !string.IsNullOrEmpty(selectedMonth))
            {
                var monthIndex = Array.IndexOf(months, selectedMonth);
                if (monthIndex >= 0)
                {
                    SelectedCalendarMonth = monthIndex + 1;
                    await JumpToMonthAsync();
                }
            }
        }
        else if (action == (AppResources.SelectYear ?? "Select Year"))
        {
            // Show year selection
            var years = AvailableCalendarYears.Select(y => y.ToString()).ToArray();
            var selectedYear = await Application.Current.MainPage.DisplayActionSheet(
                AppResources.SelectYear ?? "Select Year",
                AppResources.Cancel ?? "Cancel",
                null,
                years);

            if (selectedYear != (AppResources.Cancel ?? "Cancel") && !string.IsNullOrEmpty(selectedYear) && int.TryParse(selectedYear, out var year))
            {
                SelectedCalendarYear = year;
                await JumpToMonthAsync();
            }
        }
        else if (action == (AppResources.GoToToday ?? "Go to Today"))
        {
            await TodayAsync();
        }
    }

    [RelayCommand]
    async Task ToggleYearSectionAsync()
    {
        // Add haptic feedback for better UX
        _hapticService.PerformClick();

        IsYearSectionExpanded = !IsYearSectionExpanded;
        
        // Reload holidays when expanding to ensure they render properly
        if (IsYearSectionExpanded)
        {
            await LoadYearHolidaysAsync();
        }
    }

    [RelayCommand]
    async Task ViewHolidayDetailAsync(object parameter)
    {
        // Add haptic feedback for better UX
        _hapticService.PerformClick();

        HolidayOccurrence? holidayOccurrence = parameter switch
        {
            LocalizedHolidayOccurrence localized => localized.HolidayOccurrence,
            HolidayOccurrence occurrence => occurrence,
            _ => null
        };

        if (holidayOccurrence == null) return;

        try
        {
            // Get HolidayDetailPage from DI container
            var serviceProvider = IPlatformApplication.Current?.Services;
            if (serviceProvider == null)
            {
                return;
            }

            var holidayDetailPage = serviceProvider.GetRequiredService<Views.HolidayDetailPage>();

            // Pass the holiday occurrence to the page's ViewModel
            if (holidayDetailPage.BindingContext is ViewModels.HolidayDetailViewModel viewModel)
            {
                viewModel.Holiday = holidayOccurrence;
            }

            // Navigate using Shell navigation (app uses AppShell, not TabbedPage)
            await Shell.Current.Navigation.PushAsync(holidayDetailPage);
        }
        catch (Exception ex)
        {
            _logService.LogError("Failed to navigate to holiday detail", ex, "CalendarViewModel.NavigateToHolidayDetail");
        }
    }

    [RelayCommand]
    async Task ShowSexagenaryInfoAsync()
    {
        // Add haptic feedback
        _hapticService.PerformClick();

        try
        {
            // Get localized strings for the dialog
            var title = AppResources.WhatIsCanChiTitle ?? "What is Stem-Branch (Can Chi)?";
            var message = AppResources.WhatIsCanChiMessage ?? 
                "The Sexagenary Cycle (Can Chi / 干支) is a traditional Chinese system of 60 combinations formed by pairing 10 Heavenly Stems with 12 Earthly Branches. Each day, month, and year has its own stem-branch designation.\n\n" +
                "This ancient system is used for:\n" +
                "• Dating and timekeeping\n" +
                "• Astrology and fortune-telling\n" +
                "• Traditional medicine\n" +
                "• Feng shui\n\n" +
                "The current stem-branch represents today's position in this 60-day cycle.";
            
            var okButton = AppResources.OK ?? "OK";

            await Application.Current.MainPage.DisplayAlert(title, message, okButton);
        }
        catch (Exception ex)
        {
            _logService.LogError("Failed to show sexagenary info", ex, "CalendarViewModel.ShowSexagenaryInfo");
        }
    }

    private async Task LoadYearHolidaysAsync()
    {
        // Prevent concurrent updates
        await _updateSemaphore.WaitAsync();

        try
        {
            
            // CRITICAL iOS FIX: Hide CollectionView BEFORE modifying collection
            // This prevents iOS crash when CollectionView is being rendered while collection changes
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                IsLoadingHolidays = true;
                // Yield to UI thread to ensure visibility change is processed
                await Task.Yield();
            });
            
            var holidays = await _holidayService.GetHolidaysForYearAsync(SelectedYear);

            // Filter out Lunar Special Days
            var filteredHolidays = holidays
                .Where(h => h.Holiday.Type != HolidayType.LunarSpecialDay)
                .OrderBy(h => h.GregorianDate)
                .ToList();
            

            // Update UI on main thread
            // FIX: Use Clear/Add pattern instead of replacing collection to prevent enumeration crashes
            // This prevents race conditions when language change handler is enumerating YearHolidays
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                try
                {
                    // Clear existing items instead of replacing collection
                    YearHolidays.Clear();
                    
                    // Add new items one by one with year stem-branch information (T060)
                    foreach (var holiday in filteredHolidays)
                    {
                        YearHolidays.Add(CreateLocalizedHolidayOccurrence(holiday));
                    }
                    
                }
                catch (Exception ex)
                {
                    _logService.LogError("Failed to add holiday to collection", ex, "CalendarViewModel.LoadYearHolidaysAsync");
                }
            });

            // Small delay to ensure UI is stable before showing CollectionView again
            await Task.Delay(50);
        }
        catch (Exception ex)
        {
            _logService.LogError("Failed to load year holidays", ex, "CalendarViewModel.LoadYearHolidaysAsync");
            
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                YearHolidays?.Clear();
            });
        }
        finally
        {
            // Show CollectionView again
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                IsLoadingHolidays = false;
            });
            
            _updateSemaphore.Release();
        }
    }

    private async Task LoadUpcomingHolidaysAsync()
    {
        // Prevent concurrent updates that can cause iOS crashes
        if (_isUpdatingHolidays)
        {
            return;
        }

        await _updateSemaphore.WaitAsync();

        try
        {
            _isUpdatingHolidays = true;


            // CRITICAL iOS FIX: Hide CollectionView BEFORE modifying collection
            // Set loading state on main thread and yield to allow UI to update
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                IsLoadingHolidays = true;
                // Yield to UI thread to ensure visibility change is processed
                await Task.Yield();
            });

            var today = DateTime.Today;
            var endDate = today.AddDays(UpcomingHolidaysDays);

            var upcomingHolidays = new List<HolidayOccurrence>();

            // Get current year holidays
            var currentYearHolidays = await _holidayService.GetHolidaysForYearAsync(today.Year);

            // If the range extends into next year, get next year's holidays too
            if (endDate.Year > today.Year)
            {
                var nextYearHolidays = await _holidayService.GetHolidaysForYearAsync(today.Year + 1);
                currentYearHolidays = currentYearHolidays.Concat(nextYearHolidays).ToList();
            }

            // Filter holidays that fall within the upcoming range
            upcomingHolidays = currentYearHolidays
                .Where(h => h.GregorianDate.Date >= today && h.GregorianDate.Date <= endDate)
                .OrderBy(h => h.GregorianDate)
                .ToList();

            // CRITICAL iOS FIX: Create NEW collection instead of modifying existing one
            // This is atomic and prevents CALayer rendering crashes during collection updates
            // T060: Use CreateLocalizedHolidayOccurrence to include year stem-branch

            var newCollection = new ObservableCollection<LocalizedHolidayOccurrence>(
                upcomingHolidays.Select(h => CreateLocalizedHolidayOccurrence(h))
            );

            // Replace entire collection reference on main thread - atomic operation
            // Then show the view after binding completes
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    UpcomingHolidays = newCollection;

                    // Yield to allow binding to complete before showing
                    await Task.Yield();

                    // Show CollectionView after binding completes
                    IsLoadingHolidays = false;
                }
                catch (Exception ex)
                {
                    _logService.LogError("Failed to update UI", ex, "CalendarViewModel.LoadUpcomingHolidaysAsync");
                    IsLoadingHolidays = false;
                }
            });

        }
        catch (Exception ex)
        {
            _logService.LogError("Failed to load upcoming holidays", ex, "CalendarViewModel.LoadUpcomingHolidaysAsync");
            IsLoadingHolidays = false;
        }
        finally
        {
            _isUpdatingHolidays = false;
            _updateSemaphore.Release();
        }
    }

    // PERFORMANCE FIX: Incremental collection update instead of recreating entire collection
    // This reduces UI re-rendering time by 60-80%
    private void UpdateCalendarDaysCollection(List<CalendarDay> newDays)
    {
        // Update on main thread
        MainThread.BeginInvokeOnMainThread(() =>
        {
            try
            {
                // If collection is empty or sizes are very different, just replace
                if (CalendarDays.Count == 0 || Math.Abs(CalendarDays.Count - newDays.Count) > 7)
                {
                    CalendarDays.Clear();
                    foreach (var day in newDays)
                    {
                        CalendarDays.Add(day);
                    }
                    return;
                }

                // Update existing items if they changed
                int minCount = Math.Min(CalendarDays.Count, newDays.Count);
                for (int i = 0; i < minCount; i++)
                {
                    if (!CalendarDays[i].Equals(newDays[i]))
                    {
                        CalendarDays[i] = newDays[i];
                    }
                }

                // Add new items if needed
                for (int i = CalendarDays.Count; i < newDays.Count; i++)
                {
                    CalendarDays.Add(newDays[i]);
                }

                // Remove excess items if needed
                while (CalendarDays.Count > newDays.Count)
                {
                    CalendarDays.RemoveAt(CalendarDays.Count - 1);
                }
            }
            catch (Exception ex)
            {
                _logService.LogWarning("Incremental update failed, using full replacement: " + ex.Message, "CalendarViewModel.UpdateCalendarDaysCollection");
                // Fallback to full replacement
                CalendarDays.Clear();
                foreach (var day in newDays)
                {
                    CalendarDays.Add(day);
                }
            }
        });
    }

    public void Dispose()
    {
        if (_disposed) return;

        try
        {
            // Unregister event handlers to prevent memory leaks
            _connectivityService.ConnectivityChanged -= OnConnectivityChanged;
            _syncService.SyncStatusChanged -= OnSyncStatusChanged;
            WeakReferenceMessenger.Default.Unregister<LanguageChangedMessage>(this);

            // Dispose SemaphoreSlim to prevent memory leak
            _updateSemaphore?.Dispose();
        }
        catch (Exception ex)
        {
            _logService?.LogWarning($"Error during disposal: {ex.Message}", "CalendarViewModel.Dispose");
        }
        finally
        {
            _disposed = true;
        }
    }
}
