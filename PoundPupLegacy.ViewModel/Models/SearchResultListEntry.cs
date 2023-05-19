namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(SearchResultListEntry))]
public partial class SearchResultListEntryJsonContext : JsonSerializerContext { }

public sealed record SearchResultListEntry : TeaserListEntryBase
{
    public required string NodeTypeName { get; init; }

}
