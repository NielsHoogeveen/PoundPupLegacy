using PoundPupLegacy.Db;
using System.Data;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal sealed class NodeFileMigratorCPCT : CPCTMigrator
{

    public NodeFileMigratorCPCT(MySqlToPostgresConverter converter) : base(converter)
    {

    }

    protected override string Name => "files nodes (cpct)";

    protected override async Task MigrateImpl()
    {
        await NodeFileCreator.CreateAsync(ReadNodeFiles(), _postgresConnection);
    }
    private async IAsyncEnumerable<NodeFile> ReadNodeFiles()
    {

        var sql = $"""
                SELECT f.fid,
                n.nid
                FROM files f
                join node n on n.nid = f.nid and n.type not in ('image', 'video')
                where f.fid > 1185
                """;
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {

            yield return new NodeFile
            {
                NodeId = await _nodeIdReader.ReadAsync(Constants.CPCT, reader.GetInt32("nid")),
                FileId = await _fileIdReaderByTenantFileId.ReadAsync(Constants.CPCT, reader.GetInt32("fid")),
            };

        }
        await reader.CloseAsync();
    }
}
