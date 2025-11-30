using Microsoft.Data.Sqlite;
using System.Text.Json;

namespace VueFormsCSharp.Server;

public static class Db
{
    private readonly static string _connectionString = $"Data Source={Path.Combine(AppContext.BaseDirectory, "app.db")}";
    public record DbResult<T>(bool Success, string? Error, T? Result);
    public class AffectedFormsIds
    {
        public List<long> SucceededIds { get; set; } = [];
        public List<long> FailedIds { get; set; } = [];
    }

    public static DbResult<List<object>> GetAll(string table)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        try
        {
            using var cmd = new SqliteCommand($"SELECT id, data FROM {table};", connection);

            using var reader = cmd.ExecuteReader();
            var allData = new List<object>();

            while (reader.Read())
            {
                allData.Add(new
                {
                    id = reader.GetInt64(0),
                    data = JsonSerializer.Deserialize<JsonElement>(reader.GetString(1))
                });
            }
            return new(true, null, allData);
        }
        catch (Exception ex)
        {
            return new(false, ex.Message, null);
        }
    }
    public static DbResult<long?> Create(string table, JsonElement jsonData)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            using var cmd = new SqliteCommand($"INSERT INTO {table} (data) VALUES (@data) RETURNING id;", connection, transaction);
            cmd.Parameters.AddWithValue("@data", jsonData.GetRawText());

            var newId = Convert.ToInt64(cmd.ExecuteScalar());

            transaction.Commit();

            return new(true, null, newId);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return new(false, ex.Message, null);
        }
    }

    public static DbResult<long> Update(string table, long id, JsonElement jsonData)
    {

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            using var cmd = new SqliteCommand($"UPDATE {table} SET data = @data WHERE id = @id;", connection, transaction);

            cmd.Parameters.AddWithValue("@data", jsonData.GetRawText());
            cmd.Parameters.AddWithValue("@id", id);

            int affected = cmd.ExecuteNonQuery();

            transaction.Commit();

            if (affected <= 0)
                return new(false, "NOT FOUND", -1);

            return new(true, null, id);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return new(false, ex.Message, -1);
        }
    }
    public static DbResult<AffectedFormsIds> Delete(string table, long id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();

        var result = new AffectedFormsIds();
        try
        {
            using var cmd = new SqliteCommand($"DELETE FROM {table} WHERE id = @id RETURNING id;", connection, transaction);

            cmd.Parameters.AddWithValue("@id", id);

            object? res = cmd.ExecuteScalar();

            if (res != null)
                result.SucceededIds.Add(id);
            else
                result.FailedIds.Add(id);

            transaction.Commit();
            return new(true, null, result);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return new(false, ex.Message, null);
        }
    }
}
