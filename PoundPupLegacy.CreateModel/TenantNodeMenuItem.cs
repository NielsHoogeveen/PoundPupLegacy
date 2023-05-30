namespace PoundPupLegacy.CreateModel;

public sealed record TenantNodeMenuItem : MenuItem
{
    public required Identification.Possible Identification { get; init; }
    public required int TenantNodeId { get; init; }
    public required string Name { get; init; }
    public required double Weight { get; init; }
}
