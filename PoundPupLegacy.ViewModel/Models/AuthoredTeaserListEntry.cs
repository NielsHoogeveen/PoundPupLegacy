namespace PoundPupLegacy.ViewModel.Models;

public record AuthoredTeaserListEntryBase: TeaserListEntryBase, AuthoredTeaserListEntry
{
    public required Authoring Authoring { get; init; }
}
public interface AuthoredTeaserListEntry : TeaserListEntry
{
    Authoring Authoring { get; }

}
