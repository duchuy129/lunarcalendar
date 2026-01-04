namespace LunarCalendar.MobileApp.Helpers;

/// <summary>
/// Helper to access DI services from non-DI contexts (e.g., App.xaml.cs)
/// </summary>
public static class ServiceHelper
{
    public static T? GetService<T>() where T : class
    {
        try
        {
            // Try to get from Application.Current
            if (Application.Current?.Handler?.MauiContext != null)
            {
                return Application.Current.Handler.MauiContext.Services.GetService(typeof(T)) as T;
            }

            return null;
        }
        catch
        {
            return null;
        }
    }
}
