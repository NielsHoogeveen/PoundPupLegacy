namespace PoundPupLegacy.Db.Writers;

internal sealed class CoercedAdoptionCaseWriter : IDatabaseWriter<CoercedAdoptionCase>
{
    public static async Task<DatabaseWriter<CoercedAdoptionCase>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<CoercedAdoptionCase>(await SingleIdWriter.CreateSingleIdCommandAsync("coerced_adoption_case", connection));
    }
}
