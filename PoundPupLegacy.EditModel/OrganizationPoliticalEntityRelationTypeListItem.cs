namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(OrganizationPoliticalEntityRelationTypeListItem))]
public partial class OrganizationPoliticalEntityRelationTypeListItemJsonContext : JsonSerializerContext { }

public sealed record OrganizationPoliticalEntityRelationTypeListItem : EditListItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
