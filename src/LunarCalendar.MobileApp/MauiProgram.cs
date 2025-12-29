using Microsoft.Extensions.Logging;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.ViewModels;
using LunarCalendar.MobileApp.Views;
using LunarCalendar.MobileApp.Data;
// TODO: Add AppCenter for crash analytics when ready for production
// Note: AppCenter packages removed temporarily due to iOS simulator linking issue
// Re-add when deploying to physical devices or production
// using Microsoft.AppCenter;
// using Microsoft.AppCenter.Analytics;
// using Microsoft.AppCenter.Crashes;

namespace LunarCalendar.MobileApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		// TODO: Initialize AppCenter for crash reporting and analytics in production
		// AppCenter.Start("ios={Your-iOS-App-Secret};android={Your-Android-App-Secret}",
		//     typeof(Analytics), typeof(Crashes));

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		// Register Database
		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "lunarcalendar.db3");
		builder.Services.AddSingleton(sp => new LunarCalendarDatabase(dbPath));

		// Register Core Calculation Services (NO API REQUIRED - Bundled Architecture)
		builder.Services.AddSingleton<LunarCalendar.Core.Services.ILunarCalculationService, LunarCalendar.Core.Services.LunarCalculationService>();
		builder.Services.AddSingleton<LunarCalendar.Core.Services.IHolidayCalculationService, LunarCalendar.Core.Services.HolidayCalculationService>();

		// Register App Services
		builder.Services.AddSingleton<IConnectivityService, ConnectivityService>();
		builder.Services.AddSingleton<IUserModeService, UserModeService>();
		builder.Services.AddSingleton<ICalendarService, CalendarService>();
		builder.Services.AddSingleton<IHolidayService, HolidayService>();
		builder.Services.AddSingleton<IHapticService, HapticService>();
		builder.Services.AddSingleton<ISyncService, SyncService>();

		// Register ViewModels
		builder.Services.AddTransient<WelcomeViewModel>();
		builder.Services.AddTransient<CalendarViewModel>();
		builder.Services.AddTransient<HolidayDetailViewModel>();
		builder.Services.AddTransient<SettingsViewModel>();

		// Register Views
		builder.Services.AddTransient<WelcomePage>();
		builder.Services.AddTransient<CalendarPage>();
		builder.Services.AddTransient<HolidayDetailPage>();
		builder.Services.AddTransient<SettingsPage>();

		return builder.Build();
	}
}
