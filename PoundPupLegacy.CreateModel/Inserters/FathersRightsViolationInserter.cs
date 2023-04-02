namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FathersRightsViolationCaseWriter : IDatabaseInserter<FathersRightsViolationCase>
{
    public static async Task<DatabaseInserter<FathersRightsViolationCase>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<FathersRightsViolationCase>("fathers_rights_violation_case", connection);
    }
}
