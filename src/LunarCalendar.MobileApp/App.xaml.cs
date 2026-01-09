using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.Helpers;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;

namespace LunarCalendar.MobileApp;

public partial class App : Application
{
	public App()
	{
		try
		{
			InitializeComponent();

			// Initialize localization BEFORE creating any pages
			InitializeLocalization();

			// USE APPSHELL WITH HELLO WORLD TEST!
		MainPage = new AppShell();
		
		// Log successful app start (after services are initialized)
		var logService = ServiceHelper.GetService<ILogService>();
		logService?.LogInfo("App launched successfully", "App");
	}
	catch (Exception ex)
	{
		// Log crash before re-throwing
		var logService = ServiceHelper.GetService<ILogService>();
		logService?.LogError("App failed to launch", ex, "App.Constructor");

		throw;
	}
	}

	private void InitializeLocalization()
	{
		// Get saved language preference
		var savedLanguage = Preferences.Get("AppLanguage", string.Empty);

		if (string.IsNullOrEmpty(savedLanguage))
		{
			// Default to system language
			var systemLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
			savedLanguage = systemLanguage == "vi" ? "vi" : "en";
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

	}

	private void ApplyModernTabBarStyling(TabbedPage tabbedPage)
	{
		// Modern tab bar styling
		tabbedPage.BackgroundColor = Colors.White;
		
#if ANDROID
		// Android: Style the top tab bar with modern colors
		tabbedPage.BarBackgroundColor = Color.FromArgb("#FFFFFF");
		tabbedPage.BarTextColor = Color.FromArgb("#6B7280"); // Gray-500
		tabbedPage.SelectedTabColor = Color.FromArgb("#DC2626"); // Red-600 (primary accent)
		tabbedPage.UnselectedTabColor = Color.FromArgb("#9CA3AF"); // Gray-400
#elif IOS
		// iOS: Style the bottom tab bar with modern colors and translucency
		tabbedPage.BarBackgroundColor = Color.FromArgb("#FFFFFF");
		tabbedPage.BarTextColor = Color.FromArgb("#6B7280"); // Gray-500
		tabbedPage.SelectedTabColor = Color.FromArgb("#DC2626"); // Red-600 (primary accent)
		tabbedPage.UnselectedTabColor = Color.FromArgb("#9CA3AF"); // Gray-400
#endif
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return base.CreateWindow(activationState);
	}
}
