namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(SearchResultListEntry))]
public partial class SearchResultListEntryJsonContext : JsonSerializerContext { }

public record SearchResultListEntry : TeaserListEntry
{
    public required string Path { get; init; }

    public required string Title { get; init; }

    public required string Text { get; init; }

    public required string NodeTypeName { get; init; }

    public required bool HasBeenPublished { get; init; } = true;

}
