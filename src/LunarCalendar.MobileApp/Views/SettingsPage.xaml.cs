using LunarCalendar.MobileApp.ViewModels;

namespace LunarCalendar.MobileApp.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _viewModel;

    public SettingsPage(SettingsViewModel viewModel)
    {
        System.Diagnostics.Debug.WriteLine("!!! SettingsPage constructor START !!!");
        System.Diagnostics.Debug.WriteLine($"!!! viewModel is null: {viewModel == null} !!!");
        
        InitializeComponent();
        System.Diagnostics.Debug.WriteLine("!!! InitializeComponent done !!!");
        
        _viewModel = viewModel;
        BindingContext = viewModel;
        System.Diagnostics.Debug.WriteLine($"!!! BindingContext set: {BindingContext != null} !!!");
        System.Diagnostics.Debug.WriteLine($"!!! BindingContext type: {BindingContext?.GetType().Name} !!!");
        System.Diagnostics.Debug.WriteLine("!!! SettingsPage constructor END !!!");
    }

    private async void OnAboutClicked(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("!!! OnAboutClicked - Button clicked event handler called !!!");
        await _viewModel.AboutAsync();
    }

    private async void OnSyncClicked(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("!!! OnSyncClicked - Button clicked event handler called !!!");
        await _viewModel.SyncDataAsync();
    }

    private async void OnClearCacheClicked(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("!!! OnClearCacheClicked - Button clicked event handler called !!!");
        await _viewModel.ClearCacheAsync();
    }

    private async void OnResetSettingsClicked(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("!!! OnResetSettingsClicked - Button clicked event handler called !!!");
        await _viewModel.ResetSettingsAsync();
    }
}
