namespace PoundPupLegacy.EditModel;

public record TenantNodeMenuItem
{
    public required int MenuItemId { get; init; }
    public required int TenantNodeId { get; init; }
    public required string Name { get; init; }
    public required string Title { get; init; }

}
