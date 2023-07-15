namespace PoundPupLegacy.DomainModel;

public sealed record ActionMenuItem : MenuItem
{
    public required Identification.Possible Identification { get; init; }
    public required int ActionId { get; init; }
    public required string Name { get; init; }
    public required double Weight { get; init; }
}
