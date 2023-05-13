namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(OrganizationListEntry))]
public partial class OrganizationListEntryJsonContext : JsonSerializerContext { }

public record OrganizationListEntry : ListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }
}
