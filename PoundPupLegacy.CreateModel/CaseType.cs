namespace PoundPupLegacy.CreateModel;

public sealed record CaseType : NameableTypeBase, EventuallyIdentifiableNameableType
{
    public required string Text { get; init; }
    public required List<int> CaseRelationTypeIds { get; init; }

}
