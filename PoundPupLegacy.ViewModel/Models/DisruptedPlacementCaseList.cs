namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(DisruptedPlacementCaseList))]
public partial class DisruptedPlacementCaseListJsonContext : JsonSerializerContext { }

public sealed record DisruptedPlacementCaseList : PagedListBase<CaseTeaserListEntry>
{
}
