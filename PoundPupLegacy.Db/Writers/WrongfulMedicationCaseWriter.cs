namespace PoundPupLegacy.Db.Writers;

internal sealed class WrongfulMedicationCaseWriter : IDatabaseWriter<WrongfulMedicationCase>
{
    public static async Task<DatabaseWriter<WrongfulMedicationCase>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<WrongfulMedicationCase>(await SingleIdWriter.CreateSingleIdCommandAsync("wrongful_medication_case", connection));
    }
}
