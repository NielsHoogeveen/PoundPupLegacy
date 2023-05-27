namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingDisruptedPlacementCase))]
public partial class ExistingDisruptedPlacementCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewDisruptedPlacementCase))]
public partial class NewDisruptedPlacementCaseJsonContext : JsonSerializerContext { }

public interface DisruptedPlacementCase : Case, ResolvedNode
{
}
public sealed record ExistingDisruptedPlacementCase : ExistingCaseBase, DisruptedPlacementCase
{
}
public sealed record NewDisruptedPlacementCase : NewCaseBase, ResolvedNewNode, DisruptedPlacementCase
{
}
