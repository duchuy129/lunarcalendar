using System.Globalization;
using System.Resources;
using CommunityToolkit.Mvvm.Messaging;

namespace LunarCalendar.MobileApp.Services;

public class LocalizationService : ILocalizationService
{
    private const string LanguagePreferenceKey = "AppLanguage";
    private readonly ResourceManager _resourceManager;
    private readonly ILogService _logService;

    public LocalizationService(ILogService logService)
    {
        _logService = logService;
        _resourceManager = new ResourceManager(
            "LunarCalendar.MobileApp.Resources.Strings.AppResources",
            typeof(LocalizationService).Assembly);

        // Load saved language preference or default to system language
        var savedLanguage = Preferences.Get(LanguagePreferenceKey, string.Empty);
        if (!string.IsNullOrEmpty(savedLanguage))
        {
            SetLanguage(savedLanguage);
        }
        else
        {
            // Default to system language
            var systemLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            SetLanguage(systemLanguage == "vi" ? "vi" : "en");
        }
    }

    public string CurrentLanguage
    {
        get
        {
            var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return currentCulture == "vi" ? "vi" : "en";
        }
    }

    public List<LanguageOption> AvailableLanguages => new()
    {
        new LanguageOption
        {
            Code = "en",
            DisplayName = "English",
            NativeName = "English"
        },
        new LanguageOption
        {
            Code = "vi",
            DisplayName = "Vietnamese",
            NativeName = "Tiếng Việt"
        }
    };

    public void SetLanguage(string languageCode)
    {
        try
        {
            CultureInfo culture;

            switch (languageCode.ToLower())
            {
                case "en":
                    culture = new CultureInfo("en-US");
                    break;
                case "vi":
                default:
                    culture = new CultureInfo("vi-VN");
                    break;
            }

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            // Save preference
            Preferences.Set(LanguagePreferenceKey, languageCode);

            // Notify all subscribers that language has changed using CommunityToolkit.Mvvm.Messaging
            WeakReferenceMessenger.Default.Send(new LanguageChangedMessage());

        }
        catch (Exception ex)
        {
            // Fallback to Vietnamese
            var fallbackCulture = new CultureInfo("vi-VN");
            CultureInfo.CurrentCulture = fallbackCulture;
            CultureInfo.CurrentUICulture = fallbackCulture;
        }
    }

    public string GetString(string key)
    {
        try
        {
            var value = _resourceManager.GetString(key, CultureInfo.CurrentUICulture);
            return value ?? key; // Return key if not found
        }
        catch (Exception ex)
        {
            _logService.LogWarning($"Failed to get localized string for key: {key}", "LocalizationService.GetString");
            return key;
        }
    }

    public static string GetSavedLanguage()
    {
        return Preferences.Get(LanguagePreferenceKey, "vi");
    }
}
