namespace PoundPupLegacy.DomainModel;

public sealed record CaseTypeCasePartyType : IRequest
{
    public required int CaseTypeId { get; init; }
    public required int CasePartyTypeId { get; init; }
}
