using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using LunarCalendar.Api.DTOs;
using LunarCalendar.Api.Services;

namespace LunarCalendar.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CalendarController : ControllerBase
{
    private readonly ILunarCalendarService _lunarCalendarService;
    private readonly ILogger<CalendarController> _logger;

    public CalendarController(
        ILunarCalendarService lunarCalendarService,
        ILogger<CalendarController> logger)
    {
        _lunarCalendarService = lunarCalendarService;
        _logger = logger;
    }

    /// <summary>
    /// Converts a Gregorian date to lunar date
    /// </summary>
    /// <param name="date">Gregorian date (defaults to today if not provided)</param>
    /// <returns>Lunar date information</returns>
    [HttpGet("convert")]
    [ResponseCache(Duration = 3600, VaryByQueryKeys = new[] { "date" })] // Cache for 1 hour
    public ActionResult<LunarDateResponse> ConvertToLunar([FromQuery] DateTime? date = null)
    {
        try
        {
            var gregorianDate = date ?? DateTime.Today;
            var lunarInfo = _lunarCalendarService.ConvertToLunar(gregorianDate);

            var response = new LunarDateResponse
            {
                GregorianDate = lunarInfo.GregorianDate,
                LunarYear = lunarInfo.LunarYear,
                LunarMonth = lunarInfo.LunarMonth,
                LunarDay = lunarInfo.LunarDay,
                IsLeapMonth = lunarInfo.IsLeapMonth,
                LunarYearName = lunarInfo.LunarYearName,
                LunarMonthName = lunarInfo.LunarMonthName,
                LunarDayName = lunarInfo.LunarDayName,
                AnimalSign = lunarInfo.AnimalSign
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error converting date to lunar calendar");
            return StatusCode(500, new { message = "An error occurred while converting the date" });
        }
    }

    /// <summary>
    /// Gets comprehensive lunar calendar information for a specific date
    /// </summary>
    /// <param name="date">Gregorian date (defaults to today if not provided)</param>
    /// <returns>Complete lunar calendar information including festivals and solar terms</returns>
    [HttpGet("lunar-info")]
    [ResponseCache(Duration = 3600, VaryByQueryKeys = new[] { "date" })] // Cache for 1 hour
    public ActionResult<LunarCalendarInfoResponse> GetLunarInfo([FromQuery] DateTime? date = null)
    {
        try
        {
            var gregorianDate = date ?? DateTime.Today;
            var lunarCalendarInfo = _lunarCalendarService.GetLunarInfo(gregorianDate);

            var response = new LunarCalendarInfoResponse
            {
                DateInfo = new LunarDateResponse
                {
                    GregorianDate = lunarCalendarInfo.DateInfo.GregorianDate,
                    LunarYear = lunarCalendarInfo.DateInfo.LunarYear,
                    LunarMonth = lunarCalendarInfo.DateInfo.LunarMonth,
                    LunarDay = lunarCalendarInfo.DateInfo.LunarDay,
                    IsLeapMonth = lunarCalendarInfo.DateInfo.IsLeapMonth,
                    LunarYearName = lunarCalendarInfo.DateInfo.LunarYearName,
                    LunarMonthName = lunarCalendarInfo.DateInfo.LunarMonthName,
                    LunarDayName = lunarCalendarInfo.DateInfo.LunarDayName,
                    AnimalSign = lunarCalendarInfo.DateInfo.AnimalSign
                },
                Festival = lunarCalendarInfo.Festival,
                SolarTerm = lunarCalendarInfo.SolarTerm,
                IsHoliday = lunarCalendarInfo.IsHoliday
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting lunar calendar info");
            return StatusCode(500, new { message = "An error occurred while getting lunar calendar information" });
        }
    }

    /// <summary>
    /// Gets lunar calendar information for an entire month
    /// </summary>
    /// <param name="year">Year (1900-2100)</param>
    /// <param name="month">Month (1-12)</param>
    /// <returns>Lunar date information for all days in the month</returns>
    [HttpGet("month")]
    [ResponseCache(Duration = 86400, VaryByQueryKeys = new[] { "year", "month" })] // Cache for 24 hours
    public ActionResult<IEnumerable<LunarDateResponse>> GetMonthInfo(
        [FromQuery] int year,
        [FromQuery] int month)
    {
        try
        {
            if (year < 1900 || year > 2100)
            {
                return BadRequest(new { message = "Year must be between 1900 and 2100" });
            }

            if (month < 1 || month > 12)
            {
                return BadRequest(new { message = "Month must be between 1 and 12" });
            }

            var monthInfo = _lunarCalendarService.GetMonthInfo(year, month);

            var response = monthInfo.Select(info => new LunarDateResponse
            {
                GregorianDate = info.GregorianDate,
                LunarYear = info.LunarYear,
                LunarMonth = info.LunarMonth,
                LunarDay = info.LunarDay,
                IsLeapMonth = info.IsLeapMonth,
                LunarYearName = info.LunarYearName,
                LunarMonthName = info.LunarMonthName,
                LunarDayName = info.LunarDayName,
                AnimalSign = info.AnimalSign
            });

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting month info for {Year}-{Month}", year, month);
            return StatusCode(500, new { message = "An error occurred while getting month information" });
        }
    }
}
