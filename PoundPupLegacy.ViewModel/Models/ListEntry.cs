namespace PoundPupLegacy.ViewModel.Models;

public abstract record EntityListEntryBase: ListEntryBase, EntityListEntry
{
    public required bool HasBeenPublished { get; init; }
    public required int PublicationStatusId { get; init; }
}
public abstract record ListEntryBase: LinkBase, ListEntry
{
}
public interface EntityListEntry: ListEntry 
{
    bool HasBeenPublished { get; }
    int PublicationStatusId { get; }

}
public interface ListEntry : Link
{
}
