using PoundPupLegacy.DomainModel.Readers;

namespace PoundPupLegacy.Convert;
internal sealed class AdultAftermathMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory
) : MigratorPPL(databaseConnections)
{

    protected override string Name => "adult aftermath";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        var adultAfterMathEntries = await ReadAdultAftermaths(nodeIdReader).ToListAsync();
        await UpdateAdultAftermathEntries(adultAfterMathEntries);
    }

    private async Task UpdateAdultAftermathEntries(IEnumerable<(int, int)> entries)
    {
        var sql = $"""
                UPDATE tenant_node 
                set 
                publication_status_id = @publication_status_id,
                subgroup_id = {Constants.ADULT_AFTERMATH}
                where tenant_id = {Constants.PPL} and node_id = @node_id
                """;
        using var command = _postgresConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        command.Parameters.Add("publication_status_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("node_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        foreach (var (node_id, publication_status_id) in entries) {
            command.Parameters["node_id"].Value = node_id;
            command.Parameters["publication_status_id"].Value = publication_status_id;
            command.ExecuteNonQuery();
        }

    }

    private async IAsyncEnumerable<(int, int)> ReadAdultAftermaths(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
                SELECT
                 n.nid,
                 case
                 	when STATUS = 0 then 0
                 	when oa.is_public = true then 1
                 	ELSE 2
                 END publication_status
                 FROM og
                 JOIN node_access na ON na.gid = og.nid
                 JOIN node n ON n.nid = na.nid
                 JOIN og_ancestry oa ON oa.group_nid = og.nid AND oa.nid = n.nid
                 WHERE og.nid = 17146
                 AND n.`type` <> 'uprofile' AND n.`type` not in ('book_page', 'website', 'category_cat')
                 and n.nid not in (48006, 49927, 50280)
                 and uid  <> 0
                """;
        using var command = _mySqlConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;


        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = reader.GetInt32(0),
                TenantId = Constants.PPL
            });
            yield return (id, reader.GetInt32(1));

        }
        await reader.CloseAsync();
    }
}
