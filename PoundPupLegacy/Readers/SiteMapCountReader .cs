using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;

using Request = SiteMapCountReaderRequest;

internal sealed record SiteMapCountReaderRequest : IRequest
{
    public required int TenantId { get; init; }
}
internal sealed record SiteMapCount: IRequest
{
    public required int Count { get; init; }
}

internal sealed class SiteMapCountReaderFactory : MandatorySingleItemDatabaseReaderFactory<Request, SiteMapCount>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };

    private static readonly IntValueReader Count = new() { Name = "count" };
    
    
    public override string Sql => SQL;

    const string SQL = """
        select 
            count(*) count
        FROM(
            SELECT 
                ba.id
            FROM public.tenant t
            join access_role_privilege arp on arp.access_role_id = t.access_role_id_not_logged_in
            join basic_action ba on ba.id = arp.action_id
            where t.id = @tenant_id
            and ba.path not ilike '%{%'
            union
            select
                n.id
            from tenant_node tn
            join tenant t on t.id = tn.tenant_id
            join node n on n.id = tn.node_id
            where t.id = 1
            and tn.publication_status_id = @tenant_id
        ) as x
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
        };
    }

    protected override SiteMapCount Read(NpgsqlDataReader reader)
    {
        return new SiteMapCount {
            Count = Count.GetValue(reader),
        };
    }

    protected override string GetErrorMessage(Request request)
    {
        throw new NotImplementedException();
    }
}
