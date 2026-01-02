using LunarCalendar.MobileApp.Services;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;

namespace LunarCalendar.MobileApp;

public partial class App : Application
{
	public App()
	{
		try
		{
			System.Diagnostics.Debug.WriteLine("=== App Constructor START ===");

			InitializeComponent();
			System.Diagnostics.Debug.WriteLine("=== InitializeComponent done ===");

			// Initialize localization BEFORE creating any pages
			InitializeLocalization();
			System.Diagnostics.Debug.WriteLine("=== Localization initialized ===");

		// Using TabbedPage instead of Shell (Shell has black screen issue on iOS physical devices)
		var tabbedPage = new TabbedPage();

		// Modern tab bar styling
		ApplyModernTabBarStyling(tabbedPage);

		// Get pages from DI container
		var serviceProvider = IPlatformApplication.Current?.Services
			?? throw new InvalidOperationException("Service provider not available");

		var calendarPage = serviceProvider.GetRequiredService<Views.CalendarPage>();
		var settingsPage = serviceProvider.GetRequiredService<Views.SettingsPage>();

		// Wrap both pages in NavigationPage for consistent header/footer styling
		// Calendar page needs NavigationPage to enable push navigation for holiday details
		var calendarNav = new NavigationPage(calendarPage)
		{
			Title = calendarPage.Title,
			IconImageSource = new FontImageSource
			{
				Glyph = "📅", // Calendar emoji - works everywhere
				Size = 24
			}
		};

		// Wrap Settings page in NavigationPage for consistent appearance
		var settingsNav = new NavigationPage(settingsPage)
		{
			Title = settingsPage.Title,
			IconImageSource = new FontImageSource
			{
				Glyph = "⚙️", // Settings emoji - works everywhere
				Size = 24
			}
		};

		// Bind NavigationPage titles to update when page titles change (for language switching)
		calendarPage.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(calendarPage.Title))
			{
				calendarNav.Title = calendarPage.Title;
			}
		};

		settingsPage.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(settingsPage.Title))
			{
				settingsNav.Title = settingsPage.Title;
			}
		};

		tabbedPage.Children.Add(calendarNav);
		tabbedPage.Children.Add(settingsNav);			MainPage = tabbedPage;
			System.Diagnostics.Debug.WriteLine("=== TabbedPage with real pages created ===");
			System.Diagnostics.Debug.WriteLine("=== App Constructor END ===");
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"=== CRASH in App Constructor: {ex.Message} ===");
			System.Diagnostics.Debug.WriteLine($"=== Stack Trace: {ex.StackTrace} ===");
			if (ex.InnerException != null)
			{
				System.Diagnostics.Debug.WriteLine($"=== Inner Exception: {ex.InnerException.Message} ===");
			}
			throw;
		}
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
