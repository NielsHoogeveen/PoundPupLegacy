namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CaseTypeInserter : IDatabaseInserter<CaseType>
{
    public static async Task<DatabaseInserter<CaseType>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<CaseType>("case_type", connection);
    }
}
