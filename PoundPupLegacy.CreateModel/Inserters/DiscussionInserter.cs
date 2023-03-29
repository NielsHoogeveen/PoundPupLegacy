namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DiscussionInserter : IDatabaseInserter<Discussion>
{
    public static async Task<DatabaseInserter<Discussion>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Discussion>("discussion", connection);
    }
}
