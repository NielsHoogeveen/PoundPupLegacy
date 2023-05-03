namespace PoundPupLegacy.ViewModel.Models;

public record WrongfulRemovalCases: TermedList<WrongfulRemovalCaseList, CaseListEntry> 
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
    public required WrongfulRemovalCaseList Items { get; init; }

}
