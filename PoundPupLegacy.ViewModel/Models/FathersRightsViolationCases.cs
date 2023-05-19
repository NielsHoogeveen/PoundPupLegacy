namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(FathersRightsViolationCases))]
public partial class FathersRightsViolationCasesJsonContext : JsonSerializerContext { }

public sealed record FathersRightsViolationCases : TermedListBase<FathersRightsViolationCaseList, CaseListEntry>
{
}
