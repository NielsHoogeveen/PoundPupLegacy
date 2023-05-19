namespace PoundPupLegacy.ViewModel.Models;

public abstract record TaggedTeaserListEntryBase : TeaserListEntryBase, TaggedTeaserListEntry
{
    public required TagListEntry[] Tags { get; init; }
}
public interface TaggedTeaserListEntry : TeaserListEntry
{
    TagListEntry[] Tags { get; }

}
