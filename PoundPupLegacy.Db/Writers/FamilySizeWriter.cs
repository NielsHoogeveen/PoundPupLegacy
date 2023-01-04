namespace PoundPupLegacy.Db.Writers;

internal class FamilySizeWriter : IDatabaseWriter<FamilySize>
{
    public static async Task<DatabaseWriter<FamilySize>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<FamilySize>(await SingleIdWriter.CreateSingleIdCommandAsync("family_size", connection));
    }
}
