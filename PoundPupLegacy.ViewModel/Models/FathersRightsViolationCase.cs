namespace PoundPupLegacy.ViewModel.Models;
[JsonSerializable(typeof(FathersRightsViolationCase))]
public partial class FathersRightsViolationCaseJsonContext : JsonSerializerContext { }

public sealed record FathersRightsViolationCase : CaseBase
{
}
