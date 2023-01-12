namespace PoundPupLegacy.Db;

public class CollectiveUserCreator : IEntityCreator<Model.CollectiveUser>
{
    public static async Task CreateAsync(IAsyncEnumerable<Model.CollectiveUser> collectiveUsers, NpgsqlConnection connection)
    {

        await using var collectiveUserWriter = await CollectiveUserWriter.CreateAsync(connection);

        await foreach (var collectiveUser in collectiveUsers)
        {
            await collectiveUserWriter.WriteAsync(collectiveUser);
        }
    }
}
