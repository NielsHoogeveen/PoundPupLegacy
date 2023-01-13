namespace PoundPupLegacy.Db.Writers;

internal sealed class TypeOfAbuseWriter : IDatabaseWriter<TypeOfAbuse>
{
    public static async Task<DatabaseWriter<TypeOfAbuse>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<TypeOfAbuse>(await SingleIdWriter.CreateSingleIdCommandAsync("type_of_abuse", connection));
    }
}
