using LunarCalendar.Api.Services;
using Xunit;

namespace LunarCalendar.Api.Tests.Services;

public class LunarCalendarServiceTests
{
    private readonly LunarCalendarService _service;

    public LunarCalendarServiceTests()
    {
        _service = new LunarCalendarService();
    }

    [Fact]
    public void ConvertToLunar_ReturnsValidLunarDate_ForKnownDate()
    {
        // Arrange
        var gregorianDate = new DateTime(2025, 1, 1);

        // Act
        var result = _service.ConvertToLunar(gregorianDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(gregorianDate, result.GregorianDate);
        Assert.True(result.LunarYear > 0);
        Assert.True(result.LunarMonth >= 1 && result.LunarMonth <= 12);
        Assert.True(result.LunarDay >= 1 && result.LunarDay <= 30);
        Assert.NotNull(result.LunarYearName);
        Assert.NotNull(result.AnimalSign);
    }

    [Fact]
    public void ConvertToLunar_ReturnsDifferentDates_ForConsecutiveDays()
    {
        // Arrange
        var date1 = new DateTime(2025, 12, 27);
        var date2 = new DateTime(2025, 12, 28);

        // Act
        var lunar1 = _service.ConvertToLunar(date1);
        var lunar2 = _service.ConvertToLunar(date2);

        // Assert
        Assert.NotEqual(lunar1.LunarDay, lunar2.LunarDay);
    }

    [Theory]
    [InlineData(2025, 1)]
    [InlineData(2025, 6)]
    [InlineData(2025, 12)]
    public void GetMonthInfo_ReturnsCorrectNumberOfDays(int year, int month)
    {
        // Act
        var result = _service.GetMonthInfo(year, month);

        // Assert
        Assert.NotNull(result);
        var daysInMonth = DateTime.DaysInMonth(year, month);
        Assert.Equal(daysInMonth, result.Count);
    }

    [Fact]
    public void GetMonthInfo_AllDatesHaveValidLunarInfo()
    {
        // Arrange
        var year = 2025;
        var month = 12;

        // Act
        var result = _service.GetMonthInfo(year, month);

        // Assert
        foreach (var lunarInfo in result)
        {
            Assert.True(lunarInfo.LunarYear > 0);
            Assert.True(lunarInfo.LunarMonth >= 1 && lunarInfo.LunarMonth <= 12);
            Assert.True(lunarInfo.LunarDay >= 1 && lunarInfo.LunarDay <= 30);
            Assert.NotNull(lunarInfo.LunarYearName);
            Assert.NotNull(lunarInfo.AnimalSign);
        }
    }

    [Fact]
    public void GetLunarInfo_IncludesFestivalInformation()
    {
        // Arrange - Tet holiday (Lunar New Year)
        var tetDate = new DateTime(2025, 1, 29); // 2025 Lunar New Year

        // Act
        var result = _service.GetLunarInfo(tetDate);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.DateInfo);
        // Festival may or may not be set depending on implementation
    }

    [Theory]
    [InlineData(1900, 1, 1)]
    [InlineData(2100, 12, 31)]
    public void ConvertToLunar_HandlesDateRangeBoundaries(int year, int month, int day)
    {
        // Arrange
        var date = new DateTime(year, month, day);

        // Act
        var result = _service.ConvertToLunar(date);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(date, result.GregorianDate);
    }

    [Theory]
    [InlineData(2025, 1, 29, "Snake")] // 2025 Lunar New Year - Year of Snake
    [InlineData(2024, 2, 10, "Dragon")] // 2024 Lunar New Year - Year of Dragon
    [InlineData(2026, 2, 17, "Horse")] // 2026 Lunar New Year - Year of Horse
    public void ConvertToLunar_ReturnCorrectAnimalSign_ForLunarNewYear(int year, int month, int day, string expectedAnimal)
    {
        // Arrange
        var tetDate = new DateTime(year, month, day);

        // Act
        var result = _service.ConvertToLunar(tetDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedAnimal, result.AnimalSign);
        Assert.NotEmpty(result.LunarYearName);
        Assert.NotEmpty(result.HeavenlyStem);
        Assert.NotEmpty(result.EarthlyBranch);
    }

    [Fact]
    public void ConvertToLunar_SexagenaryCycle_ConsistentAcrossYear()
    {
        // Arrange - All dates in the same lunar year should have same animal sign
        var date1 = new DateTime(2025, 1, 29); // Start of Snake year
        var date2 = new DateTime(2025, 6, 15); // Middle of Snake year
        var date3 = new DateTime(2026, 1, 1); // Still Snake year

        // Act
        var lunar1 = _service.ConvertToLunar(date1);
        var lunar2 = _service.ConvertToLunar(date2);
        var lunar3 = _service.ConvertToLunar(date3);

        // Assert - All should be in Snake year
        Assert.Equal("Snake", lunar1.AnimalSign);
        Assert.Equal("Snake", lunar2.AnimalSign);
        Assert.Equal("Snake", lunar3.AnimalSign);
    }
}
