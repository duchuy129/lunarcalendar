using LunarCalendar.MobileApp.Views;

namespace LunarCalendar.MobileApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Register routes
		Routing.RegisterRoute("holidaydetail", typeof(HolidayDetailPage));
	}
}
