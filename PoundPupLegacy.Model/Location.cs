namespace PoundPupLegacy.Model;

public record Location
{
    public required int Id { get; set; }
    public required string? Street { get; init; }
    public required string? Additional { get; init; }
    public required string? City { get; init; }
    public required string? PostalCode { get; init; }
    public required decimal? Longitude { get; init; }
    public required decimal? Latitude { get; init; }
    public required int? CountryId { get; init; }
    public required int? SubdivisionId { get; init; }
}
