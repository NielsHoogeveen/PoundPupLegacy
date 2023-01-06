using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    private static async Task MigrateFiles(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await FileCreator.CreateAsync(ReadFiles(mysqlconnection), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }
    private static async IAsyncEnumerable<Model.File> ReadFiles(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                f.fid id,
                f.filepath path,
                f.filename `name`,
                f.filemime mime_type,
                f.filesize size
                FROM `files` f
                WHERE fileName NOT IN ('preview', 'thumbnail', '_original')
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            yield return new Model.File
            {
                Id = reader.GetInt32("id"),
                Path = reader.GetString("path"),
                Name = reader.GetString("name"),
                MimeType = reader.GetString("mime_type"),
                Size = reader.GetInt32("size"),
            };

        }
        await reader.CloseAsync();
    }
}
