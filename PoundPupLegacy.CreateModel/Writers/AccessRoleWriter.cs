namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class AccessRoleWriter : IDatabaseWriter<AccessRole>
{
    public static async Task<DatabaseWriter<AccessRole>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<AccessRole>("access_role", connection);
    }
}
