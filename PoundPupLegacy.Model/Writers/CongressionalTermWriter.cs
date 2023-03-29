namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class CongressionalTermWriter : IDatabaseWriter<CongressionalTerm>
{
    public static async Task<DatabaseWriter<CongressionalTerm>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<CongressionalTerm>("congressional_term", connection);
    }
}
