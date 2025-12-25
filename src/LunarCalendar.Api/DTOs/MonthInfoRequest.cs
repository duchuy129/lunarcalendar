using System.ComponentModel.DataAnnotations;

namespace LunarCalendar.Api.DTOs;

public class MonthInfoRequest
{
    [Required]
    [Range(1900, 2100)]
    public int Year { get; set; }

    [Required]
    [Range(1, 12)]
    public int Month { get; set; }
}
