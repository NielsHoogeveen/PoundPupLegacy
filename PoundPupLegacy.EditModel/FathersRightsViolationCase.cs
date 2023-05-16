namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingFathersRightsViolationCase))]
public partial class ExistingFathersRightsViolationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewFathersRightsViolationCase))]
public partial class NewFathersRightsViolationCaseJsonContext : JsonSerializerContext { }

public interface FathersRightsViolationCase : Case
{
}
public sealed record ExistingFathersRightsViolationCase : FathersRightsViolationCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }
}
public sealed record NewFathersRightsViolationCase : FathersRightsViolationCaseBase, NewNode
{
}
public abstract record FathersRightsViolationCaseBase : CaseBase, FathersRightsViolationCase
{

}
