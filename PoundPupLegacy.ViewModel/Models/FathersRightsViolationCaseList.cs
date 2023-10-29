namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(FathersRightsViolationCaseList))]
public partial class FathersRightsViolationCaseListJsonContext : JsonSerializerContext { }

public sealed record FathersRightsViolationCaseList : PagedListBase<CaseTeaserListEntry>
{
}
