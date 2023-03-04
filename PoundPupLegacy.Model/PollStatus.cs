namespace PoundPupLegacy.Model;

public record PollStatus : Identifiable
{
    public required int? Id { get; set; }
    public required string Name { get; init; }

}
