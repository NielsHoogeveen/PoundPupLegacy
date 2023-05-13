namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(WrongfulMedicationCases))]
public partial class WrongfulMedicationCasesJsonContext : JsonSerializerContext { }

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
