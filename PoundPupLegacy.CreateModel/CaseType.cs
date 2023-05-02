namespace PoundPupLegacy.CreateModel;

public sealed record CaseType : NameableType
{
    public required int? Id { get; set; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Text { get; init; }
    public required List<int> CaseRelationTypeIds { get; init; }
    public bool AuthorSpecific => false;

    public required string TagLabelName { get; init; }

}
