namespace PoundPupLegacy.CreateModel;

public sealed record AnonymousUser : Publisher
{
    public required Identification.Possible IdentificationForCreate { get; init; }
    public Identification Identification => IdentificationForCreate;

    public required string Name { get; init; }
}
