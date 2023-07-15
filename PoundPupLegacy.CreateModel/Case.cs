namespace PoundPupLegacy.DomainModel;
public interface CaseToUpdate : Case, LocatableToUpdate, DocumentableToUpdate, NameableToUpdate
{
    CaseDetails.CaseDetailsForUpdate CaseDetails { get; }
}
public interface CaseToCreate : Case, LocatableToCreate, DocumentableToCreate, NameableToCreate
{
    CaseDetails.CaseDetailsForCreate CaseDetails { get; }
}
public interface Case : Locatable, Documentable, Nameable
{
}
public abstract record CaseDetails
{
    public required FuzzyDate? Date { get; init; }
    public sealed record CaseDetailsForCreate : CaseDetails
    {
        public required List<CaseCaseParties.ToCreate.ForNewCase> CaseCaseParties { get; init; }
    }
    public sealed record CaseDetailsForUpdate : CaseDetails
    {
        public required List<CaseCaseParties.ToCreate.ForExistingCase> CasePartiesToAdd { get; init; }
        public required List<CaseCaseParties.ToUpdate> CasePartiesToUpdate { get; init; }
    }
}

