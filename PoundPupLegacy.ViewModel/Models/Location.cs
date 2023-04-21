namespace PoundPupLegacy.ViewModel.Models;

public record Location
{
    public required string? Street { get; init; }
    public required string? Additional { get; init; }
    public required string? City { get; init; }
    public required string? PostalCode { get; init; }
    public required BasicLink? Subdivision { get; init; }
    public required BasicLink Country { get; init; }
    public required decimal? Latitude { get; init; }
    public required decimal? Longitude { get; init; }
}
