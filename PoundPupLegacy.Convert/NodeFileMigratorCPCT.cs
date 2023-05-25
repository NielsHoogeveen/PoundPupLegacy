namespace PoundPupLegacy.Convert;

internal sealed class NodeFileMigratorCPCT(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    ISingleItemDatabaseReaderFactory<TenantNodeReaderByUrlIdRequest, NewTenantNodeForNewNode> tenantNodeReaderByUrlIdFactory,
    IMandatorySingleItemDatabaseReaderFactory<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileIdFactory,
    IEntityCreatorFactory<NodeFile> nodeFileCreatorFactory
) : MigratorCPCT(
    databaseConnections, 
    nodeIdReaderFactory, 
    tenantNodeReaderByUrlIdFactory
)
{
    protected override string Name => "files nodes (cpct)";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var fileIdReaderByTenantFileId = await fileIdReaderByTenantFileIdFactory.CreateAsync(_postgresConnection);

        await using var nodeFileCreator = await nodeFileCreatorFactory.CreateAsync(_postgresConnection);
        await nodeFileCreator.CreateAsync(ReadNodeFiles(nodeIdReader, fileIdReaderByTenantFileId));
    }
    private async IAsyncEnumerable<NodeFile> ReadNodeFiles(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileId

    )
    {
        var sql = $"""
                SELECT f.fid,
                n.nid
                FROM files f
                join node n on n.nid = f.nid and n.type not in ('image', 'video')
                where f.fid > 1185
                and not(n.nid = 22185 and f.fid = 1200)
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {

            yield return new NodeFile {
                NodeId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    UrlId = reader.GetInt32("nid"),
                    TenantId = Constants.CPCT
                }),
                FileId = await GetFileId(reader.GetInt32("fid"), fileIdReaderByTenantFileId),
            };

        }
        await reader.CloseAsync();
    }

    private async Task<int> GetFileId(
        int fid,
        IMandatorySingleItemDatabaseReader<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileId)
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
        return await fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileIdRequest {
            TenantId = tenantId,
            TenantFileId = fileId
        });
    }
}
