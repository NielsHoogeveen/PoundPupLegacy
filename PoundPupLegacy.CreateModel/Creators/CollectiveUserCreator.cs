namespace PoundPupLegacy.CreateModel.Creators;

public class CollectiveUserCreator : IEntityCreator<CollectiveUser>
{
    public async Task CreateAsync(IAsyncEnumerable<CollectiveUser> collectiveUsers, IDbConnection connection)
    {

        await using var collectiveUserWriter = await CollectiveUserInserter.CreateAsync(connection);

        await foreach (var collectiveUser in collectiveUsers) {
            await collectiveUserWriter.InsertAsync(collectiveUser);
        }
    }
}
