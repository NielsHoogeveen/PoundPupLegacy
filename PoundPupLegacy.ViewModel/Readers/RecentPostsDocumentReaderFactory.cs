using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.ViewModel.Readers;

using Request = RecentPostsDocumentReaderRequest;
public sealed record RecentPostsDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
    public required int Limit { get; init; }
    public required int Offset { get; init; }
}
internal sealed class RecentPostsDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, RecentPosts>
{

    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter LimitParameter = new() { Name = "limit" };
    private static readonly NonNullableIntegerDatabaseParameter OffsetParameter = new() { Name = "offset" };

    private static readonly FieldValueReader<RecentPosts> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;


    private const string SQL = """
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
                        'DateTime', 
                        changed_date_time
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
        	        n.changed_date_time,
        	        nt.name node_type,
        	        case
        	        when tn.publication_status_id = 0 then (
        		        select
        			        case 
        				        when count(*) > 0 then 0
        				        else -1
        			        end status
        		        from user_group_user_role_user ugu
        		        join user_group ug on ug.id = ugu.user_group_id
        		        WHERE ugu.user_group_id = 
        		        case
        			        when tn.subgroup_id is null then tn.tenant_id 
        			        else tn.subgroup_id 
        		        end 
        		        AND ugu.user_role_id = ug.administrator_role_id
        		        AND ugu.user_id = @user_id
        	        )
        	        when tn.publication_status_id = 1 then 1
        	        when tn.publication_status_id = 2 then (
        		        select
        			        case 
        				        when count(*) > 0 then 1
        				        else -1
        			        end status
        		        from user_group_user_role_user ugu
        		        WHERE ugu.user_group_id = 
        			        case
        				        when tn.subgroup_id is null then tn.tenant_id 
        				        else tn.subgroup_id 
        			        end
        			        AND ugu.user_id = @user_id
        		        )
        	        end status	
        	        from node n
        	        join tenant_node tn on tn.node_id = n.id
        	        join publisher p on p.id = n.publisher_id
        	        join node_type nt on nt.id = n.node_type_id
                    where tn.tenant_id = @tenant_id
        	        order by n.changed_date_time desc
                ) x
                where status <> -1
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
            ParameterValue.Create(LimitParameter, request.Limit),
            ParameterValue.Create(OffsetParameter, request.Offset),
        };
    }

    protected override RecentPosts Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
