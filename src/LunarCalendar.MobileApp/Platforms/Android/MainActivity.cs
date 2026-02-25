using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.View;

namespace LunarCalendar.MobileApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);

		// Android 15 (API 35) edge-to-edge compliance.
		//
		// The deprecated Window.setStatusBarColor / Window.setNavigationBarColor APIs
		// (flagged via BottomSheetDialog.onCreate → EdgeToEdgeUtils.applyEdgeToEdge) are
		// triggered when the theme still declares android:statusBarColor /
		// android:navigationBarColor.  We have removed those attributes from styles.xml and
		// switched to Theme.MaterialComponents, which lets EdgeToEdgeUtils work without
		// calling those deprecated setters.
		//
		// Here we complete the edge-to-edge setup using only the stable AndroidX WindowCompat
		// surface, which works back-compatibly from API 21+.
		if (Window != null)
		{
			// Tell the framework the app will manage its own insets — system bars become
			// transparent and draw on top of app content.
			WindowCompat.SetDecorFitsSystemWindows(Window, false);

			// Control system-bar icon/text colour.  Light bars match our warm light theme.
			var controller = WindowCompat.GetInsetsController(Window, Window.DecorView);
			if (controller != null)
			{
				controller.AppearanceLightStatusBars = true;
				controller.AppearanceLightNavigationBars = true;
			}
		}

		// Propagate insets to MAUI so Shell / ScrollView content is padded correctly and
		// does not render behind the status bar or gesture-navigation bar.
		if (Window?.DecorView != null)
		{
			ViewCompat.SetOnApplyWindowInsetsListener(Window.DecorView, new WindowInsetsCallback());
		}
	}

	// Pass insets through unchanged so that MAUI's own insets handling continues to work.
	private sealed class WindowInsetsCallback : Java.Lang.Object, IOnApplyWindowInsetsListener
	{
		public WindowInsetsCompat OnApplyWindowInsets(
			Android.Views.View view,
			WindowInsetsCompat insets)
		{
			return ViewCompat.OnApplyWindowInsets(view, insets);
		}
	}
}
