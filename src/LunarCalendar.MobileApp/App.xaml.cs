namespace LunarCalendar.MobileApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var window = base.CreateWindow(activationState);

#if IOS
		// Ensure window is properly sized for iPhone
		window.Created += (s, e) =>
		{
			if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone)
			{
				System.Diagnostics.Debug.WriteLine("=== Window created for iPhone ===");
			}
		};
#endif

		return window;
	}
}
