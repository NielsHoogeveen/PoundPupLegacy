namespace PoundPupLegacy.EditModel.Readers;

using Request = DocumentableDocumentsDocumentReaderRequest;

public sealed record DocumentableDocumentsDocumentReaderRequest : IRequest
{
    public required int NodeId { get; init; }
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required string SearchString { get; init; }
}

internal sealed class DocumentableDocumentsDocumentReaderFactory : EnumerableDatabaseReaderFactory<Request, DocumentableDocument>
{
    public override string Sql => SQL;
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    private static readonly NonNullableStringDatabaseParameter SearchString = new() { Name = "search_string" };

    private static readonly IntValueReader DocumentableId = new() { Name = "documentable_id" };
    private static readonly IntValueReader DocumentId = new() { Name = "document_id" };
    private static readonly StringValueReader Title = new() { Name = "title" };

    const string SQL = """
        select
            id document_id,
            title,
            @node_id documentable_id
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

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeId, request.NodeId),
            ParameterValue.Create(UserId, request.UserId),
            ParameterValue.Create(TenantId, request.TenantId),
            ParameterValue.Create(SearchString, $"%{request.SearchString}%"),
        };
    }

    protected override DocumentableDocument Read(NpgsqlDataReader reader)
    {
        return new DocumentableDocument {
            DocumentableId = DocumentableId.GetValue(reader),
            DocumentId = DocumentId.GetValue(reader),
            Title = Title.GetValue(reader),
            HasBeenDeleted = false,
            IsStored = false,
        };
    }
}
