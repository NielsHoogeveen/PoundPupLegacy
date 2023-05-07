namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using Request = TopicsDocumentReaderRequest;

public sealed record TopicsDocumentReaderRequest : IRequest
{
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required int Limit { get; init; }
    public required int Offset { get; init; }
    public required string SearchTerm { get; init; }
    public required SearchOption SearchOption { get; init; }
}

internal sealed class TopicsDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, Topics>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NullableIntegerDatabaseParameter LimitParameter = new() { Name = "limit" };
    private static readonly NullableIntegerDatabaseParameter OffsetParameter = new() { Name = "offset" };
    private static readonly SearchOptionDatabaseParameter PatternParameter = new() { Name = "pattern" };

    private static readonly FieldValueReader<Topics> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = """
        select
        jsonb_build_object(
            'NumberOfEntries',
            number_of_entries,
            'Entries',
            jsonb_agg(
            	jsonb_build_object(
            		'Title',
            		name,
            		'Path',
            		path,
            		'HasBeenPublished',
            		has_been_published
            	)
            ) 
        ) document
        from(
            select
                path, 
                name,
                case 
            	    when status = 1 then true
            	    else false
                end has_been_published,	
                count(*) over () number_of_entries
            from(
                select
            	    case 
            		    when tn.url_path is null then '/node/' || tn.url_id
            		    else '/' || url_path
            	    end path,
            	    tm.name,
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
                from tenant tt
                join system_group sg on sg.id = 0
                join term tm on tm.vocabulary_id = sg.vocabulary_id_tagging
                join node n on n.id = tm.nameable_id
                join tenant_node tn on tn.node_id = n.id and tn.tenant_id = tt.id
                where tt.id = @tenant_id
                and tm.name ilike @pattern
            ) x 
            where status = 1
            limit @limit offset @offset
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
            ParameterValue.Create(PatternParameter, (request.SearchTerm, request.SearchOption)),
        };
    }

    protected override Topics Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
