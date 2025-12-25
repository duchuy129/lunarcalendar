namespace LunarCalendar.Api.Models;

public class Event
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsAllDay { get; set; }
    public string CalendarType { get; set; } = "Gregorian"; // Gregorian, Lunar
    public string? RecurrenceRule { get; set; }
    public int? ReminderMinutes { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Color { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Category? Category { get; set; }
}
