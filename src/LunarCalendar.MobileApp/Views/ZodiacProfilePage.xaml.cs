using LunarCalendar.MobileApp.ViewModels;

namespace LunarCalendar.MobileApp.Views;

public partial class ZodiacProfilePage : ContentPage
{
    public ZodiacProfilePage(ZodiacProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
