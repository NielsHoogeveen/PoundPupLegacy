namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ChildPlacementTypeListItem))]
public partial class ChildPlacementTypeListItemJsonContext : JsonSerializerContext { }

public sealed record ChildPlacementTypeListItem: ListEditElement
{
}
