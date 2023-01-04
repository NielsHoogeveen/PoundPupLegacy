namespace PoundPupLegacy.Db;

public class UserCreator : IEntityCreator<User>
{
    public static async Task CreateAsync(IEnumerable<User> users, NpgsqlConnection connection)
    {

        await using var accessRoleWriter = await AccessRoleWriter.CreateAsync(connection);
        await using var userWriter = await UserWriter.CreateAsync(connection);

        foreach (var user in users)
        {
            await accessRoleWriter.WriteAsync(user);
            await userWriter.WriteAsync(user);
        }
    }
}