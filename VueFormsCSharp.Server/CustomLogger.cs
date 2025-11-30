namespace VueFormsCSharp.Server;

public static class FileLogger
{
    private static readonly string LogFilePath = Path.Combine(AppContext.BaseDirectory, "app.log");
    private static readonly Lock _lock = new();
    public static void Log(string message)
    {
        var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}";
        lock (_lock) { File.AppendAllText(LogFilePath, logEntry + Environment.NewLine); }
    }
    public static void LogInfo(string message) => Log($"[BACKEND] [INFO]: {message}");
    public static void LogWarning(string message) => Log($"[BACKEND] [WARN]: {message}");
    public static void LogError(string message, Exception? ex = null) => Log($"[BACKEND] [ERROR]: {message} {ex}");
    public static void LogCritical(string message, Exception? ex = null) => Log($"[BACKEND] [CRITICAL]: {message} {ex}");
}
