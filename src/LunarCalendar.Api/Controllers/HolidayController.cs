using Microsoft.AspNetCore.Mvc;
using LunarCalendar.Api.Services;

namespace LunarCalendar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HolidayController : ControllerBase
{
    private readonly IHolidayService _holidayService;
    private readonly ILogger<HolidayController> _logger;

    public HolidayController(IHolidayService holidayService, ILogger<HolidayController> logger)
    {
        _holidayService = holidayService;
        _logger = logger;
    }

    /// <summary>
    /// Get all holidays
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllHolidays()
    {
        try
        {
            var holidays = await _holidayService.GetAllHolidaysAsync();
            return Ok(holidays);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all holidays");
            return StatusCode(500, "An error occurred while retrieving holidays");
        }
    }

    /// <summary>
    /// Get holidays for a specific year
    /// </summary>
    [HttpGet("year/{year}")]
    public async Task<IActionResult> GetHolidaysForYear(int year)
    {
        try
        {
            if (year < 1900 || year > 2100)
            {
                return BadRequest("Year must be between 1900 and 2100");
            }

            var holidays = await _holidayService.GetHolidaysForYearAsync(year);
            return Ok(holidays);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting holidays for year {Year}", year);
            return StatusCode(500, "An error occurred while retrieving holidays");
        }
    }

    /// <summary>
    /// Get holidays for a specific month
    /// </summary>
    [HttpGet("month/{year}/{month}")]
    public async Task<IActionResult> GetHolidaysForMonth(int year, int month)
    {
        try
        {
            if (year < 1900 || year > 2100)
            {
                return BadRequest("Year must be between 1900 and 2100");
            }

            if (month < 1 || month > 12)
            {
                return BadRequest("Month must be between 1 and 12");
            }

            var holidays = await _holidayService.GetHolidaysForMonthAsync(year, month);
            return Ok(holidays);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting holidays for {Year}-{Month}", year, month);
            return StatusCode(500, "An error occurred while retrieving holidays");
        }
    }

    /// <summary>
    /// Get holiday for a specific date
    /// </summary>
    [HttpGet("date/{year}/{month}/{day}")]
    public async Task<IActionResult> GetHolidayForDate(int year, int month, int day)
    {
        try
        {
            if (year < 1900 || year > 2100)
            {
                return BadRequest("Year must be between 1900 and 2100");
            }

            if (month < 1 || month > 12)
            {
                return BadRequest("Month must be between 1 and 12");
            }

            if (day < 1 || day > 31)
            {
                return BadRequest("Day must be between 1 and 31");
            }

            var date = new DateTime(year, month, day);
            var holiday = await _holidayService.GetHolidayForDateAsync(date);

            if (holiday == null)
            {
                return NotFound("No holiday found for this date");
            }

            return Ok(holiday);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting holiday for date {Year}-{Month}-{Day}", year, month, day);
            return StatusCode(500, "An error occurred while retrieving holiday");
        }
    }
}
