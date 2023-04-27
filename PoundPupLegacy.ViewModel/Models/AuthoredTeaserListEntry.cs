namespace PoundPupLegacy.ViewModel.Models;

public interface AuthoredTeaserListEntry : TeaserListEntry
{
    Authoring Authoring { get; }

}
