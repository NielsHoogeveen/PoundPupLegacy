namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Organizations))]
public partial class OrganizationsJsonContext : JsonSerializerContext { }

public sealed record Organizations : PagedListBase<OrganizationListEntry>
{
}
