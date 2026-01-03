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
    private readonly SemaphoreSlim _updateSemaphore = new(1, 1);

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

    public YearHolidaysViewModel(IHolidayService holidayService)
    {
        _holidayService = holidayService;

        // Initialize available years - wide range for year picker
        // Users can navigate to any year using previous/next buttons
        var currentYear = DateTime.Now.Year;
        for (int i = currentYear - 50; i <= currentYear + 50; i++)
        {
            AvailableYears.Add(i);
        }

        // Set selected year to current year
        _selectedYear = currentYear;

        // Subscribe to language change events
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, async (r, m) =>
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== YearHolidaysViewModel: Language changed ===");

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
                        System.Diagnostics.Debug.WriteLine($"=== Replaced with NEW collection: {newCollection.Count} refreshed items ===");
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"=== Error in language change handler: {ex.Message} ===");
            }
        });
    }

    public async Task InitializeAsync()
    {
        await LoadYearHolidaysAsync();
    }

    partial void OnSelectedYearChanged(int value)
    {
        System.Diagnostics.Debug.WriteLine($"=== YearHolidaysViewModel: Selected year changed to {value} ===");

        // Use Task.Run to avoid blocking and properly handle async call
        Task.Run(async () =>
        {
            try
            {
                await LoadYearHolidaysAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"=== ERROR in OnSelectedYearChanged: {ex.Message} ===");
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
                System.Diagnostics.Debug.WriteLine("=== Service provider not available ===");
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
            System.Diagnostics.Debug.WriteLine($"=== Error navigating to holiday detail: {ex.Message} ===");
            System.Diagnostics.Debug.WriteLine($"=== Stack: {ex.StackTrace} ===");
        }
    }

    private async Task LoadYearHolidaysAsync()
    {
        // Prevent concurrent updates
        if (!await _updateSemaphore.WaitAsync(0))
        {
            System.Diagnostics.Debug.WriteLine($"=== LoadYearHolidaysAsync: Already loading, skipping ===");
            return;
        }

        try
        {
            System.Diagnostics.Debug.WriteLine($"=== LoadYearHolidaysAsync START for year {SelectedYear} ===");
            
            IsLoading = true;

            var holidays = await _holidayService.GetHolidaysForYearAsync(SelectedYear);
            System.Diagnostics.Debug.WriteLine($"=== Got {holidays.Count} holidays from service ===");

            // Filter out Lunar Special Days
            var filteredHolidays = holidays
                .Where(h => h.Holiday.Type != HolidayType.LunarSpecialDay)
                .OrderBy(h => h.GregorianDate)
                .ToList();

            System.Diagnostics.Debug.WriteLine($"=== After filtering: {filteredHolidays.Count} holidays ===");

            // CRITICAL iOS FIX: Create NEW ObservableCollection and replace entire reference
            // This is the SAME pattern used in CalendarViewModel.LoadUpcomingHolidaysAsync
            var newCollection = new ObservableCollection<LocalizedHolidayOccurrence>(
                filteredHolidays.Select(h => new LocalizedHolidayOccurrence(h))
            );

            // Replace entire collection reference on main thread - atomic operation
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                YearHolidays = newCollection;
                System.Diagnostics.Debug.WriteLine($"=== YearHolidays replaced with NEW collection: {YearHolidays.Count} items ===");
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"=== ERROR loading year holidays: {ex.Message} ===");

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
                System.Diagnostics.Debug.WriteLine($"=== IsLoading set to FALSE ===");
            });

            _updateSemaphore.Release();
        }
    }
}
