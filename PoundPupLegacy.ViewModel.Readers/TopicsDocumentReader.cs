﻿using Npgsql;
using PoundPupLegacy.Common;
using System.Data.Common;
using System.Data;

namespace PoundPupLegacy.ViewModel.Readers;

public class TopicsDocumentReader : DatabaseReader, IDatabaseReader<TopicsDocumentReader>
{
    private TopicsDocumentReader(NpgsqlCommand command) : base(command)
    {
    }
    public async Task<Topics> ReadAsync(int userId, int tenantId, int limit, int offset, string searchTerm, SearchOption searchOption)
    {
        string GetPattern(string searchTerm, SearchOption searchOption)
        {
            if (string.IsNullOrEmpty(searchTerm)) {
                return "%";
            }
            return searchOption switch {
                SearchOption.IsEqualTo => searchTerm,
                SearchOption.Contains => $"%{searchTerm}%",
                SearchOption.StartsWith => $"{searchTerm}%",
                SearchOption.EndsWith => $"%{searchTerm}",
                _ => throw new Exception("Cannot reach")
            };
        }
        _command.Parameters["tenant_id"].Value = tenantId;
        _command.Parameters["user_id"].Value = userId;
        _command.Parameters["limit"].Value = limit;
        _command.Parameters["offset"].Value = offset;
        _command.Parameters["pattern"].Value = GetPattern(searchTerm, searchOption);
        await using var reader = await _command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var topics = reader.GetFieldValue<Topics>(0);
        return topics;
    }

    public static async Task<TopicsDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = SQL;
        command.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("limit", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("offset", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("pattern", NpgsqlTypes.NpgsqlDbType.Varchar);
        await command.PrepareAsync();
        return new TopicsDocumentReader(command);
    }

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
                join term tm on tm.vocabulary_id = tt.vocabulary_id_tagging
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

}
