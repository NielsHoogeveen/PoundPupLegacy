namespace PoundPupLegacy.CreateModel;

public sealed record VocabularyName
{
    public required int VocabularyId { get; init; }

    public required string TermName { get; init; }

    public required List<int> ParentTermIds { get; init; }
}
