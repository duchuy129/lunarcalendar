using Foundation;
using System.Diagnostics;
using UIKit;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.Helpers;

namespace LunarCalendar.MobileApp;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	public override bool FinishedLaunching(UIKit.UIApplication application, NSDictionary launchOptions)
	{
		var logService = GetLogService();
		logService?.LogInfo("iOS app launching...", "AppDelegate.FinishedLaunching");

		try
		{
			// Initialize SQLite for iOS
			SQLitePCL.Batteries_V2.Init();
			logService?.LogInfo("SQLite initialized successfully", "AppDelegate.FinishedLaunching");

			// Apply modern iOS tab bar styling - REMOVED due to AOT JIT compilation issues
			// ConfigureModernTabBarAppearance();

			var result = base.FinishedLaunching(application, launchOptions);
			logService?.LogInfo("App launch completed successfully", "AppDelegate.FinishedLaunching");
			return result;
		}
		catch (Exception ex)
		{
			logService?.LogError("Fatal error during app launch", ex, "AppDelegate.FinishedLaunching");
			throw;
		}
	}

	private void ConfigureModernTabBarAppearance()
	{
		// Modern iOS tab bar with translucency and shadow
		var tabBarAppearance = new UITabBarAppearance();
		tabBarAppearance.ConfigureWithOpaqueBackground();
		
		// White background with subtle shadow
		tabBarAppearance.BackgroundColor = UIColor.White;
		tabBarAppearance.ShadowColor = new UIColor(0, 0, 0, 0.08f); // Use constructor instead of FromRGBA
		
		// Create colors
		var grayColor = new UIColor(156f/255f, 163f/255f, 175f/255f, 1f); // Gray-400
		var redColor = new UIColor(220f/255f, 38f/255f, 38f/255f, 1f); // Red-600
		
		// Unselected tab styling (gray) - use property setters instead of UIStringAttributes
		var normalAppearance = tabBarAppearance.StackedLayoutAppearance.Normal;
		normalAppearance.TitleTextAttributes.Font = UIFont.SystemFontOfSize(10, UIFontWeight.Medium);
		normalAppearance.IconColor = grayColor;
		
		// Selected tab styling (red accent)
		var selectedAppearance = tabBarAppearance.StackedLayoutAppearance.Selected;
		selectedAppearance.TitleTextAttributes.Font = UIFont.SystemFontOfSize(10, UIFontWeight.Semibold);
		selectedAppearance.IconColor = redColor;
		
		// Apply to all tab bars
		UITabBar.Appearance.StandardAppearance = tabBarAppearance;
		UITabBar.Appearance.ScrollEdgeAppearance = tabBarAppearance;
		
	}

	protected override MauiApp CreateMauiApp()
	{
		var logService = GetLogService();
		logService?.LogInfo("Creating MAUI app instance", "AppDelegate.CreateMauiApp");

		try
		{
			var app = MauiProgram.CreateMauiApp();
			logService?.LogInfo("MAUI app created successfully", "AppDelegate.CreateMauiApp");
			return app;
		}
		catch (Exception ex)
		{
			logService?.LogError("Failed to create MAUI app", ex, "AppDelegate.CreateMauiApp");
			throw;
		}
	}

	private static ILogService? GetLogService()
	{
		try
		{
			return ServiceHelper.GetService<ILogService>();
		}
		catch
		{
			// If ServiceHelper fails, return null - logging is best-effort
			return null;
		}
	}
}
