using LunarCalendar.Core.Models;

namespace LunarCalendar.Core.Services;

/// <summary>
/// Service interface for Sexagenary Cycle (Can Chi / 干支) calculations.
/// Provides caching and higher-level operations on top of the pure calculator.
/// </summary>
public interface ISexagenaryService
{
    /// <summary>
    /// Get complete sexagenary information for a specific date
    /// </summary>
    /// <param name="date">The date to get information for</param>
    /// <returns>Complete sexagenary information</returns>
    SexagenaryInfo GetSexagenaryInfo(DateTime date);
    
    /// <summary>
    /// Get sexagenary information for today
    /// </summary>
    /// <returns>Complete sexagenary information for current date</returns>
    SexagenaryInfo GetTodaySexagenaryInfo();
    
    /// <summary>
    /// Get day stem-branch for a specific date
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns>Day stem-branch pair</returns>
    (HeavenlyStem Stem, EarthlyBranch Branch) GetDayStemBranch(DateTime date);
    
    /// <summary>
    /// Get year information for a specific lunar year
    /// </summary>
    /// <param name="lunarYear">The lunar year</param>
    /// <returns>Year stem-branch and zodiac information</returns>
    (HeavenlyStem Stem, EarthlyBranch Branch, ZodiacAnimal Zodiac) GetYearInfo(int lunarYear);
    
    /// <summary>
    /// Get all 12 hour stem-branches for a specific day
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns>Array of 12 hour periods with stem-branches</returns>
    HourStemBranchInfo[] GetAllHourStemBranches(DateTime date);
    
    /// <summary>
    /// Get sexagenary information for a range of dates (e.g., a month)
    /// This is optimized with batch processing and caching
    /// </summary>
    /// <param name="startDate">Start date of range</param>
    /// <param name="endDate">End date of range</param>
    /// <returns>Dictionary of dates to sexagenary info</returns>
    IDictionary<DateTime, SexagenaryInfo> GetSexagenaryInfoRange(DateTime startDate, DateTime endDate);
    
    /// <summary>
    /// Clear the cache (useful for testing or memory management)
    /// </summary>
    void ClearCache();
}
