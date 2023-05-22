namespace PoundPupLegacy.CreateModel;

public sealed record NewWrongfulMedicationCase : NewCaseBase, EventuallyIdentifiableWrongfulMedicationCase
{
}
public sealed record ExistingWrongfulMedicationCase : ExistingCaseBase, ImmediatelyIdentifiableWrongfulMedicationCase
{
}
public interface ImmediatelyIdentifiableWrongfulMedicationCase : WrongfulMedicationCase, ImmediatelyIdentifiableCase
{
}
public interface EventuallyIdentifiableWrongfulMedicationCase : WrongfulMedicationCase, EventuallyIdentifiableCase
{
}

public interface WrongfulMedicationCase : Case
{
}
