namespace PoundPupLegacy.Db.Writers;

internal class TypeOfAbuserWriter : IDatabaseWriter<TypeOfAbuser>
{
    public static async Task<DatabaseWriter<TypeOfAbuser>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<TypeOfAbuser>(await SingleIdWriter.CreateSingleIdCommandAsync("type_of_abuser", connection));
    }
}
