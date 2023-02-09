namespace PoundPupLegacy.Model;

public sealed record ActionMenuItem : MenuItem
{
    public required int? Id { get; set; }
    public required int ActionId { get; init; }
    public string Name { get; init; }
    public required double Weight { get; init; }
}
