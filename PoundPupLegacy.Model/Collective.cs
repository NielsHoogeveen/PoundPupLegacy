namespace PoundPupLegacy.Model;

public sealed record Collective : Publisher
{
    public required int? Id { get; set; }

    public required string Name { get; init; }
}
