namespace PoundPupLegacy.Convert;

internal sealed class NodeFileMigratorPPL : MigratorPPL
{
    private readonly IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> _nodeIdReaderFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<FileIdReaderByTenantFileIdRequest, int> _fileIdReaderByTenantFileIdFactory;
    private readonly IEntityCreator<NodeFile> _nodeFileCreator;

    public NodeFileMigratorPPL(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileIdFactory,
        IEntityCreator<NodeFile> nodeFileCreator
    ) : base(databaseConnections)
    {
        _fileIdReaderByTenantFileIdFactory = fileIdReaderByTenantFileIdFactory;
        _nodeIdReaderFactory = nodeIdReaderFactory;
        _nodeFileCreator = nodeFileCreator;
    }

    protected override string Name => "files nodes (ppl)";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await _nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var fileIdReaderByTenantFileId = await _fileIdReaderByTenantFileIdFactory.CreateAsync(_postgresConnection);
        await _nodeFileCreator.CreateAsync(ReadNodeFiles(nodeIdReader, fileIdReaderByTenantFileId), _postgresConnection);
    }
    private async IAsyncEnumerable<NodeFile> ReadNodeFiles(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileId
    )
    {
        var sql = $"""
                SELECT f.fid,
                case
                    when n4.node_id = 19316 then 39429
                    when n4.node_id = 4806 then 17310
                    when n4.node_id = 19326 then 35715
                    when n4.node_id = 19447 then 31716
                    when n4.node_id = 19597 then 12632
                    when n4.node_id = 20242 then 48309
                    when n4.node_id = 4816 then 12635
                    when n4.node_id = 27466 then 12635
                    when n4.node_id = 47130 then 12624
                    when n4.node_id = 74755 then 31586
                    when n4.node_id = 18644 then 14670
                    when n4.node_id = 20899 then 27215
                    when n4.node_id = 6167 then 27214
                    when n4.node_id = 4209 then 12625
                    when n4.node_id = 19256 then 6135
                    when n4.node_id = 22589 then 22591
                    when n4.node_id = 45656 then 41375
                    when n4.node_id = 74730 then 55660
                    when n3.node_id IS NULL then n4.node_id
                    ELSE n3.node_id
                end nid
                FROM files f
                join node n on n.nid = f.nid and type not in ('image', 'video')
                LEFT JOIN(
                    SELECT 
                    cc.nid,
                    n.nid node_id
                    FROM content_type_category_cat cc 
                    JOIN node n3 ON n3.nid = cc.nid AND n3.vid = cc.vid
                    JOIN node n ON n.nid = cc.field_related_page_nid
                    WHERE n.`type` NOT IN ('group')
                ) n3 ON n3.nid = n.nid
                LEFT JOIN(
                    SELECT 
                    case 
                		when n2.nid is NULL then n.nid 
                		ELSE n2.nid
                	end node_id,
                    n.nid
                    FROM node n 
                    LEFT JOIN node n2 ON n2.title = n.title and n2.`type` in ('adopt_person','country_type', 'adopt_orgs', 'case', 'region_facts', 'coerced_adoption_cases', 'child_trafficking', 'child_trafficking_case')
                ) n4 ON n4.nid = n.nid
                WHERE  n.nid NOT IN (
                    22589,
                    54123,
                    45656, 
                    74250,
                    61670
                )
                AND f.fid not in (3197, 3198)
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
                    TenantId = Constants.PPL
                }),
                FileId = await fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileIdRequest {
                    TenantId = Constants.PPL,
                    TenantFileId = reader.GetInt32("fid")
                }),
            };

        }
        await reader.CloseAsync();
    }
}
