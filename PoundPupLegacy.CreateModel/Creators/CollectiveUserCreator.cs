namespace PoundPupLegacy.CreateModel.Creators;

public class CollectiveUserCreator : IEntityCreator<CollectiveUser>
{
    public static async Task CreateAsync(IAsyncEnumerable<CollectiveUser> collectiveUsers, NpgsqlConnection connection)
    {

        await using var collectiveUserWriter = await CollectiveUserInserter.CreateAsync(connection);

        await foreach (var collectiveUser in collectiveUsers) {
            await collectiveUserWriter.InsertAsync(collectiveUser);
        }
    }
}
