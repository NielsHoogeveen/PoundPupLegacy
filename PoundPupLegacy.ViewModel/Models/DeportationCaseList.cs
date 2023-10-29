namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(DeportationCaseList))]
public partial class DeportationCaseListJsonContext : JsonSerializerContext { }

public sealed record DeportationCaseList : PagedListBase<CaseTeaserListEntry>
{
}
