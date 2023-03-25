using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;

public class DocumentableDocumentsDocumentReader : DatabaseReader, IDatabaseReader<DocumentableDocumentsDocumentReader>
{
    private DocumentableDocumentsDocumentReader(NpgsqlCommand command) : base(command)
    {
    }
    public async IAsyncEnumerable<DocumentableDocument> GetDocumentableDocuments(int nodeId, int userId, int tenantId, string str)
    {
        _command.Parameters["user_id"].Value = userId;
        _command.Parameters["tenant_id"].Value = tenantId;
        _command.Parameters["search_string"].Value = $"%{str}%";
        await using var reader = await _command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            yield return new DocumentableDocument {
                DocumentableId = nodeId,
                DocumentId = reader.GetInt32(0),
                Title = reader.GetString(1),
                HasBeenDeleted = false,
                IsStored = false,
            };
        }
    }
    public static async Task<DocumentableDocumentsDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = SQL;
        command.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("search_string", NpgsqlTypes.NpgsqlDbType.Varchar);
        await command.PrepareAsync();
        return new DocumentableDocumentsDocumentReader(command);
    }

    const string SQL = """
        select
            id,
            title
        from(
            select
            d.id,
            n.title,
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
            from documentable d
            join node n on n.id = d.id
            join tenant_node tn on tn.node_id = d.id
            where tn.tenant_id = @tenant_id and n.title ilike @search_string
            limit 50
        ) x
        where status = 1
        """;
}
