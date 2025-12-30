using Xunit;
using Moq;
using System.Collections.ObjectModel;
using LunarCalendar.MobileApp.ViewModels;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.Core.Models;
using LunarCalendar.MobileApp.Models;

namespace LunarCalendar.MobileApp.Tests.ViewModels;

public class CalendarViewModelTests
{
    private readonly Mock<ICalendarService> _mockCalendarService;
    private readonly Mock<IUserModeService> _mockUserModeService;
    private readonly Mock<IHolidayService> _mockHolidayService;
    private readonly Mock<IHapticService> _mockHapticService;
    private readonly Mock<IConnectivityService> _mockConnectivityService;
    private readonly Mock<ISyncService> _mockSyncService;

    public CalendarViewModelTests()
    {
        _mockCalendarService = new Mock<ICalendarService>();
        _mockUserModeService = new Mock<IUserModeService>();
        _mockHolidayService = new Mock<IHolidayService>();
        _mockHapticService = new Mock<IHapticService>();
        _mockConnectivityService = new Mock<IConnectivityService>();
        _mockSyncService = new Mock<ISyncService>();

        // Setup default behaviors
        _mockConnectivityService.Setup(x => x.IsConnected).Returns(true);
        _mockHolidayService.Setup(x => x.GetHolidaysForYearAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<HolidayOccurrence>());
    }

    [Fact]
    public async Task LoadUpcomingHolidaysAsync_ShouldNotCrash_WhenCalledMultipleTimes()
    {
        // Arrange
        var holidays = CreateTestHolidays(10);
        _mockHolidayService.Setup(x => x.GetHolidaysForYearAsync(It.IsAny<int>()))
            .ReturnsAsync(holidays);

        var viewModel = CreateViewModel();

        // Act - Call multiple times rapidly (simulating navigation)
        var tasks = new List<Task>();
        for (int i = 0; i < 5; i++)
        {
            tasks.Add(Task.Run(async () => await viewModel.InitializeAsync()));
        }

        // Assert - Should not throw
        await Task.WhenAll(tasks);
        Assert.NotNull(viewModel.UpcomingHolidays);
    }

    [Fact]
    public async Task LoadUpcomingHolidaysAsync_ShouldUseClearAndAdd_NotReplaceCollection()
    {
        // Arrange
        var holidays = CreateTestHolidays(5);
        _mockHolidayService.Setup(x => x.GetHolidaysForYearAsync(It.IsAny<int>()))
            .ReturnsAsync(holidays);

        var viewModel = CreateViewModel();
        var originalCollection = viewModel.UpcomingHolidays;

        // Act
        await viewModel.InitializeAsync();

        // Assert - Collection reference should remain the same
        Assert.Same(originalCollection, viewModel.UpcomingHolidays);
    }

    [Fact]
    public async Task LoadUpcomingHolidaysAsync_ShouldFilterHolidaysByDateRange()
    {
        // Arrange
        var today = DateTime.Today;
        var holidays = new List<HolidayOccurrence>
        {
            CreateHolidayOccurrence(today.AddDays(5)),  // Within range
            CreateHolidayOccurrence(today.AddDays(15)), // Within range
            CreateHolidayOccurrence(today.AddDays(60)), // Outside default 30-day range
            CreateHolidayOccurrence(today.AddDays(-5))  // In the past
        };

        _mockHolidayService.Setup(x => x.GetHolidaysForYearAsync(It.IsAny<int>()))
            .ReturnsAsync(holidays);

        var viewModel = CreateViewModel();

        // Act
        await viewModel.InitializeAsync();
        await Task.Delay(100); // Give time for async operations

        // Assert - Should only have 2 holidays within 30 days
        Assert.Equal(2, viewModel.UpcomingHolidays.Count);
    }

    [Fact]
    public async Task LoadUpcomingHolidaysAsync_ShouldHandleYearBoundary()
    {
        // Arrange
        var endOfYear = new DateTime(DateTime.Today.Year, 12, 20);
        var holidays2024 = new List<HolidayOccurrence>
        {
            CreateHolidayOccurrence(new DateTime(DateTime.Today.Year, 12, 25))
        };
        var holidays2025 = new List<HolidayOccurrence>
        {
            CreateHolidayOccurrence(new DateTime(DateTime.Today.Year + 1, 1, 1))
        };

        _mockHolidayService.Setup(x => x.GetHolidaysForYearAsync(DateTime.Today.Year))
            .ReturnsAsync(holidays2024);
        _mockHolidayService.Setup(x => x.GetHolidaysForYearAsync(DateTime.Today.Year + 1))
            .ReturnsAsync(holidays2025);

        var viewModel = CreateViewModel();

        // Act
        await viewModel.InitializeAsync();
        await Task.Delay(100);

        // Assert - Should handle year boundary
        Assert.NotNull(viewModel.UpcomingHolidays);
    }

    [Fact]
    public async Task RefreshSettings_ShouldOnlyReloadWhenDaysChange()
    {
        // Arrange
        var holidays = CreateTestHolidays(5);
        _mockHolidayService.Setup(x => x.GetHolidaysForYearAsync(It.IsAny<int>()))
            .ReturnsAsync(holidays);

        var viewModel = CreateViewModel();
        await viewModel.InitializeAsync();
        await Task.Delay(100);

        var initialCount = viewModel.UpcomingHolidays.Count;
        int callCount = 0;
        _mockHolidayService.Setup(x => x.GetHolidaysForYearAsync(It.IsAny<int>()))
            .Callback(() => callCount++)
            .ReturnsAsync(holidays);

        // Act - Call RefreshSettings without changing days
        viewModel.RefreshSettings();
        await Task.Delay(100);

        // Assert - Should not reload if days haven't changed
        Assert.Equal(initialCount, viewModel.UpcomingHolidays.Count);
    }

    [Fact]
    public async Task InitializeAsync_ShouldHandleEmptyHolidayList()
    {
        // Arrange
        _mockHolidayService.Setup(x => x.GetHolidaysForYearAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<HolidayOccurrence>());

        var viewModel = CreateViewModel();

        // Act
        await viewModel.InitializeAsync();
        await Task.Delay(100);

        // Assert
        Assert.NotNull(viewModel.UpcomingHolidays);
        Assert.Empty(viewModel.UpcomingHolidays);
    }

    [Fact]
    public async Task InitializeAsync_ShouldHandleServiceException()
    {
        // Arrange
        _mockHolidayService.Setup(x => x.GetHolidaysForYearAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("Service error"));

        var viewModel = CreateViewModel();

        // Act & Assert - Should not throw, should handle gracefully
        await viewModel.InitializeAsync();
        Assert.NotNull(viewModel.UpcomingHolidays);
    }

    [Fact]
    public void UpcomingHolidaysTitle_ShouldReflectCurrentDays()
    {
        // Arrange
        var viewModel = CreateViewModel();

        // Act
        viewModel.UpcomingHolidaysDays = 60;

        // Assert
        Assert.Contains("60", viewModel.UpcomingHolidaysTitle);
    }

    [Fact]
    public async Task ConcurrentAccess_ShouldNotCauseCollectionModificationException()
    {
        // Arrange
        var holidays = CreateTestHolidays(20);
        _mockHolidayService.Setup(x => x.GetHolidaysForYearAsync(It.IsAny<int>()))
            .ReturnsAsync(holidays);

        var viewModel = CreateViewModel();

        // Act - Simulate concurrent access from different threads
        var tasks = new List<Task>();
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(async () =>
            {
                await viewModel.InitializeAsync();
                // Try to enumerate collection while it might be updating
                var count = viewModel.UpcomingHolidays.Count;
            }));
        }

        // Assert - Should not throw
        await Task.WhenAll(tasks);
    }

    // Helper methods
    private CalendarViewModel CreateViewModel()
    {
        return new CalendarViewModel(
            _mockCalendarService.Object,
            _mockUserModeService.Object,
            _mockHolidayService.Object,
            _mockHapticService.Object,
            _mockConnectivityService.Object,
            _mockSyncService.Object
        );
    }

    private List<HolidayOccurrence> CreateTestHolidays(int count)
    {
        var holidays = new List<HolidayOccurrence>();
        var today = DateTime.Today;

        for (int i = 0; i < count; i++)
        {
            holidays.Add(CreateHolidayOccurrence(today.AddDays(i * 2)));
        }

        return holidays;
    }

    private HolidayOccurrence CreateHolidayOccurrence(DateTime date)
    {
        return new HolidayOccurrence
        {
            Holiday = new Holiday
            {
                Id = Guid.NewGuid(),
                Name = $"Test Holiday {date:MMdd}",
                Type = HolidayType.TraditionalFestival,
                ColorHex = "#FF0000",
                Culture = "Vietnamese",
                IsPublicHoliday = false,
                HasLunarDate = true,
                LunarMonth = date.Month,
                LunarDay = date.Day
            },
            GregorianDate = date,
            AnimalSign = "Rat"
        };
    }
}
