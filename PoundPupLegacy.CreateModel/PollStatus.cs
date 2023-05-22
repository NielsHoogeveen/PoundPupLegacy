namespace PoundPupLegacy.CreateModel;

public sealed record PollStatus : EventuallyIdentifiable
{
    public required int? Id { get; set; }
    public required string Name { get; init; }

}
