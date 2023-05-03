namespace PoundPupLegacy.ViewModel.Models;

public record CoercedAdoptionCases: TermedList<CoercedAdoptionCaseList, CaseListEntry> 
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
    public required CoercedAdoptionCaseList Items { get; init; }

}
