using LunarCalendar.MobileApp.Resources.Strings;
using System.Globalization;

namespace LunarCalendar.MobileApp.Services;

/// <summary>
/// Helper class for localization-related functions
/// </summary>
public static class LocalizationHelper
{
    /// <summary>
    /// Gets the localized animal sign name based on the English animal sign
    /// </summary>
    public static string GetLocalizedAnimalSign(string englishAnimalSign)
    {
        return englishAnimalSign switch
        {
            "Rat" => AppResources.AnimalRat,
            "Ox" => AppResources.AnimalOx,
            "Tiger" => AppResources.AnimalTiger,
            "Rabbit" => AppResources.AnimalRabbit,
            "Dragon" => AppResources.AnimalDragon,
            "Snake" => AppResources.AnimalSnake,
            "Horse" => AppResources.AnimalHorse,
            "Goat" => AppResources.AnimalGoat,
            "Monkey" => AppResources.AnimalMonkey,
            "Rooster" => AppResources.AnimalRooster,
            "Dog" => AppResources.AnimalDog,
            "Pig" => AppResources.AnimalPig,
            _ => englishAnimalSign
        };
    }

    /// <summary>
    /// Gets the localized holiday name from a resource key
    /// </summary>
    public static string GetLocalizedHolidayName(string resourceKey, string fallbackName)
    {
        if (string.IsNullOrEmpty(resourceKey))
            return fallbackName;

        try
        {
            var value = AppResources.ResourceManager.GetString(resourceKey, CultureInfo.CurrentUICulture);
            return value ?? fallbackName;
        }
        catch
        {
            return fallbackName;
        }
    }

    /// <summary>
    /// Gets the localized holiday description from a resource key
    /// </summary>
    public static string GetLocalizedHolidayDescription(string resourceKey, string fallbackDescription)
    {
        if (string.IsNullOrEmpty(resourceKey))
            return fallbackDescription;

        try
        {
            var value = AppResources.ResourceManager.GetString(resourceKey, CultureInfo.CurrentUICulture);
            return value ?? fallbackDescription;
        }
        catch
        {
            return fallbackDescription;
        }
    }
}
