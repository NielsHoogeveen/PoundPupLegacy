namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PartyPoliticalEntityRelationTypeListItem))]
public partial class PartyPoliticalEntityRelationTypeListItemJsonContext : JsonSerializerContext { }

public record PartyPoliticalEntityRelationTypeListItem : EditListItem
{
    public required int? Id { get; init; }
    public required string Name { get; init; }
}
