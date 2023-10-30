namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(ChildPlacementType))]
public partial class ChildPlacementTypeJsonContext : JsonSerializerContext { }

public sealed record ChildPlacementType: NameableBase
{
}
