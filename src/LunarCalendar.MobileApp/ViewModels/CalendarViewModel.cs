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

public partial class CalendarViewModel : BaseViewModel
{
    private readonly ICalendarService _calendarService;
    private readonly IUserModeService _userModeService;
    private readonly IHolidayService _holidayService;
    private readonly IHapticService _hapticService;
    private readonly IConnectivityService _connectivityService;
    private readonly ISyncService _syncService;

    [ObservableProperty]
    private DateTime _currentMonth;

    [ObservableProperty]
    private string _monthYearDisplay = string.Empty;

    [ObservableProperty]
    private string _todayLunarDisplay = "Loading...";

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
        System.Diagnostics.Debug.WriteLine($"!!! UpcomingHolidays PROPERTY CHANGED - New collection has {value?.Count ?? 0} items !!!");
        System.Diagnostics.Debug.WriteLine($"!!! IsLoadingHolidays: {IsLoadingHolidays} !!!");
    }

    partial void OnIsLoadingHolidaysChanged(bool value)
    {
        System.Diagnostics.Debug.WriteLine($"!!! IsLoadingHolidays CHANGED to: {value} !!!");
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
        ISyncService syncService)
    {
        _calendarService = calendarService;
        _userModeService = userModeService;
        _holidayService = holidayService;
        _hapticService = hapticService;
        _connectivityService = connectivityService;
        _syncService = syncService;

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
            System.Diagnostics.Debug.WriteLine($"=== LanguageChangedMessage received in CalendarViewModel ===");
            System.Diagnostics.Debug.WriteLine($"=== Current Culture: {CultureInfo.CurrentUICulture.Name} ===");
            
            // Give culture change time to fully propagate
            await Task.Delay(100);
            
            System.Diagnostics.Debug.WriteLine($"=== After delay, Culture: {CultureInfo.CurrentUICulture.Name} ===");
            
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
            
            RefreshLocalizedHolidayProperties();
            await LoadCalendarAsync(); // Refresh today section and all date displays
            await LoadUpcomingHolidaysAsync(); // Refresh upcoming holidays to update localized strings
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
        // Create snapshots to avoid enumeration issues during concurrent updates
        // This is defensive programming to prevent rare race conditions
        var upcomingSnapshot = UpcomingHolidays.ToList();
        var yearSnapshot = YearHolidays.ToList();
        
        // Refresh all localized holiday occurrences in upcoming holidays
        foreach (var holiday in upcomingSnapshot)
        {
            holiday.RefreshLocalizedProperties();
        }

        // Refresh all localized holiday occurrences in year holidays
        foreach (var holiday in yearSnapshot)
        {
            holiday.RefreshLocalizedProperties();
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
            System.Diagnostics.Debug.WriteLine("=== CalendarViewModel.InitializeAsync START ===");

            // Load settings
            ShowCulturalBackground = SettingsViewModel.GetShowCulturalBackground();
            ShowLunarDates = SettingsViewModel.GetShowLunarDates();
            UpcomingHolidaysDays = SettingsViewModel.GetUpcomingHolidaysDays();

            System.Diagnostics.Debug.WriteLine("=== Loading calendar ===");
            await LoadCalendarAsync();

            System.Diagnostics.Debug.WriteLine("=== Loading year holidays ===");
            await LoadYearHolidaysAsync();

            System.Diagnostics.Debug.WriteLine("=== Loading upcoming holidays ===");
            await LoadUpcomingHolidaysAsync();
            System.Diagnostics.Debug.WriteLine($"=== After LoadUpcomingHolidaysAsync: UpcomingHolidays.Count = {UpcomingHolidays.Count}, IsLoadingHolidays = {IsLoadingHolidays} ===");

            System.Diagnostics.Debug.WriteLine("=== CalendarViewModel.InitializeAsync COMPLETE ===");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"=== INIT ERROR: {ex.Message} ===");
            System.Diagnostics.Debug.WriteLine($"=== STACK: {ex.StackTrace} ===");
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
            System.Diagnostics.Debug.WriteLine("=== RefreshSettingsAsync START ===");

            // Refresh settings when returning to calendar page
            ShowCulturalBackground = SettingsViewModel.GetShowCulturalBackground();
            ShowLunarDates = SettingsViewModel.GetShowLunarDates();
            var newDays = SettingsViewModel.GetUpcomingHolidaysDays();

            System.Diagnostics.Debug.WriteLine($"=== Current UpcomingHolidaysDays: {UpcomingHolidaysDays}, New: {newDays} ===");

            if (UpcomingHolidaysDays != newDays)
            {
                System.Diagnostics.Debug.WriteLine("=== Days changed, updating UpcomingHolidaysDays and reloading holidays ===");
                UpcomingHolidaysDays = newDays;

                // Wait for any ongoing holiday loading to complete first
                if (_isUpdatingHolidays)
                {
                    System.Diagnostics.Debug.WriteLine("=== Waiting for ongoing holiday update to complete ===");
                    // Wait up to 5 seconds for the semaphore
                    if (await _updateSemaphore.WaitAsync(5000))
                    {
                        _updateSemaphore.Release();
                        System.Diagnostics.Debug.WriteLine("=== Previous update completed ===");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("=== WARNING: Timeout waiting for previous update ===");
                        // Don't reload if previous update is stuck
                        return;
                    }
                }

                // MUST await to ensure collection update completes before page renders
                await LoadUpcomingHolidaysAsync();
            }

            System.Diagnostics.Debug.WriteLine("=== RefreshSettingsAsync COMPLETE ===");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"=== ERROR in RefreshSettingsAsync: {ex.Message} ===");
            System.Diagnostics.Debug.WriteLine($"=== Stack: {ex.StackTrace} ===");
        }
    }

    [RelayCommand]
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
            System.Diagnostics.Debug.WriteLine($"Error during refresh: {ex.Message}");
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
                // Show lunar date with animal sign
                // English: "11/15, Year of the Snake"
                // Vietnamese: "Ngày 11 Tháng 15, Năm Tỵ"
                var localizedAnimalSign = LocalizationHelper.GetLocalizedAnimalSign(todayLunar.AnimalSign);
                
                System.Diagnostics.Debug.WriteLine($"=== BEFORE FORMAT: Culture={CultureInfo.CurrentUICulture.Name}, TwoLetter={CultureInfo.CurrentUICulture.TwoLetterISOLanguageName} ===");
                
                TodayLunarDisplay = DateFormatterHelper.FormatLunarDateWithYear(
                    todayLunar.LunarDay, 
                    todayLunar.LunarMonth, 
                    localizedAnimalSign);
                
                System.Diagnostics.Debug.WriteLine($"=== Today Display Updated: Culture={CultureInfo.CurrentUICulture.Name}, Display={TodayLunarDisplay} ===");
            }

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
            // iOS needs less height due to tighter spacing
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                CalendarHeight = weeksNeeded == 5 ? 300 : 340;
            }
            else
            {
                CalendarHeight = weeksNeeded == 5 ? 320 : 360;
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
            System.Diagnostics.Debug.WriteLine($"Error loading calendar: {ex.Message}");
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
            System.Diagnostics.Debug.WriteLine($"=== UpdateTodayDisplayAsync called ===");
            
            // Get today's lunar date
            var todayLunarDates = await _calendarService.GetMonthLunarDatesAsync(
                DateTime.Today.Year,
                DateTime.Today.Month);
            
            var todayLunar = todayLunarDates.FirstOrDefault(ld => ld.GregorianDate.Date == DateTime.Today);
            
            if (todayLunar != null)
            {
                var localizedAnimalSign = LocalizationHelper.GetLocalizedAnimalSign(todayLunar.AnimalSign);
                
                System.Diagnostics.Debug.WriteLine($"=== Before format in UpdateToday: Culture={CultureInfo.CurrentUICulture.Name} ===");
                
                TodayLunarDisplay = DateFormatterHelper.FormatLunarDateWithYear(
                    todayLunar.LunarDay, 
                    todayLunar.LunarMonth, 
                    localizedAnimalSign);
                
                System.Diagnostics.Debug.WriteLine($"=== Today Display updated directly: {TodayLunarDisplay} ===");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"=== Error updating today display: {ex.Message} ===");
        }
    }

    private void UpdateUserModeText()
    {
        UserModeText = _userModeService.IsGuest ? "Guest Mode" : "Authenticated";
    }

    [RelayCommand]
    async Task PreviousYearAsync()
    {
        SelectedYear--;
        await LoadYearHolidaysAsync();
    }

    [RelayCommand]
    async Task NextYearAsync()
    {
        SelectedYear++;
        await LoadYearHolidaysAsync();
    }

    [RelayCommand]
    async Task CurrentYearAsync()
    {
        SelectedYear = DateTime.Today.Year;
        await LoadYearHolidaysAsync();
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
        System.Diagnostics.Debug.WriteLine($"!!! ToggleYearSection called - Current state: {IsYearSectionExpanded} !!!");
        IsYearSectionExpanded = !IsYearSectionExpanded;
        System.Diagnostics.Debug.WriteLine($"!!! New state: {IsYearSectionExpanded} !!!");
        
        // Reload holidays when expanding to ensure they render properly
        if (IsYearSectionExpanded)
        {
            System.Diagnostics.Debug.WriteLine($"!!! Section expanded, reloading holidays for year {SelectedYear} !!!");
            System.Diagnostics.Debug.WriteLine($"!!! Current YearHolidays count BEFORE reload: {YearHolidays?.Count ?? 0} !!!");
            await LoadYearHolidaysAsync();
            System.Diagnostics.Debug.WriteLine($"!!! Current YearHolidays count AFTER reload: {YearHolidays?.Count ?? 0} !!!");
        }
    }

    [RelayCommand]
    async Task ViewHolidayDetailAsync(object parameter)
    {
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
                System.Diagnostics.Debug.WriteLine("=== Service provider not available ===");
                return;
            }

            var holidayDetailPage = serviceProvider.GetRequiredService<Views.HolidayDetailPage>();

            // Pass the holiday occurrence to the page's ViewModel
            if (holidayDetailPage.BindingContext is ViewModels.HolidayDetailViewModel viewModel)
            {
                viewModel.Holiday = holidayOccurrence;
            }

            // Navigate using current page's navigation
            // The calendar tab is wrapped in NavigationPage, so get the NavigationPage
            if (Application.Current?.MainPage is TabbedPage tabbedPage)
            {
                var currentPage = tabbedPage.CurrentPage;

                // If current page is NavigationPage (calendar tab), use its navigation
                if (currentPage is NavigationPage navPage)
                {
                    await navPage.PushAsync(holidayDetailPage);
                }
                // Otherwise use the page's own navigation (fallback)
                else if (currentPage != null)
                {
                    await currentPage.Navigation.PushAsync(holidayDetailPage);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"=== Error navigating to holiday detail: {ex.Message} ===");
            System.Diagnostics.Debug.WriteLine($"=== Stack: {ex.StackTrace} ===");
        }
    }

    private async Task LoadYearHolidaysAsync()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine($"=== LoadYearHolidaysAsync START for year {SelectedYear} ===");
            Console.WriteLine($"!!! LoadYearHolidaysAsync START for year {SelectedYear} !!!");
            
            var holidays = await _holidayService.GetHolidaysForYearAsync(SelectedYear);
            System.Diagnostics.Debug.WriteLine($"=== Got {holidays.Count} holidays from service ===");
            Console.WriteLine($"!!! Got {holidays.Count} holidays from service !!!");

            // Filter out Lunar Special Days (Mùng 1 and Rằm) - keep them only in Upcoming Holidays
            var filteredHolidays = holidays.Where(h => h.Holiday.Type != HolidayType.LunarSpecialDay).ToList();
            System.Diagnostics.Debug.WriteLine($"=== After filtering: {filteredHolidays.Count} holidays (removed Lunar Special Days) ===");
            Console.WriteLine($"!!! After filtering: {filteredHolidays.Count} holidays (removed Lunar Special Days) !!!");

            // Get lunar info for each holiday to display both dates
            foreach (var holiday in filteredHolidays)
            {
                // For lunar-based holidays, get the lunar date from the holiday definition
                if (holiday.Holiday.LunarMonth > 0 && holiday.Holiday.LunarDay > 0)
                {
                    // Lunar date is already in the holiday model
                    continue;
                }
            }

            YearHolidays = new ObservableCollection<LocalizedHolidayOccurrence>(
                filteredHolidays.OrderBy(h => h.GregorianDate)
                    .Select(h => new LocalizedHolidayOccurrence(h)));
            
            System.Diagnostics.Debug.WriteLine($"=== YearHolidays collection updated with {YearHolidays.Count} items ===");
            Console.WriteLine($"!!! YearHolidays collection updated with {YearHolidays.Count} items !!!");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"=== ERROR loading year holidays: {ex.Message} ===");
            System.Diagnostics.Debug.WriteLine($"=== Stack: {ex.StackTrace} ===");
            Console.WriteLine($"!!! ERROR loading year holidays: {ex.Message} !!!");
            Console.WriteLine($"!!! Stack: {ex.StackTrace} !!!");
        }
    }

    private async Task LoadUpcomingHolidaysAsync()
    {
        // Prevent concurrent updates that can cause iOS crashes
        if (_isUpdatingHolidays)
        {
            System.Diagnostics.Debug.WriteLine("=== Skipping concurrent holiday update ===");
            return;
        }

        await _updateSemaphore.WaitAsync();

        try
        {
            _isUpdatingHolidays = true;

            System.Diagnostics.Debug.WriteLine($"=== LoadUpcomingHolidaysAsync START ===");
            System.Diagnostics.Debug.WriteLine($"=== UpcomingHolidaysDays: {UpcomingHolidaysDays} ===");

            // CRITICAL iOS FIX: Hide CollectionView BEFORE modifying collection
            IsLoadingHolidays = true;
            System.Diagnostics.Debug.WriteLine($"=== IsLoadingHolidays set to TRUE ===");

            // iOS-specific: Longer delay to ensure CALayer finishes hiding CollectionView
            // This prevents CALayer drawInContext crash during collection update
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Task.Delay(200);
            }
            else
            {
                await Task.Delay(50);
            }

            var today = DateTime.Today;
            var endDate = today.AddDays(UpcomingHolidaysDays);
            System.Diagnostics.Debug.WriteLine($"=== Date range: {today:yyyy-MM-dd} to {endDate:yyyy-MM-dd} ===");

            var upcomingHolidays = new List<HolidayOccurrence>();

            // Get current year holidays
            System.Diagnostics.Debug.WriteLine($"=== Fetching holidays for year {today.Year} ===");
            var currentYearHolidays = await _holidayService.GetHolidaysForYearAsync(today.Year);
            System.Diagnostics.Debug.WriteLine($"=== Got {currentYearHolidays.Count} holidays for year {today.Year} ===");

            // If the range extends into next year, get next year's holidays too
            if (endDate.Year > today.Year)
            {
                System.Diagnostics.Debug.WriteLine($"=== Fetching holidays for year {today.Year + 1} ===");
                var nextYearHolidays = await _holidayService.GetHolidaysForYearAsync(today.Year + 1);
                System.Diagnostics.Debug.WriteLine($"=== Got {nextYearHolidays.Count} holidays for year {today.Year + 1} ===");
                currentYearHolidays = currentYearHolidays.Concat(nextYearHolidays).ToList();
                System.Diagnostics.Debug.WriteLine($"=== Total holidays after concat: {currentYearHolidays.Count} ===");
            }

            // Filter holidays that fall within the upcoming range
            upcomingHolidays = currentYearHolidays
                .Where(h => h.GregorianDate.Date >= today && h.GregorianDate.Date <= endDate)
                .OrderBy(h => h.GregorianDate)
                .ToList();

            System.Diagnostics.Debug.WriteLine($"=== After filtering: Found {upcomingHolidays.Count} upcoming holidays ===");
            Console.WriteLine($"!!! FOUND {upcomingHolidays.Count} UPCOMING HOLIDAYS !!!");
            foreach (var h in upcomingHolidays)
            {
                var msg = $"  - {h.Holiday.Name} on {h.GregorianDate:yyyy-MM-dd}";
                System.Diagnostics.Debug.WriteLine($"==={msg} ===");
                Console.WriteLine($"!!!{msg} !!!");
            }
            System.Diagnostics.Debug.WriteLine($"=== Current UpcomingHolidays collection has {UpcomingHolidays.Count} items ===");

            // CRITICAL iOS FIX: Create NEW collection instead of modifying existing one
            // This is atomic and prevents CALayer rendering crashes during collection updates
            System.Diagnostics.Debug.WriteLine($"=== Creating new collection with {upcomingHolidays.Count} holidays ===");

            var newCollection = new ObservableCollection<LocalizedHolidayOccurrence>(
                upcomingHolidays.Select(h => new LocalizedHolidayOccurrence(h))
            );

            // Replace entire collection reference on main thread - atomic operation
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("=== Replacing UpcomingHolidays collection reference ===");
                    UpcomingHolidays = newCollection;
                    System.Diagnostics.Debug.WriteLine($"=== Collection replaced successfully: {UpcomingHolidays.Count} items ===");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"=== ERROR replacing collection: {ex.Message} ===");
                    System.Diagnostics.Debug.WriteLine($"=== Stack: {ex.StackTrace} ===");
                }
            });

            // iOS-specific: Longer delay to ensure collection binding completes
            // This prevents showing the CollectionView before iOS finishes binding
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Task.Delay(200);
            }
            else
            {
                await Task.Delay(100);
            }

            // Show CollectionView again
            System.Diagnostics.Debug.WriteLine("=== Setting IsLoadingHolidays to FALSE ===");
            IsLoadingHolidays = false;
            System.Diagnostics.Debug.WriteLine($"=== LoadUpcomingHolidaysAsync COMPLETE - IsLoadingHolidays: {IsLoadingHolidays}, Count: {UpcomingHolidays.Count} ===");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"=== Error loading upcoming holidays: {ex.Message} ===");
            System.Diagnostics.Debug.WriteLine($"=== Stack: {ex.StackTrace} ===");
            IsLoadingHolidays = false;
        }
        finally
        {
            System.Diagnostics.Debug.WriteLine("=== Releasing update semaphore ===");
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
                System.Diagnostics.Debug.WriteLine($"Error updating calendar days: {ex.Message}");
                // Fallback to full replacement
                CalendarDays.Clear();
                foreach (var day in newDays)
                {
                    CalendarDays.Add(day);
                }
            }
        });
    }
}
