namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(PersonOrganizationRelationType))]
public partial class PersonOrganizationRelationTypeJsonContext : JsonSerializerContext { }

public sealed record PersonOrganizationRelationType: NameableBase
{
}
