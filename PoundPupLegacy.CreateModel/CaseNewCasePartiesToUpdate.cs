namespace PoundPupLegacy.CreateModel;

public sealed record CaseNewCasePartiesToUpdate : IRequest
{
    public required int CaseId { get; init; }

    public required CaseParties.ToUpdate CaseParties { get; init; }

    public required int CasePartyTypeId { get; init; }
}
public sealed record CaseExistingCasePartiesToCreate : IRequest
{
    public required int CaseId { get; init; }

    public required CaseParties.ToCreate CaseParties { get; init; }

    public required int CasePartyTypeId { get; init; }
}
public sealed record NewCaseNewCaseParties : IRequest
{
    public required int? CaseId { get; init; }

    public required CaseParties.ToCreate CaseParties { get; init; }

    public required int CasePartyTypeId { get; init; }
}

public interface CaseCaseParties: IRequest
{
    int CasePartyTypeId { get; }
}