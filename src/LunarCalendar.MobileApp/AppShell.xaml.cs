using LunarCalendar.MobileApp.Views;

namespace LunarCalendar.MobileApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		try
		{
			System.Diagnostics.Debug.WriteLine("=== AppShell Constructor START ===");

			InitializeComponent();
			System.Diagnostics.Debug.WriteLine("=== AppShell InitializeComponent done ===");

			// Register routes
			Routing.RegisterRoute("holidaydetail", typeof(HolidayDetailPage));
			System.Diagnostics.Debug.WriteLine("=== AppShell routes registered ===");
			System.Diagnostics.Debug.WriteLine("=== AppShell Constructor END ===");
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"=== CRASH in AppShell Constructor: {ex.Message} ===");
			System.Diagnostics.Debug.WriteLine($"=== Stack Trace: {ex.StackTrace} ===");
			throw;
		}
	}
}
