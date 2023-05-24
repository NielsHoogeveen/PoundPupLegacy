namespace PoundPupLegacy.CreateModel;
public interface ImmediatelyIdentifiableCase : Case, ImmediatelyIdentifiableLocatable, ImmediatelyIdentifiableDocumentable, ImmediatelyIdentifiableNameable
{
}

public interface EventuallyIdentifiableCase : Case, EventuallyIdentifiableLocatable, EventuallyIdentifiableDocumentable, EventuallyIdentifiableNameable 
{
}

public interface Case : Locatable, Documentable, Nameable
{
    FuzzyDate? Date { get; }

}

public abstract record NewCaseBase: NewNameableBase, EventuallyIdentifiableCase
{
    public required FuzzyDate? Date { get; init; }
}
public abstract record ExistingCaseBase : ExistingNameableBase, ImmediatelyIdentifiableCase
{
    public required FuzzyDate? Date { get; init; }
}