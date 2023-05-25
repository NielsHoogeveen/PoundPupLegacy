namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UserCreatorFactory(
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<Publisher> publisherInserterFactory,
    IDatabaseInserterFactory<User> userInserterFactory
) : IEntityCreatorFactory<User>
{
    public async Task<IEntityCreator<User>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<User>(
            new() {
                await principalInserterFactory.CreateAsync(connection),
                await publisherInserterFactory.CreateAsync(connection),
                await userInserterFactory.CreateAsync(connection)
            }
        );
}