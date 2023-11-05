namespace PoundPupLegacy.EditModel.Readers;
public record NodeUpdateDocumentRequest : IRequest
{
    public int NodeId { get; init; }
    public int UserId { get; init; }
    public int TenantId { get; init; }

}
public record NodeCreateDocumentRequest : IRequest
{
    public int NodeTypeId { get; init; }
    public int UserId { get; init; }
    public int TenantId { get; init; }

}

public abstract class NodeEditDocumentReaderFactory<TRequest, TResponse> : SingleItemDatabaseReaderFactory<TRequest, TResponse>
where TRequest : IRequest
where TResponse : class, Node
{
}
