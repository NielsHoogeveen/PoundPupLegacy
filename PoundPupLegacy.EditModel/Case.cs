namespace PoundPupLegacy.EditModel;

public abstract record CaseBase : LocatableBase, Case
{

    public FuzzyDate? Date {get; set;}

    private List<CasePartyTypeCaseParties> casePartyTypesCaseParties = new List<CasePartyTypeCaseParties>();

    public List<CasePartyTypeCaseParties> CasePartyTypesCaseParties {
        get => casePartyTypesCaseParties;
        init => casePartyTypesCaseParties = value ?? new List<CasePartyTypeCaseParties>();
    }

}

public interface Case : Locatable
{
    FuzzyDate? Date { get; set; }
}
