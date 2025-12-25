namespace LunarCalendar.Api.DTOs;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public UserDto User { get; set; } = null!;
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string PreferredLanguage { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;
}
