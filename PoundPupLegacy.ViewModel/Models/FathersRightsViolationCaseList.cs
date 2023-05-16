namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(FathersRightsViolationCaseList))]
public partial class FathersRightsViolationCaseListJsonContext : JsonSerializerContext { }

public sealed record FathersRightsViolationCaseList : IPagedList<CaseListEntry>
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
