namespace PoundPupLegacy.ViewModel.Models;

public record SearchResultListEntry : TeaserListEntry
{
    public required string Path { get; init; }

    public required string Title { get; init; }

    public required string Text { get; init; }

    public required string NodeTypeName { get; init; }

    public required bool HasBeenPublished { get; init; } = true;

}
