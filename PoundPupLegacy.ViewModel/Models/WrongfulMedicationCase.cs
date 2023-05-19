namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(WrongfulMedicationCase))]
public partial class WrongfulMedicationCaseJsonContext : JsonSerializerContext { }

public sealed record WrongfulMedicationCase : CaseBase
{
}
