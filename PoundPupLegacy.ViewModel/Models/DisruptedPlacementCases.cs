namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(DisruptedPlacementCases))]
public partial class DisruptedPlacementCasesJsonContext : JsonSerializerContext { }

public sealed record DisruptedPlacementCases : TermedListBase<DisruptedPlacementCaseList, CaseListEntry>
{
}
