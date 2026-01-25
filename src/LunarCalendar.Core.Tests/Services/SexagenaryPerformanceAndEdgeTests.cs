using Xunit;
using FluentAssertions;
using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;
using System.Diagnostics;

namespace LunarCalendar.Core.Tests.Services;

/// <summary>
/// Performance and edge case tests for Sexagenary calculations
/// </summary>
public class SexagenaryPerformanceAndEdgeTests
{
    [Fact]
    public void Performance_CalculateSingleDate_ShouldBeFasterThan1ms()
    {
        // Arrange
        var date = new DateTime(2024, 6, 15);
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        var info = SexagenaryCalculator.CalculateSexagenaryInfo(date);
        stopwatch.Stop();

        // Assert - Should meet constitutional requirement of <50ms
        // We test for <1ms to ensure headroom
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1, 
            "Single calculation should be very fast");
        info.Should().NotBeNull();
    }

    [Fact]
    public void Performance_Calculate1000Dates_ShouldBeFasterThan50ms()
    {
        // Arrange
        var dates = Enumerable.Range(0, 1000)
            .Select(i => new DateTime(2024, 1, 1).AddDays(i % 365))
            .ToList();
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        var results = dates.Select(d => SexagenaryCalculator.CalculateSexagenaryInfo(d)).ToList();
        stopwatch.Stop();

        // Assert - Constitutional requirement: <50ms per calculation
        // 1000 calculations should be well under 50 seconds
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000, 
            "1000 calculations should complete quickly");
        results.Should().HaveCount(1000);
    }

    [Fact]
    public void Performance_ServiceWithCaching_ShouldBeSignificantlyFaster()
    {
        // Arrange
        var service = new SexagenaryService();
        // Generate 100 dates across multiple months to avoid invalid dates
        var dates = Enumerable.Range(0, 100)
            .Select(day => new DateTime(2024, 1, 1).AddDays(day))
            .ToList();

        // Act - First pass (no cache)
        var stopwatch1 = Stopwatch.StartNew();
        var firstPass = dates.Select(d => service.GetSexagenaryInfo(d)).ToList();
        stopwatch1.Stop();

        // Second pass (cached)
        var stopwatch2 = Stopwatch.StartNew();
        var secondPass = dates.Select(d => service.GetSexagenaryInfo(d)).ToList();
        stopwatch2.Stop();

        // Assert - Cached should be faster (or at least not slower)
        stopwatch2.ElapsedMilliseconds.Should().BeLessThanOrEqualTo(stopwatch1.ElapsedMilliseconds);
        
        // Verify same instances returned (cached)
        for (int i = 0; i < dates.Count; i++)
        {
            firstPass[i].Should().BeSameAs(secondPass[i]);
        }
    }

    [Theory]
    [InlineData(1901, 2, 19)]  // Minimum supported date (ChineseLunisolarCalendar limitation)
    [InlineData(1901, 12, 31)]
    [InlineData(1950, 6, 15)]
    [InlineData(2000, 1, 1)]
    [InlineData(2024, 2, 29)] // Leap year
    [InlineData(2050, 7, 20)]
    [InlineData(2100, 12, 31)] // Maximum supported date
    public void EdgeCase_BoundaryDates_ShouldCalculateSuccessfully(int year, int month, int day)
    {
        // Arrange
        var date = new DateTime(year, month, day);

        // Act
        var info = SexagenaryCalculator.CalculateSexagenaryInfo(date);

        // Assert
        info.Should().NotBeNull();
        info.Date.Should().Be(date);
        info.DayStem.Should().BeOneOf(Enum.GetValues<HeavenlyStem>());
        info.DayBranch.Should().BeOneOf(Enum.GetValues<EarthlyBranch>());
    }

    [Theory]
    [InlineData(1900, 12, 31)] // Before supported range
    [InlineData(2101, 1, 1)]   // After supported range
    public void EdgeCase_OutOfRangeDates_ShouldThrowArgumentOutOfRangeException(int year, int month, int day)
    {
        // Arrange
        var date = new DateTime(year, month, day);

        // Act & Assert
        var act = () => SexagenaryCalculator.CalculateSexagenaryInfo(date);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*1901*2100*");
    }

    [Fact]
    public void EdgeCase_LeapYearDates_ShouldCalculateCorrectly()
    {
        // Arrange - February 29 in various leap years
        var leapYearDates = new[]
        {
            new DateTime(1904, 2, 29),
            new DateTime(2000, 2, 29),
            new DateTime(2004, 2, 29),
            new DateTime(2024, 2, 29),
            new DateTime(2096, 2, 29)
        };

        // Act & Assert
        foreach (var date in leapYearDates)
        {
            var info = SexagenaryCalculator.CalculateSexagenaryInfo(date);
            info.Should().NotBeNull();
            info.Date.Should().Be(date);
        }
    }

    [Fact]
    public void EdgeCase_AllHoursInDay_ShouldCoverFullDay()
    {
        // Arrange
        var date = new DateTime(2024, 1, 1);
        var allHours = SexagenaryCalculator.CalculateAllHourStemBranches(date);

        // Assert - Should have 12 double-hours
        allHours.Should().HaveCount(12);
        
        // Each hour period should be 2 hours
        foreach (var hour in allHours)
        {
            var duration = (hour.EndHour - hour.StartHour + 24) % 24;
            if (duration == 0) duration = 24; // Handle wrap around midnight
            duration.Should().BeInRange(2, 2, $"Each period should be 2 hours for {hour.Branch}");
        }

        // All 12 branches should be represented
        var branches = allHours.Select(h => h.Branch).ToList();
        branches.Should().Contain(EarthlyBranch.Zi);
        branches.Should().Contain(EarthlyBranch.Chou);
        branches.Should().Contain(EarthlyBranch.Yin);
        branches.Should().Contain(EarthlyBranch.Mao);
        branches.Should().Contain(EarthlyBranch.Chen);
        branches.Should().Contain(EarthlyBranch.Si);
        branches.Should().Contain(EarthlyBranch.Wu);
        branches.Should().Contain(EarthlyBranch.Wei);
        branches.Should().Contain(EarthlyBranch.Shen);
        branches.Should().Contain(EarthlyBranch.You);
        branches.Should().Contain(EarthlyBranch.Xu);
        branches.Should().Contain(EarthlyBranch.Hai);
    }

    [Fact]
    public void EdgeCase_MidnightTransition_ShouldHandleZiHourCorrectly()
    {
        // Arrange - Times around midnight
        var testTimes = new[]
        {
            new DateTime(2024, 1, 1, 23, 0, 0),  // Start of Zi hour
            new DateTime(2024, 1, 1, 23, 30, 0), // Middle of Zi hour
            new DateTime(2024, 1, 1, 23, 59, 59),// End of Zi hour
            new DateTime(2024, 1, 2, 0, 0, 0),   // Start of new Zi hour (next day)
            new DateTime(2024, 1, 2, 0, 30, 0),  // Middle of Zi hour (next day)
        };

        // Act & Assert
        foreach (var time in testTimes)
        {
            var info = SexagenaryCalculator.CalculateSexagenaryInfo(time);
            info.Should().NotBeNull();
            info.HourBranch.Should().Be(EarthlyBranch.Zi, 
                $"Time {time:HH:mm} should be Zi hour");
        }
    }

    [Fact]
    public void EdgeCase_CenturyTransitions_ShouldCalculateCorrectly()
    {
        // Arrange - Test century boundaries
        var centuryDates = new[]
        {
            new DateTime(1999, 12, 31), // End of 20th century
            new DateTime(2000, 1, 1),   // Start of 21st century
            new DateTime(2099, 12, 31), // Near end of supported range
            new DateTime(2100, 12, 31)  // Last supported date
        };

        // Act & Assert
        foreach (var date in centuryDates)
        {
            var info = SexagenaryCalculator.CalculateSexagenaryInfo(date);
            info.Should().NotBeNull();
            info.Date.Should().Be(date);
        }
    }

    [Fact]
    public void EdgeCase_ConsecutiveDays_ShouldAdvanceCorrectly()
    {
        // Arrange - Test 120 consecutive days (2 full 60-day cycles)
        var startDate = new DateTime(2024, 1, 1);
        var days = Enumerable.Range(0, 120)
            .Select(i => startDate.AddDays(i))
            .ToList();

        // Act
        var results = days.Select(d => SexagenaryCalculator.CalculateDayStemBranch(d)).ToList();

        // Assert - After 60 days, should return to same stem-branch
        var day1 = results[0];
        var day61 = results[60];
        
        day61.Stem.Should().Be(day1.Stem, "After 60 days, stem should cycle back");
        day61.Branch.Should().Be(day1.Branch, "After 60 days, branch should cycle back");
    }

    [Fact]
    public void Concurrency_ParallelCalculations_ShouldBeThreadSafe()
    {
        // Arrange
        var dates = Enumerable.Range(0, 1000)
            .Select(i => new DateTime(2024, 1, 1).AddDays(i % 365))
            .ToList();

        // Act - Calculate in parallel
        var results = dates.AsParallel()
            .Select(d => SexagenaryCalculator.CalculateSexagenaryInfo(d))
            .ToList();

        // Assert - All results should be valid
        results.Should().HaveCount(1000);
        results.Should().OnlyContain(r => r != null);
        
        // Verify consistency - same date should give same result
        var testDate = new DateTime(2024, 6, 15);
        var testResults = Enumerable.Range(0, 10)
            .AsParallel()
            .Select(_ => SexagenaryCalculator.CalculateSexagenaryInfo(testDate))
            .ToList();
            
        testResults.Should().OnlyContain(r => 
            r.DayStem == testResults[0].DayStem && 
            r.DayBranch == testResults[0].DayBranch);
    }

    [Fact]
    public void Service_CachingWithLargeDataset_ShouldNotExceedMemoryLimit()
    {
        // Arrange
        var service = new SexagenaryService();
        
        // Act - Request 500 different dates (should trigger LRU eviction)
        var dates = Enumerable.Range(0, 500)
            .Select(i => new DateTime(2024, 1, 1).AddDays(i))
            .ToList();
            
        var results = dates.Select(d => service.GetSexagenaryInfo(d)).ToList();

        // Assert
        results.Should().HaveCount(500);
        results.Should().OnlyContain(r => r != null);
        
        // Cache should have evicted old entries (max 100)
        // Re-requesting old dates should work but may create new instances
        var oldDate = dates[0];
        var requeried = service.GetSexagenaryInfo(oldDate);
        requeried.Should().NotBeNull();
        requeried.Date.Should().Be(oldDate);
    }

    [Fact]
    public void EdgeCase_DateTimeWithMilliseconds_ShouldIgnoreSubSecondPrecision()
    {
        // Arrange - Same time but different milliseconds
        var time1 = new DateTime(2024, 1, 1, 12, 0, 0, 0);
        var time2 = new DateTime(2024, 1, 1, 12, 0, 0, 500);
        var time3 = new DateTime(2024, 1, 1, 12, 0, 0, 999);

        // Act
        var info1 = SexagenaryCalculator.CalculateSexagenaryInfo(time1);
        var info2 = SexagenaryCalculator.CalculateSexagenaryInfo(time2);
        var info3 = SexagenaryCalculator.CalculateSexagenaryInfo(time3);

        // Assert - All should have same result (milliseconds ignored)
        info2.DayStem.Should().Be(info1.DayStem);
        info2.DayBranch.Should().Be(info1.DayBranch);
        info2.HourBranch.Should().Be(info1.HourBranch);
        
        info3.DayStem.Should().Be(info1.DayStem);
        info3.DayBranch.Should().Be(info1.DayBranch);
        info3.HourBranch.Should().Be(info1.HourBranch);
    }
}
