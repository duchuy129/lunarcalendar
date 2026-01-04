using System.ComponentModel;
using System.Globalization;
using System.Resources;
using CommunityToolkit.Mvvm.Messaging;

namespace LunarCalendar.MobileApp.Services;

/// <summary>
/// Message sent when language changes
/// </summary>
public class LanguageChangedMessage
{
}

/// <summary>
/// Message sent when cultural background setting changes
/// </summary>
public class CulturalBackgroundChangedMessage
{
    public bool ShowCulturalBackground { get; set; }
}

/// <summary>
/// Provides a reactive wrapper for localized strings that updates when language changes
/// </summary>
public class LocalizedString : INotifyPropertyChanged
{
    private readonly string _key;
    private static readonly ResourceManager ResourceManager = new(
        "LunarCalendar.MobileApp.Resources.Strings.AppResources",
        typeof(LocalizedString).Assembly);

    public event PropertyChangedEventHandler? PropertyChanged;

    public LocalizedString(string key)
    {
        _key = key;

        // Subscribe to language changes using CommunityToolkit.Mvvm.Messaging
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (r, m) =>
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        });
    }

    public string Value
    {
        get
        {
            try
            {
                var translation = ResourceManager.GetString(_key, CultureInfo.CurrentUICulture);
                return translation ?? _key;
            }
            catch
            {
                return _key;
            }
        }
    }

    public static implicit operator string(LocalizedString localizedString) => localizedString.Value;

    public override string ToString() => Value;
}
