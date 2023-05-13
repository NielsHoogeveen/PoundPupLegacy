namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(SubgroupListEntry))]
public partial class SubgroupListEntryJsonContext : JsonSerializerContext { }

public record SubgroupListEntry : ListEntry
{
    public required string Title { get; init; }

    public required string Path { get; init; }
    public required Authoring Authoring { get; init; }

    public required bool HasBeenPublished { get; init; }
}
