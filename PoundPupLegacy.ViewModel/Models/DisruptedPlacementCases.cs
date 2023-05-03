namespace PoundPupLegacy.ViewModel.Models;

public record DisruptedPlacementCases: TermedList<DisruptedPlacementCaseList, CaseListEntry> 
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
    public required DisruptedPlacementCaseList Items { get; init; }

}
