using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.Data;
using LunarCalendar.MobileApp.Resources.Strings;
using LunarCalendar.MobileApp.Models;
using System.Windows.Input;

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
    private readonly ILogService? _logService;

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
    private string _connectionStatusText = string.Empty;

    [ObservableProperty]
    private string _lastSyncTime = "Never";

    [ObservableProperty]
    private bool _isSyncing = false;

    [ObservableProperty]
    private List<LanguageOption> _availableLanguages = new();

    [ObservableProperty]
    private LanguageOption? _selectedLanguage;

    // Manual command properties
    public ICommand SyncDataCommand { get; }
    public ICommand ClearCacheCommand { get; }
    public ICommand AboutCommand { get; }
    public ICommand ResetSettingsCommand { get; }

    public SettingsViewModel()
    {
        
        // Initialize commands manually
        SyncDataCommand = new AsyncRelayCommand(SyncDataAsync);
        ClearCacheCommand = new AsyncRelayCommand(ClearCacheAsync);
        AboutCommand = new AsyncRelayCommand(AboutAsync);
        ResetSettingsCommand = new AsyncRelayCommand(ResetSettingsAsync);
        
        Title = AppResources.Settings;
        ConnectionStatusText = AppResources.Online; // Initialize with default
        LoadSettings();
        LoadAppInfo();
    }

    public SettingsViewModel(
        IConnectivityService connectivityService,
        ISyncService syncService,
        LunarCalendarDatabase database,
        ILocalizationService localizationService,
        ILogService logService)
    {
        
        _connectivityService = connectivityService;
        _syncService = syncService;
        _database = database;
        _localizationService = localizationService;
        _logService = logService;


        // Initialize commands manually
        SyncDataCommand = new AsyncRelayCommand(SyncDataAsync);
        ClearCacheCommand = new AsyncRelayCommand(ClearCacheAsync);
        AboutCommand = new AsyncRelayCommand(AboutAsync);
        ResetSettingsCommand = new AsyncRelayCommand(ResetSettingsAsync);

        Title = AppResources.Settings;
        LoadSettings();
        LoadAppInfo();
        LoadLanguageSettings();
        UpdateSyncStatus();
        UpdateConnectionStatus();

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

        // Subscribe to language changes
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (r, m) =>
        {
            Title = AppResources.Settings; // Update title with new language
            UpdateConnectionStatus(); // Update connection status text with new language
            UpdateSyncStatus(); // Update sync status with new language
            LoadAppInfo(); // Update app version format with new language
        });
    }

    private void OnConnectivityChanged(object? sender, bool isConnected)
    {
        IsOnline = isConnected;
        UpdateConnectionStatus();
    }

    private void UpdateConnectionStatus()
    {
        ConnectionStatusText = IsOnline ? AppResources.Online : AppResources.Offline;
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
                // Use culture-aware date formatting based on current language
                var culture = System.Globalization.CultureInfo.CurrentCulture;
                LastSyncTime = _syncService.LastSyncTime.Value.ToString("g", culture); // Short date and time pattern
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

        }
    }

    partial void OnShowCulturalBackgroundChanged(bool value)
    {
        SaveSetting(ShowCulturalBackgroundKey, value);
        WeakReferenceMessenger.Default.Send(new CulturalBackgroundChangedMessage { ShowCulturalBackground = value });
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
        AppVersion = string.Format(AppResources.AppVersionFormat, AppInfo.VersionString, AppInfo.BuildString);
    }

    public async Task SyncDataAsync()
    {
        try
        {
            
            if (_connectivityService == null || _syncService == null)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.ErrorTitle, AppResources.SyncServiceNotAvailable, AppResources.OK);
                return;
            }

            if (!_connectivityService.IsConnected)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.Offline, AppResources.OfflineMessage, AppResources.OK);
                return;
            }

            IsSyncing = true;
            var currentYear = DateTime.Today.Year;
            var currentMonth = DateTime.Today.Month;

            var success = await _syncService.SyncAllAsync(currentYear, currentMonth);

            if (success)
            {
                UpdateSyncStatus();
                await Application.Current.MainPage.DisplayAlert(AppResources.SyncComplete, AppResources.SyncCompleteMessage, AppResources.OK);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.SyncFailed, AppResources.SyncFailedMessage, AppResources.OK);
            }
        }
        catch (Exception ex)
        {
            try
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.SyncError, string.Format(AppResources.SyncErrorMessage, ex.Message), AppResources.OK);
            }
            catch (Exception displayEx)
            {
            }
        }
        finally
        {
            IsSyncing = false;
        }
    }

    public async Task ClearCacheAsync()
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

            await Application.Current.MainPage.DisplayAlert(AppResources.Success, AppResources.CacheClearedMessage, AppResources.OK);
        }
        catch (Exception ex)
        {
            try
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.ErrorTitle, string.Format(AppResources.ClearCacheError, ex.Message), AppResources.OK);
            }
            catch (Exception displayEx)
            {
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task AboutAsync()
    {
        try
        {
            await Application.Current.MainPage.DisplayAlert(
                AppResources.About,
                AppResources.AboutMessage,
                AppResources.OK);
        }
        catch (Exception ex)
        {
        }
    }

    public async Task ResetSettingsAsync()
    {
        try
        {
            var confirmed = await Application.Current.MainPage.DisplayAlert(
                AppResources.ResetSettingsTitle,
                AppResources.ResetSettingsMessage,
                AppResources.Yes,
                AppResources.No);

            
            if (confirmed)
            {
                Preferences.Clear();
                LoadSettings();
                
                await Application.Current.MainPage.DisplayAlert(AppResources.Success, AppResources.SettingsResetMessage, AppResources.OK);
            }
        }
        catch (Exception ex)
        {
            try
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.ErrorTitle, $"Failed to reset settings: {ex.Message}", AppResources.OK);
            }
            catch (Exception displayEx)
            {
            }
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

    // Diagnostic Commands
    [RelayCommand]
    private async Task ViewLogsAsync()
    {
        try
        {
            if (_logService == null)
            {
                await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.LogServiceNotAvailable, AppResources.OK);
                return;
            }

            var logs = await _logService.GetLogsAsync();

            if (string.IsNullOrEmpty(logs))
            {
                await Shell.Current.DisplayAlert(AppResources.LogsTitle, AppResources.NoLogsAvailable, AppResources.OK);
                return;
            }

            // For MVP: Show last 2000 characters (most recent logs)
            var displayLogs = logs.Length > 2000
                ? "..." + logs.Substring(logs.Length - 2000)
                : logs;

            await Shell.Current.DisplayAlert(
                AppResources.DiagnosticLogsTitle,
                displayLogs,
                AppResources.OK);
        }
        catch (Exception ex)
        {
            _logService?.LogError("Failed to view logs", ex, "SettingsViewModel.ViewLogs");
            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, string.Format(AppResources.FailedToLoadLogs, ex.Message), AppResources.OK);
        }
    }

    [RelayCommand]
    private async Task ClearLogsAsync()
    {
        try
        {
            if (_logService == null)
            {
                await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.LogServiceNotAvailable, AppResources.OK);
                return;
            }

            var confirmed = await Shell.Current.DisplayAlert(
                AppResources.ClearLogsConfirmTitle,
                AppResources.ClearLogsConfirmMessage,
                AppResources.Yes,
                AppResources.No);

            if (!confirmed) return;

            await _logService.ClearLogsAsync();
            _logService.LogInfo("Logs cleared by user", "SettingsViewModel.ClearLogs");

            await Shell.Current.DisplayAlert(AppResources.Success, AppResources.LogsClearedSuccess, AppResources.OK);
        }
        catch (Exception ex)
        {
            _logService?.LogError("Failed to clear logs", ex, "SettingsViewModel.ClearLogs");
            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, string.Format(AppResources.FailedToClearLogs, ex.Message), AppResources.OK);
        }
    }
}
