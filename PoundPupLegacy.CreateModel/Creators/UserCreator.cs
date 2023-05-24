namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UserCreatorFactory(
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<Publisher> publisherInserterFactory,
    IDatabaseInserterFactory<User> userInserterFactory
) : IInsertingEntityCreatorFactory<User>
{
    public async Task<InsertingEntityCreator<User>> CreateAsync(IDbConnection connection) =>
        new(
            new() {
                await principalInserterFactory.CreateAsync(connection),
                await publisherInserterFactory.CreateAsync(connection),
                await userInserterFactory.CreateAsync(connection)
            }
        );
}