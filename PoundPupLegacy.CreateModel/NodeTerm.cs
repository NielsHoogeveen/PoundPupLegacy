namespace PoundPupLegacy.CreateModel;

public sealed record NodeTerm: IRequest
{
    public required int NodeId { get; init; }
    public required int TermId { get; init; }
}
