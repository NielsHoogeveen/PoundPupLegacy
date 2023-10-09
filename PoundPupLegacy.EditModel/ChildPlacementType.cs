namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ChildPlacementType))]
public partial class ChildPlacementTypeJsonContext : JsonSerializerContext { }

public sealed record ChildPlacementType: ListEditElement
{
}
