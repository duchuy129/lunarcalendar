using LunarCalendar.MobileApp.Services;

namespace LunarCalendar.MobileApp.Extensions;

/// <summary>
/// XAML markup extension for reactive localized strings that update when language changes
/// Usage: Text="{ext:Translate AppName}"
/// </summary>
[ContentProperty(nameof(Key))]
public class TranslateExtension : IMarkupExtension<BindingBase>
{
    public string? Key { get; set; }

    public BindingBase ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrWhiteSpace(Key))
            return new Binding();

        // Create a LocalizedString instance for this key
        var localizedString = new LocalizedString(Key);

        // Return a binding to the Value property of LocalizedString
        // This binding will automatically update when the language changes
        return new Binding
        {
            Source = localizedString,
            Path = nameof(LocalizedString.Value),
            Mode = BindingMode.OneWay
        };
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}
