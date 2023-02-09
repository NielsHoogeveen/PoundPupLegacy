namespace PoundPupLegacy.Model;

public record CaseCaseParties
{
    public required int CaseId { get; init; }

    public required CaseParties CaseParties { get; init; }

    public required int CasePartyTypeId { get; init; }
}
