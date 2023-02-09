namespace PoundPupLegacy.Db.Writers;

internal sealed class CaseTypeWriter : IDatabaseWriter<CaseType>
{
    public static async Task<DatabaseWriter<CaseType>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<CaseType>("case_type", connection);
    }
}
