namespace PoundPupLegacy.EditModel.Readers;

using Factory = NodeUpdateDocumentReaderFactory;
public static class NodeUpdateDocumentReaderFactory
{
    internal readonly static NonNullableIntegerDatabaseParameter UrlIdParameter = new() { Name = "url_id" };
    internal readonly static NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    internal readonly static NonNullableIntegerDatabaseParameter NodeTypeIdParameter = new() { Name = "node_type_id" };
    internal readonly static NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };

}
public abstract class NodeUpdateDocumentReaderFactory<TResponse, TReader> : NodeEditDocumentReaderFactory<NodeUpdateDocumentRequest, TResponse, TReader>
where TResponse : Node
where TReader: ISingleItemDatabaseReader<NodeUpdateDocumentRequest, TResponse>
{
    internal readonly static NonNullableIntegerDatabaseParameter UrlIdParameter = Factory.UrlIdParameter;
    internal readonly static NonNullableIntegerDatabaseParameter TenantIdParameter = Factory.TenantIdParameter;
    internal readonly static NonNullableIntegerDatabaseParameter NodeTypeIdParameter = Factory.NodeTypeIdParameter;
    internal readonly static NonNullableIntegerDatabaseParameter UserIdParameter = Factory.UserIdParameter;
}

public class NodeUpdateDocumentReader<T> : NodeEditDocumentReader<NodeUpdateDocumentRequest, T>
where T : class, Node
{

    private int _nodeTypeId;
    protected NodeUpdateDocumentReader(NpgsqlCommand command, int nodeTypeId) : base(command)
    {
        _nodeTypeId = nodeTypeId;
    }

    private readonly FieldValueReader<T> Document = new() { Name = "document" };

    protected override IEnumerable<ParameterValue> GetParameterValues(NodeUpdateDocumentRequest request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.UrlIdParameter, request.UrlId),
            ParameterValue.Create(Factory.TenantIdParameter, request.TenantId),
            ParameterValue.Create(Factory.NodeTypeIdParameter, _nodeTypeId),
            ParameterValue.Create(Factory.UserIdParameter, request.UserId)
        };
    }

    protected override T Read(NpgsqlDataReader reader)
    {
        return Document.GetValue(reader);
    }
}
