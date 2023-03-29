namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class TypeOfAbuseWriter : IDatabaseWriter<TypeOfAbuse>
{
    public static async Task<DatabaseWriter<TypeOfAbuse>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<TypeOfAbuse>("type_of_abuse", connection);
    }
}
