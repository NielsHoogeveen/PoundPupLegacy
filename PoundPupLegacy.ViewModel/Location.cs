namespace PoundPupLegacy.ViewModel;

public record Location
{
    public string? Street { get; set; }
    public string? Addition { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public Link? Subdivision { get; set; }
    public Link Country { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}
