using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.Data;
using LunarCalendar.MobileApp.Resources.Strings;
using LunarCalendar.MobileApp.Models;

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
    private readonly ILocalizationService? _localizationService;

    [ObservableProperty]
    private bool _showCulturalBackground;

    [ObservableProperty]
    private bool _enableHapticFeedback;

    [ObservableProperty]
    private bool _showLunarDates;

    [ObservableProperty]
    private int _upcomingHolidaysDays;

    [ObservableProperty]
    private List<int> _availableHolidaysDays = new() { 7, 14, 30, 60, 90 };

    [ObservableProperty]
    private string _appVersion = string.Empty;

    [ObservableProperty]
    private bool _isOnline = true;

    [ObservableProperty]
    private string _lastSyncTime = "Never";

    [ObservableProperty]
    private bool _isSyncing = false;

    [ObservableProperty]
    private List<LanguageOption> _availableLanguages = new();

    [ObservableProperty]
    private LanguageOption? _selectedLanguage;

    public SettingsViewModel()
    {
        Title = AppResources.Settings;
        LoadSettings();
        LoadAppInfo();
    }

    public SettingsViewModel(
        IConnectivityService connectivityService,
        ISyncService syncService,
        LunarCalendarDatabase database,
        ILocalizationService localizationService)
    {
        _connectivityService = connectivityService;
        _syncService = syncService;
        _database = database;
        _localizationService = localizationService;

        Title = AppResources.Settings;
        LoadSettings();
        LoadAppInfo();
        LoadLanguageSettings();
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
                LastSyncTime = AppResources.JustNow;
            }
            else if (timeSince.TotalHours < 1)
            {
                LastSyncTime = string.Format(AppResources.MinutesAgo, (int)timeSince.TotalMinutes);
            }
            else if (timeSince.TotalDays < 1)
            {
                LastSyncTime = string.Format(AppResources.HoursAgo, (int)timeSince.TotalHours);
            }
            else
            {
                LastSyncTime = _syncService.LastSyncTime.Value.ToString("MMM dd, yyyy HH:mm");
            }
        }
        else
        {
            LastSyncTime = AppResources.Never;
        }
    }

    private void LoadLanguageSettings()
    {
        if (_localizationService != null)
        {
            AvailableLanguages = _localizationService.AvailableLanguages;
            var currentLang = _localizationService.CurrentLanguage;
            SelectedLanguage = AvailableLanguages.FirstOrDefault(l => l.Code == currentLang);
        }
    }

    partial void OnSelectedLanguageChanged(LanguageOption? value)
    {
        if (value != null && _localizationService != null)
        {
            // Simply set the language - UI will update automatically via bindings
            _localizationService.SetLanguage(value.Code);

            System.Diagnostics.Debug.WriteLine($"=== Language switched to: {value.Code} ===");
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
            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.SyncServiceNotAvailable, AppResources.OK);
            return;
        }

        if (!_connectivityService.IsConnected)
        {
            await Shell.Current.DisplayAlert(AppResources.Offline, AppResources.OfflineMessage, AppResources.OK);
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
                await Shell.Current.DisplayAlert(AppResources.SyncComplete, AppResources.SyncCompleteMessage, AppResources.OK);
            }
            else
            {
                await Shell.Current.DisplayAlert(AppResources.SyncFailed, AppResources.SyncFailedMessage, AppResources.OK);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(AppResources.SyncError, string.Format(AppResources.SyncErrorMessage, ex.Message), AppResources.OK);
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

            await Shell.Current.DisplayAlert(AppResources.Success, AppResources.CacheClearedMessage, AppResources.OK);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, string.Format(AppResources.ClearCacheError, ex.Message), AppResources.OK);
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
            AppResources.About,
            AppResources.AboutMessage,
            AppResources.OK);
    }

    [RelayCommand]
    async Task ResetSettingsAsync()
    {
        var confirmed = await Shell.Current.DisplayAlert(
            AppResources.ResetSettingsTitle,
            AppResources.ResetSettingsMessage,
            AppResources.Yes,
            AppResources.No);

        if (confirmed)
        {
            Preferences.Clear();
            LoadSettings();
            await Shell.Current.DisplayAlert(AppResources.Success, AppResources.SettingsResetMessage, AppResources.OK);
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
