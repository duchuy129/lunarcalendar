namespace LunarCalendar.MobileApp.Services;

public interface ILocalizationService
{
    string CurrentLanguage { get; }
    List<LanguageOption> AvailableLanguages { get; }
    void SetLanguage(string languageCode);
    string GetString(string key);
}

public class LanguageOption
{
    public string Code { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string NativeName { get; set; } = string.Empty;
}
