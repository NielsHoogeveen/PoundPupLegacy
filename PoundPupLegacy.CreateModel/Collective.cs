namespace PoundPupLegacy.DomainModel;

public sealed record Collective : PublisherToCreate
{
    public required Identification.Possible Identification { get; init; }
    public required string Name { get; init; }
}
