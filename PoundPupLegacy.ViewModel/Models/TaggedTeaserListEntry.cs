namespace PoundPupLegacy.ViewModel.Models;

public interface TaggedTeaserListEntry : TeaserListEntry
{
    TagListEntry[] Tags { get; }

}
