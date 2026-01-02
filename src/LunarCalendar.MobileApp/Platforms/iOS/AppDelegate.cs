using Foundation;
using System.Diagnostics;
using UIKit;

namespace LunarCalendar.MobileApp;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	public override bool FinishedLaunching(UIKit.UIApplication application, NSDictionary launchOptions)
	{
		try
		{
			Debug.WriteLine("=== AppDelegate: FinishedLaunching START ===");

			// Initialize SQLite for iOS
			Debug.WriteLine("=== Initializing SQLite ===");
			SQLitePCL.Batteries_V2.Init();
			Debug.WriteLine("=== SQLite initialized successfully ===");

			// Apply modern iOS tab bar styling
			ConfigureModernTabBarAppearance();

			var result = base.FinishedLaunching(application, launchOptions);
			Debug.WriteLine($"=== AppDelegate: FinishedLaunching END - Result: {result} ===");
			return result;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"=== CRASH in AppDelegate.FinishedLaunching: {ex.Message} ===");
			Debug.WriteLine($"=== Stack Trace: {ex.StackTrace} ===");
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
		tabBarAppearance.ShadowColor = UIColor.FromRGBA(0, 0, 0, 0.08f);
		
		// Unselected tab styling (gray)
		var unselectedAttributes = new UIStringAttributes
		{
			Font = UIFont.SystemFontOfSize(10, UIFontWeight.Medium),
			ForegroundColor = UIColor.FromRGB(156, 163, 175) // Gray-400
		};
		tabBarAppearance.StackedLayoutAppearance.Normal.TitleTextAttributes = unselectedAttributes;
		tabBarAppearance.StackedLayoutAppearance.Normal.IconColor = UIColor.FromRGB(156, 163, 175);
		
		// Selected tab styling (red accent)
		var selectedAttributes = new UIStringAttributes
		{
			Font = UIFont.SystemFontOfSize(10, UIFontWeight.Semibold),
			ForegroundColor = UIColor.FromRGB(220, 38, 38) // Red-600
		};
		tabBarAppearance.StackedLayoutAppearance.Selected.TitleTextAttributes = selectedAttributes;
		tabBarAppearance.StackedLayoutAppearance.Selected.IconColor = UIColor.FromRGB(220, 38, 38);
		
		// Apply to all tab bars
		UITabBar.Appearance.StandardAppearance = tabBarAppearance;
		UITabBar.Appearance.ScrollEdgeAppearance = tabBarAppearance;
		
		Debug.WriteLine("=== Modern iOS tab bar styling applied ===");
	}

	protected override MauiApp CreateMauiApp()
	{
		try
		{
			Debug.WriteLine("=== Creating MauiApp ===");
			var app = MauiProgram.CreateMauiApp();
			Debug.WriteLine("=== MauiApp created successfully ===");
			return app;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"=== CRASH in CreateMauiApp: {ex.Message} ===");
			Debug.WriteLine($"=== Stack Trace: {ex.StackTrace} ===");
			throw;
		}
	}
}
