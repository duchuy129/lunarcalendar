using LunarCalendar.Core.Models;
using LunarCalendar.MobileApp.ViewModels;

namespace LunarCalendar.MobileApp.Views;

public partial class HolidayDetailPage : ContentPage
{
    private readonly HolidayDetailViewModel _viewModel;

    public HolidayDetailPage(HolidayDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public async void SetHoliday(HolidayOccurrence holidayOccurrence)
    {
        await _viewModel.InitializeAsync(holidayOccurrence);
    }
}
