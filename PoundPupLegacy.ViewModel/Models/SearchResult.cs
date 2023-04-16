namespace PoundPupLegacy.ViewModel.Models;

public record SearchResult : PagedList<SearchResultListEntry>
{
    public required SearchResultListEntry[] Entries { get; init; }

    public int NumberOfEntries { get; set; }
    public int PageNumber { get; set; }
    public int NumberOfPages { get; set; }
    public string QueryString { get; set; } = "";

    public string Path => "search";
}
