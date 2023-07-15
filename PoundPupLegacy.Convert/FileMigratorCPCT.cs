using PoundPupLegacy.DomainModel;
using PoundPupLegacy.DomainModel.Creators;
using PoundPupLegacy.DomainModel.Readers;
using File = PoundPupLegacy.DomainModel.File;

namespace PoundPupLegacy.Convert;

internal sealed class FileMigratorCPCT(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    ISingleItemDatabaseReaderFactory<TenantNodeReaderByUrlIdRequest, TenantNode.ToCreate.ForExistingNode> tenantNodeReaderByUrlIdFactory,
    IEntityCreatorFactory<File> fileCreatorFactory
) : MigratorCPCT(
    databaseConnections, 
    nodeIdReaderFactory, 
    tenantNodeReaderByUrlIdFactory
)
{
    protected override string Name => "files (cpct)";

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
                and f.fid > 1185
                and fid not in (
                    1200,
                    1214,
                    1265,
                    1419,
                    1468,
                    1586,
                    1647,
                    1867,
                    3045,
                    3507,
                    3968
                )
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");

            yield return new File {
                Identification = new Identification.Possible {
                    Id = null,
                },
                Path = GetPath(id, reader.GetString("path")),
                Name = GetName(id, reader.GetString("name")),
                MimeType = reader.GetString("mime_type"),
                Size = reader.GetInt32("size"),
                TenantFiles = new List<TenantFile>{
                    new TenantFile
                    {
                        TenantId = Constants.CPCT,
                        FileId = null,
                        TenantFileId = id
                    },
                    new TenantFile
                    {
                        TenantId = Constants.PPL,
                        FileId = null,
                        TenantFileId = null
                    },
                }
            };

        }
        await reader.CloseAsync();
    }
    private string GetName(int id, string name)
    {
        return id switch {
            3913 => "Günther Verheugen 25-9-2017.pdf",
            2084 => "Panţîru%2C_Maria-Cristina.pdf",
            _ => name
        };
    }
    private string GetPath(int id, string path)
    {
        return id switch {
            3913 => "files/Günther Verheugen 25-9-2017.pdf",
            2084 => "files/Panţîru%2C_Maria-Cristina.pdf",
            _ => path
        };
    }
}
