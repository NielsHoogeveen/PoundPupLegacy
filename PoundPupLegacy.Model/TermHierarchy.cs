namespace PoundPupLegacy.CreateModel;

public sealed record TermHierarchy
{
    public required int TermIdPartent { get; init; }

    public required int TermIdChild { get; init; }
}
