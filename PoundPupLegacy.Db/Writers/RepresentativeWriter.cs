namespace PoundPupLegacy.Db.Writers;

internal sealed class RepresentativeWriter : IDatabaseWriter<Representative>
{
    public static async Task<DatabaseWriter<Representative>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<Representative>("representative", connection);
    }
}
