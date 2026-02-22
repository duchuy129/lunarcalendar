using Microsoft.Extensions.Logging;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.ViewModels;
using LunarCalendar.MobileApp.Views;
using LunarCalendar.MobileApp.Data;
using LunarCalendar.MobileApp.Helpers;

namespace LunarCalendar.MobileApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		// Register global exception handlers early
		RegisterGlobalExceptionHandlers();

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

		// Register LogService first (needed by other services and ILogger integration)
		builder.Services.AddSingleton<ILogService, LogService>();

		// Add LogService integration for framework-level logging
		// Use a factory to get the same LogService instance from DI
		builder.Logging.Services.AddSingleton<ILoggerProvider>(sp =>
			new LogServiceProvider(sp.GetRequiredService<ILogService>()));

		// Register Database
		// CRITICAL FIX: Don't access FileSystem during DI registration - iOS 26.1 crashes
		// Use factory to defer FileSystem access until first use
		builder.Services.AddSingleton<LunarCalendarDatabase>(sp =>
		{
			var dbPath = Path.Combine(FileSystem.AppDataDirectory, "lunarcalendar.db3");
			return new LunarCalendarDatabase(dbPath);
		});

		// Register Core Calculation Services (NO API REQUIRED - Bundled Architecture)
		builder.Services.AddSingleton<LunarCalendar.Core.Services.ILunarCalculationService, LunarCalendar.Core.Services.LunarCalculationService>();
		builder.Services.AddSingleton<LunarCalendar.Core.Services.IHolidayCalculationService, LunarCalendar.Core.Services.HolidayCalculationService>();
		builder.Services.AddSingleton<LunarCalendar.Core.Services.ISexagenaryService, LunarCalendar.Core.Services.SexagenaryService>();
		builder.Services.AddSingleton<LunarCalendar.Core.Services.IZodiacService, LunarCalendar.Core.Services.ZodiacService>();
		builder.Services.AddSingleton<LunarCalendar.Core.Services.IZodiacDataRepository, LunarCalendar.Core.Services.ZodiacDataRepository>();

		// Register App Services (LogService already registered above)
		builder.Services.AddSingleton<ILocalizationService, LocalizationService>();
		builder.Services.AddSingleton<IConnectivityService, ConnectivityService>();
		builder.Services.AddSingleton<IUserModeService, UserModeService>();
		builder.Services.AddSingleton<ICalendarService, CalendarService>();
		builder.Services.AddSingleton<IHolidayService, HolidayService>();
		builder.Services.AddSingleton<IHapticService, HapticService>();
		builder.Services.AddSingleton<ISyncService, SyncService>();

		// Register ViewModels
		builder.Services.AddTransient<WelcomeViewModel>();
		builder.Services.AddTransient<CalendarViewModel>();
		builder.Services.AddTransient<YearHolidaysViewModel>();
		builder.Services.AddTransient<HolidayDetailViewModel>();
		builder.Services.AddTransient<SettingsViewModel>();
		// REMOVED: ZodiacInformationViewModel - using simple popup instead
		// builder.Services.AddTransient<ZodiacInformationViewModel>();

		// Register Views
		builder.Services.AddTransient<WelcomePage>();
		builder.Services.AddTransient<CalendarPage>();
		builder.Services.AddTransient<YearHolidaysPage>();
		builder.Services.AddTransient<HolidayDetailPage>();
		builder.Services.AddTransient<SettingsPage>();
		// REMOVED: ZodiacInformationPage - using simple popup instead
		// builder.Services.AddTransient<ZodiacInformationPage>();

		return builder.Build();
	}

	private static void RegisterGlobalExceptionHandlers()
	{
		// Register global unhandled exception handler
		AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

		// Register unobserved task exception handler
		TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
	}

	private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		var exception = e.ExceptionObject as Exception;
		var logService = GetLogServiceSafe();

		logService?.LogError(
			$"Unhandled exception - app will crash (IsTerminating: {e.IsTerminating})",
			exception,
			"AppDomain.UnhandledException"
		);

		// Note: AppCenter crash reporting can be added here when re-enabled
	}

	private static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
	{
		var logService = GetLogServiceSafe();

		logService?.LogError(
			"Unobserved task exception",
			e.Exception,
			"TaskScheduler.UnobservedTaskException"
		);

		// Mark exception as observed to prevent app crash
		e.SetObserved();

		// Note: AppCenter crash reporting can be added here when re-enabled
	}

	private static ILogService? GetLogServiceSafe()
	{
		try
		{
			return ServiceHelper.GetService<ILogService>();
		}
		catch
		{
			// If service resolution fails, return null - logging is best-effort
			return null;
		}
	}
}
