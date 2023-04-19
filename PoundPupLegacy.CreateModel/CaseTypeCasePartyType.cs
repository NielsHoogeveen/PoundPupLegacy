namespace PoundPupLegacy.CreateModel;

public record CaseTypeCasePartyType : IRequest
{
    public required int CaseTypeId { get; init; }
    public required int CasePartyTypeId { get; init; }
}
