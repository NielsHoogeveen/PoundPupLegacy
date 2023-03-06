using Npgsql;
using PoundPupLegacy.EditModel;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

public class DocumentableDocumentsSearchService: IDocumentableDocumentsSearchService
{

    private readonly ISiteDataService _siteDataService;
    private readonly NpgsqlConnection _connection;
    private readonly ILogger<DocumentableDocumentsSearchService> _logger;

    private SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);

    public DocumentableDocumentsSearchService(
        NpgsqlConnection connection, 
        ILogger<DocumentableDocumentsSearchService> logger, 
        ISiteDataService siteDataService)
    {
        _siteDataService = siteDataService;
        _connection = connection;
        _logger = logger;
    }

    public async Task<List<DocumentableDocument>> GetDocumentableDocuments(int nodeId, string str)
    {
        await semaphore.WaitAsync(TimeSpan.FromMilliseconds(100));
        await _connection.OpenAsync();
        List<DocumentableDocument> tags = new();
        try {
            var sql = """
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
            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            readCommand.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("search_string", NpgsqlTypes.NpgsqlDbType.Varchar);
            await readCommand.PrepareAsync();
            readCommand.Parameters["user_id"].Value = _siteDataService.GetUserId();
            readCommand.Parameters["tenant_id"].Value = _siteDataService.GetTenantId();
            readCommand.Parameters["search_string"].Value = $"%{str}%";
            await using var reader = await readCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync()) {
                tags.Add(new DocumentableDocument {
                    DocumentableId = nodeId,
                    DocumentId = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    HasBeenDeleted = false,
                    IsStored = false,
                });
            }
            return tags;
        }
        finally {
            await _connection.CloseAsync();
            semaphore.Release();
        }

    }
}
