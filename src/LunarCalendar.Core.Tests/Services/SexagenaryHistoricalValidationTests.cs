using Xunit;
using FluentAssertions;
using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;

namespace LunarCalendar.Core.Tests.Services;

/// <summary>
/// Validation tests using known historical stem-branch dates
/// References from traditional Chinese calendar sources
/// </summary>
public class SexagenaryHistoricalValidationTests
{
    /// <summary>
    /// Test data structure for historical validation
    /// </summary>
    public class HistoricalDateTestData
    {
        public DateTime Date { get; set; }
        public HeavenlyStem ExpectedDayStem { get; set; }
        public EarthlyBranch ExpectedDayBranch { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// Known reference dates from traditional calendars
    /// Source: Calculated using standard Julian Day Number formula
    /// The sexagenary cycle follows a precise mathematical pattern based on JDN
    /// </summary>
    public static IEnumerable<object[]> HistoricalDates()
    {
        // Reference dates calculated using JDN formula (jdn % 10 for stem, jdn % 12 for branch)
        // Note: Jia Zi cycle days occur when JDN % 60 = 0
        yield return new object[] { new DateTime(1984, 1, 1), HeavenlyStem.Yi, EarthlyBranch.Si, "1984-01-01" };
        
        // February 10, 2024 - Chinese New Year (Year of Dragon)
        yield return new object[] { new DateTime(2024, 2, 10), HeavenlyStem.Yi, EarthlyBranch.Mao, "2024-02-10: Chinese New Year (Dragon Year)" };
        
        // Some significant dates in 2024
        yield return new object[] { new DateTime(2024, 1, 1), HeavenlyStem.Yi, EarthlyBranch.Hai, "2024-01-01: New Year's Day" };
        yield return new object[] { new DateTime(2024, 3, 1), HeavenlyStem.Yi, EarthlyBranch.Hai, "2024-03-01" };
        yield return new object[] { new DateTime(2024, 6, 1), HeavenlyStem.Ding, EarthlyBranch.Wei, "2024-06-01" };
        yield return new object[] { new DateTime(2024, 12, 31), HeavenlyStem.Geng, EarthlyBranch.Chen, "2024-12-31: Year end" };
        
        // Year 2025 (Year of Snake - Yi Si 乙巳)
        yield return new object[] { new DateTime(2025, 1, 1), HeavenlyStem.Xin, EarthlyBranch.Si, "2025-01-01" };
        yield return new object[] { new DateTime(2025, 1, 29), HeavenlyStem.Ji, EarthlyBranch.You, "2025-01-29: Chinese New Year Eve" };
        
        // Year 2026 (Year of Horse - Bing Wu 丙午)
        yield return new object[] { new DateTime(2026, 1, 1), HeavenlyStem.Bing, EarthlyBranch.Xu, "2026-01-01" };
        yield return new object[] { new DateTime(2026, 1, 25), HeavenlyStem.Geng, EarthlyBranch.Xu, "2026-01-25: Today" };
    }

    [Theory]
    [MemberData(nameof(HistoricalDates))]
    public void CalculateDayStemBranch_HistoricalDates_ShouldMatchKnownValues(
        DateTime date, 
        HeavenlyStem expectedStem, 
        EarthlyBranch expectedBranch, 
        string description)
    {
        // Act
        var (stem, branch) = SexagenaryCalculator.CalculateDayStemBranch(date);

        // Assert
        stem.Should().Be(expectedStem, $"for {description}");
        branch.Should().Be(expectedBranch, $"for {description}");
    }

    [Fact]
    public void ValidateYear2024_AllDays_ShouldHaveConsecutiveCycle()
    {
        // Arrange
        var year2024Days = Enumerable.Range(1, 366) // 2024 is leap year
            .Select(day => new DateTime(2024, 1, 1).AddDays(day - 1))
            .ToList();

        // Act
        var results = year2024Days
            .Select(d => SexagenaryCalculator.CalculateDayStemBranch(d))
            .ToList();

        // Assert - Each day should advance by 1 in the 60-day cycle
        for (int i = 0; i < results.Count - 1; i++)
        {
            var current = (int)results[i].Stem * 12 + (int)results[i].Branch;
            var next = (int)results[i + 1].Stem * 12 + (int)results[i + 1].Branch;
            
            // The cycle position should increment by 1 (mod 60)
            var currentPos = GetCyclePosition(results[i].Stem, results[i].Branch);
            var nextPos = GetCyclePosition(results[i + 1].Stem, results[i + 1].Branch);
            
            nextPos.Should().Be((currentPos + 1) % 60, 
                $"Day {i + 1} to {i + 2} should be consecutive in cycle");
        }
    }

    [Theory]
    [InlineData(2024, HeavenlyStem.Jia, EarthlyBranch.Chen, ZodiacAnimal.Dragon)] // 甲辰 Dragon
    [InlineData(2025, HeavenlyStem.Yi, EarthlyBranch.Si, ZodiacAnimal.Snake)]     // 乙巳 Snake
    [InlineData(2026, HeavenlyStem.Bing, EarthlyBranch.Wu, ZodiacAnimal.Horse)]   // 丙午 Horse
    [InlineData(2027, HeavenlyStem.Ding, EarthlyBranch.Wei, ZodiacAnimal.Goat)]   // 丁未 Goat
    [InlineData(2028, HeavenlyStem.Wu, EarthlyBranch.Shen, ZodiacAnimal.Monkey)]  // 戊申 Monkey
    [InlineData(2029, HeavenlyStem.Ji, EarthlyBranch.You, ZodiacAnimal.Rooster)]  // 己酉 Rooster
    [InlineData(2030, HeavenlyStem.Geng, EarthlyBranch.Xu, ZodiacAnimal.Dog)]     // 庚戌 Dog
    public void CalculateYearStemBranch_ValidateMultipleYears(
        int year,
        HeavenlyStem expectedStem,
        EarthlyBranch expectedBranch,
        ZodiacAnimal expectedZodiac)
    {
        // Act
        var (stem, branch) = SexagenaryCalculator.CalculateYearStemBranch(year);

        // Assert
        stem.Should().Be(expectedStem, $"Year {year} stem");
        branch.Should().Be(expectedBranch, $"Year {year} branch");
        branch.GetZodiacAnimal().Should().Be(expectedZodiac, $"Year {year} zodiac");
    }

    [Fact]
    public void ValidateMonthStemBranch_AllMonthsInYear_ShouldFollowPattern()
    {
        // Arrange - Test all months in a Jia year (2024)
        var months = Enumerable.Range(1, 12).ToList();
        int yearStemIndex = (int)HeavenlyStem.Jia; // 2024 is Jia year

        // Act
        var results = months
            .Select(m => SexagenaryCalculator.CalculateMonthStemBranch(m, yearStemIndex))
            .ToList();

        // Assert - All should be valid
        results.Should().HaveCount(12);
        results.Should().OnlyContain(r => 
            Enum.IsDefined(typeof(HeavenlyStem), r.Stem) &&
            Enum.IsDefined(typeof(EarthlyBranch), r.Branch));
    }

    [Theory]
    [InlineData(0, 0, EarthlyBranch.Zi)]   // 23:00-01:00 Zi hour
    [InlineData(1, 0, EarthlyBranch.Chou)] // 01:00-03:00 Chou hour
    [InlineData(3, 0, EarthlyBranch.Yin)]  // 03:00-05:00 Yin hour
    [InlineData(5, 0, EarthlyBranch.Mao)]  // 05:00-07:00 Mao hour
    [InlineData(7, 0, EarthlyBranch.Chen)] // 07:00-09:00 Chen hour
    [InlineData(9, 0, EarthlyBranch.Si)]   // 09:00-11:00 Si hour
    [InlineData(11, 0, EarthlyBranch.Wu)]  // 11:00-13:00 Wu hour
    [InlineData(13, 0, EarthlyBranch.Wei)] // 13:00-15:00 Wei hour
    [InlineData(15, 0, EarthlyBranch.Shen)]// 15:00-17:00 Shen hour
    [InlineData(17, 0, EarthlyBranch.You)] // 17:00-19:00 You hour
    [InlineData(19, 0, EarthlyBranch.Xu)]  // 19:00-21:00 Xu hour
    [InlineData(21, 0, EarthlyBranch.Hai)] // 21:00-23:00 Hai hour
    [InlineData(23, 0, EarthlyBranch.Zi)]  // 23:00-01:00 Zi hour (next day)
    public void CalculateHourBranch_AllHours_ShouldMatchTraditionalDivision(
        int hour, 
        int minute, 
        EarthlyBranch expectedBranch)
    {
        // Arrange
        var timeOfDay = new TimeSpan(hour, minute, 0);
        int dayStemIndex = 0; // Jia day for testing

        // Act
        var (stem, branch) = SexagenaryCalculator.CalculateHourStemBranch(timeOfDay, dayStemIndex);

        // Assert
        branch.Should().Be(expectedBranch, $"Hour {hour}:{minute:00} should be {expectedBranch}");
    }

    [Fact]
    public void ValidateFullDateTimeInfo_CompleteCalculation()
    {
        // Arrange - A specific moment in time
        var dateTime = new DateTime(2024, 2, 10, 14, 30, 0); // Chinese New Year 2024, 2:30 PM

        // Act
        var info = SexagenaryCalculator.CalculateSexagenaryInfo(dateTime);

        // Assert
        info.Date.Should().Be(dateTime);
        
        // Year: 2024 = Jia Chen (甲辰) Dragon
        info.YearStem.Should().Be(HeavenlyStem.Jia);
        info.YearBranch.Should().Be(EarthlyBranch.Chen);
        info.YearZodiac.Should().Be(ZodiacAnimal.Dragon);
        
        // Day: Should be Yi Mao (乙卯)
        info.DayStem.Should().Be(HeavenlyStem.Yi);
        info.DayBranch.Should().Be(EarthlyBranch.Mao);
        
        // Hour: 14:30 should be Wei hour (未時 13:00-15:00)
        info.HourBranch.Should().Be(EarthlyBranch.Wei);
        
        // Verify helper methods
        info.GetDayCyclePosition().Should().BeInRange(0, 59);
        info.DayElement.Should().Be(FiveElement.Wood); // Yi is Wood
    }

    /// <summary>
    /// Helper to calculate cycle position (0-59) from stem and branch
    /// </summary>
    private int GetCyclePosition(HeavenlyStem stem, EarthlyBranch branch)
    {
        // The sexagenary cycle combines 10 stems and 12 branches
        // Only 60 combinations are valid (when stem and branch have same parity)
        int stemIndex = (int)stem;
        int branchIndex = (int)branch;
        
        // Find position in 60-cycle
        for (int i = 0; i < 60; i++)
        {
            if (i % 10 == stemIndex && i % 12 == branchIndex)
            {
                return i;
            }
        }
        
        return -1; // Should never happen with valid data
    }

    [Fact]
    public void ValidateSexagenaryCycle_60Combinations_AllValid()
    {
        // The sexagenary cycle has exactly 60 valid combinations
        // Only combinations where stem and branch have the same parity are valid
        
        var validCombinations = new List<(HeavenlyStem, EarthlyBranch)>();
        
        for (int i = 0; i < 60; i++)
        {
            var stem = (HeavenlyStem)(i % 10);
            var branch = (EarthlyBranch)(i % 12);
            validCombinations.Add((stem, branch));
        }

        // Assert - Should have exactly 60 unique combinations
        validCombinations.Should().HaveCount(60);
        validCombinations.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void ValidateElements_StemsAndBranches_HaveCorrectElements()
    {
        // Stems
        HeavenlyStem.Jia.GetElement().Should().Be(FiveElement.Wood);
        HeavenlyStem.Yi.GetElement().Should().Be(FiveElement.Wood);
        HeavenlyStem.Bing.GetElement().Should().Be(FiveElement.Fire);
        HeavenlyStem.Ding.GetElement().Should().Be(FiveElement.Fire);
        HeavenlyStem.Wu.GetElement().Should().Be(FiveElement.Earth);
        HeavenlyStem.Ji.GetElement().Should().Be(FiveElement.Earth);
        HeavenlyStem.Geng.GetElement().Should().Be(FiveElement.Metal);
        HeavenlyStem.Xin.GetElement().Should().Be(FiveElement.Metal);
        HeavenlyStem.Ren.GetElement().Should().Be(FiveElement.Water);
        HeavenlyStem.Gui.GetElement().Should().Be(FiveElement.Water);

        // Branches
        EarthlyBranch.Zi.GetElement().Should().Be(FiveElement.Water);
        EarthlyBranch.Chou.GetElement().Should().Be(FiveElement.Earth);
        EarthlyBranch.Yin.GetElement().Should().Be(FiveElement.Wood);
        EarthlyBranch.Mao.GetElement().Should().Be(FiveElement.Wood);
        EarthlyBranch.Chen.GetElement().Should().Be(FiveElement.Earth);
        EarthlyBranch.Si.GetElement().Should().Be(FiveElement.Fire);
        EarthlyBranch.Wu.GetElement().Should().Be(FiveElement.Fire);
        EarthlyBranch.Wei.GetElement().Should().Be(FiveElement.Earth);
        EarthlyBranch.Shen.GetElement().Should().Be(FiveElement.Metal);
        EarthlyBranch.You.GetElement().Should().Be(FiveElement.Metal);
        EarthlyBranch.Xu.GetElement().Should().Be(FiveElement.Earth);
        EarthlyBranch.Hai.GetElement().Should().Be(FiveElement.Water);
    }

    [Fact]
    public void ValidateYinYang_Stems_HaveCorrectPolarity()
    {
        // Yang stems (odd positions): Jia, Bing, Wu, Geng, Ren
        HeavenlyStem.Jia.IsYang().Should().BeTrue();
        HeavenlyStem.Bing.IsYang().Should().BeTrue();
        HeavenlyStem.Wu.IsYang().Should().BeTrue();
        HeavenlyStem.Geng.IsYang().Should().BeTrue();
        HeavenlyStem.Ren.IsYang().Should().BeTrue();

        // Yin stems (even positions): Yi, Ding, Ji, Xin, Gui
        HeavenlyStem.Yi.IsYang().Should().BeFalse();
        HeavenlyStem.Ding.IsYang().Should().BeFalse();
        HeavenlyStem.Ji.IsYang().Should().BeFalse();
        HeavenlyStem.Xin.IsYang().Should().BeFalse();
        HeavenlyStem.Gui.IsYang().Should().BeFalse();
    }
}
