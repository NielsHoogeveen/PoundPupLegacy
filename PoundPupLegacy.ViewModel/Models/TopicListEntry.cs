namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(TopicListEntry))]
public partial class TopicListEntryJsonContext : JsonSerializerContext { }

public sealed record TopicListEntry : ListEntryBase
{
    public required bool HasBeenPublished { get; init; }

}
