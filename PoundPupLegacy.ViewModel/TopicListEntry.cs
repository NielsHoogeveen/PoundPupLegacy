namespace PoundPupLegacy.ViewModel;

public record TopicListEntry : ListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }
    public required bool HasBeenPublished { get; init; }

}
