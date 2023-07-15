namespace PoundPupLegacy.DomainModel;

public abstract record Location
{
    private Location() { }
    public required string? Street { get; init; }
    public required string? Additional { get; init; }
    public required string? City { get; init; }
    public required string? PostalCode { get; init; }
    public required decimal? Longitude { get; init; }
    public required decimal? Latitude { get; init; }
    public required int CountryId { get; init; }
    public required int? SubdivisionId { get; init; }

    public sealed record ToCreate : Location, PossiblyIdentifiable
    {
        public required Identification.Possible Identification { get; init; }

    }
    public sealed record ToUpdate : Location, CertainlyIdentifiable
    {
        public required Identification.Certain Identification { get; init; }

    }
}

