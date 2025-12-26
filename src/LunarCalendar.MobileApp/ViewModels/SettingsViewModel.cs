using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LunarCalendar.MobileApp.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private const string ShowCulturalBackgroundKey = "ShowCulturalBackground";
    private const string EnableHapticFeedbackKey = "EnableHapticFeedback";
    private const string ShowLunarDatesKey = "ShowLunarDates";

    [ObservableProperty]
    private bool _showCulturalBackground;

    [ObservableProperty]
    private bool _enableHapticFeedback;

    [ObservableProperty]
    private bool _showLunarDates;

    [ObservableProperty]
    private string _appVersion = string.Empty;

    public SettingsViewModel()
    {
        Title = "Settings";
        LoadSettings();
        LoadAppInfo();
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

    private void LoadSettings()
    {
        ShowCulturalBackground = Preferences.Get(ShowCulturalBackgroundKey, true);
        EnableHapticFeedback = Preferences.Get(EnableHapticFeedbackKey, true);
        ShowLunarDates = Preferences.Get(ShowLunarDatesKey, true);
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

            await Shell.Current.DisplayAlert("Success", "Cache cleared successfully", "OK");
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
}
