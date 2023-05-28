namespace PoundPupLegacy.CreateModel;

public sealed record CaseType : NameableTypeBase, NameableTypeToAdd
{
    public required string Text { get; init; }
    public required List<int> CaseRelationTypeIds { get; init; }

}
