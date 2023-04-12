using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public abstract class NodeUpdateDocumentReaderFactory<T> : NodeEditDocumentReaderFactory<T>
where T : class, IDatabaseReader
{
    internal static NonNullableIntegerDatabaseParameter UrlId = new() { Name = "url_id" };
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };
    internal static NonNullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };
}

public class NodeUpdateDocumentReader<T> : NodeEditDocumentReader<NodeUpdateDocumentRequest, T>
where T : class, Node
{

    private int _nodeTypeId;
    protected NodeUpdateDocumentReader(NpgsqlCommand command, int nodeTypeId) : base(command)
    {
        _nodeTypeId = nodeTypeId;
    }


    public override async Task<T> ReadAsync(NodeUpdateDocumentRequest request)
    {
        _command.Parameters["url_id"].Value = request.UrlId;
        _command.Parameters["user_id"].Value = request.UserId;
        _command.Parameters["tenant_id"].Value = request.TenantId;
        _command.Parameters["node_type_id"].Value = _nodeTypeId;
        await using var reader = await _command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var text = reader.GetString(0); ;
        var node = reader.GetFieldValue<T>(0);
        return node;
    }
}
