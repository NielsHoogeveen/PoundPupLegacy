namespace PoundPupLegacy.Model;

public sealed record TenantNodeMenuItem: MenuItem
{
    public required int? Id { get; set; }
    public required int TenantNodeId { get; init; }

    public required string Name { get; init; }

    public required double Weight { get; init; }
}
