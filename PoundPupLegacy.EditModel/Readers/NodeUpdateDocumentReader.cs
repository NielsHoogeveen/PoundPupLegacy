namespace PoundPupLegacy.EditModel.Readers;

public static class NodeUpdateDocumentReaderFactory
{

}
public abstract class NodeUpdateDocumentReaderFactory<TResponse> : NodeEditDocumentReaderFactory<NodeUpdateDocumentRequest, TResponse>
where TResponse : class, ExistingNode
{
    private static readonly NonNullableIntegerDatabaseParameter UrlIdParameter = new() { Name = "url_id" };
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeTypeIdParameter = new() { Name = "node_type_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };

    protected abstract int NodeTypeId { get; }

    private readonly FieldValueReader<TResponse> Document = new() { Name = "document" };

    protected override IEnumerable<ParameterValue> GetParameterValues(NodeUpdateDocumentRequest request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(UrlIdParameter, request.UrlId),
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(NodeTypeIdParameter, NodeTypeId),
            ParameterValue.Create(UserIdParameter, request.UserId)
        };
    }

    protected override TResponse Read(NpgsqlDataReader reader)
    {
        var result =  Document.GetValue(reader);
        return result;
    }
}
