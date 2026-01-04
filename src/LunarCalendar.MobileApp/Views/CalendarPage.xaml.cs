using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.ViewModels;

namespace LunarCalendar.MobileApp.Views;

public partial class CalendarPage : ContentPage
{
    private readonly CalendarViewModel _viewModel;
    private readonly ILogService _logService;
    private bool _isInitialized = false;

    public CalendarPage(CalendarViewModel viewModel, ILogService logService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _logService = logService;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            if (!_isInitialized)
            {
                await _viewModel.InitializeAsync();
                _isInitialized = true;
            }
            else
            {
                // FIXED: Now using atomic collection replacement, safe to call directly
                // Collection replacement doesn't trigger intermediate CollectionChanged events
                // Added try-catch to prevent crashes when navigating back from detail pages
                await _viewModel.RefreshSettingsAsync();
            }
        }
        catch (Exception ex)
        {
            _logService.LogError("Failed to initialize or refresh calendar page", ex, "CalendarPage");
        }
    }
}
