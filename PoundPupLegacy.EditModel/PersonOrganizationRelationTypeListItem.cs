namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PersonOrganizationRelationTypeListItem))]
public partial class PersonOrganizationRelationTypeListItemJsonContext : JsonSerializerContext { }

public sealed record PersonOrganizationRelationTypeListItem : EditListItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
