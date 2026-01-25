namespace LunarCalendar.Core.Models;

/// <summary>
/// The Five Elements (五行 wǔxíng / Ngũ hành) in Chinese philosophy.
/// These elements interact through generating and overcoming cycles.
/// </summary>
public enum FiveElement
{
    /// <summary>木 (mù) - Mộc - Wood</summary>
    Wood = 0,
    
    /// <summary>火 (huǒ) - Hỏa - Fire</summary>
    Fire = 1,
    
    /// <summary>土 (tǔ) - Thổ - Earth</summary>
    Earth = 2,
    
    /// <summary>金 (jīn) - Kim - Metal</summary>
    Metal = 3,
    
    /// <summary>水 (shuǐ) - Thủy - Water</summary>
    Water = 4
}

/// <summary>
/// Extension methods for Five Elements
/// </summary>
public static class FiveElementExtensions
{
    /// <summary>
    /// Get the Chinese character for this element
    /// </summary>
    public static string GetChineseCharacter(this FiveElement element)
    {
        return element switch
        {
            FiveElement.Wood => "木",
            FiveElement.Fire => "火",
            FiveElement.Earth => "土",
            FiveElement.Metal => "金",
            FiveElement.Water => "水",
            _ => throw new ArgumentOutOfRangeException(nameof(element))
        };
    }
    
    /// <summary>
    /// Get the element that this element generates (producing cycle)
    /// Wood → Fire → Earth → Metal → Water → Wood
    /// </summary>
    public static FiveElement Generates(this FiveElement element)
    {
        return element switch
        {
            FiveElement.Wood => FiveElement.Fire,
            FiveElement.Fire => FiveElement.Earth,
            FiveElement.Earth => FiveElement.Metal,
            FiveElement.Metal => FiveElement.Water,
            FiveElement.Water => FiveElement.Wood,
            _ => throw new ArgumentOutOfRangeException(nameof(element))
        };
    }
    
    /// <summary>
    /// Get the element that this element overcomes (controlling cycle)
    /// Wood → Earth → Water → Fire → Metal → Wood
    /// </summary>
    public static FiveElement Overcomes(this FiveElement element)
    {
        return element switch
        {
            FiveElement.Wood => FiveElement.Earth,
            FiveElement.Fire => FiveElement.Metal,
            FiveElement.Earth => FiveElement.Water,
            FiveElement.Metal => FiveElement.Wood,
            FiveElement.Water => FiveElement.Fire,
            _ => throw new ArgumentOutOfRangeException(nameof(element))
        };
    }
    
    /// <summary>
    /// Get a color commonly associated with this element
    /// </summary>
    public static string GetAssociatedColor(this FiveElement element)
    {
        return element switch
        {
            FiveElement.Wood => "#228B22", // Green
            FiveElement.Fire => "#DC143C", // Red
            FiveElement.Earth => "#D2691E", // Brown/Orange
            FiveElement.Metal => "#C0C0C0", // Silver
            FiveElement.Water => "#4169E1", // Blue
            _ => throw new ArgumentOutOfRangeException(nameof(element))
        };
    }
}
