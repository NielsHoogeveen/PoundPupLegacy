namespace PoundPupLegacy.EditModel;

public record Location
{
    public int? Int { get; set; }
    public string? Street { get; set; }
    public string? Addition { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public int? SubdivisionId { get; set; }
    public int CountryId { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

}
