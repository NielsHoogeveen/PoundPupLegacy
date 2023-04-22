namespace PoundPupLegacy.ViewModel.Models;

public record PollListEntry : TeaserListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }

    public required string Text { get; init; }
    public required bool HasBeenPublished { get; init; }

}
