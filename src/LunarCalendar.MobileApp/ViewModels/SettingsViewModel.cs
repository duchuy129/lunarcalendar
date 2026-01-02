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
    
    // Manual command properties
    public ICommand SyncDataCommand { get; }
    public ICommand ClearCacheCommand { get; }
    public ICommand AboutCommand { get; }
    public ICommand ResetSettingsCommand { get; }

    public SettingsViewModel()
    {
        System.Diagnostics.Debug.WriteLine("!!! SettingsViewModel() - PARAMETERLESS constructor called !!!");
        
        // Initialize commands manually
        SyncDataCommand = new AsyncRelayCommand(SyncDataAsync);
        ClearCacheCommand = new AsyncRelayCommand(ClearCacheAsync);
        AboutCommand = new AsyncRelayCommand(AboutAsync);
        ResetSettingsCommand = new AsyncRelayCommand(ResetSettingsAsync);
        
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
        System.Diagnostics.Debug.WriteLine("!!! SettingsViewModel(services) - DI constructor called !!!");
        
        _connectivityService = connectivityService;
        _syncService = syncService;
        _database = database;
        _localizationService = localizationService;

        System.Diagnostics.Debug.WriteLine($"!!! _connectivityService injected: {_connectivityService != null} !!!");
        System.Diagnostics.Debug.WriteLine($"!!! _syncService injected: {_syncService != null} !!!");
        System.Diagnostics.Debug.WriteLine($"!!! _database injected: {_database != null} !!!");
        System.Diagnostics.Debug.WriteLine($"!!! _localizationService injected: {_localizationService != null} !!!");

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
        });
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

    public async Task SyncDataAsync()
    {
        System.Diagnostics.Debug.WriteLine("!!! SyncDataAsync CALLED !!!");
        try
        {
            System.Diagnostics.Debug.WriteLine($"!!! _connectivityService: {_connectivityService != null} !!!");
            System.Diagnostics.Debug.WriteLine($"!!! _syncService: {_syncService != null} !!!");
            
            if (_connectivityService == null || _syncService == null)
            {
                System.Diagnostics.Debug.WriteLine("!!! Services are null, showing alert !!!");
                await Application.Current.MainPage.DisplayAlert(AppResources.ErrorTitle, AppResources.SyncServiceNotAvailable, AppResources.OK);
                return;
            }

            if (!_connectivityService.IsConnected)
            {
                System.Diagnostics.Debug.WriteLine("!!! Not connected, showing alert !!!");
                await Application.Current.MainPage.DisplayAlert(AppResources.Offline, AppResources.OfflineMessage, AppResources.OK);
                return;
            }

            System.Diagnostics.Debug.WriteLine("!!! Starting sync !!!");
            IsSyncing = true;
            var currentYear = DateTime.Today.Year;
            var currentMonth = DateTime.Today.Month;

            var success = await _syncService.SyncAllAsync(currentYear, currentMonth);

            if (success)
            {
                System.Diagnostics.Debug.WriteLine("!!! Sync successful !!!");
                UpdateSyncStatus();
                await Application.Current.MainPage.DisplayAlert(AppResources.SyncComplete, AppResources.SyncCompleteMessage, AppResources.OK);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("!!! Sync failed !!!");
                await Application.Current.MainPage.DisplayAlert(AppResources.SyncFailed, AppResources.SyncFailedMessage, AppResources.OK);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"!!! SyncDataAsync error: {ex.Message} !!!");
            System.Diagnostics.Debug.WriteLine($"!!! Stack trace: {ex.StackTrace} !!!");
            try
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.SyncError, string.Format(AppResources.SyncErrorMessage, ex.Message), AppResources.OK);
            }
            catch (Exception displayEx)
            {
                System.Diagnostics.Debug.WriteLine($"!!! Failed to display error alert: {displayEx.Message} !!!");
            }
        }
        finally
        {
            IsSyncing = false;
        }
    }

    public async Task ClearCacheAsync()
    {
        System.Diagnostics.Debug.WriteLine("!!! ClearCacheAsync CALLED !!!");
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

            System.Diagnostics.Debug.WriteLine("!!! Cache cleared, showing alert !!!");
            await Application.Current.MainPage.DisplayAlert(AppResources.Success, AppResources.CacheClearedMessage, AppResources.OK);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"!!! ClearCacheAsync error: {ex.Message} !!!");
            System.Diagnostics.Debug.WriteLine($"!!! Stack trace: {ex.StackTrace} !!!");
            try
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.ErrorTitle, string.Format(AppResources.ClearCacheError, ex.Message), AppResources.OK);
            }
            catch (Exception displayEx)
            {
                System.Diagnostics.Debug.WriteLine($"!!! Failed to display error alert: {displayEx.Message} !!!");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task AboutAsync()
    {
        System.Diagnostics.Debug.WriteLine("!!! AboutAsync CALLED !!!");
        try
        {
            System.Diagnostics.Debug.WriteLine("!!! Showing About alert !!!");
            await Application.Current.MainPage.DisplayAlert(
                AppResources.About,
                AppResources.AboutMessage,
                AppResources.OK);
            System.Diagnostics.Debug.WriteLine("!!! About alert completed !!!");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"!!! AboutAsync error: {ex.Message} !!!");
            System.Diagnostics.Debug.WriteLine($"!!! Stack trace: {ex.StackTrace} !!!");
        }
    }

    public async Task ResetSettingsAsync()
    {
        System.Diagnostics.Debug.WriteLine("!!! ResetSettingsAsync CALLED !!!");
        try
        {
            System.Diagnostics.Debug.WriteLine("!!! Showing confirmation dialog !!!");
            var confirmed = await Application.Current.MainPage.DisplayAlert(
                AppResources.ResetSettingsTitle,
                AppResources.ResetSettingsMessage,
                AppResources.Yes,
                AppResources.No);

            System.Diagnostics.Debug.WriteLine($"!!! User confirmed: {confirmed} !!!");
            
            if (confirmed)
            {
                Preferences.Clear();
                LoadSettings();
                
                System.Diagnostics.Debug.WriteLine("!!! Settings reset, showing success alert !!!");
                await Application.Current.MainPage.DisplayAlert(AppResources.Success, AppResources.SettingsResetMessage, AppResources.OK);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"!!! ResetSettingsAsync error: {ex.Message} !!!");
            System.Diagnostics.Debug.WriteLine($"!!! Stack trace: {ex.StackTrace} !!!");
            try
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.ErrorTitle, $"Failed to reset settings: {ex.Message}", AppResources.OK);
            }
            catch (Exception displayEx)
            {
                System.Diagnostics.Debug.WriteLine($"!!! Failed to display error alert: {displayEx.Message} !!!");
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
}
