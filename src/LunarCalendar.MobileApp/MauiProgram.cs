using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.ViewModels;
using LunarCalendar.MobileApp.Views;
using LunarCalendar.MobileApp.Data;
using Refit;
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

		// Configure appsettings.json
		var assembly = Assembly.GetExecutingAssembly();
		using var stream = assembly.GetManifestResourceStream("LunarCalendar.MobileApp.appsettings.json");

		var config = new ConfigurationBuilder()
			.AddJsonStream(stream!)
			.Build();

		builder.Configuration.AddConfiguration(config);

#if DEBUG
		builder.Logging.AddDebug();

		// Load environment-specific configuration for Debug mode
		using var devStream = assembly.GetManifestResourceStream("LunarCalendar.MobileApp.appsettings.Development.json");
		if (devStream != null)
		{
			var devConfig = new ConfigurationBuilder()
				.AddJsonStream(devStream)
				.Build();
			builder.Configuration.AddConfiguration(devConfig);
		}
#else
		// Load environment-specific configuration for Production mode
		using var prodStream = assembly.GetManifestResourceStream("LunarCalendar.MobileApp.appsettings.Production.json");
		if (prodStream != null)
		{
			var prodConfig = new ConfigurationBuilder()
				.AddJsonStream(prodStream)
				.Build();
			builder.Configuration.AddConfiguration(prodConfig);
		}
#endif

		// Register Database
		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "lunarcalendar.db3");
		builder.Services.AddSingleton(sp => new LunarCalendarDatabase(dbPath));

		// Register Services
		builder.Services.AddSingleton<IConnectivityService, ConnectivityService>();
		builder.Services.AddSingleton<IUserModeService, UserModeService>();
		builder.Services.AddSingleton<ICalendarService, CalendarService>();
		builder.Services.AddSingleton<IHapticService, HapticService>();
		builder.Services.AddSingleton<ISyncService, SyncService>();

		// Get API base URL from configuration
		var baseUrl = builder.Configuration["ApiSettings:BaseUrl"]
			?? throw new InvalidOperationException("API BaseUrl not configured in appsettings.json");

		Console.WriteLine($"========== CONFIGURING API CLIENTS WITH BASE URL: {baseUrl} ==========");

#if DEBUG
		// DEVELOPMENT ONLY: Bypass SSL certificate validation for local HTTPS testing
		// WARNING: Never use this in production!
		var httpMessageHandler = new HttpClientHandler
		{
			ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
			{
				// Accept all certificates in development mode
				return true;
			}
		};

		// Register HTTP client for Holiday Service with base URL and SSL bypass
		builder.Services.AddHttpClient<IHolidayService, HolidayService>(client =>
		{
			client.BaseAddress = new Uri(baseUrl);
		})
		.ConfigurePrimaryHttpMessageHandler(() => httpMessageHandler);

		// Register Refit API client with base URL and SSL bypass
		builder.Services.AddRefitClient<ICalendarApiClient>()
			.ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl))
			.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
			});
#else
		// PRODUCTION: Use default SSL validation
		builder.Services.AddHttpClient<IHolidayService, HolidayService>(client =>
		{
			client.BaseAddress = new Uri(baseUrl);
		});

		builder.Services.AddRefitClient<ICalendarApiClient>()
			.ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));
#endif

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
