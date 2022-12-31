namespace PoundPupLegacy.Db.Writers;

internal class WrongfulMedicationCaseWriter : IDatabaseWriter<WrongfulMedicationCase>
{
    public static DatabaseWriter<WrongfulMedicationCase> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<WrongfulMedicationCase>(SingleIdWriter.CreateSingleIdCommand("wrongful_medication_case", connection));
    }
}
