namespace PoundPupLegacy.EditModel;

public abstract record ExistingCaseBase: ExistingLocatableBase, ExistingCase
{
    public FuzzyDate? Date { get; set; }

    private List<CasePartyTypeCaseParties> casePartyTypesCaseParties = new List<CasePartyTypeCaseParties>();

    public List<CasePartyTypeCaseParties> CasePartyTypesCaseParties {
        get => casePartyTypesCaseParties;
        init => casePartyTypesCaseParties = value ?? new List<CasePartyTypeCaseParties>();
    }

}
public abstract record NewCaseBase : NewLocatableBase, NewCase
{
    public FuzzyDate? Date { get; set; }

    private List<CasePartyTypeCaseParties> casePartyTypesCaseParties = new List<CasePartyTypeCaseParties>();

    public List<CasePartyTypeCaseParties> CasePartyTypesCaseParties {
        get => casePartyTypesCaseParties;
        init => casePartyTypesCaseParties = value ?? new List<CasePartyTypeCaseParties>();
    }

}

public interface NewCase : Case, NewLocatable
{

}
public interface ExistingCase : Case, ExistingLocatable
{

}
public interface Case : Locatable
{
    FuzzyDate? Date { get; set; }
    List<CasePartyTypeCaseParties> CasePartyTypesCaseParties { get; }
}
