namespace PoundPupLegacy.DomainModel;

public sealed record TermHierarchy : IRequest
{
    public required int TermIdParent { get; init; }
    public required int TermIdChild { get; init; }
}
