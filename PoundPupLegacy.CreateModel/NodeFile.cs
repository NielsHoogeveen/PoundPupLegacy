namespace PoundPupLegacy.DomainModel;

public sealed record NodeFile : IRequest
{
    public required int NodeId { get; init; }
    public required int FileId { get; init; }
}
