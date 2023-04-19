using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;

using Request = TenantNodesReaderRequest;

public sealed record TenantNodesReaderRequest : IRequest
{
}

internal sealed class TenantNodesReaderFactory : EnumerableDatabaseReaderFactory<Request, TenantNode>
{
    private static readonly IntValueReader TenantIdReader = new() { Name = "tenant_id" };
    private static readonly IntValueReader UrlIdReader = new() { Name = "url_id" };
    private static readonly StringValueReader UrlPathReader = new() { Name = "url_path" };

    public override string Sql => SQL;
    const string SQL = """
        select
        tn.tenant_id,
        tn.url_id,
        tn.url_path
        from tenant_node tn 
        where tn.url_path is not null
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] { };
    }

    protected override TenantNode Read(NpgsqlDataReader reader)
    {
        return new TenantNode {
            TenantId = TenantIdReader.GetValue(reader),
            UrlId = UrlIdReader.GetValue(reader),
            UrlPath = UrlPathReader.GetValue(reader),
        };
    }
}
