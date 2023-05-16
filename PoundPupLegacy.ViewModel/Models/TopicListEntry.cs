namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(TopicListEntry))]
public partial class TopicListEntryJsonContext : JsonSerializerContext { }

public sealed record TopicListEntry : ListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }

    public required bool HasBeenPublished { get; init; }

}
