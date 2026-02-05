using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Core.View;

namespace LunarCalendar.MobileApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);
		
		// Enable edge-to-edge display for Android 15+ compatibility
		// This replaces deprecated setStatusBarColor/setNavigationBarColor APIs
		if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop && Window != null)
		{
			// Use AndroidX WindowCompat for modern edge-to-edge support
			WindowCompat.SetDecorFitsSystemWindows(Window, false);
			
			// Configure window insets controller for proper system bar appearance
			var windowInsetsController = WindowCompat.GetInsetsController(Window, Window.DecorView);
			if (windowInsetsController != null)
			{
				// Light status bar icons for better visibility on light backgrounds
				windowInsetsController.AppearanceLightStatusBars = true;
				windowInsetsController.AppearanceLightNavigationBars = true;
			}
		}
	}
}
