namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(TagListEntry))]
public partial class TagListEntryJsonContext : JsonSerializerContext { }

public sealed record TagListEntry : ListEntryBase
{
    public required string NodeTypeName { get; init; }
}
