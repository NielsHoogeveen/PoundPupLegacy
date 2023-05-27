namespace PoundPupLegacy.CreateModel;

public sealed record NodeTermToAdd : NodeTermBase
{
}

public sealed record NodeTermToRemove : NodeTermBase
{
}

public abstract record NodeTermBase: IRequest
{
    public required int NodeId { get; init; }
    public required int TermId { get; init; }
}