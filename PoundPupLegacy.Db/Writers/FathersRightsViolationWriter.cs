namespace PoundPupLegacy.Db.Writers;

internal sealed class FathersRightsViolationCaseWriter : IDatabaseWriter<FathersRightsViolationCase>
{
    public static async Task<DatabaseWriter<FathersRightsViolationCase>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<FathersRightsViolationCase>("fathers_rights_violation_case", connection);
    }
}
