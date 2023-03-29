namespace PoundPupLegacy.CreateModel.Creators;

public class UserCreator : IEntityCreator<User>
{
    public static async Task CreateAsync(IAsyncEnumerable<User> users, NpgsqlConnection connection)
    {

        await using var principalWriter = await PrincipalInserter.CreateAsync(connection);
        await using var publisherWriter = await PublisherInserter.CreateAsync(connection);
        await using var userWriter = await UserInserter.CreateAsync(connection);

        await foreach (var user in users) {
            await principalWriter.WriteAsync(user);
            await publisherWriter.WriteAsync(user);
            await userWriter.WriteAsync(user);
        }
    }
}