namespace LunarCalendar.Core.Models;

/// <summary>
/// The 10 Heavenly Stems (天干 tiāngān / Thiên Can) used in the Sexagenary Cycle.
/// Each stem is associated with one of the Five Elements and has a Yin or Yang polarity.
/// </summary>
public enum HeavenlyStem
{
    /// <summary>甲 (jiǎ) - Giáp - Wood (Yang)</summary>
    Jia = 0,
    
    /// <summary>乙 (yǐ) - Ất - Wood (Yin)</summary>
    Yi = 1,
    
    /// <summary>丙 (bǐng) - Bính - Fire (Yang)</summary>
    Bing = 2,
    
    /// <summary>丁 (dīng) - Đinh - Fire (Yin)</summary>
    Ding = 3,
    
    /// <summary>戊 (wù) - Mậu - Earth (Yang)</summary>
    Wu = 4,
    
    /// <summary>己 (jǐ) - Kỷ - Earth (Yin)</summary>
    Ji = 5,
    
    /// <summary>庚 (gēng) - Canh - Metal (Yang)</summary>
    Geng = 6,
    
    /// <summary>辛 (xīn) - Tân - Metal (Yin)</summary>
    Xin = 7,
    
    /// <summary>壬 (rén) - Nhâm - Water (Yang)</summary>
    Ren = 8,
    
    /// <summary>癸 (guǐ) - Quý - Water (Yin)</summary>
    Gui = 9
}

/// <summary>
/// Extension methods and metadata for Heavenly Stems
/// </summary>
public static class HeavenlyStemExtensions
{
    /// <summary>
    /// Get the Five Element associated with this Heavenly Stem
    /// </summary>
    public static FiveElement GetElement(this HeavenlyStem stem)
    {
        return stem switch
        {
            HeavenlyStem.Jia => FiveElement.Wood,
            HeavenlyStem.Yi => FiveElement.Wood,
            HeavenlyStem.Bing => FiveElement.Fire,
            HeavenlyStem.Ding => FiveElement.Fire,
            HeavenlyStem.Wu => FiveElement.Earth,
            HeavenlyStem.Ji => FiveElement.Earth,
            HeavenlyStem.Geng => FiveElement.Metal,
            HeavenlyStem.Xin => FiveElement.Metal,
            HeavenlyStem.Ren => FiveElement.Water,
            HeavenlyStem.Gui => FiveElement.Water,
            _ => throw new ArgumentOutOfRangeException(nameof(stem))
        };
    }
    
    /// <summary>
    /// Get the Yin/Yang polarity of this Heavenly Stem
    /// </summary>
    public static bool IsYang(this HeavenlyStem stem)
    {
        // Even indices (0, 2, 4, 6, 8) are Yang
        return ((int)stem) % 2 == 0;
    }
    
    /// <summary>
    /// Get the Chinese character for this stem
    /// </summary>
    public static string GetChineseCharacter(this HeavenlyStem stem)
    {
        return stem switch
        {
            HeavenlyStem.Jia => "甲",
            HeavenlyStem.Yi => "乙",
            HeavenlyStem.Bing => "丙",
            HeavenlyStem.Ding => "丁",
            HeavenlyStem.Wu => "戊",
            HeavenlyStem.Ji => "己",
            HeavenlyStem.Geng => "庚",
            HeavenlyStem.Xin => "辛",
            HeavenlyStem.Ren => "壬",
            HeavenlyStem.Gui => "癸",
            _ => throw new ArgumentOutOfRangeException(nameof(stem))
        };
    }
}
