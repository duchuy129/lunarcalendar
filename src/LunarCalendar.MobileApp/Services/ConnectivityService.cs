namespace LunarCalendar.MobileApp.Services;

public class ConnectivityService : IConnectivityService
{
    public event EventHandler<bool>? ConnectivityChanged;

    public ConnectivityService()
    {
        Connectivity.ConnectivityChanged += OnConnectivityChanged;
    }

    public bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

    public async Task<bool> CheckConnectivityAsync()
    {
        await Task.Delay(10); // Small delay to ensure network state is current
        return IsConnected;
    }

    private void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
    {
        var isConnected = e.NetworkAccess == NetworkAccess.Internet;
        ConnectivityChanged?.Invoke(this, isConnected);
    }
}
