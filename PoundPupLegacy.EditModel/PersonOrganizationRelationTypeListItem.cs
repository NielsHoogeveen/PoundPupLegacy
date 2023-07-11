namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PersonOrganizationRelationTypeListItem))]
public partial class PersonOrganizationRelationTypeListItemJsonContext : JsonSerializerContext { }

public sealed record PersonOrganizationRelationTypeListItem : EditListItemBase<PersonOrganizationRelationTypeListItem>
{
}
