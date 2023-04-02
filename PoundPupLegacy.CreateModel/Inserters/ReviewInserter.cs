namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ReviewInserter : IDatabaseInserter<Review>
{
    public static async Task<DatabaseInserter<Review>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Review>("review", connection);
    }
}
