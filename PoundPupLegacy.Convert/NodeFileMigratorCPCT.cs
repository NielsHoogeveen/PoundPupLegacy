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
                and not(n.nid = 22185 and f.fid = 1200)
                """;
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {

            yield return new NodeFile {
                NodeId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                    UrlId = reader.GetInt32("nid"),
                    TenantId = Constants.CPCT
                }),
                FileId = await GetFileId(reader.GetInt32("fid")),
            };

        }
        await reader.CloseAsync();
    }

    private async Task<int> GetFileId(int fid)
    {
        (int tenantId, int fileId) = fid switch {
            1200 => (Constants.PPL, 1293),
            1214 => (Constants.PPL, 1242),
            1265 => (Constants.PPL, 1793),
            1419 => (Constants.PPL, 1811),
            1468 => (Constants.PPL, 1820),
            1586 => (Constants.PPL, 1880),
            1647 => (Constants.PPL, 1940),
            1867 => (Constants.PPL, 2022),
            3045 => (Constants.PPL, 1691),
            3507 => (Constants.PPL, 1829),
            3968 => (Constants.PPL, 1801),
            _ => (Constants.CPCT, fid)
        };
        return await _fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileId.Request {
            TenantId = tenantId,
            TenantFileId = fileId
        });
    }
}
