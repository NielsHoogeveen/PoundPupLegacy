namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class FamilySizeWriter : IDatabaseWriter<FamilySize>
{
    public static async Task<DatabaseWriter<FamilySize>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<FamilySize>("family_size", connection);
    }
}
