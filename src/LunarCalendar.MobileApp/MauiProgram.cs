using Microsoft.Extensions.Logging;
using LunarCalendar.MobileApp.Services;
using LunarCalendar.MobileApp.ViewModels;
using LunarCalendar.MobileApp.Views;

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

		// Register ViewModels
		builder.Services.AddTransient<WelcomeViewModel>();

		// Register Views
		builder.Services.AddTransient<WelcomePage>();
		builder.Services.AddTransient<CalendarPage>();

		return builder.Build();
	}
}
