namespace PoundPupLegacy.DomainModel;

public sealed record CountrySubdivisionType : IRequest
{
    public required int CountryId { get; set; }
    public required int SubdivisionTypeId { get; set; }
}
