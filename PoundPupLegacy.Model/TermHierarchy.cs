namespace PoundPupLegacy.Model;

public record TermHierarchy
{
    public required int TermIdPartent { get; init; }

    public required int TermIdChild { get; init; }
}
