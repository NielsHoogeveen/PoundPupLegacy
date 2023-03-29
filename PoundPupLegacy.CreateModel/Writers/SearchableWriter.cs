namespace PoundPupLegacy.CreateModel.Writers;
public class SearchableWriter : IDatabaseWriter<Searchable>
{
    public static async Task<DatabaseWriter<Searchable>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<Searchable>("searchable", connection);
    }
}
