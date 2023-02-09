using Npgsql;
using PoundPupLegacy.ViewModel;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchSearchService : IFetchSearchService
{
    private NpgsqlConnection _connection;

    public FetchSearchService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<SearchResult> FetchSearch(int limit, int offset, int tenantId, int userId, string searchString)
    {
        try
        {
            await _connection.OpenAsync();
            var sql = $"""
            with simple_text_node_teaser as(
            	select
            	id,
            	teaser
            	from simple_text_node stn
            )

            select
            	jsonb_build_object(
            		'NodeTypes', node_types,
            		'Entries', jsonb_agg(
            			jsonb_build_object(
            				'Title', title,
            				'Path', path,
            				'Teaser', teaser,
            				'NodeTypeName', node_type_name,
            				'Status', status
            			)
            		)
            	)
            from(
            	select
            		title,
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
            			path,
            			teaser,
            			rank,
            			node_type_name,
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
            					when nt.id in(35,36,37,42) then (select teaser from simple_text_node_teaser stnt where stnt.id = n.id)
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
                    LIMIT @limit OFFSET @offset
            	) x
            	order by rank desc
            ) x
            group by node_types
            """;

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("limit", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("offset", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("search_string", NpgsqlTypes.NpgsqlDbType.Varchar);
            await readCommand.PrepareAsync();
            readCommand.Parameters["tenant_id"].Value = tenantId;
            readCommand.Parameters["user_id"].Value = userId;
            readCommand.Parameters["limit"].Value = limit;
            readCommand.Parameters["offset"].Value = offset;
            readCommand.Parameters["search_string"].Value = searchString;
            await using var reader = await readCommand.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                await reader.ReadAsync();
                var organizations = reader.GetFieldValue<SearchResult>(0);
                return organizations!;
            }
            else
            {
                return new SearchResult
                {
                };
            }
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

}
