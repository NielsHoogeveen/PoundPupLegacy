namespace PoundPupLegacy.ViewModel.Models;

public interface TaggedTeaserListEntry: TeaserListEntry
{
     BasicLink[] Tags { get; }

}
