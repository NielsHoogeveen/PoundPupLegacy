namespace PoundPupLegacy.ViewModel.Models;

public record AbuseCases: TermedList<AbuseCaseList, CaseListEntry> 
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
    public required AbuseCaseList Items { get; init; }

}
