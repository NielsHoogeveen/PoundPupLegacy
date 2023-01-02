namespace PoundPupLegacy.Model;

public record NodeStatus : Identifiable
{
    public required int? Id { get; set; }
    public required string Name { get; init; }
}
