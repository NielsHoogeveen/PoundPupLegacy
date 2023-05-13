namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(InterOrganizationalRelationTypeListItem))]
public partial class InterOrganizationalRelationTypeListItemJsonContext : JsonSerializerContext { }

public record InterOrganizationalRelationTypeListItem: EditListItem
{
    public required int? Id { get; init; }

    public required string Name { get; init; }

    public bool IsSymmetric { get; init; }

}
