using PoundPupLegacy.CreateModel.Creators;
using File = PoundPupLegacy.CreateModel.File;

namespace PoundPupLegacy.Convert;

internal sealed class FileMigratorPPL(
    IDatabaseConnections databaseConnections,
    IEntityCreatorFactory<File> fileCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "files (ppl)";

    protected override async Task MigrateImpl()
    {
        await using var fileCreator = await fileCreatorFactory.CreateAsync(_postgresConnection);
        await fileCreator.CreateAsync(ReadFiles());
    }
    private async IAsyncEnumerable<File> ReadFiles()
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
                AND f.fid not in (3197, 3198)
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");

            yield return new File {
                Id = null,
                Path = reader.GetString("path"),
                Name = reader.GetString("name"),
                MimeType = reader.GetString("mime_type"),
                Size = reader.GetInt32("size"),
                TenantFiles = new List<TenantFile>{
                    new TenantFile
                    {
                        TenantId = Constants.PPL,
                        FileId = null,
                        TenantFileId = id
                    },
                    new TenantFile
                    {
                        TenantId = Constants.CPCT,
                        FileId = null,
                        TenantFileId = id < 1186 ? id : null
                    }
                }
            };

        }
        await reader.CloseAsync();
    }
}
