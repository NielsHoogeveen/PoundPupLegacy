namespace PoundPupLegacy.Common;

public record CreateableNodeTypes
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string GetCreatePath(int subgroupId) => $"/{Name.ToLower().Replace(" ", "_")}/create?subgroup={subgroupId}";
    public string DisplayName => $"Create {Name.CapitalizeFirstCharacter()}";
}
