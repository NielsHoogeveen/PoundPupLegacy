namespace PoundPupLegacy.ViewModel.Models;

public record DeportationCases: TermedList<DeportationCaseList, CaseListEntry> 
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
    public required DeportationCaseList Items { get; init; }

}
