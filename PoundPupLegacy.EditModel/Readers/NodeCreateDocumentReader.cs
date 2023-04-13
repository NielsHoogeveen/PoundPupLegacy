namespace PoundPupLegacy.EditModel.Readers;

using Factory = NodeCreateDocumentReaderFactory;

public static class NodeCreateDocumentReaderFactory
{
    internal static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    internal static readonly NonNullableIntegerDatabaseParameter NodeTypeIdParameter = new() { Name = "node_type_id" };
    internal static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
}
public abstract class NodeCreateDocumentReaderFactory<TResponse, TReader> : NodeEditDocumentReaderFactory<NodeCreateDocumentRequest, TResponse, TReader>
where TResponse : Node
where TReader: ISingleItemDatabaseReader<NodeCreateDocumentRequest, TResponse>
{
    internal static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = Factory.TenantIdParameter;
    internal static readonly NonNullableIntegerDatabaseParameter NodeTypeIdParameter = Factory.NodeTypeIdParameter;
    internal static readonly NonNullableIntegerDatabaseParameter UserIdParameter = Factory.UserIdParameter;
}

public class NodeCreateDocumentReader<T> : NodeEditDocumentReader<NodeCreateDocumentRequest, T>
where T : class, Node
{

    private readonly int _nodeTypeId;

    internal readonly FieldValueReader<T> DocumentReader = new() { Name = "document" };
    protected NodeCreateDocumentReader(NpgsqlCommand command, int nodeTypeId) : base(command)
    {
        _nodeTypeId = nodeTypeId;
    }
    protected override IEnumerable<ParameterValue> GetParameterValues(NodeCreateDocumentRequest request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.TenantIdParameter, request.TenantId),
            ParameterValue.Create(Factory.NodeTypeIdParameter, _nodeTypeId),
            ParameterValue.Create(Factory.UserIdParameter, request.UserId)
        };  
    }
    protected override T Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
