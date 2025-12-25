using LunarCalendar.Api.Models;

namespace LunarCalendar.Api.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
