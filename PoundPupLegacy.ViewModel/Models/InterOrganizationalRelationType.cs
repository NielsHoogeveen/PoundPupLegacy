namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(InterOrganizationalRelationType))]
public partial class InterOrganizationalRelationTypeJsonContext : JsonSerializerContext { }

public sealed record InterOrganizationalRelationType: NameableBase
{
}
