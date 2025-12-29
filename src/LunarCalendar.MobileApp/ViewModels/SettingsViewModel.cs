using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.Data;

namespace LunarCalendar.MobileApp.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private const string ShowCulturalBackgroundKey = "ShowCulturalBackground";
    private const string EnableHapticFeedbackKey = "EnableHapticFeedback";
    private const string ShowLunarDatesKey = "ShowLunarDates";
    private const string UpcomingHolidaysDaysKey = "UpcomingHolidaysDays";

    private readonly IConnectivityService? _connectivityService;
    private readonly ISyncService? _syncService;
    private readonly LunarCalendarDatabase? _database;

    [ObservableProperty]
    private bool _showCulturalBackground;

    [ObservableProperty]
    private bool _enableHapticFeedback;

    [ObservableProperty]
    private bool _showLunarDates;

    [ObservableProperty]
    private int _upcomingHolidaysDays;

    [ObservableProperty]
    private string _appVersion = string.Empty;

    [ObservableProperty]
    private bool _isOnline = true;

    [ObservableProperty]
    private string _lastSyncTime = "Never";

    [ObservableProperty]
    private bool _isSyncing = false;

    public SettingsViewModel()
    {
        Title = "Settings";
        LoadSettings();
        LoadAppInfo();
    }

    public SettingsViewModel(
        IConnectivityService connectivityService,
        ISyncService syncService,
        LunarCalendarDatabase database)
    {
        _connectivityService = connectivityService;
        _syncService = syncService;
        _database = database;

        Title = "Settings";
        LoadSettings();
        LoadAppInfo();
        UpdateSyncStatus();

        // Monitor connectivity
        if (_connectivityService != null)
        {
            IsOnline = _connectivityService.IsConnected;
            _connectivityService.ConnectivityChanged += OnConnectivityChanged;
        }

        // Monitor sync status
        if (_syncService != null)
        {
            _syncService.SyncStatusChanged += OnSyncStatusChanged;
        }
    }

    private void OnConnectivityChanged(object? sender, bool isConnected)
    {
        IsOnline = isConnected;
    }

    private void OnSyncStatusChanged(object? sender, SyncStatusChangedEventArgs e)
    {
        IsSyncing = e.IsSyncing;
        if (!e.IsSyncing && e.Success)
        {
            UpdateSyncStatus();
        }
    }

    private void UpdateSyncStatus()
    {
        if (_syncService?.LastSyncTime != null)
        {
            var timeSince = DateTime.Now - _syncService.LastSyncTime.Value;
            if (timeSince.TotalMinutes < 1)
            {
                LastSyncTime = "Just now";
            }
            else if (timeSince.TotalHours < 1)
            {
                LastSyncTime = $"{(int)timeSince.TotalMinutes} minutes ago";
            }
            else if (timeSince.TotalDays < 1)
            {
                LastSyncTime = $"{(int)timeSince.TotalHours} hours ago";
            }
            else
            {
                LastSyncTime = _syncService.LastSyncTime.Value.ToString("MMM dd, yyyy HH:mm");
            }
        }
        else
        {
            LastSyncTime = "Never";
        }
    }

    partial void OnShowCulturalBackgroundChanged(bool value)
    {
        SaveSetting(ShowCulturalBackgroundKey, value);
    }

    partial void OnEnableHapticFeedbackChanged(bool value)
    {
        SaveSetting(EnableHapticFeedbackKey, value);
    }

    partial void OnShowLunarDatesChanged(bool value)
    {
        SaveSetting(ShowLunarDatesKey, value);
    }

    partial void OnUpcomingHolidaysDaysChanged(int value)
    {
        Preferences.Set(UpcomingHolidaysDaysKey, value);
    }

    private void LoadSettings()
    {
        ShowCulturalBackground = Preferences.Get(ShowCulturalBackgroundKey, true);
        EnableHapticFeedback = Preferences.Get(EnableHapticFeedbackKey, true);
        ShowLunarDates = Preferences.Get(ShowLunarDatesKey, true);
        UpcomingHolidaysDays = Preferences.Get(UpcomingHolidaysDaysKey, 30);
    }

    private void SaveSetting(string key, bool value)
    {
        Preferences.Set(key, value);
    }

    private void LoadAppInfo()
    {
        AppVersion = $"Version {AppInfo.VersionString} (Build {AppInfo.BuildString})";
    }

    [RelayCommand]
    async Task SyncDataAsync()
    {
        if (_connectivityService == null || _syncService == null)
        {
            await Shell.Current.DisplayAlert("Error", "Sync service not available", "OK");
            return;
        }

        if (!_connectivityService.IsConnected)
        {
            await Shell.Current.DisplayAlert("Offline", "Cannot sync while offline. Please check your internet connection.", "OK");
            return;
        }

        try
        {
            IsSyncing = true;
            var currentYear = DateTime.Today.Year;
            var currentMonth = DateTime.Today.Month;

            var success = await _syncService.SyncAllAsync(currentYear, currentMonth);

            if (success)
            {
                UpdateSyncStatus();
                await Shell.Current.DisplayAlert("Sync Complete", "Calendar data has been synchronized successfully.", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Sync Failed", "Failed to synchronize calendar data. Please try again later.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Sync Error", $"An error occurred during sync: {ex.Message}", "OK");
        }
        finally
        {
            IsSyncing = false;
        }
    }

    [RelayCommand]
    async Task ClearCacheAsync()
    {
        try
        {
            IsBusy = true;

            // Clear app cache
            var cacheDir = FileSystem.CacheDirectory;
            if (Directory.Exists(cacheDir))
            {
                var files = Directory.GetFiles(cacheDir);
                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch { /* Ignore errors for individual files */ }
                }
            }

            // Clear database cache
            if (_database != null)
            {
                await _database.ClearAllDataAsync();
            }

            await Shell.Current.DisplayAlert("Success", "Cache and offline data cleared successfully", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to clear cache: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    async Task AboutAsync()
    {
        await Shell.Current.DisplayAlert(
            "About",
            "Vietnamese Lunar Calendar\n\nA beautiful calendar app that displays both Gregorian and Vietnamese Lunar dates with traditional holidays.\n\n© 2025 All rights reserved.",
            "OK");
    }

    [RelayCommand]
    async Task ResetSettingsAsync()
    {
        var confirmed = await Shell.Current.DisplayAlert(
            "Reset Settings",
            "Are you sure you want to reset all settings to default values?",
            "Yes",
            "No");

        if (confirmed)
        {
            Preferences.Clear();
            LoadSettings();
            await Shell.Current.DisplayAlert("Success", "Settings have been reset to default values", "OK");
        }
    }

    public static bool GetShowCulturalBackground()
    {
        return Preferences.Get(ShowCulturalBackgroundKey, true);
    }

    public static bool GetEnableHapticFeedback()
    {
        return Preferences.Get(EnableHapticFeedbackKey, true);
    }

    public static bool GetShowLunarDates()
    {
        return Preferences.Get(ShowLunarDatesKey, true);
    }

    public static int GetUpcomingHolidaysDays()
    {
        return Preferences.Get(UpcomingHolidaysDaysKey, 30);
    }
}
