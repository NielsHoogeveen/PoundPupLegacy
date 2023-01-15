namespace PoundPupLegacy.Db.Writers;

internal sealed class CoercedAdoptionCaseWriter : IDatabaseWriter<CoercedAdoptionCase>
{
    public static async Task<DatabaseWriter<CoercedAdoptionCase>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<CoercedAdoptionCase>("coerced_adoption_case", connection);
    }
}
