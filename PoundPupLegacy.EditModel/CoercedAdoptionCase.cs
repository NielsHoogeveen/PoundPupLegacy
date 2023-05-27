namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingCoercedAdoptionCase))]
public partial class ExistingCoercedAdoptionCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewCoercedAdoptionCase))]
public partial class NewCoercedAdoptionCaseJsonContext : JsonSerializerContext { }

public interface CoercedAdoptionCase : Case, ResolvedNode
{
}
public sealed record ExistingCoercedAdoptionCase : ExistingCaseBase, CoercedAdoptionCase
{
}
public sealed record NewCoercedAdoptionCase : NewCaseBase, ResolvedNewNode, CoercedAdoptionCase
{
}
