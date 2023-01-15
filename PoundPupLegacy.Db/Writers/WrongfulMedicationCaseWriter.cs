namespace PoundPupLegacy.Db.Writers;

internal sealed class WrongfulMedicationCaseWriter : IDatabaseWriter<WrongfulMedicationCase>
{
    public static async Task<DatabaseWriter<WrongfulMedicationCase>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<WrongfulMedicationCase>("wrongful_medication_case", connection);
    }
}
