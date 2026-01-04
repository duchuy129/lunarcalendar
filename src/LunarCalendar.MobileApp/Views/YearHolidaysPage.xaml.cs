using LunarCalendar.MobileApp.ViewModels;
using LunarCalendar.MobileApp.Resources.Strings;
using LunarCalendar.MobileApp.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace LunarCalendar.MobileApp.Views;

public partial class YearHolidaysPage : ContentPage
{
    private readonly YearHolidaysViewModel _viewModel;

    public YearHolidaysPage(YearHolidaysViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // Subscribe to language change events to update title
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (r, m) =>
        {
            Title = AppResources.YearHolidays;
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}
