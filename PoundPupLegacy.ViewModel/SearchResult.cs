namespace PoundPupLegacy.ViewModel;

public record SearchResult : PagedList<SearchResultListEntry>
{
    public SearchResultListEntry[] Entries { get; set; }

    public int NumberOfEntries { get; set; }
    public int PageNumber { get; set; }
    public int NumberOfPages { get; set; }
    public string QueryString { get; set; }

    public string Path => "search";
}
