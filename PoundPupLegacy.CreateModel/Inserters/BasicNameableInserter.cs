namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BasicNameableInserter : IDatabaseInserter<BasicNameable>
{
    public static async Task<DatabaseInserter<BasicNameable>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<BasicNameable>("basic_nameable", connection);
    }
}
