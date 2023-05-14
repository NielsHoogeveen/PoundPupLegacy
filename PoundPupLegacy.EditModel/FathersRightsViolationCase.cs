namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingFathersRightsViolationCase))]
public partial class ExistingFathersRightsViolationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewFathersRightsViolationCase))]
public partial class NewFathersRightsViolationCaseJsonContext : JsonSerializerContext { }

public interface FathersRightsViolationCase : Case
{
}
public record ExistingFathersRightsViolationCase : FathersRightsViolationCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }
}
public record NewFathersRightsViolationCase : FathersRightsViolationCaseBase, NewNode
{
}
public record FathersRightsViolationCaseBase : CaseBase, FathersRightsViolationCase
{

}
