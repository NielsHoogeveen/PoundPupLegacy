namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using Request = OrganizationsDocumentReaderRequest;

public sealed record OrganizationsDocumentReaderRequest : IRequest
{
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required int Limit { get; init; }
    public required int Offset { get; init; }
    public required string SearchTerm { get; init; }
    public required SearchOption SearchOption { get; init; }
    public required int? OrganizationTypeId { get; init; }
    public required int? CountryId { get; init; }
}

internal sealed class OrganizationsDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, OrganizationSearch>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter LimitParameter = new() { Name = "limit" };
    private static readonly NonNullableIntegerDatabaseParameter OffsetParameter = new() { Name = "offset" };
    private static readonly NullableIntegerDatabaseParameter OrganizationTypeIdParameter = new() { Name = "organization_type_id" };
    private static readonly NullableIntegerDatabaseParameter CountryIdParameter = new() { Name = "country_id" };
    private static readonly SearchOptionDatabaseParameter PatternParameter = new() { Name = "pattern" };

    private static readonly FieldValueReader<OrganizationSearch> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        with
        {SharedSql.ACCESSIBLE_PUBLICATIONS_STATUS}
        select
            jsonb_build_object(
                'Countries', 
                (
                    select
                        jsonb_agg(
                            jsonb_build_object(
            	                'Name', "name",
            	                'Id', "url_id"
                            )
                        ) "document"
                        from(
                            select
                            distinct
                            n.title "name",
                            tn.url_id
                            from organization o
                            join location_locatable ll on ll.locatable_id = o.id
                            join location l on l.id = ll.location_id
                            join node n on n.id = l.country_id
                            join tenant_node tn on tn.node_id = n.id and tn.tenant_id = 1
                            ORDER BY n.title
                        ) x
                    ),
                'OrganizationTypes', 
                (
                    select
                        jsonb_agg(
                            jsonb_build_object(
            	                'Name', "name",
            	                'Id', "id"
                            )
                        ) "document"
                        from(
                            select
                                distinct
                                n.title "name",
                                tn.node_id "id"
                            from organization_type ot
                            join node n on n.id = ot.id
                            join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id
                            ORDER BY n.title
                        ) x
                ),
                'Organizations', 
                (
                    select
            	        jsonb_build_object(
            	            'NumberOfEntries', 
                            number_of_entries,
            	            'Entries', 
                            jsonb_agg(
            	                jsonb_build_object(
            	                    'Path', 
                                    url_path,
            	                    'Title', 
                                    title,
            	                    'HasBeenPublished', 
                                    case 
            		                    when publication_status_id = 0 then false
            		                    else true
            	                    end
            	                )
                            )
            	        )
                        from
                        (
            	            select
                                distinct
            	                tn.id,
            	                n.title,
            	                n.node_type_id,
            	                tn.tenant_id,
                                tn.publication_status_id,
            	                tn.node_id,
            	                n.publisher_id,
            	                n.created_date_time,
            	                n.changed_date_time,
            	                tn.url_id,
            	                count(tn.id) over() number_of_entries,
            	                case 
            		                when tn.url_path is null then '/node/' || tn.url_id
            		                else '/' || url_path
            	                end url_path,
            	                tn.subgroup_id
            	            from tenant_node tn
            	            join node n on n.id = tn.node_id
            	            join organization o on o.id = n.id
                            left join (
                                select 
                                ll.locatable_id,
                                tn.url_id
                                from location_locatable ll 
                                join location l on l.id = ll.location_id
                                join tenant_node tn on tn.node_id = l.country_id
                                where tn.tenant_id = @tenant_id
                            ) ll on ll.locatable_id = o.id
                            left join organization_organization_type oot on oot.organization_id = o.id
            	            WHERE tn.tenant_id = @tenant_id and n.title ilike @pattern 
                            AND tn.publication_status_id in 
                            (
                                select 
                                id 
                                from accessible_publication_status 
                                where tenant_id = tn.tenant_id 
                                and (
                                    subgroup_id = tn.subgroup_id 
                                    or subgroup_id is null and tn.subgroup_id is null
                                )
                            )
                            AND (@country_id is null or ll.url_id = @country_id)
                            AND (@organization_type_id is null or oot.organization_type_id = @organization_type_id)
            	            ORDER BY n.title
                            LIMIT @limit OFFSET @offset
                        ) an
                        group by number_of_entries
                 )
            ) document
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
            ParameterValue.Create(LimitParameter, request.Limit),
            ParameterValue.Create(OffsetParameter, request.Offset),
            ParameterValue.Create(PatternParameter, (request.SearchTerm, request.SearchOption)),
            ParameterValue.Create(OrganizationTypeIdParameter, request.OrganizationTypeId),
            ParameterValue.Create(CountryIdParameter, request.CountryId),
        };
    }

    protected override OrganizationSearch Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
