namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(SearchResult))]
public partial class SearchResultJsonContext : JsonSerializerContext { }

public record SearchResult : IPagedList<SearchResultListEntry>
{
    public required SearchResultListEntry[] Entries { get; init; }

    public required int NumberOfEntries { get; init; }
}
