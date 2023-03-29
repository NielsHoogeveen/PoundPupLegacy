namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DocumentableInserter : IDatabaseInserter<Documentable>
{
    public static async Task<DatabaseInserter<Documentable>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Documentable>("documentable", connection);
    }
}
