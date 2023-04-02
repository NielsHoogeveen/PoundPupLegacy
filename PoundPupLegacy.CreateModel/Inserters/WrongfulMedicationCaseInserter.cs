namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class WrongfulMedicationCaseInserter : IDatabaseInserter<WrongfulMedicationCase>
{
    public static async Task<DatabaseInserter<WrongfulMedicationCase>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<WrongfulMedicationCase>("wrongful_medication_case", connection);
    }
}
