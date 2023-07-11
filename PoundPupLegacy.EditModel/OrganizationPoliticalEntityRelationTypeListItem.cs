namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(OrganizationPoliticalEntityRelationTypeListItem))]
public partial class OrganizationPoliticalEntityRelationTypeListItemJsonContext : JsonSerializerContext { }

public sealed record OrganizationPoliticalEntityRelationTypeListItem : EditListItemBase<OrganizationPoliticalEntityRelationTypeListItem>
{
}
