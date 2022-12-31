namespace PoundPupLegacy.Model;

public record VocabularyName
{
    public required int VocabularyId { get; init; }
    public required string Name { get; init; }

    public required List<string> ParentNames { get; init; }
}
