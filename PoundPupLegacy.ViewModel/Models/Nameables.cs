namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Nameables))]
public partial class NameablesJsonContext : JsonSerializerContext { }

public sealed record Nameables
{
    public required string NodeTypeName { get; init; }
    public required NameableListEntry[] NameableListEntries { get; init; }
}
public sealed record NameableListEntry : EntityListEntryBase
{
}
