namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(InterPersonalRelationTypeListItem))]
public partial class InterPersonalRelationTypeListItemJsonContext : JsonSerializerContext { }

public sealed record InterPersonalRelationTypeListItem : EditListItemBase<InterPersonalRelationTypeListItem>
{
    public bool IsSymmetric { get; init; }
}
