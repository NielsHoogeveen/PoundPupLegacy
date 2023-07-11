namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(InterOrganizationalRelationTypeListItem))]
public partial class InterOrganizationalRelationTypeListItemJsonContext : JsonSerializerContext { }

public sealed record InterOrganizationalRelationTypeListItem : EditListItemBase<InterOrganizationalRelationTypeListItem>
{
    public bool IsSymmetric { get; init; }
}
