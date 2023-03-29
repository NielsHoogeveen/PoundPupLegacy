namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class OwnerInserter : IDatabaseInserter<Owner>
{
    public static async Task<DatabaseInserter<Owner>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<Owner>("owner", connection);
    }
}
