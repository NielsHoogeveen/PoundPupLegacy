namespace PoundPupLegacy.CreateModel;

public sealed record LocationToCreate : LocationBase, EventuallyIdentifiableLocation
{
    public required int? Id { get; set; }

    public required Identification.IdentificationForCreate IdentificationForCreate { get; init;}

    public Identification Identification => IdentificationForCreate;
}
public sealed record LocationToUpdate : LocationBase, ImmediatelyIdentifiableLocation
{
    public required int Id { get; init; }
    public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }

    public Identification Identification => IdentificationForUpdate;
}

public abstract record LocationBase : Location
{
    public required string? Street { get; init; }
    public required string? Additional { get; init; }
    public required string? City { get; init; }
    public required string? PostalCode { get; init; }
    public required decimal? Longitude { get; init; }
    public required decimal? Latitude { get; init; }
    public required int CountryId { get; init; }
    public required int? SubdivisionId { get; init; }
}
public interface ImmediatelyIdentifiableLocation : Location, ImmediatelyIdentifiable
{

}
public interface EventuallyIdentifiableLocation : Location, EventuallyIdentifiable
{

}

public interface Location
{
    string? Street { get;  }
    string? Additional { get; }
    string? City { get; }
    string? PostalCode { get; }
    decimal? Longitude { get; }
    decimal? Latitude { get; }
    int CountryId { get; }
    int? SubdivisionId { get; }
}