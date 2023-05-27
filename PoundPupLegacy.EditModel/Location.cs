namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingLocation))]
public partial class LocationJsonContext : JsonSerializerContext { }

public abstract record Location
{
    public string? Street { get; set; }
    public string? Addition { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public int? SubdivisionId { get; set; }
    public string? SubdivisionName { get; set; }
    public required int CountryId { get; set; }
    public required string CountryName { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    private Location() { }

    private List<SubdivisionListItem> subdivivisions = new();
    public required List<SubdivisionListItem> Subdivisions {
        get => subdivivisions;
        set {
            if (value != null) {
                subdivivisions = value;
            }
        }
    }
    public sealed record ExistingLocation : Location
    {
        public int Id { get; init; }
        public bool HasBeenDeleted { get; set; } = false;
    }
    public sealed record NewLocation : Location
    {
    }
}