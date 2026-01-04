using Microsoft.Extensions.Logging;

namespace LunarCalendar.MobileApp.Services;

/// <summary>
/// ILoggerProvider that integrates LogService with Microsoft.Extensions.Logging
/// This allows framework-level logs (MAUI, SQLite, etc.) to flow through LogService
/// </summary>
public class LogServiceProvider : ILoggerProvider
{
    private readonly ILogService _logService;

    public LogServiceProvider(ILogService logService)
    {
        _logService = logService;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new LogServiceLogger(_logService, categoryName);
    }

    public void Dispose()
    {
        // LogService is singleton, don't dispose
    }
}

internal class LogServiceLogger : ILogger
{
    private readonly ILogService _logService;
    private readonly string _categoryName;

    public LogServiceLogger(ILogService logService, string categoryName)
    {
        _logService = logService;
        _categoryName = categoryName;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        // Log Information and above (skip Debug and Trace to reduce noise)
        return logLevel >= LogLevel.Information;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
        Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var message = formatter(state, exception);

        switch (logLevel)
        {
            case LogLevel.Critical:
            case LogLevel.Error:
                _logService.LogError(message, exception, _categoryName);
                break;
            case LogLevel.Warning:
                _logService.LogWarning(message, _categoryName);
                break;
            case LogLevel.Information:
            case LogLevel.Debug:
            case LogLevel.Trace:
                _logService.LogInfo(message, _categoryName);
                break;
        }
    }
}
