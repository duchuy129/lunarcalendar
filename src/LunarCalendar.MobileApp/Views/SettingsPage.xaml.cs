using LunarCalendar.MobileApp.ViewModels;

namespace LunarCalendar.MobileApp.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _viewModel;

    public SettingsPage(SettingsViewModel viewModel)
    {
        
        InitializeComponent();
        
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    private async void OnAboutClicked(object sender, EventArgs e)
    {
        await _viewModel.AboutAsync();
    }

    private async void OnSyncClicked(object sender, EventArgs e)
    {
        await _viewModel.SyncDataAsync();
    }

    private async void OnClearCacheClicked(object sender, EventArgs e)
    {
        await _viewModel.ClearCacheAsync();
    }

    private async void OnResetSettingsClicked(object sender, EventArgs e)
    {
        await _viewModel.ResetSettingsAsync();
    }
}
