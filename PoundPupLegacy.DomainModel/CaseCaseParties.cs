namespace PoundPupLegacy.DomainModel;

public abstract record CaseCaseParties : IRequest
{
    private CaseCaseParties() { }
    public required int CasePartyTypeId { get; init; }
    public abstract record ToCreate : CaseCaseParties
    {
        private ToCreate() { }
        public required CaseParties.ToCreate CaseParties { get; init; }
        public sealed record ForExistingCase : ToCreate
        {
            public required int CaseId { get; init; }
        }
        public sealed record ForNewCase : ToCreate
        {
            public ForExistingCase ResolvedCase(int caseId)
            {
                return new ForExistingCase {
                    CaseId = caseId,
                    CaseParties = CaseParties,
                    CasePartyTypeId = CasePartyTypeId
                };
            }
        }
    }
    public sealed record ToUpdate : CaseCaseParties
    {
        public required int CaseId { get; init; }
        public required CaseParties.ToUpdate CaseParties { get; init; }
    }
}
