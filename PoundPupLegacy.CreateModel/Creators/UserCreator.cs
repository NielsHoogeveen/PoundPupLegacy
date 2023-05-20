namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UserCreator(
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<Publisher> publisherInserterFactory,
    IDatabaseInserterFactory<User> userInserterFactory
) : EntityCreator<User>
{
    public override async Task CreateAsync(IAsyncEnumerable<User> users, IDbConnection connection)
    {
        await using var principalWriter = await principalInserterFactory.CreateAsync(connection);
        await using var publisherWriter = await publisherInserterFactory.CreateAsync(connection);
        await using var userWriter = await userInserterFactory.CreateAsync(connection);

        await foreach (var user in users) {
            await principalWriter.InsertAsync(user);
            await publisherWriter.InsertAsync(user);
            await userWriter.InsertAsync(user);
        }
    }
}