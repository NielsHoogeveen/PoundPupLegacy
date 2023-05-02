namespace PoundPupLegacy.ViewModel.Models;

public record ArticleListEntry : TeaserListEntry, TaggedTeaserListEntry
{
    public required string Path { get; init; }

    public required string Title { get; init; }

    public required string Text { get; init; }

    public required TagListEntry[] Tags { get; init; }

    public required bool HasBeenPublished { get; init; }

}
