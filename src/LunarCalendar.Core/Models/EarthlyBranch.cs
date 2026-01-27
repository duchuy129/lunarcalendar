namespace LunarCalendar.Core.Models;

/// <summary>
/// The 12 Earthly Branches (地支 dìzhī / Địa Chi) used in the Sexagenary Cycle.
/// Each branch is associated with a zodiac animal, element, and two-hour time period.
/// </summary>
public enum EarthlyBranch
{
    /// <summary>子 (zǐ) - Tý - Rat - 23:00-01:00</summary>
    Zi = 0,
    
    /// <summary>丑 (chǒu) - Sửu - Ox - 01:00-03:00</summary>
    Chou = 1,
    
    /// <summary>寅 (yín) - Dần - Tiger - 03:00-05:00</summary>
    Yin = 2,
    
    /// <summary>卯 (mǎo) - Mão - Rabbit - 05:00-07:00</summary>
    Mao = 3,
    
    /// <summary>辰 (chén) - Thìn - Dragon - 07:00-09:00</summary>
    Chen = 4,
    
    /// <summary>巳 (sì) - Tỵ - Snake - 09:00-11:00</summary>
    Si = 5,
    
    /// <summary>午 (wǔ) - Ngọ - Horse - 11:00-13:00</summary>
    Wu = 6,
    
    /// <summary>未 (wèi) - Mùi - Goat - 13:00-15:00</summary>
    Wei = 7,
    
    /// <summary>申 (shēn) - Thân - Monkey - 15:00-17:00</summary>
    Shen = 8,
    
    /// <summary>酉 (yǒu) - Dậu - Rooster - 17:00-19:00</summary>
    You = 9,
    
    /// <summary>戌 (xū) - Tuất - Dog - 19:00-21:00</summary>
    Xu = 10,
    
    /// <summary>亥 (hài) - Hợi - Pig - 21:00-23:00</summary>
    Hai = 11
}

/// <summary>
/// Extension methods and metadata for Earthly Branches
/// </summary>
public static class EarthlyBranchExtensions
{
    /// <summary>
    /// Get the zodiac animal associated with this Earthly Branch
    /// </summary>
    public static ZodiacAnimal GetZodiacAnimal(this EarthlyBranch branch)
    {
        return (ZodiacAnimal)((int)branch);
    }
    
    /// <summary>
    /// Get the Five Element associated with this Earthly Branch
    /// </summary>
    public static FiveElement GetElement(this EarthlyBranch branch)
    {
        return branch switch
        {
            EarthlyBranch.Zi => FiveElement.Water,
            EarthlyBranch.Chou => FiveElement.Earth,
            EarthlyBranch.Yin => FiveElement.Wood,
            EarthlyBranch.Mao => FiveElement.Wood,
            EarthlyBranch.Chen => FiveElement.Earth,
            EarthlyBranch.Si => FiveElement.Fire,
            EarthlyBranch.Wu => FiveElement.Fire,
            EarthlyBranch.Wei => FiveElement.Earth,
            EarthlyBranch.Shen => FiveElement.Metal,
            EarthlyBranch.You => FiveElement.Metal,
            EarthlyBranch.Xu => FiveElement.Earth,
            EarthlyBranch.Hai => FiveElement.Water,
            _ => throw new ArgumentOutOfRangeException(nameof(branch))
        };
    }
    
    /// <summary>
    /// Get the Chinese character for this branch
    /// </summary>
    public static string GetChineseCharacter(this EarthlyBranch branch)
    {
        return branch switch
        {
            EarthlyBranch.Zi => "子",
            EarthlyBranch.Chou => "丑",
            EarthlyBranch.Yin => "寅",
            EarthlyBranch.Mao => "卯",
            EarthlyBranch.Chen => "辰",
            EarthlyBranch.Si => "巳",
            EarthlyBranch.Wu => "午",
            EarthlyBranch.Wei => "未",
            EarthlyBranch.Shen => "申",
            EarthlyBranch.You => "酉",
            EarthlyBranch.Xu => "戌",
            EarthlyBranch.Hai => "亥",
            _ => throw new ArgumentOutOfRangeException(nameof(branch))
        };
    }
    
    /// <summary>
    /// Get the time period (hour range) for this branch
    /// </summary>
    /// <returns>Tuple of (startHour, endHour) in 24-hour format</returns>
    public static (int StartHour, int EndHour) GetTimePeriod(this EarthlyBranch branch)
    {
        int branchIndex = (int)branch;
        int startHour = (branchIndex * 2 - 1 + 24) % 24; // -1 because Zi starts at 23:00
        int endHour = (startHour + 2) % 24;
        return (startHour, endHour);
    }
}
