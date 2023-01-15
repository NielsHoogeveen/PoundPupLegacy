namespace PoundPupLegacy.Db.Writers;

internal sealed class UserRoleWriter : IDatabaseWriter<UserRole>
{
    public static async Task<DatabaseWriter<UserRole>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<UserRole>("user_role", connection);
    }
}
