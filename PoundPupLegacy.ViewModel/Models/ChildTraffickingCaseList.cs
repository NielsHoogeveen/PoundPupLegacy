namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(ChildTraffickingCaseList))]
public partial class ChildTraffickingCaseListJsonContext : JsonSerializerContext { }

public record ChildTraffickingCaseList : IPagedList<CaseListEntry>
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
