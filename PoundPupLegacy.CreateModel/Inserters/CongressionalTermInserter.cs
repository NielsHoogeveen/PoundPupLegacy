namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CongressionalTermInserter : IDatabaseInserter<CongressionalTerm>
{
    public static async Task<DatabaseInserter<CongressionalTerm>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<CongressionalTerm>("congressional_term", connection);
    }
}
