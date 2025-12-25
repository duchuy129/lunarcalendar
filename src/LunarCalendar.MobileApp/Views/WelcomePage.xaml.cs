using LunarCalendar.MobileApp.ViewModels;

namespace LunarCalendar.MobileApp.Views;

public partial class WelcomePage : ContentPage
{
    public WelcomePage(WelcomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
