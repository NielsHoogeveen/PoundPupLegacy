namespace PoundPupLegacy.ViewModel.Models;

public record Polls : PagedList<PollListEntry>
{
    private PollListEntry[] _entries = Array.Empty<PollListEntry>();
    public required PollListEntry[] Entries
    {
        get => _entries;
        init
        {
            if (value is not null)
            {
                _entries = value;
            }
        }
    }

    public int NumberOfEntries { get; set; }
    public int PageNumber { get; set; }
    public int NumberOfPages { get; set; }
    public string QueryString { get; set; } = "";
    public string Path => "polls";

}
