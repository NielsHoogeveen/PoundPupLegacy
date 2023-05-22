namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ChildPlacementType))]
public partial class ChildPlacementTypeJsonContext : JsonSerializerContext { }

public sealed record ChildPlacementType
{
    public int Id { get; init; }

    public required string Name { get; init; }
}
