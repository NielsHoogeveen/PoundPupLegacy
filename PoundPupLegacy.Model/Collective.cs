namespace PoundPupLegacy.Model;

public record Collective: Publisher
{
    public required int? Id { get; set; }

    public required string Name { get; init; }
}
