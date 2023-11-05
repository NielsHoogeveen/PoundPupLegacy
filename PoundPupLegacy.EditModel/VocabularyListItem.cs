namespace PoundPupLegacy.EditModel;

public sealed record VocabularyListItem
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public bool SupportsTermHierarchy { get; init; }

}
