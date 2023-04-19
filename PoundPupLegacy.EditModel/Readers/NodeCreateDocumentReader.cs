namespace PoundPupLegacy.EditModel.Readers;

public abstract class NodeCreateDocumentReaderFactory<TResponse> : NodeEditDocumentReaderFactory<NodeCreateDocumentRequest, TResponse>
where TResponse : class, Node
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeTypeIdParameter = new() { Name = "node_type_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };

    protected abstract int NodeTypeId { get; }

    internal readonly FieldValueReader<TResponse> DocumentReader = new() { Name = "document" };

    protected override IEnumerable<ParameterValue> GetParameterValues(NodeCreateDocumentRequest request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(NodeTypeIdParameter, NodeTypeId),
            ParameterValue.Create(UserIdParameter, request.UserId)
        };
    }
    protected override TResponse Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
