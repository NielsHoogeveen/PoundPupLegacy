namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PersonPoliticalEntityRelationTypeListItem))]
public partial class PersonPoliticalEntityRelationTypeListItemJsonContext : JsonSerializerContext { }

public sealed record PersonPoliticalEntityRelationTypeListItem : EditListItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
