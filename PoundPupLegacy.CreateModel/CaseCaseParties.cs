namespace PoundPupLegacy.CreateModel;

public sealed record CaseCaseParties : IRequest
{
    public required int CaseId { get; init; }

    public required CaseParties CaseParties { get; init; }

    public required int CasePartyTypeId { get; init; }
}
