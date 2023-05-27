namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingFathersRightsViolationCase))]
public partial class ExistingFathersRightsViolationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewFathersRightsViolationCase))]
public partial class NewFathersRightsViolationCaseJsonContext : JsonSerializerContext { }

public interface FathersRightsViolationCase : Case, ResolvedNode
{
}
public sealed record ExistingFathersRightsViolationCase : ExistingCaseBase, FathersRightsViolationCase 
{
}
public sealed record NewFathersRightsViolationCase : NewCaseBase, FathersRightsViolationCase, ResolvedNewNode
{
}
