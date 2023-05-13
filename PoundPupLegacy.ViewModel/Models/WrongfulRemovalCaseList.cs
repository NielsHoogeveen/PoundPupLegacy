namespace PoundPupLegacy.ViewModel.Models;
[JsonSerializable(typeof(WrongfulRemovalCaseList))]
public partial class WrongfulRemovalCaseListJsonContext : JsonSerializerContext { }

public record WrongfulRemovalCaseList : IPagedList<CaseListEntry>
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
