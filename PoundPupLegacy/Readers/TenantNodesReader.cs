using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;
internal sealed class TenantNodesReaderFactory : DatabaseReaderFactory<TenantNodesReader>
{
    public override string Sql => SQL;
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
    public TenantNodesReader(NpgsqlCommand command) : base(command)
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
