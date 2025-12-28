using LunarCalendar.Api.Controllers;
using LunarCalendar.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LunarCalendar.Api.Tests.Controllers;

public class CalendarControllerTests
{
    private readonly Mock<ILunarCalendarService> _mockLunarService;
    private readonly Mock<ILogger<CalendarController>> _mockLogger;
    private readonly CalendarController _controller;

    public CalendarControllerTests()
    {
        _mockLunarService = new Mock<ILunarCalendarService>();
        _mockLogger = new Mock<ILogger<CalendarController>>();
        _controller = new CalendarController(_mockLunarService.Object, _mockLogger.Object);
    }

    [Fact]
    public void ConvertToLunar_ReturnsOkResult_WithDefaultDate()
    {
        // Arrange
        var expectedLunarInfo = new Models.LunarInfo
        {
            GregorianDate = DateTime.Today,
            LunarYear = 2025,
            LunarMonth = 11,
            LunarDay = 28,
            LunarYearName = "Ất Tỵ",
            LunarMonthName = "Tháng Mười Một",
            LunarDayName = "Mồng 28",
            AnimalSign = "Rắn",
            IsLeapMonth = false
        };

        _mockLunarService
            .Setup(s => s.ConvertToLunar(It.IsAny<DateTime>()))
            .Returns(expectedLunarInfo);

        // Act
        var result = _controller.ConvertToLunar(null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
        _mockLunarService.Verify(s => s.ConvertToLunar(It.IsAny<DateTime>()), Times.Once);
    }

    [Fact]
    public void ConvertToLunar_ReturnsInternalServerError_OnException()
    {
        // Arrange
        _mockLunarService
            .Setup(s => s.ConvertToLunar(It.IsAny<DateTime>()))
            .Throws(new Exception("Test exception"));

        // Act
        var result = _controller.ConvertToLunar(null);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public void GetMonthInfo_ReturnsOkResult_WithValidYearAndMonth()
    {
        // Arrange
        var year = 2025;
        var month = 12;
        var expectedList = new List<Models.LunarInfo>
        {
            new Models.LunarInfo
            {
                GregorianDate = new DateTime(2025, 12, 1),
                LunarDay = 1,
                LunarMonth = 11,
                LunarYear = 2025
            }
        };

        _mockLunarService
            .Setup(s => s.GetMonthInfo(year, month))
            .Returns(expectedList);

        // Act
        var result = _controller.GetMonthInfo(year, month);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void GetMonthInfo_ReturnsBadRequest_WithInvalidYear()
    {
        // Arrange
        var year = 1800; // Out of range
        var month = 12;

        // Act
        var result = _controller.GetMonthInfo(year, month);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    [InlineData(-1)]
    public void GetMonthInfo_ReturnsBadRequest_WithInvalidMonth(int month)
    {
        // Arrange
        var year = 2025;

        // Act
        var result = _controller.GetMonthInfo(year, month);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }
}
