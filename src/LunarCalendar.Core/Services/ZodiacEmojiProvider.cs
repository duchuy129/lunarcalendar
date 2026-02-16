using LunarCalendar.Core.Models;

namespace LunarCalendar.Core.Services;

/// <summary>
/// Centralized mapping for Sprint 10: zodiac animal -> Unicode emoji.
/// Keeping this in Core allows reuse across MobileApp and other front-ends.
/// </summary>
public static class ZodiacEmojiProvider
{
    public static string GetEmoji(ZodiacAnimal animal) => animal switch
    {
        ZodiacAnimal.Rat => "🐭",
        ZodiacAnimal.Ox => "🐮",
        ZodiacAnimal.Tiger => "🐯",
        ZodiacAnimal.Rabbit => "🐰",
        ZodiacAnimal.Dragon => "🐲",
        ZodiacAnimal.Snake => "🐍",
        ZodiacAnimal.Horse => "🐴",
        ZodiacAnimal.Goat => "🐑",
        ZodiacAnimal.Monkey => "🐵",
        ZodiacAnimal.Rooster => "🐔",
        ZodiacAnimal.Dog => "🐶",
        ZodiacAnimal.Pig => "🐷",
        _ => "❓"
    };

    /// <summary>
    /// Short accessibility-friendly label. (Not localized yet.)
    /// </summary>
    public static string GetA11yLabel(ZodiacAnimal animal) => animal switch
    {
        ZodiacAnimal.Rat => "Rat",
        ZodiacAnimal.Ox => "Ox",
        ZodiacAnimal.Tiger => "Tiger",
        ZodiacAnimal.Rabbit => "Rabbit",
        ZodiacAnimal.Dragon => "Dragon",
        ZodiacAnimal.Snake => "Snake",
        ZodiacAnimal.Horse => "Horse",
        ZodiacAnimal.Goat => "Goat",
        ZodiacAnimal.Monkey => "Monkey",
        ZodiacAnimal.Rooster => "Rooster",
        ZodiacAnimal.Dog => "Dog",
        ZodiacAnimal.Pig => "Pig",
        _ => "Unknown"
    };
}
