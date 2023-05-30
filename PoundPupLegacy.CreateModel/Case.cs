namespace PoundPupLegacy.CreateModel;
public interface CaseToUpdate : Case, LocatableToUpdate, DocumentableToUpdate, NameableToUpdate
{
    CaseDetails.CaseDetailsForUpdate CaseDetailsForUpdate { get; }
}
public interface CaseToCreate : Case, LocatableToCreate, DocumentableToCreate, NameableToCreate 
{
    CaseDetails.CaseDetailsForCreate CaseDetailsForCreate { get; }
}
public interface Case: Locatable, Documentable, Nameable
{
    CaseDetails CaseDetails { get; }
}
public abstract record CaseDetails
{
    public required FuzzyDate? Date { get; init; }
    public abstract T Match<T>(Func<CaseDetailsForCreate, T> create, Func<CaseDetailsForUpdate, T> update);
    public abstract void Match(Action<CaseDetailsForCreate> create, Action<CaseDetailsForUpdate> update);
    public sealed record CaseDetailsForCreate : CaseDetails
    {
        public required List<NewCaseNewCaseParties> CaseParties { get; init; }
        public override T Match<T>(Func<CaseDetailsForCreate, T> create, Func<CaseDetailsForUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<CaseDetailsForCreate> create, Action<CaseDetailsForUpdate> update)
        {
            create(this);
        }
    }
    public sealed record CaseDetailsForUpdate : CaseDetails
    {
        public required List<CaseExistingCasePartiesToCreate> CasePartiesToUpdate { get; init; }
        public required List<CaseNewCasePartiesToUpdate> CasePartiesToAdd { get; init; }
        public override T Match<T>(Func<CaseDetailsForCreate, T> create, Func<CaseDetailsForUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<CaseDetailsForCreate> create, Action<CaseDetailsForUpdate> update)
        {
            update(this);
        }
    }
}

