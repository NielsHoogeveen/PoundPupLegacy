namespace PoundPupLegacy.EditModel;

public interface Case: Locatable
{
    CaseDetails CaseDetails { get; }
}


public sealed record CaseDetails
{
    public FuzzyDate? Date { get; set; }

    private List<CasePartyTypeCaseParties> casePartyTypesCaseParties = new List<CasePartyTypeCaseParties>();

    public List<CasePartyTypeCaseParties> CasePartyTypesCaseParties {
        get => casePartyTypesCaseParties;
        init => casePartyTypesCaseParties = value ?? new List<CasePartyTypeCaseParties>();
    }
}
