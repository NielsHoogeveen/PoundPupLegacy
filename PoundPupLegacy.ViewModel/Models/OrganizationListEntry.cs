namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(OrganizationListEntry))]
public partial class OrganizationListEntryJsonContext : JsonSerializerContext { }

public sealed record OrganizationListEntry : EntityListEntryBase
{
}
