namespace PoundPupLegacy.DomainModel;

public sealed record AuthoringStatus : PossiblyIdentifiable
{
    public required Identification.Possible Identification { get; init; }
    public required string Name { get; init; }
}
