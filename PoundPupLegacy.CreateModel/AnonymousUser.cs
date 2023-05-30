namespace PoundPupLegacy.CreateModel;

public sealed record AnonymousUser : Publisher
{
    public required Identification.Possible Identification { get; init; }
    public required string Name { get; init; }
}
