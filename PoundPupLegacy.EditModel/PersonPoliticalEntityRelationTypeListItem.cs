namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PersonPoliticalEntityRelationTypeListItem))]
public partial class PersonPoliticalEntityRelationTypeListItemJsonContext : JsonSerializerContext { }

public sealed record PersonPoliticalEntityRelationTypeListItem : EditListItemBase<PersonPoliticalEntityRelationTypeListItem>
{
}
