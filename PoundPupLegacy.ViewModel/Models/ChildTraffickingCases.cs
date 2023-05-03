namespace PoundPupLegacy.ViewModel.Models;

public record ChildTraffickingCases: TermedList<ChildTraffickingCaseList, CaseListEntry> 
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
    public required ChildTraffickingCaseList Items { get; init; }

}
