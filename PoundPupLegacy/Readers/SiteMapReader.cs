using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;

using Request = SiteMapReaderRequest;

internal sealed record SiteMapReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int Index { get; init; }
}

internal sealed class SiteMapReaderFactory : EnumerableDatabaseReaderFactory<Request, SiteMapElement>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter IndexParameter = new() { Name = "index" };

    private static readonly StringValueReader Path = new() { Name = "path" };
    private static readonly NullableDateTimeValueReader LastChanged = new() { Name = "last_changed" };
    private static readonly NullableStringValueReader ChangeFrequency = new() { Name = "change_frequency" };
    
    
    public override string Sql => SQL;

    const string SQL = """
        SELECT 
        'http://' || t.domain_name || ba.path path,
        null last_changed,
        'always' change_frequency
        	FROM public.tenant t
            join publishing_user_group pug on pug.id = t.id
        	join access_role_privilege arp on arp.access_role_id = pug.access_role_id_not_logged_in
        	join basic_action ba on ba.id = arp.action_id
        	where t.id = @tenant_id
        	and ba.path not ilike '%{%'
        union
        select
        case 
        	when tn.url_path is not null then 'http://' || t.domain_name || '/' || url_path
        	else 'http://' || t.domain_name || '/node/' || tn.url_id
        end path,
        n.changed_date_time last_changed,
        null change_frequency
        from tenant_node tn
        join tenant t on t.id = tn.tenant_id
        join node n on n.id = tn.node_id
        join node_type nt on nt.id = n.node_type_id
        where t.id = 1
        and nt.has_viewing_support = true
        and tn.publication_status_id = @tenant_id
        order by path
        limit 5000 offset @index * 5000
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(IndexParameter, request.Index),};
    }

    protected override SiteMapElement Read(NpgsqlDataReader reader)
    {
        return new SiteMapElement {
            Path = Path.GetValue(reader),
            LastChanged = LastChanged.GetValue(reader),
            ChangeFrequency = ChangeFrequency.GetValue(reader),
        };
    }
}
