namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(PersonListEntry))]
public partial class PersonListEntryJsonContext : JsonSerializerContext { }

public sealed record PersonListEntry : ListEntryBase
{
    public required bool HasBeenPublished { get; init; }

}
