using LunarCalendar.MobileApp.ViewModels;

namespace LunarCalendar.MobileApp.Views;

public partial class ZodiacCompatibilityPage : ContentPage
{
    public ZodiacCompatibilityPage(ZodiacCompatibilityViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
