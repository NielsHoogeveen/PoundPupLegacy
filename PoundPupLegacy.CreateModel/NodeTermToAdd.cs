namespace PoundPupLegacy.CreateModel;

public sealed record NodeTermToAdd : IRequest
{
    public required int TermId { get; init; }

    public ResolvedNodeTermToAdd Resolve(int nodeId)
    {
        return new ResolvedNodeTermToAdd {
            NodeId = nodeId,
            TermId = TermId
        };
    }
}
public sealed record ResolvedNodeTermToAdd : IRequest
{
    public required int NodeId { get; init; }
    public required int TermId { get; init; }
}

public sealed record NodeTermToRemove : IRequest
{
    public required int NodeId { get; init; }
    public required int TermId { get; init; }

}

