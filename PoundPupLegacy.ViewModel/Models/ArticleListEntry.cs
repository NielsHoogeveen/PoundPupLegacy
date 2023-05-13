using System.Text.Json.Serialization;

namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(ArticleListEntry))]
public partial class ArticleListEntryJsonContext : JsonSerializerContext { }

public record ArticleListEntry : TeaserListEntry, TaggedTeaserListEntry
{
    public required string Path { get; init; }

    public required string Title { get; init; }

    public required string Text { get; init; }

    public required TagListEntry[] Tags { get; init; }

    public required bool HasBeenPublished { get; init; }

}
