namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(WrongfulMedicationCaseList))]
public partial class WrongfulMedicationCaseListJsonContext : JsonSerializerContext { }

public sealed record WrongfulMedicationCaseList : PagedListBase<CaseTeaserListEntry>
{
}
