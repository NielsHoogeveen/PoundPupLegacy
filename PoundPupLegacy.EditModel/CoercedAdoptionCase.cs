namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingCoercedAdoptionCase))]
public partial class ExistingCoercedAdoptionCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewCoercedAdoptionCase))]
public partial class NewCoercedAdoptionCaseJsonContext : JsonSerializerContext { }

public interface CoercedAdoptionCase : Case
{
}
public record ExistingCoercedAdoptionCase : CoercedAdoptionCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }
}
public record NewCoercedAdoptionCase : CoercedAdoptionCaseBase, NewNode
{
}
public record CoercedAdoptionCaseBase : CaseBase, CoercedAdoptionCase
{

}
