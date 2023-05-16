namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(PersonListEntry))]
public partial class PersonListEntryJsonContext : JsonSerializerContext { }

public sealed record PersonListEntry : ListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }
    public required bool HasBeenPublished { get; init; }

}
