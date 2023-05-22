namespace PoundPupLegacy.EditModel;

public abstract record CaseBase : NameableBase, Case
{

    public FuzzyDate? Date {get; set;}

    private List<CasePartyTypeCaseParties> casePartyTypesCaseParties = new List<CasePartyTypeCaseParties>();

    public List<CasePartyTypeCaseParties> CasePartyTypesCaseParties {
        get => casePartyTypesCaseParties;
        init => casePartyTypesCaseParties = value ?? new List<CasePartyTypeCaseParties>();
    }

}

public interface Case : Nameable
{
    FuzzyDate? Date { get; set; }
}
