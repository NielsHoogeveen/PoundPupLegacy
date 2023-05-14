namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingDisruptedPlacementCase))]
public partial class ExistingDisruptedPlacementCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewDisruptedPlacementCase))]
public partial class NewDisruptedPlacementCaseJsonContext : JsonSerializerContext { }

public interface DisruptedPlacementCase : Case
{
}
public record ExistingDisruptedPlacementCase : DisruptedPlacementCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }
}
public record NewDisruptedPlacementCase : DisruptedPlacementCaseBase, NewNode
{
}
public record DisruptedPlacementCaseBase : CaseBase, DisruptedPlacementCase
{

}
