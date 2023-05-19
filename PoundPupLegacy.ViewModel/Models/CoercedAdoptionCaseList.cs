namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CoercedAdoptionCaseList))]
public partial class CoercedAdoptionCaseListJsonContext : JsonSerializerContext { }

public sealed record CoercedAdoptionCaseList : PagedListBase<CaseListEntry>
{
}
