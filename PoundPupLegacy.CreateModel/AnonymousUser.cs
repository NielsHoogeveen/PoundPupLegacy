namespace PoundPupLegacy.CreateModel;

public sealed record AnonymousUser : PublisherToCreate
{
    public required Identification.Possible Identification { get; init; }
    public required string Name { get; init; }
}
