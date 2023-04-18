namespace PoundPupLegacy.ViewModel.Models;

public record SearchResult: IPagedList<SearchResultListEntry> 
{
    public required SearchResultListEntry[] Entries { get; init; }

    public required int NumberOfEntries { get; init; }
}
