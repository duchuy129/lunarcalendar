using LunarCalendar.MobileApp.ViewModels;

namespace LunarCalendar.MobileApp.Views;

/// <summary>
/// Date Detail Page - displays comprehensive information for a selected date
/// including Gregorian, Lunar, Stem-Branch, and Holiday details
/// </summary>
public partial class DateDetailPage : ContentPage
{
    private readonly DateDetailViewModel _viewModel;

    public DateDetailPage(DateDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    /// <summary>
    /// Initialize the page with a specific date
    /// Called before navigation
    /// </summary>
    public async Task InitializeWithDateAsync(DateTime date)
    {
        await _viewModel.InitializeAsync(date);
    }
}
