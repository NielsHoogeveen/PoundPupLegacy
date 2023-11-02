using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.ViewModel.Readers;

using Request = RecentUserPostsDocumentReaderRequest;
public sealed record RecentUserPostsDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
    public required int UserIdPublisher { get; init; }
    public required int Limit { get; init; }
    public required int Offset { get; init; }
}
internal sealed class RecentUserPostsDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, RecentPosts>
{

    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdPublisherParameter = new() { Name = "user_id_publisher" };
    private static readonly NonNullableIntegerDatabaseParameter LimitParameter = new() { Name = "limit" };
    private static readonly NonNullableIntegerDatabaseParameter OffsetParameter = new() { Name = "offset" };

    private static readonly FieldValueReader<RecentPosts> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;


    private const string SQL = $"""
        with
        {SharedSql.ACCESSIBLE_PUBLICATIONS_STATUS}
        select
            jsonb_build_object(
                'NumberOfEntries', 
                number_of_entries,
                'Entries', 
                jsonb_agg(
                    jsonb_build_object(
                        'Title', 
                        title,
                        'Path', 
                        path,
                        'Publisher', 
                        publisher_name,
                        'NodeType', 
                        node_type,
                        'Subgroup',
                        subgroup_name,
                        'CreatedDateTime', 
                        created_date_time,
                        'LastChangedDateTime', 
                        changed_date_time,
                        'PublicationStatusId',
                        publication_status_id
                    )
                ) 
            ) document
        from (
            select
            *
            from(
                select
                title,
                path,
                publisher_name,
                node_type,
                subgroup_name,
                publication_status_id,
                created_date_time,
                changed_date_time,
                count(*) over() number_of_entries
                from(
        	        select
        	        n.title,
        	        p.name publisher_name,
        	        case 
        		        when tn.url_path is null then '/node/' || tn.url_id
        		        else '/' || tn.url_path
        	        end path,
                    n.created_date_time,
        	        n.changed_date_time,
        	        nt.name node_type,
                    tn.publication_status_id,
                    ug.name subgroup_name
        	        from node n
        	        join tenant_node tn on tn.node_id = n.id
                    left join user_group ug on ug.id = tn.subgroup_id
        	        join publisher p on p.id = n.publisher_id
        	        join node_type nt on nt.id = n.node_type_id
                    where tn.tenant_id = @tenant_id
                    and p.id = @user_id_publisher
                    and nt.author_specific
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
                    and nt.has_viewing_support = true
        	        order by n.changed_date_time desc
                ) x
                LIMIT @limit OFFSET @offset
            ) x
        ) x
        group by 
        number_of_entries
        
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
            ParameterValue.Create(UserIdPublisherParameter, request.UserIdPublisher),
            ParameterValue.Create(LimitParameter, request.Limit),
            ParameterValue.Create(OffsetParameter, request.Offset),
        };
    }

    protected override RecentPosts Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
