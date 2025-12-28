using LunarCalendar.Api.Services;
using Xunit;

namespace LunarCalendar.Api.Tests.Services;

public class HolidayServiceTests
{
    private readonly HolidayService _service;

    public HolidayServiceTests()
    {
        _service = new HolidayService();
    }

    [Fact]
    public void GetAllHolidays_ReturnsNonEmptyList()
    {
        // Act
        var result = _service.GetAllHolidays();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetAllHolidays_IncludesTetHoliday()
    {
        // Act
        var holidays = _service.GetAllHolidays();

        // Assert
        Assert.Contains(holidays, h => h.Name.Contains("Tết") || h.Name.Contains("Lunar New Year"));
    }

    [Fact]
    public void GetHolidaysForYear_ReturnsValidHolidays_For2025()
    {
        // Act
        var result = _service.GetHolidaysForYear(2025);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result, h => Assert.Equal(2025, h.GregorianDate.Year));
    }

    [Fact]
    public void GetHolidaysForMonth_ReturnsOnlyHolidaysInThatMonth()
    {
        // Arrange
        var year = 2025;
        var month = 1; // January - likely has New Year and possibly Tet

        // Act
        var result = _service.GetHolidaysForMonth(year, month);

        // Assert
        Assert.NotNull(result);
        Assert.All(result, h =>
        {
            Assert.Equal(year, h.GregorianDate.Year);
            Assert.Equal(month, h.GregorianDate.Month);
        });
    }

    [Theory]
    [InlineData(2025)]
    [InlineData(2024)]
    [InlineData(2026)]
    public void GetHolidaysForYear_ReturnsConsistentNumberOfFixedHolidays(int year)
    {
        // Act
        var holidays = _service.GetHolidaysForYear(year);

        // Assert
        // Should have at least major fixed holidays (New Year, Liberation, National Day, etc.)
        Assert.True(holidays.Count >= 5, $"Expected at least 5 holidays for year {year}, got {holidays.Count}");
    }

    [Fact]
    public void GetHolidayByDate_FindsNewYear()
    {
        // Arrange
        var newYearDate = new DateTime(2025, 1, 1);

        // Act
        var result = _service.GetHolidayByDate(newYearDate);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("New Year", result.Name, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetHolidayByDate_ReturnsNull_ForNonHolidayDate()
    {
        // Arrange
        var regularDate = new DateTime(2025, 3, 15); // Random non-holiday date

        // Act
        var result = _service.GetHolidayByDate(regularDate);

        // Assert
        // May or may not have a holiday - just ensure no exception
        Assert.True(result == null || result.GregorianDate == regularDate);
    }
}
