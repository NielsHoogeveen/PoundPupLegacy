namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(SubgroupListEntry))]
public partial class SubgroupListEntryJsonContext : JsonSerializerContext { }

public sealed record SubgroupListEntry : ListEntryBase
{
    public required Authoring Authoring { get; init; }
    public required int PublicationStatusId { get; init; }
    public required bool HasBeenPublished { get; init; }
}
