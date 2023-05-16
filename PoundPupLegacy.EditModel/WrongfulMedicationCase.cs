namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingWrongfulMedicationCase))]
public partial class ExistingWrongfulMedicationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewWrongfulMedicationCase))]
public partial class NewWrongfulMedicationCaseJsonContext : JsonSerializerContext { }

public interface WrongfulMedicationCase : Case
{
}
public sealed record ExistingWrongfulMedicationCase : WrongfulMedicationCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }
}
public sealed record NewWrongfulMedicationCase : WrongfulMedicationCaseBase, NewNode
{
}
public abstract record WrongfulMedicationCaseBase : CaseBase, WrongfulMedicationCase
{

}
