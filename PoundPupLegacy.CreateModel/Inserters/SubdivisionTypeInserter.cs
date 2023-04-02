namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SubdivisionTypeInserter : IDatabaseInserter<SubdivisionType>
{
    public static async Task<DatabaseInserter<SubdivisionType>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<SubdivisionType>("subdivision_type", connection);
    }
}
