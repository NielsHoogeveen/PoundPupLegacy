namespace PoundPupLegacy.Model;

public record VocabularyName
{
    public required int OwnerId { get; init; }
    public string Name { get; init; }
    public required string TermName { get; init; }

    public required List<string> ParentNames { get; init; }
}
