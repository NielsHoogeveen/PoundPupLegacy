namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(ChildTraffickingCaseList))]
public partial class ChildTraffickingCaseListJsonContext : JsonSerializerContext { }

public sealed record ChildTraffickingCaseList : PagedListBase<CaseListEntry>
{
}
