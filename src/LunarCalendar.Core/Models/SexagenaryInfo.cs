using System;
using System.Collections.Generic;

namespace LunarCalendar.Core.Models;

/// <summary>
/// Complete Sexagenary (Can Chi / 干支) information for a specific date and time.
/// Includes year, month, day, and hour stem-branch pairs along with derived information.
/// </summary>
public class SexagenaryInfo
{
    /// <summary>
    /// The date this information applies to
    /// </summary>
    public DateTime Date { get; set; }
    
    // ===== Year Information =====
    
    /// <summary>
    /// Lunar year number (e.g., 2024)
    /// </summary>
    public int LunarYear { get; set; }
    
    /// <summary>
    /// Heavenly Stem for the year
    /// </summary>
    public HeavenlyStem YearStem { get; set; }
    
    /// <summary>
    /// Earthly Branch for the year
    /// </summary>
    public EarthlyBranch YearBranch { get; set; }
    
    /// <summary>
    /// Zodiac animal for the year
    /// </summary>
    public ZodiacAnimal YearZodiac => YearBranch.GetZodiacAnimal();
    
    /// <summary>
    /// Five Element for the year stem
    /// </summary>
    public FiveElement YearElement => YearStem.GetElement();
    
    // ===== Month Information =====
    
    /// <summary>
    /// Lunar month number (1-12, or 13 for leap month)
    /// </summary>
    public int LunarMonth { get; set; }
    
    /// <summary>
    /// Whether this is a leap month
    /// </summary>
    public bool IsLeapMonth { get; set; }
    
    /// <summary>
    /// Heavenly Stem for the month
    /// </summary>
    public HeavenlyStem MonthStem { get; set; }
    
    /// <summary>
    /// Earthly Branch for the month
    /// </summary>
    public EarthlyBranch MonthBranch { get; set; }
    
    /// <summary>
    /// Five Element for the month stem
    /// </summary>
    public FiveElement MonthElement => MonthStem.GetElement();
    
    // ===== Day Information =====
    
    /// <summary>
    /// Heavenly Stem for the day
    /// </summary>
    public HeavenlyStem DayStem { get; set; }
    
    /// <summary>
    /// Earthly Branch for the day
    /// </summary>
    public EarthlyBranch DayBranch { get; set; }
    
    /// <summary>
    /// Five Element for the day stem
    /// </summary>
    public FiveElement DayElement => DayStem.GetElement();
    
    /// <summary>
    /// Yin or Yang polarity for the day
    /// </summary>
    public bool IsDayYang => DayStem.IsYang();
    
    // ===== Hour Information (optional) =====
    
    /// <summary>
    /// Heavenly Stem for the current hour (if time is specified)
    /// </summary>
    public HeavenlyStem? HourStem { get; set; }
    
    /// <summary>
    /// Earthly Branch for the current hour (if time is specified)
    /// </summary>
    public EarthlyBranch? HourBranch { get; set; }
    
    /// <summary>
    /// Five Element for the hour stem (if time is specified)
    /// </summary>
    public FiveElement? HourElement => HourStem?.GetElement();
    
    // ===== Formatted Strings =====
    
    /// <summary>
    /// Get year stem-branch in Chinese characters (e.g., "甲辰")
    /// </summary>
    public string GetYearChineseString()
    {
        return $"{YearStem.GetChineseCharacter()}{YearBranch.GetChineseCharacter()}";
    }
    
    /// <summary>
    /// Get month stem-branch in Chinese characters (e.g., "丙寅")
    /// </summary>
    public string GetMonthChineseString()
    {
        return $"{MonthStem.GetChineseCharacter()}{MonthBranch.GetChineseCharacter()}";
    }
    
    /// <summary>
    /// Get day stem-branch in Chinese characters (e.g., "壬戌")
    /// </summary>
    public string GetDayChineseString()
    {
        return $"{DayStem.GetChineseCharacter()}{DayBranch.GetChineseCharacter()}";
    }
    
    /// <summary>
    /// Get hour stem-branch in Chinese characters (e.g., "辛未")
    /// </summary>
    public string GetHourChineseString()
    {
        if (HourStem == null || HourBranch == null)
            return string.Empty;
        
        return $"{HourStem.Value.GetChineseCharacter()}{HourBranch.Value.GetChineseCharacter()}";
    }
    
    /// <summary>
    /// Get full stem-branch representation
    /// Year-Month-Day-Hour in Chinese characters
    /// </summary>
    public string GetFullChineseString()
    {
        var parts = new List<string>
        {
            GetYearChineseString(),
            GetMonthChineseString(),
            GetDayChineseString()
        };
        
        if (HourStem != null && HourBranch != null)
        {
            parts.Add(GetHourChineseString());
        }
        
        return string.Join(" ", parts);
    }
    
    /// <summary>
    /// Get the 60-cycle position for the day (0-59)
    /// </summary>
    public int GetDayCyclePosition()
    {
        // Use Chinese Remainder Theorem to find position in 60-cycle
        // Position = (10 * dayBranch + 6 * dayStem) mod 60
        int stemIndex = (int)DayStem;
        int branchIndex = (int)DayBranch;
        return (stemIndex * 6 + branchIndex * 10) % 60;
    }
    
    public override string ToString()
    {
        return $"Date: {Date:yyyy-MM-dd}, " +
               $"Year: {GetYearChineseString()} ({YearZodiac}), " +
               $"Day: {GetDayChineseString()}, " +
               $"Element: {DayElement}";
    }
}
