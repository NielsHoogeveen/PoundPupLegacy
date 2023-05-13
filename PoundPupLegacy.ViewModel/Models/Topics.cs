namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Topics))]
public partial class TopicsJsonContext : JsonSerializerContext { }

public record Topics : IPagedList<TopicListEntry>
{
    private TopicListEntry[] _entries = Array.Empty<TopicListEntry>();
    public required TopicListEntry[] Entries {
        get => _entries;
        init {
            if (value is not null) {
                _entries = value;
            }
        }
    }

    public required int NumberOfEntries { get; init; }

}
