namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingWrongfulMedicationCase))]
public partial class ExistingWrongfulMedicationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewWrongfulMedicationCase))]
public partial class NewWrongfulMedicationCaseJsonContext : JsonSerializerContext { }

public interface WrongfulMedicationCase : Case, ResolvedNode
{
}
public sealed record ExistingWrongfulMedicationCase : ExistingCaseBase, ExistingNode, WrongfulMedicationCase
{
}
public sealed record NewWrongfulMedicationCase : NewCaseBase, ResolvedNewNode, WrongfulMedicationCase
{
}
