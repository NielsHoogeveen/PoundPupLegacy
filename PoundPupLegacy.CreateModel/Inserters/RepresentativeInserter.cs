namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class RepresentativeInserter : IDatabaseInserter<Representative>
{
    public static async Task<DatabaseInserter<Representative>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Representative>("representative", connection);
    }
}
