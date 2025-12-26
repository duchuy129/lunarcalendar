using LunarCalendar.MobileApp.ViewModels;

namespace LunarCalendar.MobileApp.Views;

public partial class CalendarPage : ContentPage
{
    private readonly CalendarViewModel _viewModel;

    public CalendarPage(CalendarViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }

    private async void OnYearPickerSelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_viewModel != null)
        {
            await _viewModel.YearSelectedCommand.ExecuteAsync(null);
        }
    }

    private void OnMonthPickerSelectedIndexChanged(object? sender, EventArgs e)
    {
        if (sender is Picker picker && _viewModel != null)
        {
            // Update the selected month from picker index (0-based to 1-based)
            _viewModel.SelectedCalendarMonth = picker.SelectedIndex + 1;
        }
    }
}
