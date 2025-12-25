namespace LunarCalendar.Api.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string PreferredLanguage { get; set; } = "en";
    public string TimeZone { get; set; } = "UTC";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
