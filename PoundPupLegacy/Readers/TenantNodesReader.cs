using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using System.Data;
using System.Data.Common;

namespace PoundPupLegacy.Readers;

public class TenantNodesReader: DatabaseReader, IEnumerableDatabaseReader<TenantNodesReader, TenantNodesReader.TenantNodesRequest, TenantNode>
{
    public record TenantNodesRequest
    {

    }
    private TenantNodesReader(NpgsqlCommand command) : base(command)
    {
    }

    public async IAsyncEnumerable<TenantNode> ReadAsync(TenantNodesRequest request)
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

    public async static Task<TenantNodesReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
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
