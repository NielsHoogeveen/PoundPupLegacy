namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(InterPersonalRelationTypeListItem))]
public partial class InterPersonalRelationTypeListItemJsonContext : JsonSerializerContext { }

public record InterPersonalRelationTypeListItem : EditListItem
{
    public required int? Id { get; init; }
    public required string Name { get; init; }
    public bool IsSymmetric { get; init; }
}
