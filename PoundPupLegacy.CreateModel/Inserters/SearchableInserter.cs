namespace PoundPupLegacy.CreateModel.Inserters;
public class SearchableInserter : IDatabaseInserter<Searchable>
{
    public static async Task<DatabaseInserter<Searchable>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Searchable>("searchable", connection);
    }
}
