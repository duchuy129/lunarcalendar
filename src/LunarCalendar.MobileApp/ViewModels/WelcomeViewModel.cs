using CommunityToolkit.Mvvm.Input;
using LunarCalendar.MobileApp.Models;
using LunarCalendar.MobileApp.Services;

namespace LunarCalendar.MobileApp.ViewModels;

public partial class WelcomeViewModel : BaseViewModel
{
    private readonly IUserModeService _userModeService;

    public WelcomeViewModel(IUserModeService userModeService)
    {
        _userModeService = userModeService;
        Title = "Welcome";
    }

    [RelayCommand]
    async Task ContinueAsGuestAsync()
    {
        await _userModeService.SetModeAsync(UserMode.Guest);
        await Shell.Current.GoToAsync("//calendar");
    }

    [RelayCommand]
    async Task NavigateToSignInAsync()
    {
        await Shell.Current.GoToAsync("//login");
    }

    [RelayCommand]
    async Task NavigateToSignUpAsync()
    {
        await Shell.Current.GoToAsync("//register");
    }
}
