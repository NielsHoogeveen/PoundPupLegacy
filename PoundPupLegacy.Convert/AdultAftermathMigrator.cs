using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;
internal sealed class AdultAftermathMigrator : PPLMigrator
{
    public AdultAftermathMigrator(MySqlToPostgresConverter converter) : base(converter)
    {

    }

    protected override string Name => "adult aftermath";

    protected override async Task MigrateImpl()
    {
        var adultAfterMathEntries = await ReadAdultAftermaths().ToListAsync();
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
        foreach(var (node_id, publication_status_id) in entries) {
            command.Parameters["node_id"].Value = node_id;
            command.Parameters["publication_status_id"].Value = publication_status_id;
            command.ExecuteNonQuery();
        }

    }

    private async IAsyncEnumerable<(int, int)> ReadAdultAftermaths()
    {

        var sql = $"""
                SELECT
                n.nid,
                case
                	when STATUS = 0 then 2
                	ELSE 1
                END publication_status
                FROM og
                JOIN node_access na ON na.gid = og.nid
                JOIN node n ON n.nid = na.nid
                WHERE og.nid = 17146
                AND n.`type` <> 'uprofile' AND n.`type` <> 'book_page'
                and n.nid not in (48006, 49927, 50280, 35801)
                and uid  <> 0
                """;
        using var command = MysqlConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;


        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var id = await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32(0));
            yield return (id, reader.GetInt32(1));

        }
        await reader.CloseAsync();
    }
}
