namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingFathersRightsViolationCase))]
public partial class ExistingFathersRightsViolationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewFathersRightsViolationCase))]
public partial class NewFathersRightsViolationCaseJsonContext : JsonSerializerContext { }

public interface FathersRightsViolationCase : Case, ResolvedNode
{
}
public sealed record ExistingFathersRightsViolationCase : FathersRightsViolationCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }
}
public sealed record NewFathersRightsViolationCase : FathersRightsViolationCaseBase, ResolvedNewNode
{
}
public abstract record FathersRightsViolationCaseBase : CaseBase, FathersRightsViolationCase
{

}
