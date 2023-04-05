namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UserCreator : EntityCreator<User>
{
    private readonly IDatabaseInserterFactory<Principal> _principalInserterFactory;
    private readonly IDatabaseInserterFactory<Publisher> _publisherInserterFactory;
    private readonly IDatabaseInserterFactory<User> _userInserterFactory;
    public UserCreator(
        IDatabaseInserterFactory<Principal> principalInserterFactory,
        IDatabaseInserterFactory<Publisher> publisherInserterFactory,
        IDatabaseInserterFactory<User> userInserterFactory
    )
    {
        _principalInserterFactory = principalInserterFactory;
        _publisherInserterFactory = publisherInserterFactory;
        _userInserterFactory = userInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<User> users, IDbConnection connection)
    {
        await using var principalWriter = await _principalInserterFactory.CreateAsync(connection);
        await using var publisherWriter = await _publisherInserterFactory.CreateAsync(connection);
        await using var userWriter = await _userInserterFactory.CreateAsync(connection);

        await foreach (var user in users) {
            await principalWriter.InsertAsync(user);
            await publisherWriter.InsertAsync(user);
            await userWriter.InsertAsync(user);
        }
    }
}