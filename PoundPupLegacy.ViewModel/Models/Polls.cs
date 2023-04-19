namespace PoundPupLegacy.ViewModel.Models;

public record Polls : IPagedList<PollListEntry>
{
    private PollListEntry[] _entries = Array.Empty<PollListEntry>();
    public required PollListEntry[] Entries {
        get => _entries;
        init {
            if (value is not null) {
                _entries = value;
            }
        }
    }

    public required int NumberOfEntries { get; init; }
}
