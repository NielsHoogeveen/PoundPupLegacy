namespace PoundPupLegacy.ViewModel.Models;

public interface TeaserListEntry : ListEntry
{
    string? Text { get; }

    bool HasBeenPublished { get; }

}
