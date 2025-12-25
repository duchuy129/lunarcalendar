using LunarCalendar.MobileApp.Models;

namespace LunarCalendar.MobileApp.Services;

public interface IUserModeService
{
    UserMode CurrentMode { get; }
    bool IsGuest { get; }
    bool IsAuthenticated { get; }
    Task SetModeAsync(UserMode mode);
    Task UpgradeToAuthenticatedAsync(string userId);
    Task<bool> CheckAuthenticationStatusAsync();
}
