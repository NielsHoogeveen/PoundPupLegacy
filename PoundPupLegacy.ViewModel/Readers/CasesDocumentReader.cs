namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using Request = CasesDocumentReaderRequest;

public sealed record CasesDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
    public required int Limit { get; init; }
    public required int Offset { get; init; }
}
internal sealed class CasesDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, Cases>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter LimitParameter = new() { Name = "limit" };
    private static readonly NonNullableIntegerDatabaseParameter OffsetParameter = new() { Name = "offset" };

    private static readonly FieldValueReader<Cases> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    private const string SQL = $"""
            with 
            {SharedSql.ACCESSIBLE_PUBLICATIONS_STATUS}
            select
            	jsonb_build_object(
                    'CaseTypes',
                    (
                        select jsonb_agg
                        (
                            jsonb_build_object(
                            'Path', 
            				ba.path,
                            'Title', 
                            nt.name,
                            'Text',
                            ct.text
                            )
                        ) 
                        from case_type ct
                        join node_type nt on nt.id = ct.id
            			join view_node_type_list_action nta on nta.node_type_id = nt.id
            			join basic_action ba on ba.id = nta.basic_action_id
                    ),
            		'NumberOfEntries', 
                    number_of_entries,
            		'Entries', 
                    jsonb_agg(
            			jsonb_build_object
            			(
            				'Title', 
                            title,
                            'Path', 
                            path,
            				'Text', 
                            description,
                            'Date',
                            fuzzy_date,
            				'CaseType',	
                            node_type_name,
            				'HasBeenPublished', 
                            case 
            					when publication_status_id = 0 then false
            					else true
            				end,
                            'PublicationStatusId',
                            publication_status_id,
                            'Tags',
                            (
                                select jsonb_agg
                                (
                                    jsonb_build_object(
                                        'Path',
                                        '/' || nt.viewer_path || '/'  || tn.node_id,
                                        'Title',
                                        n.title,
                                        'NodeTypeName',
                                        nty.tag_label_name
                                    )
                                )
                                from node_term ntm
                                join term t on t.id = ntm.term_id
                                join node n on n.id = t.nameable_id
                                join node_type nt on n.node_type_id = nt.id
                                left join nameable_type nty on nty.id = n.node_type_id and nty.tag_label_name is not null
                                join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id
                                where ntm.node_id = an.id
                                and tn.publication_status_id in 
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
                            )
            			)
            		)
            	) "document"
            from(
            	select
            	*
            	from(
            		select
                    n.id,
            		n.title,
            		nm.description,
            		nt.name node_type_name,
            		n.node_type_id,
                    tn.publication_status_id,
            		COUNT(*) OVER() number_of_entries,
            		'/' || nt.viewer_path || '/' || tn.node_id path,
            		c.fuzzy_date
            		from
            		tenant_node tn
            		join node n on n.id = tn.node_id
                    join node_type nt on n.node_type_id = nt.id
                    join nameable nm on nm.id = n.id
            		join "case" c on c.id = n.id
            		WHERE tn.tenant_id = @tenant_id
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
            	) an
            	order by lower(fuzzy_date) desc
            	LIMIT @limit OFFSET @offset
            ) an
            group by number_of_entries
            """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
            ParameterValue.Create(LimitParameter, request.Limit),
            ParameterValue.Create(OffsetParameter, request.Offset),
        };
    }

    protected override Cases Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
