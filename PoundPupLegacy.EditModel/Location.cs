namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Location))]
public partial class LocationJsonContext : JsonSerializerContext { }

public sealed record Location
{
    public int? LocationId { get; set; }
    public int? LocatableId { get; set; }
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

    public bool HasBeenDeleted { get; set; } = false;

    private List<SubdivisionListItem> subdivivisions = new();
    public required List<SubdivisionListItem> Subdivisions {
        get => subdivivisions;
        set {
            if (value != null) {
                subdivivisions = value;
            }
        }
    }
}