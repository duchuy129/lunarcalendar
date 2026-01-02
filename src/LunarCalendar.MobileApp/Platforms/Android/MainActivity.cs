using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Core.Content;

namespace LunarCalendar.MobileApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);
		
		// Modern Android styling with edge-to-edge
		if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop && Window != null)
		{
			// Enable edge-to-edge for modern look
			Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
			Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
				(int)SystemUiFlags.LayoutStable |
				(int)SystemUiFlags.LayoutFullscreen
			);
			
			// Light status bar icons for white background
			if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
			{
				var flags = Window.DecorView.SystemUiVisibility;
				Window.DecorView.SystemUiVisibility = (StatusBarVisibility)((int)flags | (int)SystemUiFlags.LightStatusBar);
			}
		}
	}
}
