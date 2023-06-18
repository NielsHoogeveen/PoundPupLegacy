namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = SearchDocumentReaderRequest;

public sealed record SearchDocumentReaderRequest : IRequest
{
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required int Limit { get; init; }
    public required int Offset { get; init; }
    public required string SearchString { get; init; }
}
internal sealed class SearchDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, SearchResult>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter LimitParameter = new() { Name = "limit" };
    private static readonly NonNullableIntegerDatabaseParameter OffsetParameter = new() { Name = "offset" };
    private static readonly SearchStringDatabaseParameter SearchStringParameter = new() { Name = "search_string" };

    private static readonly FieldValueReader<SearchResult> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = """
             with 
                simple_text_node_teaser as(
                    select
                    id,
                    teaser
                    from simple_text_node stn
                ),
                case_teaser as(
                    select
                    c.id,
                    description
                    from nameable c
                )
                select
                    jsonb_build_object(
            			'NumberOfEntries', number_of_entries,
                        'Entries', jsonb_agg(
                        	jsonb_build_object(
                        		'Title', title,
                        		'Path', path,
                        		'Text', teaser,
                        		'NodeTypeName', node_type_name,
                        		'HasBeenPublished', case when status = 1 then true else false end
                        	)
                        )
                    ) document
                from(
                    select
                        title,
            			number_of_entries,
                        path,
                        teaser,
                        node_type_name,
                        rank,
                        (  select jsonb_agg(
                        		jsonb_build_object(
                        			'Id', nt.id,
                        			'Name', nt.name
                        		)
                        	)
                        	from node_type nt
                        	where nt.id in (
                        		select 
                        			distinct x
                        		from unnest( node_type_ids) x
                        	) 
                        ) node_types,
                        status
                    from(
                        select
                        	    id,
                        	    title,
            				    count(id) over() number_of_entries,
                        	    path,
                        	    teaser,
                        	    rank,
                        	    node_type_name,
                                node_type_id,   
                        	    array_agg(node_type_id) over() node_type_ids,
                        	    status
                        from(
                            select
                        	    id,
                        	    title,
                        	    path,
                        	    teaser,
                        	    rank,
                        	    node_type_name,
                                node_type_id,   
                        	    array_agg(node_type_id) over() node_type_ids,
                        	    status
                            from(
                        	    select
                        		    n.id,
                        		    n.title,
                        		    case 
                        			    when tn.url_path is null then '/node/' || tn.url_id
                        			    else tn.url_path
                        		    end "path",
                        		    case 
                        			    when nt.id in (10, 35,37,42) then (select teaser from simple_text_node_teaser stnt where stnt.id = n.id)
                                        when nt.id in (26,29,30,31,32,33,34,44) then (select description from case_teaser ct where ct.id = n.id)
                        			    else ''
                        		    end teaser,	
                        		    nt.name node_type_name,
                        		    nt.id node_type_id,
                        		    ts_rank_cd(s.tsvector, 'masha') rank,

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
                        	    from searchable s
                        	    join node n on n.id = s.id
                        	    join node_type nt on nt.id = n.node_type_id
                        	    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id
                        	    where s.tsvector @@ to_tsquery(@search_string)
                            ) x
                            where status <> -1
                        ) x
                        LIMIT @limit OFFSET @offset
                    ) x
                    order by rank desc
                ) x
                group by number_of_entries
            """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
            ParameterValue.Create(LimitParameter, request.Limit),
            ParameterValue.Create(OffsetParameter, request.Offset),
            ParameterValue.Create(SearchStringParameter, request.SearchString)

        };
    }
    protected override SearchResult Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
