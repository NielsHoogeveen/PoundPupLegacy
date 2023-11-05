namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = SubgroupsDocumentReaderRequest;

public sealed record SubgroupsDocumentReaderRequest : IRequest
{
    public required int UserId { get; init; }
    public required int SubgroupId { get; init; }
    public required int Limit { get; init; }
    public required int Offset { get; init; }

}
internal sealed class SubgroupsDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, SubgroupPagedList>
{
    private static readonly NonNullableIntegerDatabaseParameter SubgroupIdParameter = new() { Name = "subgroup_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NullableIntegerDatabaseParameter LimitParameter = new() { Name = "limit" };
    private static readonly NullableIntegerDatabaseParameter OffsetParameter = new() { Name = "offset" };

    private static readonly FieldValueReader<SubgroupPagedList> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        with
        {SharedSql.ACCESSIBLE_PUBLICATIONS_STATUS}
        select
        jsonb_build_object(
            'NumberOfEntries',
            number_of_entries,
            'Name',
            group_name,
            'Description',
            group_description,
            'Entries',
            jsonb_agg(
            	jsonb_build_object(
            		'Title',
            		title,
            		'Path',
            		path,
            		'Authoring', 
            		jsonb_build_object(
            			'Id', 
                        publisher_id, 
            			'Name', 
                        publisher_name,
            			'CreatedDateTime', 
                        created_date_time,
            			'ChangedDateTime', 
                        changed_date_time
            		),
            		'HasBeenPublished',
            		case 
            			when publication_status_id = 0 then false
            			else true
            		end,
                    'PublicationStatusId',
                    publication_status_id
            	)
                order by changed_date_time desc
            )
        ) document
        from(
        select
            id,
            title,
            node_type_id,
            path,
            tenant_id,
            node_id,
            created_date_time,
            changed_date_time,
            node_id,
            publisher_id,
            publisher_name,
            group_name,
            group_description,
            count(id) over() number_of_entries,
            subgroup_id,
            publication_status_id
        from(
            select
            	tn.id,
            	n.title,
            	n.node_type_id,
            	tn.tenant_id,
            	tn.node_id,
            	n.created_date_time,
            	n.publisher_id,
            	n.changed_date_time,
            	p.name publisher_name,
            	count(tn.id) over() number_of_entries,
                ug.name group_name,
                ug.description group_description,
            	'/' || nt.viewer_path || '/' || tn.node_id path,
            	tn.subgroup_id,
            	tn.publication_status_id
            	from tenant_node tn
            	join subgroup s on s.id = tn.subgroup_id
                join user_group ug on ug.id = s.id
            	join node n on n.id = tn.node_id
                join node_type nt on nt.id = n.node_type_id
            	JOIN publisher p on p.id = n.publisher_id
            	WHERE s.id = @subgroup_id
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
            order by changed_date_time desc
            LIMIT @limit OFFSET @offset
        ) an
        group by 
        group_name, 
        group_description,
        number_of_entries
        """
        ;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(SubgroupIdParameter, request.SubgroupId),
            ParameterValue.Create(UserIdParameter, request.UserId),
            ParameterValue.Create(LimitParameter, request.Limit),
            ParameterValue.Create(OffsetParameter, request.Offset),
        };
    }
    protected override SubgroupPagedList Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
