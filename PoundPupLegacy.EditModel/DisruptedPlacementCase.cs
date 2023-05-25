namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingDisruptedPlacementCase))]
public partial class ExistingDisruptedPlacementCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewDisruptedPlacementCase))]
public partial class NewDisruptedPlacementCaseJsonContext : JsonSerializerContext { }

public interface DisruptedPlacementCase : Case, ResolvedNode
{
}
public sealed record ExistingDisruptedPlacementCase : DisruptedPlacementCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }
}
public sealed record NewDisruptedPlacementCase : DisruptedPlacementCaseBase, ResolvedNewNode
{
}
public abstract record DisruptedPlacementCaseBase : CaseBase, DisruptedPlacementCase
{

}
