namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(WrongfulMedicationCaseList))]
public partial class WrongfulMedicationCaseListJsonContext : JsonSerializerContext { }

public record WrongfulMedicationCaseList : IPagedList<CaseListEntry>
{
    private CaseListEntry[] _entries = Array.Empty<CaseListEntry>();
    public CaseListEntry[] Entries {
        get => _entries;
        set {
            if (value != null) {
                _entries = value;
            }
        }
    }

    public required int NumberOfEntries { get; init; }

}
