namespace PoundPupLegacy.CreateModel;

public sealed record AuthoringStatus : EventuallyIdentifiable
{
    public required int? Id { get; set; }
    public required string Name { get; init; }
}
