namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CollectiveUserCreator(IDatabaseInserterFactory<CollectiveUser> collectiveUserInserterFactory) : EntityCreator<CollectiveUser>
{
    public override async Task CreateAsync(IAsyncEnumerable<CollectiveUser> collectiveUsers, IDbConnection connection)
    {
        await using var collectiveUserWriter = await collectiveUserInserterFactory.CreateAsync(connection);

        await foreach (var collectiveUser in collectiveUsers) {
            await collectiveUserWriter.InsertAsync(collectiveUser);
        }
    }
}
