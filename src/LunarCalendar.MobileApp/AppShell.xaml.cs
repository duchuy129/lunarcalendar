using LunarCalendar.MobileApp.Resources.Strings;
using LunarCalendar.MobileApp.Views;
using LunarCalendar.MobileApp.Services;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using CommunityToolkit.Mvvm.Messaging;

namespace LunarCalendar.MobileApp;

// REMOVED "partial" - we're NOT using XAML!
public class AppShell : Shell
{
	private ShellContent? _calendarTab;
	private ShellContent? _yearHolidaysTab;
	private ShellContent? _settingsTab;

	public AppShell()
	{
		// BYPASS XAML - Create everything in code!
		// InitializeComponent();

		CreateUIInCode();

		// Register routes
		Routing.RegisterRoute("holidaydetail", typeof(HolidayDetailPage));
		Routing.RegisterRoute("zodiacinfo", typeof(ZodiacInformationPage));

		// Subscribe to language change events
		WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (r, m) =>
		{
			UpdateTabTitles();
		});
	}

	private void UpdateTabTitles()
	{
		if (_calendarTab != null)
		{
			_calendarTab.Title = AppResources.Calendar;
		}
		if (_yearHolidaysTab != null)
		{
			_yearHolidaysTab.Title = AppResources.YearHolidays;
		}
		if (_settingsTab != null)
		{
			_settingsTab.Title = AppResources.Settings;
		}
	}

	private void CreateUIInCode()
	{
		// Set Shell properties
		this.FlyoutBehavior = FlyoutBehavior.Disabled;

		// NOTE: Tab bar styling is handled entirely in AppDelegate.cs for iOS
		// This ensures smooth transitions without flickering
		// The native iOS UITabBarAppearance provides better performance and consistency

		// Shell automatically positions tabs at bottom on both iOS and Android!

		// Create TabBar with real app pages
		var tabBar = new Microsoft.Maui.Controls.TabBar();

		// Use FontImageSource for reliable cross-platform icon display
		_calendarTab = new ShellContent
		{
			Title = AppResources.Calendar,
			ContentTemplate = new DataTemplate(typeof(CalendarPage))
		};
		
		// Set icon using FontImageSource - color will be managed by Shell
		_calendarTab.Icon = new FontImageSource
		{
			Glyph = "📅", // Calendar emoji
			FontFamily = "Arial",
			Size = 30
		};

		_yearHolidaysTab = new ShellContent
		{
			Title = AppResources.YearHolidays,
			ContentTemplate = new DataTemplate(typeof(YearHolidaysPage))
		};

		// Set icon using FontImageSource - color will be managed by Shell
		_yearHolidaysTab.Icon = new FontImageSource
		{
			Glyph = "🎉", // Party popper emoji for holidays
			FontFamily = "Arial",
			Size = 30
		};

		_settingsTab = new ShellContent
		{
			Title = AppResources.Settings,
			ContentTemplate = new DataTemplate(typeof(SettingsPage))
		};

		// Set icon using FontImageSource - color will be managed by Shell
		_settingsTab.Icon = new FontImageSource
		{
			Glyph = "⚙️", // Settings gear emoji
			FontFamily = "Arial",
			Size = 30
		};

		tabBar.Items.Add(_calendarTab);
		tabBar.Items.Add(_yearHolidaysTab);
		tabBar.Items.Add(_settingsTab);

		this.Items.Add(tabBar);
	}
}
