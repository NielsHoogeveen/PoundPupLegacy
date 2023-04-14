namespace PoundPupLegacy.CreateModel;

public record CountrySubdivisionType: IRequest
{
    public required int CountryId { get; set; }

    public required int SubdivisionTypeId { get; set; }
}
