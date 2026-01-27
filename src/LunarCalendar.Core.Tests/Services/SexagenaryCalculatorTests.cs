using Xunit;
using FluentAssertions;
using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;

namespace LunarCalendar.Core.Tests.Services;

/// <summary>
/// Unit tests for SexagenaryCalculator to validate calculation accuracy
/// Tests against known historical dates
/// </summary>
public class SexagenaryCalculatorTests
{
    // ========================================
    // Day Stem-Branch Calculation Tests
    // Reference: Vietnamese Lunar Calendar
    // ========================================
    
    [Theory]
    [InlineData(2026, 1, 25, HeavenlyStem.Ji, EarthlyBranch.Hai)] // Verified: Kỷ Hợi
    [InlineData(2026, 1, 26, HeavenlyStem.Geng, EarthlyBranch.Zi)] // Verified: Canh Tý
    public void CalculateDayStemBranch_KnownDates_ShouldMatchReference(
        int year, int month, int day, 
        HeavenlyStem expectedStem, EarthlyBranch expectedBranch)
    {
        // Arrange
        var date = new DateTime(year, month, day);

        // Act
        var (stem, branch) = SexagenaryCalculator.CalculateDayStemBranch(date);

        // Assert
        stem.Should().Be(expectedStem, 
            $"Day stem for {date:yyyy-MM-dd} should be {expectedStem}");
        branch.Should().Be(expectedBranch, 
            $"Day branch for {date:yyyy-MM-dd} should be {expectedBranch}");
    }
    
    [Fact]
    public void CalculateDayStemBranch_ReturnsValidValues()
    {
        // Test various dates return valid stem-branch values
        var testDates = new[] {
            new DateTime(2024, 1, 1),
            new DateTime(2024, 2, 10),
            new DateTime(2024, 6, 15),
            new DateTime(2024, 12, 31),
            new DateTime(2025, 1, 29),
            new DateTime(2026, 2, 17)
        };
        
        foreach (var date in testDates)
        {
            var (stem, branch) = SexagenaryCalculator.CalculateDayStemBranch(date);
            
            stem.Should().BeOneOf(Enum.GetValues<HeavenlyStem>());
            branch.Should().BeOneOf(Enum.GetValues<EarthlyBranch>());
        }
    }
    
    [Fact]
    public void CalculateDayStemBranch_ConsecutiveDays_ShouldFollowCycle()
    {
        // Arrange - Test that consecutive days follow the 60-day cycle
        var startDate = new DateTime(2026, 1, 25); // Kỷ Hợi
        
        // Act & Assert - Next day should be Canh Tý
        var day1 = SexagenaryCalculator.CalculateDayStemBranch(startDate);
        var day2 = SexagenaryCalculator.CalculateDayStemBranch(startDate.AddDays(1));
        
        // Verify stem advances by 1 (Kỷ=5 -> Canh=6)
        day1.Stem.Should().Be(HeavenlyStem.Ji);
        day2.Stem.Should().Be(HeavenlyStem.Geng);
        
        // Verify branch advances by 1 (Hợi=11 -> Tý=0)
        day1.Branch.Should().Be(EarthlyBranch.Hai);
        day2.Branch.Should().Be(EarthlyBranch.Zi);
    }
    
    [Fact]
    public void CalculateDayStemBranch_60DaysCycle_ShouldRepeat()
    {
        // Arrange - Test that stem-branch repeats after 60 days
        var startDate = new DateTime(2026, 1, 1);
        
        // Act
        var original = SexagenaryCalculator.CalculateDayStemBranch(startDate);
        var after60Days = SexagenaryCalculator.CalculateDayStemBranch(startDate.AddDays(60));
        
        // Assert - Should be identical after one complete cycle
        after60Days.Stem.Should().Be(original.Stem);
        after60Days.Branch.Should().Be(original.Branch);
    }

    // ========================================
    // Year Stem-Branch Calculation Tests
    // ========================================
    
    [Theory]
    [InlineData(2024, HeavenlyStem.Jia, EarthlyBranch.Chen, ZodiacAnimal.Dragon)] // 甲辰 - Wood Dragon
    [InlineData(2025, HeavenlyStem.Yi, EarthlyBranch.Si, ZodiacAnimal.Snake)] // 乙巳 - Wood Snake
    [InlineData(2026, HeavenlyStem.Bing, EarthlyBranch.Wu, ZodiacAnimal.Horse)] // 丙午 - Fire Horse
    [InlineData(2027, HeavenlyStem.Ding, EarthlyBranch.Wei, ZodiacAnimal.Goat)] // 丁未 - Fire Goat
    [InlineData(2028, HeavenlyStem.Wu, EarthlyBranch.Shen, ZodiacAnimal.Monkey)] // 戊申 - Earth Monkey
    [InlineData(2023, HeavenlyStem.Gui, EarthlyBranch.Mao, ZodiacAnimal.Rabbit)] // 癸卯 - Water Rabbit
    [InlineData(2020, HeavenlyStem.Geng, EarthlyBranch.Zi, ZodiacAnimal.Rat)] // 庚子 - Metal Rat
    [InlineData(2019, HeavenlyStem.Ji, EarthlyBranch.Hai, ZodiacAnimal.Pig)] // 己亥 - Earth Pig
    [InlineData(2000, HeavenlyStem.Geng, EarthlyBranch.Chen, ZodiacAnimal.Dragon)] // 庚辰 - Metal Dragon
    [InlineData(1984, HeavenlyStem.Jia, EarthlyBranch.Zi, ZodiacAnimal.Rat)] // 甲子 - Wood Rat (cycle start)
    public void CalculateYearStemBranch_KnownYears_ShouldMatchReference(
        int year, HeavenlyStem expectedStem, EarthlyBranch expectedBranch, ZodiacAnimal expectedZodiac)
    {
        // Act
        var (stem, branch) = SexagenaryCalculator.CalculateYearStemBranch(year);
        var zodiac = branch.GetZodiacAnimal();

        // Assert
        stem.Should().Be(expectedStem, $"Year {year} stem should be {expectedStem}");
        branch.Should().Be(expectedBranch, $"Year {year} branch should be {expectedBranch}");
        zodiac.Should().Be(expectedZodiac, $"Year {year} zodiac should be {expectedZodiac}");
    }

    [Fact]
    public void CalculateYearStemBranch_60YearCycle_ShouldRepeat()
    {
        // Arrange - Test that year stem-branch repeats after 60 years
        int baseYear = 1984; // Giáp Tý (Jia Zi) - First in cycle

        // Act
        var original = SexagenaryCalculator.CalculateYearStemBranch(baseYear);
        var after60Years = SexagenaryCalculator.CalculateYearStemBranch(baseYear + 60);

        // Assert - Should be identical after one complete cycle
        after60Years.Stem.Should().Be(original.Stem);
        after60Years.Branch.Should().Be(original.Branch);
    }

    [Fact]
    public void CalculateYearStemBranch_ConsecutiveYears_ShouldFollowPattern()
    {
        // Arrange
        var year2024 = SexagenaryCalculator.CalculateYearStemBranch(2024);
        var year2025 = SexagenaryCalculator.CalculateYearStemBranch(2025);

        // Assert - Both stem and branch should advance by 1
        year2024.Stem.Should().Be(HeavenlyStem.Jia); // Giáp
        year2025.Stem.Should().Be(HeavenlyStem.Yi);  // Ất
        
        year2024.Branch.Should().Be(EarthlyBranch.Chen); // Thìn (Dragon)
        year2025.Branch.Should().Be(EarthlyBranch.Si);   // Tỵ (Snake)
    }

    // ========================================
    // Month Stem-Branch Calculation Tests
    // ========================================
    
    [Fact]
    public void CalculateMonthStemBranch_ReturnsValidValues()
    {
        // Test that month calculation returns valid stem-branch for various inputs
        for (int month = 1; month <= 12; month++)
        {
            for (int yearStem = 0; yearStem < 10; yearStem++)
            {
                var (stem, branch) = SexagenaryCalculator.CalculateMonthStemBranch(month, yearStem);
                
                stem.Should().BeOneOf(Enum.GetValues<HeavenlyStem>());
                branch.Should().BeOneOf(Enum.GetValues<EarthlyBranch>());
            }
        }
    }
    
    [Fact]
    public void CalculateMonthStemBranch_BranchFollowsPattern()
    {
        // Month branch should follow: month 1 = Yin (2), month 2 = Mao (3), etc.
        var (_, branch1) = SexagenaryCalculator.CalculateMonthStemBranch(1, 0);
        var (_, branch2) = SexagenaryCalculator.CalculateMonthStemBranch(2, 0);
        var (_, branch12) = SexagenaryCalculator.CalculateMonthStemBranch(12, 0);
        
        branch1.Should().Be(EarthlyBranch.Yin);
        branch2.Should().Be(EarthlyBranch.Mao);
        branch12.Should().Be(EarthlyBranch.Chou);
    }

    [Fact]
    public void CalculateMonthStemBranch_InvalidMonth_ShouldThrow()
    {
        // Act & Assert
        var actZero = () => SexagenaryCalculator.CalculateMonthStemBranch(0, 0);
        actZero.Should().Throw<ArgumentOutOfRangeException>();
        
        var actTooHigh = () => SexagenaryCalculator.CalculateMonthStemBranch(14, 0);
        actTooHigh.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void CalculateMonthStemBranch_InvalidYearStem_ShouldThrow()
    {
        // Act & Assert
        var actNegative = () => SexagenaryCalculator.CalculateMonthStemBranch(1, -1);
        actNegative.Should().Throw<ArgumentOutOfRangeException>();
        
        var actTooHigh = () => SexagenaryCalculator.CalculateMonthStemBranch(1, 10);
        actTooHigh.Should().Throw<ArgumentOutOfRangeException>();
    }

    // ========================================
    // Hour Stem-Branch Calculation Tests
    // ========================================
    
    [Theory]
    [InlineData(0, 0, 0, HeavenlyStem.Jia, EarthlyBranch.Zi)] // Midnight, Jia day
    [InlineData(1, 0, 0, HeavenlyStem.Yi, EarthlyBranch.Chou)] // 1 AM, Jia day
    [InlineData(12, 0, 0, HeavenlyStem.Geng, EarthlyBranch.Wu)] // Noon, Jia day
    [InlineData(23, 0, 0, HeavenlyStem.Jia, EarthlyBranch.Zi)] // 11 PM, Jia day (new day starts)
    public void CalculateHourStemBranch_VariousTimes_ShouldMatchFormula(
        int hour, int minute, int dayStemIndex, 
        HeavenlyStem expectedStem, EarthlyBranch expectedBranch)
    {
        // Arrange
        var timeOfDay = new TimeSpan(hour, minute, 0);

        // Act
        var (stem, branch) = SexagenaryCalculator.CalculateHourStemBranch(timeOfDay, dayStemIndex);

        // Assert
        stem.Should().Be(expectedStem, $"Hour {hour}:00 on day stem {dayStemIndex} should have stem {expectedStem}");
        branch.Should().Be(expectedBranch, $"Hour {hour}:00 should have branch {expectedBranch}");
    }

    [Fact]
    public void CalculateHourStemBranch_InvalidDayStem_ShouldThrow()
    {
        // Arrange
        var timeOfDay = new TimeSpan(12, 0, 0);

        // Act & Assert
        var actNegative = () => SexagenaryCalculator.CalculateHourStemBranch(timeOfDay, -1);
        actNegative.Should().Throw<ArgumentOutOfRangeException>();
        
        var actTooHigh = () => SexagenaryCalculator.CalculateHourStemBranch(timeOfDay, 10);
        actTooHigh.Should().Throw<ArgumentOutOfRangeException>();
    }

    // ========================================
    // Complete Sexagenary Info Tests
    // ========================================

    [Fact]
    public void CalculateSexagenaryInfo_CompleteInfo_ShouldIncludeAllComponents()
    {
        // Arrange
        var date = new DateTime(2024, 2, 10, 14, 30, 0); // Lunar New Year 2024 at 2:30 PM

        // Act
        var info = SexagenaryCalculator.CalculateSexagenaryInfo(date);

        // Assert - All components should be populated
        info.Date.Should().Be(date);
        info.YearStem.Should().Be(HeavenlyStem.Jia); // 2024 = Giáp Thìn
        info.YearBranch.Should().Be(EarthlyBranch.Chen);
        info.DayStem.Should().BeOneOf(Enum.GetValues<HeavenlyStem>());
        info.DayBranch.Should().BeOneOf(Enum.GetValues<EarthlyBranch>());
        info.MonthStem.Should().BeOneOf(Enum.GetValues<HeavenlyStem>());
        info.MonthBranch.Should().BeOneOf(Enum.GetValues<EarthlyBranch>());
        info.HourStem.Should().BeOneOf(Enum.GetValues<HeavenlyStem>());
        info.HourBranch.Should().BeOneOf(Enum.GetValues<EarthlyBranch>());
        info.LunarYear.Should().Be(2024);
    }
    
    [Fact]
    public void CalculateSexagenaryInfo_WithoutTime_ShouldDefaultToMidnight()
    {
        // Arrange - Date without specific time
        var date = new DateTime(2026, 1, 25);

        // Act
        var info = SexagenaryCalculator.CalculateSexagenaryInfo(date);

        // Assert - Hour should be calculated for midnight (00:00)
        info.HourStem.Should().BeOneOf(Enum.GetValues<HeavenlyStem>());
        info.HourBranch.Should().Be(EarthlyBranch.Zi); // Midnight = Zi hour
    }

    // ========================================
    // All Hour Calculation Tests
    // ========================================

    [Fact]
    public void CalculateAllHourStemBranches_ShouldReturn12Hours()
    {
        // Arrange
        var date = new DateTime(2026, 1, 25);

        // Act
        var hours = SexagenaryCalculator.CalculateAllHourStemBranches(date);

        // Assert
        hours.Should().HaveCount(12);
        
        // Verify each hour has valid stem-branch
        foreach (var hour in hours)
        {
            hour.Stem.Should().BeOneOf(Enum.GetValues<HeavenlyStem>());
            hour.Branch.Should().BeOneOf(Enum.GetValues<EarthlyBranch>());
            hour.StartHour.Should().BeInRange(0, 23);
            hour.EndHour.Should().BeInRange(0, 24);
        }
        
        // Verify branches are in order
        for (int i = 0; i < 12; i++)
        {
            hours[i].Branch.Should().Be((EarthlyBranch)i);
        }
    }

    [Fact]
    public void CalculateAllHourStemBranches_CurrentHourFlag_ShouldBeSet()
    {
        // Arrange
        var date = new DateTime(2026, 1, 25, 14, 30, 0); // 2:30 PM

        // Act
        var hours = SexagenaryCalculator.CalculateAllHourStemBranches(date);

        // Assert - Only one hour should be marked as current
        var currentHours = hours.Where(h => h.IsCurrentHour).ToList();
        currentHours.Should().HaveCount(1);
        
        // 14:00 falls in Wu hour (11:00-13:00 becomes 13:00-15:00 after adjustment)
        var currentHour = currentHours.First();
        currentHour.Branch.Should().Be(EarthlyBranch.Wei); // 13:00-15:00
    }

    // ========================================
    // Edge Cases and Validation Tests
    // ========================================

    [Theory]
    [InlineData(1900)] // Too early
    [InlineData(2101)] // Too late
    public void CalculateSexagenaryInfo_OutOfRange_ShouldThrow(int year)
    {
        // Arrange
        var date = new DateTime(year, 1, 1);

        // Act & Assert
        var act = () => SexagenaryCalculator.CalculateSexagenaryInfo(date);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*1901*2100*");
    }
    
    [Fact]
    public void CalculateSexagenaryInfo_BoundaryDates_ShouldWork()
    {
        // Arrange & Act - Chinese lunisolar calendar support range: 1901-02-19 to 2101-01-28
        var minDate = SexagenaryCalculator.CalculateSexagenaryInfo(new DateTime(1901, 2, 19));
        var maxDate = SexagenaryCalculator.CalculateSexagenaryInfo(new DateTime(2100, 12, 31));

        // Assert
        minDate.Should().NotBeNull();
        maxDate.Should().NotBeNull();
        minDate.YearStem.Should().BeOneOf(Enum.GetValues<HeavenlyStem>());
        maxDate.YearStem.Should().BeOneOf(Enum.GetValues<HeavenlyStem>());
    }

    [Fact]
    public void CalculateDayStemBranch_LeapYear_ShouldWork()
    {
        // Arrange - February 29, 2024 (leap year)
        var date = new DateTime(2024, 2, 29);

        // Act
        var (stem, branch) = SexagenaryCalculator.CalculateDayStemBranch(date);

        // Assert
        stem.Should().BeOneOf(Enum.GetValues<HeavenlyStem>());
        branch.Should().BeOneOf(Enum.GetValues<EarthlyBranch>());
    }

    // ========================================
    // Extension Method Tests
    // ========================================

    [Fact]
    public void GetDayCyclePosition_ShouldReturnValidPosition()
    {
        // Arrange
        var info = SexagenaryCalculator.CalculateSexagenaryInfo(new DateTime(2024, 1, 1));

        // Act
        int position = info.GetDayCyclePosition();

        // Assert - Position should be 0-59 (60-day cycle)
        position.Should().BeInRange(0, 59);
    }

    [Fact]
    public void GetYearZodiac_2024_ShouldBeDragon()
    {
        // Arrange
        var info = SexagenaryCalculator.CalculateSexagenaryInfo(new DateTime(2024, 2, 10));

        // Act
        var zodiac = info.YearZodiac;

        // Assert
        zodiac.Should().Be(ZodiacAnimal.Dragon);
    }

    [Fact]
    public void GetDayElement_ShouldMatchStem()
    {
        // Arrange
        var info = SexagenaryCalculator.CalculateSexagenaryInfo(new DateTime(2024, 1, 1));

        // Act
        var dayElement = info.DayElement;
        var stemElement = info.DayStem.GetElement();

        // Assert
        dayElement.Should().Be(stemElement);
    }

    [Fact]
    public void GetChineseString_ShouldReturnTwoCharacters()
    {
        // Arrange
        var info = SexagenaryCalculator.CalculateSexagenaryInfo(new DateTime(2024, 1, 1));

        // Act
        var dayString = info.GetDayChineseString();
        var yearString = info.GetYearChineseString();
        var monthString = info.GetMonthChineseString();

        // Assert
        dayString.Should().HaveLength(2); // e.g., "辛亥"
        yearString.Should().HaveLength(2); // e.g., "甲辰"
        monthString.Should().HaveLength(2); // e.g., "丙寅"
    }
    
    [Fact]
    public void GetZodiacAnimal_AllBranches_ShouldReturnCorrectAnimal()
    {
        // Test all 12 branches map to correct zodiac animals
        EarthlyBranch.Zi.GetZodiacAnimal().Should().Be(ZodiacAnimal.Rat);
        EarthlyBranch.Chou.GetZodiacAnimal().Should().Be(ZodiacAnimal.Ox);
        EarthlyBranch.Yin.GetZodiacAnimal().Should().Be(ZodiacAnimal.Tiger);
        EarthlyBranch.Mao.GetZodiacAnimal().Should().Be(ZodiacAnimal.Rabbit);
        EarthlyBranch.Chen.GetZodiacAnimal().Should().Be(ZodiacAnimal.Dragon);
        EarthlyBranch.Si.GetZodiacAnimal().Should().Be(ZodiacAnimal.Snake);
        EarthlyBranch.Wu.GetZodiacAnimal().Should().Be(ZodiacAnimal.Horse);
        EarthlyBranch.Wei.GetZodiacAnimal().Should().Be(ZodiacAnimal.Goat);
        EarthlyBranch.Shen.GetZodiacAnimal().Should().Be(ZodiacAnimal.Monkey);
        EarthlyBranch.You.GetZodiacAnimal().Should().Be(ZodiacAnimal.Rooster);
        EarthlyBranch.Xu.GetZodiacAnimal().Should().Be(ZodiacAnimal.Dog);
        EarthlyBranch.Hai.GetZodiacAnimal().Should().Be(ZodiacAnimal.Pig);
    }
    
    [Fact]
    public void GetElement_AllStems_ShouldReturnCorrectElement()
    {
        // Test all 10 stems map to correct elements
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
    }
}
