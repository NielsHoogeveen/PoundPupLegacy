namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(DisruptedPlacementCaseList))]
public partial class DisruptedPlacementCaseListJsonContext : JsonSerializerContext { }

public sealed record DisruptedPlacementCaseList : IPagedList<CaseListEntry>
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
