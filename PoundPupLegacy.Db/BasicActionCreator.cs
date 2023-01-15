namespace PoundPupLegacy.Db;

public class BasicActionCreator : IEntityCreator<BasicAction>
{
    public static async Task CreateAsync(IAsyncEnumerable<BasicAction> actions, NpgsqlConnection connection)
    {

        await using var actionWriter = await ActionWriter.CreateAsync(connection);
        await using var basicActionWriter = await BasicActionWriter.CreateAsync(connection);

        await foreach (var action in actions)
        {
            await actionWriter.WriteAsync(action);
            await basicActionWriter.WriteAsync(action);
        }
    }
}
