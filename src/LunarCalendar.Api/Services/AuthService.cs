using LunarCalendar.Api.Data;
using LunarCalendar.Api.DTOs;
using LunarCalendar.Api.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace LunarCalendar.Api.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthService(ApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Check if user already exists
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        // Create new user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            PreferredLanguage = request.PreferredLanguage,
            TimeZone = request.TimeZone,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Generate token
        var token = _tokenService.GenerateToken(user);
        var expiration = DateTime.UtcNow.AddMinutes(1440); // 24 hours

        return new AuthResponse
        {
            Token = token,
            Expiration = expiration,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PreferredLanguage = user.PreferredLanguage,
                TimeZone = user.TimeZone
            }
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // Find user
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        // Generate token
        var token = _tokenService.GenerateToken(user);
        var expiration = DateTime.UtcNow.AddMinutes(1440); // 24 hours

        return new AuthResponse
        {
            Token = token,
            Expiration = expiration,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PreferredLanguage = user.PreferredLanguage,
                TimeZone = user.TimeZone
            }
        };
    }
}
