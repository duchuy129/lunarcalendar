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

		// Register HTTP client for Holiday Service
		builder.Services.AddHttpClient<IHolidayService, HolidayService>();

		// Register Refit API clients
		// Android emulator uses 10.0.2.2 to access host machine's localhost
		var baseUrl = DeviceInfo.Platform == DevicePlatform.Android
			? "http://10.0.2.2:5090"
			: "http://localhost:5090";
		builder.Services.AddRefitClient<ICalendarApiClient>()
			.ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));

		// Register ViewModels
		builder.Services.AddTransient<WelcomeViewModel>();
		builder.Services.AddTransient<CalendarViewModel>();
		builder.Services.AddTransient<HolidayDetailViewModel>();

		// Register Views
		builder.Services.AddTransient<WelcomePage>();
		builder.Services.AddTransient<CalendarPage>();
		builder.Services.AddTransient<HolidayDetailPage>();

		return builder.Build();
	}
}
