namespace PoundPupLegacy.EditModel;

public record Location
{
    public int? Int { get; set; }
    public string? Street { get; set; }
    public string? Addition { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public int? SubdivisionId { get; set; }
    public string? SubdivisionName { get; set; }
    public int CountryId { get; set; }
    public string CountryName { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

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