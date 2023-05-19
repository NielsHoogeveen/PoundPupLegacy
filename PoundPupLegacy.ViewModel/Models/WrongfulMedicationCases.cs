namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(WrongfulMedicationCases))]
public partial class WrongfulMedicationCasesJsonContext : JsonSerializerContext { }

public sealed record WrongfulMedicationCases : TermedListBase<WrongfulMedicationCaseList, CaseListEntry>
{
}
