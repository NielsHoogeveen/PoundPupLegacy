namespace PoundPupLegacy.Db.Writers;

internal sealed class OwnerWriter : IDatabaseWriter<Owner>
{
    public static async Task<DatabaseWriter<Owner>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<Owner>(await SingleIdWriter.CreateSingleIdCommandAsync("owner", connection));
    }
}
