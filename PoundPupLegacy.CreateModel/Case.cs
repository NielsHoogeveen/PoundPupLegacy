namespace PoundPupLegacy.CreateModel;
public interface ImmediatelyIdentifiableCase : Case, ImmediatelyIdentifiableLocatable, ImmediatelyIdentifiableDocumentable, ImmediatelyIdentifiableNameable
{
    List<ExistingCaseExistingCaseParties> CasePartiesToUpdate { get; }

    List<ExistingCaseNewCaseParties> CasePartiesToAdd { get; }
    List<int> CasePartiesToRemove { get; }
}

public interface EventuallyIdentifiableCase : Case, EventuallyIdentifiableLocatable, EventuallyIdentifiableDocumentable, EventuallyIdentifiableNameable 
{
    List<NewCaseNewCaseParties> CaseParties { get; }
}

public interface Case : Locatable, Documentable, Nameable
{
    FuzzyDate? Date { get; }
}

public abstract record NewCaseBase: NewLocatableBase, EventuallyIdentifiableCase
{
    public required FuzzyDate? Date { get; init; }
    public required List<NewCaseNewCaseParties> CaseParties { get; init; }
}
public abstract record ExistingCaseBase : ExistingLocatableBase, ImmediatelyIdentifiableCase
{
    public required FuzzyDate? Date { get; init; }
    public required List<ExistingCaseExistingCaseParties> CasePartiesToUpdate { get; init; }
    public required List<ExistingCaseNewCaseParties> CasePartiesToAdd { get; init; }
    public required List<int> CasePartiesToRemove { get; init; }
}