using LunarCalendar.MobileApp.Models;

namespace LunarCalendar.MobileApp.Services;

public class UserModeService : IUserModeService
{
    private UserMode _currentMode;

    public UserModeService()
    {
        _currentMode = LoadSavedMode();
    }

    public UserMode CurrentMode => _currentMode;
    public bool IsGuest => _currentMode == UserMode.Guest;
    public bool IsAuthenticated => _currentMode == UserMode.Authenticated;

    public async Task SetModeAsync(UserMode mode)
    {
        _currentMode = mode;
        Preferences.Set("user_mode", mode.ToString());

        // Notify app of mode change
        await Task.CompletedTask;
    }

    public async Task UpgradeToAuthenticatedAsync(string userId)
    {
        await SetModeAsync(UserMode.Authenticated);
        Preferences.Set("user_id", userId);
    }

    public async Task<bool> CheckAuthenticationStatusAsync()
    {
        var token = await SecureStorage.GetAsync("access_token");
        if (!string.IsNullOrEmpty(token))
        {
            await SetModeAsync(UserMode.Authenticated);
            return true;
        }
        return false;
    }

    private UserMode LoadSavedMode()
    {
        var modeName = Preferences.Get("user_mode", UserMode.Guest.ToString());
        return Enum.Parse<UserMode>(modeName);
    }
}
