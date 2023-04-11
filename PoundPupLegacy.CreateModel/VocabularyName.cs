namespace PoundPupLegacy.CreateModel;

public sealed record VocabularyName
{
    public required int OwnerId { get; init; }
    public required string Name { get; init; }
    public required string TermName { get; init; }

    public required List<string> ParentNames { get; init; }
}
