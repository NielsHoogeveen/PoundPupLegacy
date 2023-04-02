namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class AccessRoleInserter : IDatabaseInserter<AccessRole>
{
    public static async Task<DatabaseInserter<AccessRole>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<AccessRole>("access_role", connection);
    }
}
