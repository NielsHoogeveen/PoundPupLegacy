namespace PoundPupLegacy.Db.Writers;

internal class UserGroupWriter : IDatabaseWriter<UserGroup>
{
    public static async Task<DatabaseWriter<UserGroup>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<UserGroup>(await SingleIdWriter.CreateSingleIdCommandAsync("user_group", connection));
    }
}
