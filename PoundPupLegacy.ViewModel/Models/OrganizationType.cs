namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(OrganizationType))]
public partial class OrganizationTypeJsonContext : JsonSerializerContext { }

public sealed record OrganizationType: NameableBase
{
}
