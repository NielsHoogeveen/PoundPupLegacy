using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;

using Request = TenantNodesReaderRequest;
using Factory = TenantNodesReaderFactory;
using Reader = TenantNodesReader;

public sealed record TenantNodesReaderRequest : IRequest
{
}

internal sealed class TenantNodesReaderFactory : EnumerableDatabaseReaderFactory<Request, TenantNode, Reader>
{
    internal static readonly IntValueReader TenantIdReader = new() { Name = "tenant_id" };
    internal static readonly IntValueReader UrlIdReader = new() { Name = "url_id" };
    internal static readonly StringValueReader UrlPathReader = new() { Name = "url_path" };

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
internal sealed class TenantNodesReader : EnumerableDatabaseReader<Request, TenantNode>
{
    public TenantNodesReader(NpgsqlCommand command) : base(command)
    {
    }
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] { };
    }

    protected override TenantNode Read(NpgsqlDataReader reader)
    {
        return new TenantNode {
            TenantId = Factory.TenantIdReader.GetValue(reader),
            UrlId = Factory.UrlIdReader.GetValue(reader),
            UrlPath = Factory.UrlPathReader.GetValue(reader),
        };
    }
}
