using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using System.Data;

namespace PoundPupLegacy.Readers;
internal sealed class TenantNodesReaderFactory : IDatabaseReaderFactory<TenantNodesReader>
{
    public async Task<TenantNodesReader> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = SQL;
        await command.PrepareAsync();
        return new TenantNodesReader(command);
    }
    const string SQL = """
        select
        tn.tenant_id,
        tn.url_id,
        tn.url_path
        from tenant_node tn 
        where tn.url_path is not null
        """;


}
internal sealed class TenantNodesReader : EnumerableDatabaseReader<TenantNodesReader.Request, TenantNode>
{
    public record Request
    {

    }
    internal TenantNodesReader(NpgsqlCommand command) : base(command)
    {
    }

    public override async IAsyncEnumerable<TenantNode> ReadAsync(Request request)
    {
        await using var reader = await _command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            yield return new TenantNode {
                TenantId = reader.GetInt32(0),
                UrlId = reader.GetInt32(1),
                UrlPath = reader.GetString(2)
            };

        }
    }

}
