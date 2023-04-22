namespace PoundPupLegacy.ViewModel.Models;

public record BlogPostTeaser : AuthoredTeaserListEntry
{
    public required string Path { get; init; }

    public required string Title { get; init; }

    public required Authoring Authoring { get; init; }

    public required string Text { get; init; }

    public required bool HasBeenPublished { get; init; }

}