namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingWrongfulMedicationCase))]
public partial class ExistingWrongfulMedicationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewWrongfulMedicationCase))]
public partial class NewWrongfulMedicationCaseJsonContext : JsonSerializerContext { }

public interface WrongfulMedicationCase : Case
{
}
public record ExistingWrongfulMedicationCase : WrongfulMedicationCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }
}
public record NewWrongfulMedicationCase : WrongfulMedicationCaseBase, NewNode
{
}
public record WrongfulMedicationCaseBase : CaseBase, WrongfulMedicationCase
{

}
