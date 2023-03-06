namespace PoundPupLegacy.ViewModel;

public record Topics : PagedList<TopicListEntry>
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

    public int NumberOfEntries { get; set; }
    public int PageNumber { get; set; }
    public int NumberOfPages { get; set; }
    public string QueryString { get; set; } = "";
    public SearchOption SelectedSearchOption { get; set; }
    public string? SearchTerm { get; set; }
    public string Path => "topics";

}
