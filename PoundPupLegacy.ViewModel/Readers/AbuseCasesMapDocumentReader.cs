namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using Request = AbuseCasesMapDocumentReaderRequest;

public sealed record AbuseCasesMapDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
}
internal sealed class AbuseCasesMapDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, AbuseCaseMapEntry[]>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };

    private static readonly FieldValueReader<AbuseCaseMapEntry[]> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        select
        jsonb_agg(
            jsonb_build_object(
            'Path',
            '/' || viewer_path || '/' || id,
            'Title',
            title,
            'Latitude',
            latitude,
            'Longitude',
            longitude
            ) 
        ) document
        from(
            select
            id,
            title,
            viewer_path,
            min(latitude) latitude,
            min(longitude) longitude
            from(
                select
                ac.id,
                n.title,
                nt.viewer_path,
                case 
        	        when ll.latitude is not null then ll.latitude 
        	        else c.latitude
                end latitude,
                case 
        	        when ll.longitude is not null then ll.longitude 
        	        else c.longitude
                end longitude
                from abuse_case ac
                join node n on n.id = ac.id
                join node_type nt on nt.id = n.node_type_id
                left join (
        	        select
        	        ll.locatable_id,
        	        l.latitude,
        	        l.longitude
        	        from location_locatable ll
        	        join location l on l.id = ll.location_id 
        	        join united_states_state uss on uss.id = l.subdivision_id
                ) ll on ll.locatable_id = ac.id
                left join(
        	        select
        	        nt.node_id,
        	        c.latitude,
        	        c.longitude
        	        from united_states_city c
        	        join term t on t.nameable_id = c.id
        	        join node_term nt on nt.term_id = t.id
                ) c on c.node_id = ac.id
                join tenant_node tn on tn.tenant_id = @tenant_id and tn.node_id = n.id
                where tn.publication_status_id in 
                (
                    select 
                    publication_status_id  
                    from user_publication_status 
                    where tenant_id = tn.tenant_id 
                    and user_id = @user_id
                    and (
                        subgroup_id = tn.subgroup_id 
                        or subgroup_id is null and tn.subgroup_id is null
                    )
                )
            ) x
            where latitude is not null and longitude is not null
            group by
            id,
            title,
            viewer_path
        ) x
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
        };
    }

    protected override AbuseCaseMapEntry[] Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
