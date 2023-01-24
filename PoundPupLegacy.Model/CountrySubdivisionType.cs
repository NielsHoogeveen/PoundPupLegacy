namespace PoundPupLegacy.Model;

public record CountrySubdivisionType
{
    public required int CountryId { get; set; }

    public required int SubdivisionTypeId { get; set; }
}
