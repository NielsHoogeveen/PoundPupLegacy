namespace PoundPupLegacy.Model;

public sealed record CaseType : NodeType
{
    public int? Id { get; set; }
    public string Name { get; }
    public string Description { get; }
    public List<int> CaseRelationTypeIds { get; }
    public bool AuthorSpecific => false;

    public CaseType(int id, string name, string description, List<int> caseRelationTypeIds)
    {
        Id = id;
        Name = name;
        Description = description;
        CaseRelationTypeIds = caseRelationTypeIds;
    }
}
