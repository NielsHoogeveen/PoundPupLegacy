namespace PoundPupLegacy.Model;

public record Subgroup: UserGroup
{
    public required int? Id { get; set; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int TenantId { get; init; }
}
