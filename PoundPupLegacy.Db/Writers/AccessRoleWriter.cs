namespace PoundPupLegacy.Db.Writers;

internal sealed class AccessRoleWriter : IDatabaseWriter<AccessRole>
{
    public static async Task<DatabaseWriter<AccessRole>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<AccessRole>(await SingleIdWriter.CreateSingleIdCommandAsync("access_role", connection));
    }
}
