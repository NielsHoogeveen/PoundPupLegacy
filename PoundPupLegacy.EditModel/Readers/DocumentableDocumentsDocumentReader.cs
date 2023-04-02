using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;

public class DocumentableDocumentsDocumentReaderFactory : IDatabaseReaderFactory<DocumentableDocumentsDocumentReader>
{
    public async Task<DocumentableDocumentsDocumentReader> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();

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

public class DocumentableDocumentsDocumentReader : DatabaseReader, IEnumerableDatabaseReader<DocumentableDocumentsDocumentReader.DocumentableDocumentsDocumentRequest, DocumentableDocument>
{
    public record DocumentableDocumentsDocumentRequest
    {
        public required int NodeId { get; init; }
        public required int UserId { get; init; }
        public required int TenantId { get; init; }
        public required string SearchString { get; init; }

    }
    internal DocumentableDocumentsDocumentReader(NpgsqlCommand command) : base(command)
    {
    }
    public async IAsyncEnumerable<DocumentableDocument> ReadAsync(DocumentableDocumentsDocumentRequest request)
    {
        _command.Parameters["user_id"].Value = request.UserId;
        _command.Parameters["tenant_id"].Value = request.TenantId;
        _command.Parameters["search_string"].Value = $"%{request.SearchString}%";
        await using var reader = await _command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            yield return new DocumentableDocument {
                DocumentableId = request.NodeId,
                DocumentId = reader.GetInt32(0),
                Title = reader.GetString(1),
                HasBeenDeleted = false,
                IsStored = false,
            };
        }
    }
}
