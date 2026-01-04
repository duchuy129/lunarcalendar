using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LunarCalendar.Core.Models;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.Models;
using LunarCalendar.MobileApp.Resources.Strings;

namespace LunarCalendar.MobileApp.ViewModels;

public partial class YearHolidaysViewModel : ObservableObject
{
    private readonly IHolidayService _holidayService;
    private readonly ILogService _logService;
    private readonly SemaphoreSlim _updateSemaphore = new(1, 1);
    private volatile bool _isLanguageChanging = false;

    [ObservableProperty]
    private ObservableCollection<int> _availableYears = new();

    [ObservableProperty]
    private int _selectedYear;

    [ObservableProperty]
    private ObservableCollection<LocalizedHolidayOccurrence> _yearHolidays = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _todayButtonText = AppResources.Today;

    [ObservableProperty]
    private bool _showCulturalBackground = true;

    public YearHolidaysViewModel(IHolidayService holidayService, ILogService logService)
    {
        _holidayService = holidayService;
        _logService = logService;

        // Initialize available years - wide range for year picker
        // Users can navigate to any year using previous/next buttons
        var currentYear = DateTime.Now.Year;
        for (int i = currentYear - 50; i <= currentYear + 50; i++)
        {
            AvailableYears.Add(i);
        }

        // Set selected year to current year
        _selectedYear = currentYear;

        // Initialize settings
        ShowCulturalBackground = SettingsViewModel.GetShowCulturalBackground();

        // Subscribe to cultural background setting changes
        WeakReferenceMessenger.Default.Register<CulturalBackgroundChangedMessage>(this, (r, m) =>
        {
            ShowCulturalBackground = m.ShowCulturalBackground;
        });

        // Subscribe to language change events
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, async (r, m) =>
        {
            // Acquire semaphore to prevent race condition with year navigation
            await _updateSemaphore.WaitAsync();
            
            try
            {
                _isLanguageChanging = true;

                // Update button text
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    TodayButtonText = AppResources.Today;
                });

                // CRITICAL iOS FIX: Create NEW ObservableCollection with refreshed items
                // Same pattern as CalendarViewModel - do NOT modify existing collection
                if (YearHolidays != null && YearHolidays.Any())
                {
                    var currentItems = YearHolidays.ToList();
                    
                    // Refresh localized properties on each item
                    foreach (var holiday in currentItems)
                    {
                        holiday?.RefreshLocalizedProperties();
                    }
                    
                    // Create NEW ObservableCollection and replace entire reference
                    var newCollection = new ObservableCollection<LocalizedHolidayOccurrence>(currentItems);
                    
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        YearHolidays = newCollection;
                    });
                }
            }
            catch (Exception ex)
            {
                _logService.LogWarning("Failed to refresh UI after language change - non-critical", "YearHolidaysViewModel.OnLanguageChanged");
            }
            finally
            {
                _isLanguageChanging = false;
                _updateSemaphore.Release();
            }
        });
    }

    public async Task InitializeAsync()
    {
        await LoadYearHolidaysAsync();
    }

    partial void OnSelectedYearChanged(int value)
    {
        // Skip if language is currently changing to avoid race condition
        if (_isLanguageChanging)
        {
            return;
        }

        // Use Task.Run to avoid blocking and properly handle async call
        Task.Run(async () =>
        {
            try
            {
                await LoadYearHolidaysAsync();
            }
            catch (Exception ex)
            {
                _logService.LogError("Failed to load holidays for selected year", ex, "YearHolidaysViewModel.OnSelectedYearChanged");
            }
        });
    }

    [RelayCommand]
    private void PreviousYear()
    {
        var currentIndex = AvailableYears.IndexOf(SelectedYear);
        if (currentIndex > 0)
        {
            SelectedYear = AvailableYears[currentIndex - 1];
        }
    }

    [RelayCommand]
    private void NextYear()
    {
        var currentIndex = AvailableYears.IndexOf(SelectedYear);
        if (currentIndex < AvailableYears.Count - 1)
        {
            SelectedYear = AvailableYears[currentIndex + 1];
        }
    }

    [RelayCommand]
    private void CurrentYear()
    {
        SelectedYear = DateTime.Now.Year;
    }

    [RelayCommand]
    private async Task NavigateToHolidayDetail(LocalizedHolidayOccurrence holidayOccurrence)
    {
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
                viewModel.Holiday = holidayOccurrence.HolidayOccurrence;
            }

            // Navigate using Shell navigation
            await Shell.Current.Navigation.PushAsync(holidayDetailPage);
        }
        catch (Exception ex)
        {
            _logService.LogError("Failed to navigate to holiday detail", ex, "YearHolidaysViewModel.SelectHoliday");
        }
    }

    private async Task LoadYearHolidaysAsync()
    {
        // Skip if language change is in progress
        if (_isLanguageChanging)
        {
            return;
        }

        // Prevent concurrent updates
        if (!await _updateSemaphore.WaitAsync(0))
        {
            return;
        }

        try
        {
            // Double-check after acquiring semaphore
            if (_isLanguageChanging)
            {
                return;
            }
            
            IsLoading = true;

            var holidays = await _holidayService.GetHolidaysForYearAsync(SelectedYear);

            // Filter out Lunar Special Days
            var filteredHolidays = holidays
                .Where(h => h.Holiday.Type != HolidayType.LunarSpecialDay)
                .OrderBy(h => h.GregorianDate)
                .ToList();


            // CRITICAL iOS FIX: Create NEW ObservableCollection and replace entire reference
            // This is the SAME pattern used in CalendarViewModel.LoadUpcomingHolidaysAsync
            var newCollection = new ObservableCollection<LocalizedHolidayOccurrence>(
                filteredHolidays.Select(h => new LocalizedHolidayOccurrence(h))
            );

            // Replace entire collection reference on main thread - atomic operation
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                YearHolidays = newCollection;
            });
        }
        catch (Exception ex)
        {

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                YearHolidays = new ObservableCollection<LocalizedHolidayOccurrence>();
            });
        }
        finally
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                IsLoading = false;
            });

            _updateSemaphore.Release();
        }
    }
}
