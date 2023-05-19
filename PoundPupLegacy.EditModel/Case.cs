namespace PoundPupLegacy.EditModel;

public abstract record CaseBase : NameableBase, Case
{

    public FuzzyDate? Date {get; set;}
}

public interface Case : Nameable
{
    FuzzyDate? Date { get; set; }
}
