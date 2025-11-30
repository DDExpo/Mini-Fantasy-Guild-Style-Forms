using Microsoft.Data.Sqlite;

namespace VueFormsCSharp.Server;

public static class DbLoggerInitializer
{
    public static void InitializeLogDb()
    {
        var dbFile = Path.Combine(AppContext.BaseDirectory, "app.db");
        var logFile = Path.Combine(AppContext.BaseDirectory, "app.log");

        File.WriteAllText(logFile, string.Empty);
        FileLogger.LogInfo($"Log file created: {logFile}");

        if (!File.Exists(dbFile))
        {
            FileLogger.LogInfo($"Database file not found. Creating: {dbFile}");

            using var connection = new SqliteConnection($"Data Source={dbFile}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS forms (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    data TEXT
                );
            ";
            cmd.ExecuteNonQuery();

            FileLogger.LogInfo($"Database initialized: {dbFile}");
        }
        else
        {
            FileLogger.LogInfo($"Database file exists: {dbFile}");
        }
    }
}