using LunarCalendar.MobileApp.Services;
using System.Globalization;

namespace LunarCalendar.MobileApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// Initialize localization BEFORE creating any pages
		InitializeLocalization();

		MainPage = new AppShell();
	}

	private void InitializeLocalization()
	{
		// Get saved language preference
		var savedLanguage = Preferences.Get("AppLanguage", string.Empty);

		if (string.IsNullOrEmpty(savedLanguage))
		{
			// Default to Vietnamese
			savedLanguage = "vi";
		}

		// Set the culture
		CultureInfo culture;
		switch (savedLanguage.ToLower())
		{
			case "en":
				culture = new CultureInfo("en-US");
				break;
			case "vi":
			default:
				culture = new CultureInfo("vi-VN");
				break;
		}

		CultureInfo.CurrentCulture = culture;
		CultureInfo.CurrentUICulture = culture;
		CultureInfo.DefaultThreadCurrentCulture = culture;
		CultureInfo.DefaultThreadCurrentUICulture = culture;

		System.Diagnostics.Debug.WriteLine($"=== App Language Set: {culture.Name} ===");
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
