namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CollectiveUserCreator : EntityCreator<CollectiveUser>
{
    private readonly IDatabaseInserterFactory<CollectiveUser> _collectiveUserInserterFactory;
    public CollectiveUserCreator(IDatabaseInserterFactory<CollectiveUser> collectiveUserInserterFactory)
    {
        _collectiveUserInserterFactory = collectiveUserInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<CollectiveUser> collectiveUsers, IDbConnection connection)
    {

        await using var collectiveUserWriter = await _collectiveUserInserterFactory.CreateAsync(connection);

        await foreach (var collectiveUser in collectiveUsers) {
            await collectiveUserWriter.InsertAsync(collectiveUser);
        }
    }
}
