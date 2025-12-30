using LunarCalendar.MobileApp.ViewModels;

namespace LunarCalendar.MobileApp.Views;

public partial class CalendarPage : ContentPage
{
    private readonly CalendarViewModel _viewModel;
    private bool _isInitialized = false;

    public CalendarPage(CalendarViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        if (!_isInitialized)
        {
            await _viewModel.InitializeAsync();
            _isInitialized = true;
        }
        else
        {
            // CRITICAL: Must await RefreshSettings to prevent iOS crash
            // When navigating back from Settings, collection updates must complete
            // before iOS UICollectionView tries to render
            await _viewModel.RefreshSettingsAsync();
            
            // iOS-specific: Small delay to ensure collection is fully bound before rendering
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Task.Delay(50);
            }
        }
    }
}
