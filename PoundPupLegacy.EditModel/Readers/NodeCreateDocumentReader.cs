using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public abstract class NodeCreateDocumentReaderFactory<T> : NodeEditDocumentReaderFactory<T>
where T : class, IDatabaseReader
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };
    internal static NonNullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };
}

public class NodeCreateDocumentReader<T> : NodeEditDocumentReader<NodeCreateDocumentRequest, T>
where T : class, Node
{

    private int _nodeTypeId;
    protected NodeCreateDocumentReader(NpgsqlCommand command, int nodeTypeId) : base(command)
    {
        _nodeTypeId = nodeTypeId;
    }

    public override async Task<T> ReadAsync(NodeCreateDocumentRequest request)
    {
        _command.Parameters["tenant_id"].Value = request.TenantId;
        _command.Parameters["node_type_id"].Value = request.NodeTypeId;
        _command.Parameters["user_id"].Value = request.UserId;
        await using var reader = await _command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var text = reader.GetString(0); ;
        var node = reader.GetFieldValue<T>(0);
        return node;
    }
}
