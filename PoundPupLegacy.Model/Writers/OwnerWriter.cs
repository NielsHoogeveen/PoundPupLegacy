namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class OwnerWriter : IDatabaseWriter<Owner>
{
    public static async Task<DatabaseWriter<Owner>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<Owner>("owner", connection);
    }
}
