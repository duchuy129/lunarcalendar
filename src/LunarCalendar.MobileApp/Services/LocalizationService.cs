using System.Globalization;
using System.Resources;
using CommunityToolkit.Mvvm.Messaging;

namespace LunarCalendar.MobileApp.Services;

public class LocalizationService : ILocalizationService
{
    private const string LanguagePreferenceKey = "AppLanguage";
    private readonly ResourceManager _resourceManager;

    public LocalizationService()
    {
        _resourceManager = new ResourceManager(
            "LunarCalendar.MobileApp.Resources.Strings.AppResources",
            typeof(LocalizationService).Assembly);

        // Load saved language preference or default to Vietnamese
        var savedLanguage = Preferences.Get(LanguagePreferenceKey, string.Empty);
        if (!string.IsNullOrEmpty(savedLanguage))
        {
            SetLanguage(savedLanguage);
        }
        else
        {
            // Default to Vietnamese
            SetLanguage("vi");
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

            System.Diagnostics.Debug.WriteLine($"=== Language changed to: {culture.Name} ===");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error setting language: {ex.Message}");
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
        catch
        {
            return key;
        }
    }

    public static string GetSavedLanguage()
    {
        return Preferences.Get(LanguagePreferenceKey, "vi");
    }
}
