using Npgsql;
using PoundPupLegacy.ViewModel;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchCasesService: IFetchCasesService
{
    private NpgsqlConnection _connection;

    public FetchCasesService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Cases> FetchCases(int limit, int offset, int tenantId, int userId)
    {
        _connection.Open();
        var sql = $"""
            select
            	json_build_object(
            		'NumberOfEntries', number_of_entries,
            		'CaseListEntries', json_agg(
            			json_build_object
            			(
            				'Title', title,
                            'Path', url_path,
            				'Text', description,
            				'Date', date,
            				'CaseType',	node_type_name,
            				'HasBeenPublished', case 
            										when status = 0 then false
            										else true
            									end 
            			)
            		)::jsonb

            	)::jsonb "document"
            from(
            	select
            	*
            	from(
            		select
            		n.title,
            		c.description,
            		nt.name node_type_name,
            		n.node_type_id,
            		COUNT(*) OVER() number_of_entries,
            		case 
            			when tn.url_path is null then '/node/' || tn.url_id
            			else '/' || url_path
            		end url_path,
            		case 
            			when c.date is not null then c.date
            			else lower(date_range)
            		end date,
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
            		from
            		tenant_node tn
            		join node n on n.id = tn.node_id
            		join "case" c on c.id = n.id
            		join node_type nt on nt.id = n.node_type_id
            		WHERE tn.tenant_id = @tenant_id
            	) an
            	order by date desc
            	LIMIT @limit OFFSET @offset
            ) an
            where an.status <> -1
            group by number_of_entries
            
            """;

        using var readCommand = _connection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;
        readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
        readCommand.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
        readCommand.Parameters.Add("limit", NpgsqlTypes.NpgsqlDbType.Integer);
        readCommand.Parameters.Add("offset", NpgsqlTypes.NpgsqlDbType.Integer);
        await readCommand.PrepareAsync();
        readCommand.Parameters["tenant_id"].Value = tenantId;
        readCommand.Parameters["user_id"].Value = userId;
        readCommand.Parameters["limit"].Value = limit;
        readCommand.Parameters["offset"].Value = offset;
        await using var reader = await readCommand.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var organizations = reader.GetFieldValue<Cases>(0);
            _connection.Close();
            return organizations!;
        }
        else
        {
            _connection.Close();
            return new Cases
            {
                CaseListEntries = new CaseListEntry[] { },
                NumberOfEntries = 0,
            };
        }
    }

}
