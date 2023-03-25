﻿using Npgsql;
using PoundPupLegacy.Common;
using System.Data.Common;
using System.Data;

namespace PoundPupLegacy.ViewModel.Readers;

public class PollsDocumentReader: DatabaseReader, IDatabaseReader<PollsDocumentReader>
{
    private PollsDocumentReader(NpgsqlCommand command) : base(command)
    {
    }
    public async Task<Polls> ReadAsync(int userId, int tenantId, int limit, int offset)
    {
        _command.Parameters["tenant_id"].Value = tenantId;
        _command.Parameters["user_id"].Value = userId;
        _command.Parameters["limit"].Value = limit;
        _command.Parameters["offset"].Value = offset;
        await using var reader = await _command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var organizations = reader.GetFieldValue<Polls>(0);
        return organizations;
    }

    public static async Task<PollsDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = SQL;
        command.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("limit", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("offset", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return new PollsDocumentReader(command);
    }

    const string SQL = """
            select
                jsonb_build_object(
                    'NumberOfEntries', number_of_entries,
                    'Entries', json_agg(
                        jsonb_build_object(
                        	'Path', url_path,
                        	'Title', title,
                            'Text', text,
                        	'HasBeenPublished', case 
                        		when status = 0 then false
                        		else true
                        	end
                        )
                    )
                ) document
            from(
                select
                    tn.id,
                    n.title,
                    n.node_type_id,
                    tn.tenant_id,
                    tn.node_id,
                    stn.teaser text,
                    n.publisher_id,
                    n.created_date_time,
                    n.changed_date_time,
                    tn.url_id,
                    count(tn.id) over() number_of_entries,
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || url_path
                    end url_path,
                    tn.subgroup_id,
                    tn.publication_status_id,
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
                        		AND ugu.user_id = 2
                        	)
                        end status	
                from tenant_node tn
                join node n on n.id = tn.node_id
                join poll o on o.id = n.id
            	join simple_text_node stn on stn.id = n.id
                left join organization_organization_type oot on oot.organization_id = o.id
                WHERE tn.tenant_id = @tenant_id
                ORDER BY n.created_date_time 
                LIMIT @limit OFFSET @offset
            ) x 
            where status <> -1
            group by number_of_entries
        """;
}