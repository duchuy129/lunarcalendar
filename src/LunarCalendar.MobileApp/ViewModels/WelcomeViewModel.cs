using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using LunarCalendar.MobileApp.Models;
using LunarCalendar.MobileApp.Services;

namespace LunarCalendar.MobileApp.ViewModels;

public partial class WelcomeViewModel : BaseViewModel
{
    private readonly IUserModeService _userModeService;

    [ObservableProperty]
    private bool _showCulturalBackground = true;

    public WelcomeViewModel(IUserModeService userModeService)
    {
        _userModeService = userModeService;
        Title = "Welcome";
        
        // Initialize settings
        ShowCulturalBackground = SettingsViewModel.GetShowCulturalBackground();
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
