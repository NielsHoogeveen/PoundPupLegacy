namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingCoercedAdoptionCase))]
public partial class ExistingCoercedAdoptionCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewCoercedAdoptionCase))]
public partial class NewCoercedAdoptionCaseJsonContext : JsonSerializerContext { }

public interface CoercedAdoptionCase : Case, ResolvedNode
{
}
public sealed record ExistingCoercedAdoptionCase : CoercedAdoptionCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }
}
public sealed record NewCoercedAdoptionCase : CoercedAdoptionCaseBase, ResolvedNewNode
{
}
public abstract record CoercedAdoptionCaseBase : CaseBase, CoercedAdoptionCase
{

}
