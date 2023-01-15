namespace PoundPupLegacy.Db.Writers;

internal sealed class TypeOfAbuserWriter : IDatabaseWriter<TypeOfAbuser>
{
    public static async Task<DatabaseWriter<TypeOfAbuser>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<TypeOfAbuser>("type_of_abuser", connection);
    }
}
