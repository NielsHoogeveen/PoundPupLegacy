namespace PoundPupLegacy.CreateModel;

public sealed record TenantNodeMenuItem : MenuItem
{
    public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
    public Identification Identification => IdentificationForCreate;
    public required int TenantNodeId { get; init; }
    public required string Name { get; init; }
    public required double Weight { get; init; }
}
