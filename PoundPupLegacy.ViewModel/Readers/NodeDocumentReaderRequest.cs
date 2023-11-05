namespace PoundPupLegacy.ViewModel.Readers;

public sealed class NodeDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
    public required int NodeId { get; init; }
}
