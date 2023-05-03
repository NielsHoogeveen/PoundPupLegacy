namespace PoundPupLegacy.ViewModel.Models;

public record WrongfulMedicationCases: TermedList<WrongfulMedicationCaseList, CaseListEntry> 
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
    public required WrongfulMedicationCaseList Items { get; init; }

}
