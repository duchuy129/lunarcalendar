using Microsoft.Extensions.Logging;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.ViewModels;
using LunarCalendar.MobileApp.Views;
using Refit;

namespace LunarCalendar.MobileApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
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

		// Register Services
		builder.Services.AddSingleton<IUserModeService, UserModeService>();
		builder.Services.AddSingleton<ICalendarService, CalendarService>();
		builder.Services.AddSingleton<IHapticService, HapticService>();

		// Register HTTP client for Holiday Service
		builder.Services.AddHttpClient<IHolidayService, HolidayService>();

		// Register Refit API clients
		// Configure base URL based on platform and device type
		string baseUrl;
		if (DeviceInfo.Platform == DevicePlatform.Android && DeviceInfo.DeviceType == DeviceType.Virtual)
		{
			// Android emulator uses special IP to access host machine
			baseUrl = "http://10.0.2.2:5090";
		}
		else if (DeviceInfo.DeviceType == DeviceType.Physical)
		{
			// For physical devices, use your computer's actual IP address on the local network
			baseUrl = "http://10.0.0.72:5090"; // Your computer's IP
		}
		else
		{
			// iOS simulator and other virtual devices use localhost
			baseUrl = "http://localhost:5090";
		}

		Console.WriteLine($"========== CONFIGURING REFIT CLIENT WITH BASE URL: {baseUrl} ==========");
		builder.Services.AddRefitClient<ICalendarApiClient>()
			.ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));

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
