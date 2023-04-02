namespace PoundPupLegacy.CreateModel.Creators;

public class UserCreator : IEntityCreator<User>
{
    public async Task CreateAsync(IAsyncEnumerable<User> users, IDbConnection connection)
    {

        await using var principalWriter = await PrincipalInserter.CreateAsync(connection);
        await using var publisherWriter = await PublisherInserter.CreateAsync(connection);
        await using var userWriter = await UserInserter.CreateAsync(connection);

        await foreach (var user in users) {
            await principalWriter.InsertAsync(user);
            await publisherWriter.InsertAsync(user);
            await userWriter.InsertAsync(user);
        }
    }
}