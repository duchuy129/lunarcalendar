namespace LunarCalendar.Core.Models;

/// <summary>
/// The 12 Chinese Zodiac Animals (生肖 shēngxiào / Con Giáp)
/// Each animal corresponds to an Earthly Branch
/// </summary>
public enum ZodiacAnimal
{
    /// <summary>鼠 (shǔ) - Chuột - Rat</summary>
    Rat = 0,
    
    /// <summary>牛 (niú) - Trâu - Ox</summary>
    Ox = 1,
    
    /// <summary>虎 (hǔ) - Hổ - Tiger</summary>
    Tiger = 2,
    
    /// <summary>兔 (tù) - Mèo/Thỏ - Rabbit (Cat in Vietnamese)</summary>
    Rabbit = 3,
    
    /// <summary>龙 (lóng) - Rồng - Dragon</summary>
    Dragon = 4,
    
    /// <summary>蛇 (shé) - Rắn - Snake</summary>
    Snake = 5,
    
    /// <summary>马 (mǎ) - Ngựa - Horse</summary>
    Horse = 6,
    
    /// <summary>羊 (yáng) - Dê - Goat</summary>
    Goat = 7,
    
    /// <summary>猴 (hóu) - Khỉ - Monkey</summary>
    Monkey = 8,
    
    /// <summary>鸡 (jī) - Gà - Rooster</summary>
    Rooster = 9,
    
    /// <summary>狗 (gǒu) - Chó - Dog</summary>
    Dog = 10,
    
    /// <summary>猪 (zhū) - Lợn/Heo - Pig</summary>
    Pig = 11
}

/// <summary>
/// Extension methods for Zodiac Animals
/// </summary>
public static class ZodiacAnimalExtensions
{
    /// <summary>
    /// Get the Chinese character for this zodiac animal
    /// </summary>
    public static string GetChineseCharacter(this ZodiacAnimal animal)
    {
        return animal switch
        {
            ZodiacAnimal.Rat => "鼠",
            ZodiacAnimal.Ox => "牛",
            ZodiacAnimal.Tiger => "虎",
            ZodiacAnimal.Rabbit => "兔",
            ZodiacAnimal.Dragon => "龙",
            ZodiacAnimal.Snake => "蛇",
            ZodiacAnimal.Horse => "马",
            ZodiacAnimal.Goat => "羊",
            ZodiacAnimal.Monkey => "猴",
            ZodiacAnimal.Rooster => "鸡",
            ZodiacAnimal.Dog => "狗",
            ZodiacAnimal.Pig => "猪",
            _ => throw new ArgumentOutOfRangeException(nameof(animal))
        };
    }
    
    /// <summary>
    /// Get the Earthly Branch associated with this zodiac animal
    /// </summary>
    public static EarthlyBranch GetEarthlyBranch(this ZodiacAnimal animal)
    {
        return (EarthlyBranch)((int)animal);
    }
}
