namespace PoundPupLegacy.Db.Writers;

internal class FathersRightsViolationCaseWriter : IDatabaseWriter<FathersRightsViolationCase>
{
    public static async Task<DatabaseWriter<FathersRightsViolationCase>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<FathersRightsViolationCase>(await SingleIdWriter.CreateSingleIdCommandAsync("fathers_rights_violation_case", connection));
    }
}
