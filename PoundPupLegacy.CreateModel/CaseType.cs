namespace PoundPupLegacy.CreateModel;

public sealed record CaseType : NodeType
{
    public required int? Id { get; set; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<int> CaseRelationTypeIds { get; init; }
    public bool AuthorSpecific => false;

    public CaseType() { }

    public static CaseType Create(int id, string name, string description, List<int> caseRelationTypeIds)
    {
        return new CaseType {
            Id = id,
            Name = name,
            Description = description,
            CaseRelationTypeIds = caseRelationTypeIds
        };
    }
}
