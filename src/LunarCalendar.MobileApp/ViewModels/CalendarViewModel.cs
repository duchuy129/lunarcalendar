using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LunarCalendar.MobileApp.Models;
using LunarCalendar.MobileApp.Services;

namespace LunarCalendar.MobileApp.ViewModels;

public partial class CalendarViewModel : BaseViewModel
{
    private readonly ICalendarService _calendarService;
    private readonly IUserModeService _userModeService;
    private readonly IHolidayService _holidayService;

    [ObservableProperty]
    private DateTime _currentMonth;

    [ObservableProperty]
    private string _monthYearDisplay = string.Empty;

    [ObservableProperty]
    private string _todayLunarDisplay = string.Empty;

    [ObservableProperty]
    private ObservableCollection<CalendarDay> _calendarDays = new();

    [ObservableProperty]
    private string _userModeText = string.Empty;

    [ObservableProperty]
    private int _selectedYear;

    [ObservableProperty]
    private ObservableCollection<HolidayOccurrence> _yearHolidays = new();

    [ObservableProperty]
    private ObservableCollection<int> _availableYears = new();

    [ObservableProperty]
    private bool _isYearSectionExpanded = false;

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

    public CalendarViewModel(
        ICalendarService calendarService,
        IUserModeService userModeService,
        IHolidayService holidayService)
    {
        _calendarService = calendarService;
        _userModeService = userModeService;
        _holidayService = holidayService;

        Title = "Vietnamese Calendar";
        _currentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        _selectedYear = DateTime.Today.Year;

        // Initialize available years (current year ± 10 years)
        for (int year = DateTime.Today.Year - 10; year <= DateTime.Today.Year + 10; year++)
        {
            AvailableYears.Add(year);
            AvailableCalendarYears.Add(year);
        }

        // Initialize month names
        AvailableMonths = new ObservableCollection<string>
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };

        // Set current month/year for pickers
        _selectedCalendarYear = DateTime.Today.Year;
        _selectedCalendarMonth = DateTime.Today.Month;

        UpdateUserModeText();
    }

    public async Task InitializeAsync()
    {
        try
        {
            await LoadCalendarAsync();
            await LoadYearHolidaysAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"=== INIT ERROR: {ex.Message} ===");
            System.Diagnostics.Debug.WriteLine($"=== STACK: {ex.StackTrace} ===");
            // Don't throw - let the app continue even if holiday loading fails
        }
    }

    [RelayCommand]
    async Task PreviousMonthAsync()
    {
        CurrentMonth = CurrentMonth.AddMonths(-1);
        await LoadCalendarAsync();
    }

    [RelayCommand]
    async Task NextMonthAsync()
    {
        CurrentMonth = CurrentMonth.AddMonths(1);
        await LoadCalendarAsync();
    }

    [RelayCommand]
    async Task TodayAsync()
    {
        CurrentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        await LoadCalendarAsync();
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

            // Update today's lunar display
            var todayLunar = lunarDates.FirstOrDefault(ld => ld.GregorianDate.Date == DateTime.Today);
            if (todayLunar != null)
            {
                TodayLunarDisplay = $"Today: {todayLunar.LunarDay}/{todayLunar.LunarMonth}/{todayLunar.LunarYear}";
            }

            // Create calendar days
            var days = new List<CalendarDay>();

            // Get first day of month and calculate start of calendar grid
            var firstDayOfMonth = CurrentMonth;
            var firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek; // 0 = Sunday
            var startDate = firstDayOfMonth.AddDays(-firstDayOfWeek);

            // Generate 42 days (6 weeks) for consistent calendar grid
            for (int i = 0; i < 42; i++)
            {
                var date = startDate.AddDays(i);
                var isCurrentMonth = date.Month == CurrentMonth.Month;
                var isToday = date.Date == DateTime.Today;

                // Find lunar info for this date
                var lunarInfo = lunarDates.FirstOrDefault(ld => ld.GregorianDate.Date == date.Date);

                // Find holiday for this date
                var holidayOccurrence = holidays.FirstOrDefault(h => h.GregorianDate.Date == date.Date);

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

            CalendarDays = new ObservableCollection<CalendarDay>(days);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading calendar: {ex.Message}");
            // Show error to user
            await Shell.Current.DisplayAlert("Error", "Failed to load calendar data", "OK");
        }
        finally
        {
            IsBusy = false;
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
    void ToggleYearSection()
    {
        IsYearSectionExpanded = !IsYearSectionExpanded;
    }

    [RelayCommand]
    async Task ViewHolidayDetailAsync(HolidayOccurrence holidayOccurrence)
    {
        if (holidayOccurrence == null) return;

        var navigationParameter = new Dictionary<string, object>
        {
            { "Holiday", holidayOccurrence }
        };

        await Shell.Current.GoToAsync("holidaydetail", navigationParameter);
    }

    private async Task LoadYearHolidaysAsync()
    {
        try
        {
            var holidays = await _holidayService.GetHolidaysForYearAsync(SelectedYear);

            // Get lunar info for each holiday to display both dates
            foreach (var holiday in holidays)
            {
                // For lunar-based holidays, get the lunar date from the holiday definition
                if (holiday.Holiday.LunarMonth > 0 && holiday.Holiday.LunarDay > 0)
                {
                    // Lunar date is already in the holiday model
                    continue;
                }
            }

            YearHolidays = new ObservableCollection<HolidayOccurrence>(
                holidays.OrderBy(h => h.GregorianDate));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading year holidays: {ex.Message}");
        }
    }
}
