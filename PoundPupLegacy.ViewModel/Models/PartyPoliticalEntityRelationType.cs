namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(PartyPoliticalEntityRelationType))]
public partial class PartyPoliticalEntityRelationTypeJsonContext : JsonSerializerContext { }

public sealed record PartyPoliticalEntityRelationType: NameableBase
{
}
