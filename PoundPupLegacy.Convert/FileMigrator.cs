using PoundPupLegacy.Db;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class FileMigrator : PPLMigrator
{

    public FileMigrator(MySqlToPostgresConverter converter) : base(converter)
    {

    }

    protected override string Name => "files";

    protected override async Task MigrateImpl()
    {
        await FileCreator.CreateAsync(ReadFiles(), _postgresConnection);
    }
    private async IAsyncEnumerable<Model.File> ReadFiles()
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
        using var readCommand = MysqlConnection.CreateCommand();
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
