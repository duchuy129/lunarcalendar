using System.Globalization;
using LunarCalendar.Core.Models;

namespace LunarCalendar.Core.Services;

/// <summary>
/// Pure calculation engine for Sexagenary Cycle (Can Chi / 干支) conversions.
/// Contains the mathematical algorithms to calculate stem-branch pairs for dates and times.
/// All methods are static and stateless for easy testing.
/// </summary>
public static class SexagenaryCalculator
{
    private static readonly ChineseLunisolarCalendar LunarCalendar = new ChineseLunisolarCalendar();
    
    // Reference date: January 1, 2000 = 壬戌 (Ren Xu / Nhâm Tuất / Water Dog)
    // JDN for Jan 1, 2000 = 2,451,545
    private const int ReferenceJdn = 2451545;
    private const int ReferenceDayStem = 8; // Ren (Nhâm)
    private const int ReferenceDayBranch = 10; // Xu (Tuất)
    
    /// <summary>
    /// Calculate the complete sexagenary information for a given date
    /// </summary>
    /// <param name="date">The Gregorian date to calculate</param>
    /// <returns>Complete sexagenary information including year, month, day, and hour</returns>
    /// <exception cref="ArgumentOutOfRangeException">If date is outside 1901-2100 range</exception>
    public static SexagenaryInfo CalculateSexagenaryInfo(DateTime date)
    {
        ValidateDateRange(date);
        
        var info = new SexagenaryInfo
        {
            Date = date
        };
        
        // Calculate lunar date information
        int lunarYear = LunarCalendar.GetYear(date);
        int lunarMonth = LunarCalendar.GetMonth(date);
        int lunarDay = LunarCalendar.GetDayOfMonth(date);
        int leapMonth = LunarCalendar.GetLeapMonth(lunarYear);
        
        info.LunarYear = lunarYear;
        info.LunarMonth = lunarMonth;
        info.IsLeapMonth = (leapMonth > 0 && lunarMonth == leapMonth);
        
        // Calculate day stem-branch
        var (dayStem, dayBranch) = CalculateDayStemBranch(date);
        info.DayStem = dayStem;
        info.DayBranch = dayBranch;
        
        // Calculate year stem-branch
        var (yearStem, yearBranch) = CalculateYearStemBranch(lunarYear);
        info.YearStem = yearStem;
        info.YearBranch = yearBranch;
        
        // Calculate month stem-branch
        var (monthStem, monthBranch) = CalculateMonthStemBranch(lunarMonth, (int)yearStem);
        info.MonthStem = monthStem;
        info.MonthBranch = monthBranch;
        
        // Always calculate hour stem-branch (time defaults to midnight if not specified)
        var (hourStem, hourBranch) = CalculateHourStemBranch(date.TimeOfDay, (int)dayStem);
        info.HourStem = hourStem;
        info.HourBranch = hourBranch;
        
        return info;
    }
    
    /// <summary>
    /// Calculate the day stem-branch for a given date
    /// </summary>
    /// <param name="date">The date to calculate</param>
    /// <returns>Tuple of (Heavenly Stem, Earthly Branch) for the day</returns>
    public static (HeavenlyStem Stem, EarthlyBranch Branch) CalculateDayStemBranch(DateTime date)
    {
        // The day stem-branch follows a continuous 60-day cycle based on the lunar calendar.
        // We use Julian Day Number (JDN) as a continuous day counter.
        //
        // Verified reference points (from Vietnamese lunar calendar):
        // - January 25, 2026 (Gregorian) = Lunar Month 12, Day 7 = Kỷ Hợi (stem=5, branch=11)
        // - January 26, 2026 (Gregorian) = Lunar Month 12, Day 8 = Canh Tý (stem=6, branch=0)
        //
        // Through empirical testing, the correct formula is:
        //   Stem index = (JDN + 49) % 10
        //   Branch index = (JDN + 1) % 12
        //
        // These offsets align the JDN (which starts from November 24, 4714 BCE) 
        // with the sexagenary cycle used in Vietnamese/Chinese lunar calendars.
        
        long jdn = CalculateJulianDayNumber(date);
        
        // Apply offsets to align with Vietnamese lunar calendar sexagenary cycle
        int stemIndex = (int)((jdn + 49) % 10);
        int branchIndex = (int)((jdn + 1) % 12);
        
        return ((HeavenlyStem)stemIndex, (EarthlyBranch)branchIndex);
    }
    
    /// <summary>
    /// Calculate the year stem-branch for a given lunar year
    /// </summary>
    /// <param name="lunarYear">The lunar year (e.g., 2024)</param>
    /// <returns>Tuple of (Heavenly Stem, Earthly Branch) for the year</returns>
    public static (HeavenlyStem Stem, EarthlyBranch Branch) CalculateYearStemBranch(int lunarYear)
    {
        // Year Stem: (Year + 6) mod 10
        // This formula aligns with historical records
        // Example: 2024 → (2024 + 6) % 10 = 0 (Jia/Giáp)
        int stemIndex = (lunarYear + 6) % 10;
        
        // Year Branch: (Year + 8) mod 12
        // Example: 2024 → (2024 + 8) % 12 = 4 (Chen/Thìn/Dragon)
        int branchIndex = (lunarYear + 8) % 12;
        
        return ((HeavenlyStem)stemIndex, (EarthlyBranch)branchIndex);
    }
    
    /// <summary>
    /// Calculate the month stem-branch for a given lunar month
    /// </summary>
    /// <param name="lunarMonth">The lunar month (1-12)</param>
    /// <param name="yearStemIndex">The year's heavenly stem index (0-9)</param>
    /// <returns>Tuple of (Heavenly Stem, Earthly Branch) for the month</returns>
    public static (HeavenlyStem Stem, EarthlyBranch Branch) CalculateMonthStemBranch(int lunarMonth, int yearStemIndex)
    {
        if (lunarMonth < 1 || lunarMonth > 13)
            throw new ArgumentOutOfRangeException(nameof(lunarMonth), "Lunar month must be between 1 and 13");
        
        if (yearStemIndex < 0 || yearStemIndex > 9)
            throw new ArgumentOutOfRangeException(nameof(yearStemIndex), "Year stem index must be between 0 and 9");
        
        // Month Branch: Starts from Yin (Tiger/2) for month 1
        // Month 1 = Yin (2), Month 2 = Mao (3), Month 3 = Chen (4), etc.
        int branchIndex = (lunarMonth + 1) % 12;
        
        // Month Stem: (2 × Year Stem + Lunar Month) mod 10
        // This traditional formula ties the month stem to the year
        int stemIndex = (2 * yearStemIndex + lunarMonth) % 10;
        
        return ((HeavenlyStem)stemIndex, (EarthlyBranch)branchIndex);
    }
    
    /// <summary>
    /// Calculate the hour stem-branch for a given time
    /// </summary>
    /// <param name="timeOfDay">The time of day</param>
    /// <param name="dayStemIndex">The day's heavenly stem index (0-9)</param>
    /// <returns>Tuple of (Heavenly Stem, Earthly Branch) for the hour</returns>
    public static (HeavenlyStem Stem, EarthlyBranch Branch) CalculateHourStemBranch(TimeSpan timeOfDay, int dayStemIndex)
    {
        if (dayStemIndex < 0 || dayStemIndex > 9)
            throw new ArgumentOutOfRangeException(nameof(dayStemIndex), "Day stem index must be between 0 and 9");
        
        // Calculate hour branch based on time
        // Note: The day starts at 23:00 (Zi hour), not midnight
        int branchIndex = GetHourBranchFromTime(timeOfDay);
        
        // Hour Stem: (2 × Day Stem + Hour Branch) mod 10
        int stemIndex = (2 * dayStemIndex + branchIndex) % 10;
        
        return ((HeavenlyStem)stemIndex, (EarthlyBranch)branchIndex);
    }
    
    /// <summary>
    /// Get all 12 hour stem-branches for a given day
    /// </summary>
    /// <param name="date">The date to calculate hours for</param>
    /// <returns>Array of 12 hour periods with their stem-branches</returns>
    public static HourStemBranchInfo[] CalculateAllHourStemBranches(DateTime date)
    {
        var dayStemBranch = CalculateDayStemBranch(date);
        int dayStemIndex = (int)dayStemBranch.Stem;
        
        var hours = new HourStemBranchInfo[12];
        
        for (int i = 0; i < 12; i++)
        {
            var branch = (EarthlyBranch)i;
            int stemIndex = (2 * dayStemIndex + i) % 10;
            var stem = (HeavenlyStem)stemIndex;
            
            var (startHour, endHour) = branch.GetTimePeriod();
            
            hours[i] = new HourStemBranchInfo
            {
                Branch = branch,
                Stem = stem,
                StartHour = startHour,
                EndHour = endHour,
                IsCurrentHour = IsCurrentHour(date.TimeOfDay, startHour, endHour)
            };
        }
        
        return hours;
    }
    
    /// <summary>
    /// Calculate Julian Day Number for a given date
    /// Used as the basis for day stem-branch calculation
    /// </summary>
    private static int CalculateJulianDayNumber(DateTime date)
    {
        int a = (14 - date.Month) / 12;
        int y = date.Year + 4800 - a;
        int m = date.Month + 12 * a - 3;
        
        int jdn = date.Day 
                  + (153 * m + 2) / 5 
                  + 365 * y 
                  + y / 4 
                  - y / 100 
                  + y / 400 
                  - 32045;
        
        return jdn;
    }
    
    /// <summary>
    /// Get the hour branch (Earthly Branch) from time of day
    /// </summary>
    private static int GetHourBranchFromTime(TimeSpan timeOfDay)
    {
        double totalHours = timeOfDay.TotalHours;
        
        // The day starts at 23:00 (Zi hour), not midnight
        // Adjust the hour to align with traditional Chinese time
        double adjustedHour = totalHours + 1.0;
        if (adjustedHour >= 24.0) adjustedHour -= 24.0;
        
        // Each branch represents 2 hours
        int branchIndex = ((int)adjustedHour / 2) % 12;
        
        return branchIndex;
    }
    
    /// <summary>
    /// Check if a given time falls within a specific hour period
    /// </summary>
    private static bool IsCurrentHour(TimeSpan timeOfDay, int startHour, int endHour)
    {
        int currentHour = timeOfDay.Hours;
        
        // Handle wraparound (e.g., 23:00-01:00)
        if (startHour > endHour)
        {
            return currentHour >= startHour || currentHour < endHour;
        }
        
        return currentHour >= startHour && currentHour < endHour;
    }
    
    /// <summary>
    /// Validate that the date is within the supported range (1901-2100)
    /// </summary>
    private static void ValidateDateRange(DateTime date)
    {
        if (date.Year < 1901 || date.Year > 2100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(date),
                $"Date must be between 1901 and 2100. Provided: {date.Year}");
        }
    }
}

/// <summary>
/// Information about a specific hour period and its stem-branch
/// </summary>
public class HourStemBranchInfo
{
    public HeavenlyStem Stem { get; set; }
    public EarthlyBranch Branch { get; set; }
    public int StartHour { get; set; }
    public int EndHour { get; set; }
    public bool IsCurrentHour { get; set; }
    
    public string GetTimeRange()
    {
        return $"{StartHour:D2}:00 - {EndHour:D2}:00";
    }
}
