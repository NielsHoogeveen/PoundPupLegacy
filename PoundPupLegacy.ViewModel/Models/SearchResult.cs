namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(SearchResult))]
public partial class SearchResultJsonContext : JsonSerializerContext { }

public sealed record SearchResult : PagedListBase<SearchResultListEntry>
{
}
