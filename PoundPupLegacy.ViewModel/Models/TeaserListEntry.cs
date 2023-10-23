namespace PoundPupLegacy.ViewModel.Models;

public abstract record TeaserListEntryBase : ListEntryBase, TeaserListEntry
{
    public required string Text { get; init; }

    public required bool HasBeenPublished { get; init; }

    public required int PublicationStatusId { get; init; }


}
public interface TeaserListEntry : ListEntry
{
    string? Text { get; }

    bool HasBeenPublished { get; }

    int PublicationStatusId { get; }


}
