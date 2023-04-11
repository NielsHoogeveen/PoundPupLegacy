namespace PoundPupLegacy.CreateModel;

public sealed record ActionMenuItem : MenuItem
{
    public required int? Id { get; set; }
    public required int ActionId { get; init; }
    public required string Name { get; init; }
    public required double Weight { get; init; }
}
