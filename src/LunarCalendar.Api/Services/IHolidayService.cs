using LunarCalendar.Api.Models;

namespace LunarCalendar.Api.Services;

public interface IHolidayService
{
    /// <summary>
    /// Gets all holidays
    /// </summary>
    Task<List<Holiday>> GetAllHolidaysAsync();

    /// <summary>
    /// Gets holidays for a specific year (converted to Gregorian dates for that year)
    /// </summary>
    Task<List<HolidayOccurrence>> GetHolidaysForYearAsync(int year);

    /// <summary>
    /// Gets holidays for a specific month and year
    /// </summary>
    Task<List<HolidayOccurrence>> GetHolidaysForMonthAsync(int year, int month);

    /// <summary>
    /// Gets holiday for a specific Gregorian date
    /// </summary>
    Task<Holiday?> GetHolidayForDateAsync(DateTime date);
}

/// <summary>
/// Represents a specific occurrence of a holiday in a Gregorian year
/// </summary>
public class HolidayOccurrence
{
    public Holiday Holiday { get; set; } = null!;
    public DateTime GregorianDate { get; set; }
}
