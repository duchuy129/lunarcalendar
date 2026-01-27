using Xunit;
using FluentAssertions;
using LunarCalendar.Core.Models;
using LunarCalendar.Core.Services;

namespace LunarCalendar.Core.Tests.Services;

/// <summary>
/// Unit tests for SexagenaryService to validate caching behavior
/// </summary>
public class SexagenaryServiceTests
{
    private readonly ISexagenaryService _service;

    public SexagenaryServiceTests()
    {
        _service = new SexagenaryService();
    }

    [Fact]
    public void GetSexagenaryInfo_ShouldReturnValidInfo()
    {
        // Arrange
        var date = new DateTime(2024, 1, 1);

        // Act
        var info = _service.GetSexagenaryInfo(date);

        // Assert
        info.Should().NotBeNull();
        info.Date.Should().Be(date);
        info.DayStem.Should().BeOneOf(Enum.GetValues<HeavenlyStem>());
        info.DayBranch.Should().BeOneOf(Enum.GetValues<EarthlyBranch>());
    }

    [Fact]
    public void GetTodaySexagenaryInfo_ShouldReturnInfoForToday()
    {
        // Act
        var info = _service.GetTodaySexagenaryInfo();

        // Assert
        info.Should().NotBeNull();
        info.Date.Date.Should().Be(DateTime.Today);
    }

    [Fact]
    public void GetSexagenaryInfo_CalledTwiceWithSameDate_ShouldReturnSameInstance()
    {
        // Arrange
        var date = new DateTime(2024, 6, 15);

        // Act
        var info1 = _service.GetSexagenaryInfo(date);
        var info2 = _service.GetSexagenaryInfo(date);

        // Assert - Should return cached instance
        info1.Should().BeSameAs(info2);
    }

    [Fact]
    public void GetDayStemBranch_ShouldReturnCorrectValues()
    {
        // Arrange
        var date = new DateTime(2024, 1, 1);

        // Act
        var (stem, branch) = _service.GetDayStemBranch(date);

        // Assert
        stem.Should().BeOneOf(Enum.GetValues<HeavenlyStem>());
        branch.Should().BeOneOf(Enum.GetValues<EarthlyBranch>());
    }

    [Fact]
    public void GetYearInfo_ShouldReturnYearStemBranchAndZodiac()
    {
        // Arrange
        int lunarYear = 2024;

        // Act
        var (stem, branch, zodiac) = _service.GetYearInfo(lunarYear);

        // Assert
        stem.Should().Be(HeavenlyStem.Jia);
        branch.Should().Be(EarthlyBranch.Chen);
        zodiac.Should().Be(ZodiacAnimal.Dragon);
    }

    [Fact]
    public void GetAllHourStemBranches_ShouldReturn12Hours()
    {
        // Arrange
        var date = new DateTime(2024, 1, 1);

        // Act
        var hours = _service.GetAllHourStemBranches(date);

        // Assert
        hours.Should().HaveCount(12);
        hours.Select(h => h.Branch).Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void GetSexagenaryInfoRange_ShouldReturnCorrectCount()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 1, 10);

        // Act
        var infos = _service.GetSexagenaryInfoRange(startDate, endDate);

        // Assert - endDate is exclusive (standard C# range convention)
        infos.Should().HaveCount(9); // Jan 1-9 = 9 days
        infos.Keys.First().Should().Be(startDate);
        infos.Keys.Last().Should().Be(endDate.AddDays(-1));
    }

    [Fact]
    public void GetSexagenaryInfoRange_OneMonth_ShouldReturnAllDays()
    {
        // Arrange
        var startDate = new DateTime(2024, 2, 1);
        var endDate = new DateTime(2024, 3, 1); // Feb 2024 has 29 days (leap year)

        // Act
        var infos = _service.GetSexagenaryInfoRange(startDate, endDate);

        // Assert
        infos.Should().HaveCount(29);
    }

    [Fact]
    public void ClearCache_ShouldRemoveAllCachedEntries()
    {
        // Arrange
        var date = new DateTime(2024, 1, 1);
        var info1 = _service.GetSexagenaryInfo(date);

        // Act
        _service.ClearCache();
        var info2 = _service.GetSexagenaryInfo(date);

        // Assert - After clearing cache, should get new instance
        info1.Should().NotBeSameAs(info2);
    }

    [Fact]
    public void GetSexagenaryInfo_WithTime_ShouldIncludeHourInfo()
    {
        // Arrange
        var dateTime = new DateTime(2024, 1, 1, 14, 30, 0);

        // Act
        var info = _service.GetSexagenaryInfo(dateTime);

        // Assert
        info.HourStem.Should().NotBeNull();
        info.HourBranch.Should().NotBeNull();
    }

    [Fact]
    public void GetSexagenaryInfo_MultipleCallsForMonth_ShouldBeCached()
    {
        // Arrange
        var dates = Enumerable.Range(1, 31)
            .Select(day => new DateTime(2024, 1, day))
            .ToList();

        // Act - First pass: populate cache
        var firstPassInfos = dates.Select(d => _service.GetSexagenaryInfo(d)).ToList();
        
        // Second pass: should return cached instances
        var secondPassInfos = dates.Select(d => _service.GetSexagenaryInfo(d)).ToList();

        // Assert
        for (int i = 0; i < dates.Count; i++)
        {
            firstPassInfos[i].Should().BeSameAs(secondPassInfos[i]);
        }
    }
}
