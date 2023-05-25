namespace PoundPupLegacy.CreateModel;

public sealed record ExistingCaseNewCaseParties : IRequest
{
    public required int CaseId { get; init; }

    public required NewCaseParties CaseParties { get; init; }

    public required int CasePartyTypeId { get; init; }
}
public sealed record ExistingCaseExistingCaseParties : IRequest
{
    public required int CaseId { get; init; }

    public required ExistingCaseParties CaseParties { get; init; }

    public required int CasePartyTypeId { get; init; }
}
public sealed record NewCaseNewCaseParties : IRequest
{
    public required int? CaseId { get; init; }

    public required NewCaseParties CaseParties { get; init; }

    public required int CasePartyTypeId { get; init; }
}

public interface CaseCaseParties: IRequest
{
    int CasePartyTypeId { get; }
}