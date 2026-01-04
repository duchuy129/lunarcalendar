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
