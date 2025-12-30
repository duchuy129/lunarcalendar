using LunarCalendar.MobileApp.Resources.Strings;

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
}
