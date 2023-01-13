namespace PoundPupLegacy.Model;

public sealed record NodeTerm
{
    public required int NodeId { get; init; }
    public required int TermId { get; init; }
}
