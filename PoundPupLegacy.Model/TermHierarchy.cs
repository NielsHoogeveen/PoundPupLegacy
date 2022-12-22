namespace PoundPupLegacy.Model;

public record TermHierarchy
{
    public required int ParentId { get; init; }

    public required int ChildId { get; init; }
}
