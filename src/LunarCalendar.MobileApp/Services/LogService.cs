using System.Text;

namespace LunarCalendar.MobileApp.Services;

/// <summary>
/// Lightweight file-based logging service for production diagnostics
/// Logs to app-private storage with automatic rotation
/// </summary>
public interface ILogService
{
    void LogInfo(string message, string? source = null);
    void LogWarning(string message, string? source = null);
    void LogError(string message, Exception? exception = null, string? source = null);
    Task<string> GetLogsAsync();
    Task ClearLogsAsync();
}

public class LogService : ILogService
{
    private string? _logDirectory;
    private string? _logFileName;
    private readonly int _maxLogDays = 7; // Keep logs for 7 days
    private readonly SemaphoreSlim _writeLock = new(1, 1);
    private bool _initialized = false;
    private readonly object _initLock = new();

    public LogService()
    {
        // CRITICAL iOS 26.1 FIX: Don't access FileSystem in constructor
        // FileSystem APIs not available during DI container creation
        // Will initialize on first use
    }

    private void EnsureInitialized()
    {
        if (_initialized) return;

        lock (_initLock)
        {
            if (_initialized) return;

            try
            {
                _logDirectory = Path.Combine(FileSystem.AppDataDirectory, "Logs");
                _logFileName = $"app-{DateTime.Now:yyyy-MM-dd}.log";
                Directory.CreateDirectory(_logDirectory);
                RotateOldLogs();
                _initialized = true;
            }
            catch
            {
                // If initialization fails, just use Debug output
                _initialized = true;
            }
        }
    }

    public void LogInfo(string message, string? source = null)
    {
        EnsureInitialized();
        WriteLog("INFO", message, null, source);
    }

    public void LogWarning(string message, string? source = null)
    {
        EnsureInitialized();
        WriteLog("WARN", message, null, source);
    }

    public void LogError(string message, Exception? exception = null, string? source = null)
    {
        EnsureInitialized();
        WriteLog("ERROR", message, exception, source);
    }

    private void WriteLog(string level, string message, Exception? exception, string? source)
    {
        try
        {
            // If not initialized, just use debug output
            if (_logDirectory == null || _logFileName == null)
            {
                System.Diagnostics.Debug.WriteLine($"{level}: {message}");
                if (exception != null)
                    System.Diagnostics.Debug.WriteLine($"Exception: {exception}");
                return;
            }

            // Use async fire-and-forget for non-blocking logging
            Task.Run(async () =>
            {
                await _writeLock.WaitAsync();
                try
                {
                    var logPath = Path.Combine(_logDirectory!, _logFileName!);
                    var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    var sourceInfo = source != null ? $" [{source}]" : "";
                    
                    var logEntry = new StringBuilder();
                    logEntry.AppendLine($"{timestamp} {level}{sourceInfo}: {message}");
                    
                    if (exception != null)
                    {
                        logEntry.AppendLine($"  Exception: {exception.GetType().Name}");
                        logEntry.AppendLine($"  Message: {exception.Message}");
                        logEntry.AppendLine($"  StackTrace: {exception.StackTrace}");
                        
                        if (exception.InnerException != null)
                        {
                            logEntry.AppendLine($"  InnerException: {exception.InnerException.Message}");
                        }
                    }
                    
                    await File.AppendAllTextAsync(logPath, logEntry.ToString());
                }
                finally
                {
                    _writeLock.Release();
                }
            });
        }
        catch (Exception ex)
        {
            // Fallback to console if file logging fails - logging should never crash the app
            Console.WriteLine($"[LogService] Failed to write log: {ex.Message}");
            Console.WriteLine($"[LogService] Original log: {level} - {message}");
        }
    }

    public async Task<string> GetLogsAsync()
    {
        var logs = new StringBuilder();
        
        try
        {
            var logFiles = Directory.GetFiles(_logDirectory, "app-*.log")
                .OrderByDescending(f => f)
                .Take(7); // Last 7 days
            
            foreach (var logFile in logFiles)
            {
                var fileName = Path.GetFileName(logFile);
                logs.AppendLine($"=== {fileName} ===");
                logs.AppendLine(await File.ReadAllTextAsync(logFile));
                logs.AppendLine();
            }
        }
        catch (Exception ex)
        {
            logs.AppendLine($"Error reading logs: {ex.Message}");
        }
        
        return logs.ToString();
    }

    public async Task ClearLogsAsync()
    {
        EnsureInitialized();
        if (_logDirectory == null) return;

        await _writeLock.WaitAsync();
        try
        {
            foreach (var file in Directory.GetFiles(_logDirectory, "app-*.log"))
            {
                File.Delete(file);
            }
        }
        finally
        {
            _writeLock.Release();
        }
    }

    private void RotateOldLogs()
    {
        try
        {
            if (_logDirectory == null) return;

            var cutoffDate = DateTime.Now.AddDays(-_maxLogDays);
            var logFiles = Directory.GetFiles(_logDirectory, "app-*.log");
            
            foreach (var logFile in logFiles)
            {
                var fileInfo = new FileInfo(logFile);
                if (fileInfo.CreationTime < cutoffDate)
                {
                    File.Delete(logFile);
                }
            }
        }
        catch
        {
            // Silently fail - rotation is best-effort
        }
    }
}
