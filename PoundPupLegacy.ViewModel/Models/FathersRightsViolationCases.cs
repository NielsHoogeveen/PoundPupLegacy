namespace PoundPupLegacy.ViewModel.Models;

public record FathersRightsViolationCases: TermedList<FathersRightsViolationCaseList, CaseListEntry> 
{
    private SelectionItem[] termNames = Array.Empty<SelectionItem>();
    public SelectionItem[] TermNames {
        get => termNames;
        set {
            if (value != null) {
                termNames = value;
            }
        }
    }
    public required FathersRightsViolationCaseList Items { get; init; }

}
